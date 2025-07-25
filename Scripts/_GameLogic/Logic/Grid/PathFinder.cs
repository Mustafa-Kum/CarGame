using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public interface IHeuristicCalculator
    {
        int Calculate(Vector2Int pointA, Vector2Int pointB);
    }

    public class HeuristicCalculator : IHeuristicCalculator
    {
        public int Calculate(Vector2Int pointA, Vector2Int pointB)
        {
            return Math.Abs(pointA.x - pointB.x) + Math.Abs(pointA.y - pointB.y);
        }
    }

    internal class PathFinder
    {
        #region Private Variables

        private static readonly Vector2Int DirectionRight = new(1, 0);
        private static readonly Vector2Int DirectionLeft = new(-1, 0);
        private static readonly Vector2Int DirectionUp = new(0, 1);
        private static readonly Vector2Int DirectionDown = new(0, -1);

        private readonly GridTile[,] _matrix;
        private readonly Vector2Int? _start;
        private readonly Vector2Int? _end;
        private readonly IHeuristicCalculator _heuristicCalculator;

        #endregion

        #region Constructor

        public PathFinder(GridTile[,] matrix, Vector2Int? start, Vector2Int? end,
            IHeuristicCalculator heuristicCalculator)
        {
            _matrix = matrix;
            _start = start;
            _end = end;
            _heuristicCalculator = heuristicCalculator;
        }

        #endregion

        #region Public Methods

        public List<GridTile> FindPath()
        {
            var path = new List<GridTile>();
            if (_start == null || _end == null) return path;

            var openSet = new PathfindingPriorityQueue();
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, int> { [_start.Value] = 0 };
            var fScore = ComputeFScore(_start.Value);

            openSet.Enqueue(_start.Value, fScore[_start.Value]);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current == _end.Value) return ReconstructPath(cameFrom, current);

                foreach (var neighbor in GetAdjacentGridTiles(current))
                    if (IsValidGridObject(neighbor))
                        HandleScoreCalculations(cameFrom, gScore, fScore, openSet, neighbor, current);
            }

            return path;
        }

        #endregion

        #region Private Methods

        private Dictionary<Vector2Int, int> ComputeFScore(Vector2Int start)
        {
            return new Dictionary<Vector2Int, int>
            {
                [start] = _heuristicCalculator.Calculate(start, _end.Value)
            };
        }

        private bool IsValidGridObject(Vector2Int neighbor)
        {
            return _matrix[neighbor.x, neighbor.y].GetGridObjectType() == GridObjectType.Matched;
        }

        private void HandleScoreCalculations(
            Dictionary<Vector2Int, Vector2Int> cameFrom,
            Dictionary<Vector2Int, int> gScore,
            Dictionary<Vector2Int, int> fScore,
            PathfindingPriorityQueue openSet,
            Vector2Int neighbor,
            Vector2Int current)
        {
            var tentativeGScore = gScore[current] + 1;

            if (gScore.TryGetValue(neighbor, out var gScoreValue) && tentativeGScore >= gScoreValue) return;

            UpdateScoresAndCameFrom(cameFrom, gScore, fScore, neighbor, current, tentativeGScore);
            var neighborFScore = fScore[neighbor];

            EnqueueOrUpdate(openSet, neighbor, neighborFScore);
        }

        private void UpdateScoresAndCameFrom(Dictionary<Vector2Int, Vector2Int> cameFrom,
            Dictionary<Vector2Int, int> gScore,
            Dictionary<Vector2Int, int> fScore,
            Vector2Int neighbor,
            Vector2Int current,
            int tentativeGScore)
        {
            cameFrom[neighbor] = current;
            gScore[neighbor] = tentativeGScore;
            fScore[neighbor] = tentativeGScore + _heuristicCalculator.Calculate(neighbor, _end.Value);
        }

        private void EnqueueOrUpdate(PathfindingPriorityQueue openSet, Vector2Int neighbor, int neighborFScore)
        {
            if (!openSet.Contains(neighbor))
                openSet.Enqueue(neighbor, neighborFScore);
            else
                openSet.UpdatePriority(neighbor, neighborFScore);
        }

        private List<GridTile> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var totalPath = new List<GridTile> { _matrix[current.x, current.y] };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Add(_matrix[current.x, current.y]);
            }
            
            return totalPath;
        }

        private IEnumerable<Vector2Int> GetAdjacentGridTiles(Vector2Int current)
        {
            var adjacent = new List<Vector2Int>();
            var directions = new List<Vector2Int>
            {
                DirectionRight,
                DirectionLeft,
                DirectionUp,
                DirectionDown
            };

            foreach (var direction in directions)
            {
                var adjacentPosition = current + direction;
                if (IsInBounds(adjacentPosition)) adjacent.Add(adjacentPosition);
            }

            return adjacent;
        }

        private bool IsInBounds(Vector2Int point)
        {
            return point.x >= 0 && point.x < _matrix.GetLength(0) &&
                   point.y >= 0 && point.y < _matrix.GetLength(1);
        }

        #endregion
    }
}