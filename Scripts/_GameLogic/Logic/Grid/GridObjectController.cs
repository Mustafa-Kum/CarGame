using _Game.Scripts._GameLogic.Data.Grid;
using Sirenix.OdinInspector;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    using UnityEngine;

    namespace _Game.Scripts._GameLogic.Logic.Grid
    {
        public class GridObjectController : MonoBehaviour
        {
            [ShowInInspector]
            private IMatchableAction[] _matchableActions;
            private IPathAction[] _pathActions;
            private IConnectedAction[] _connectedActions;
            private HiglihtAction[] _higlihtActions;
            private DehighlightAction[] _dehighlightActions;

            private void Awake()
            {
                _matchableActions = GetComponentsInChildren<IMatchableAction>();
                  _pathActions = GetComponentsInChildren<IPathAction>();
                _connectedActions = GetComponentsInChildren<IConnectedAction>();
                _higlihtActions = GetComponentsInChildren<HiglihtAction>();
                _dehighlightActions = GetComponentsInChildren<DehighlightAction>();
                  
            }

            public void PerformMatchableAction(GridTile gridTile, GridObjectType prevGridObjectType)
            {
                //Debug.Log("Matchable from " + gridTile.GridPosition);
                foreach (var action in _matchableActions)
                {             
                    //Debug.Log("Matchable " +   action.GetType());
                              action.MatchAction(gridTile, prevGridObjectType);
                }
            }

            public void PerformPathAction(GridTile gridTile)
            {
                foreach (var action in _pathActions)
                {
                    action.PathAction();
                }
            }
            
            public void PerformBackToNormal(GridTile gridTile)
            {
                foreach (var action in _higlihtActions)
                {
                    action.BackToNormal();
                }
            }

            public void PerformConnectedAction(GridTile gridTile)
            {
                if (_connectedActions == null) return;
 
                foreach (var action in _connectedActions)
                {
                    //Debug.Log("Connected action in " + gridTile.GridPosition);
                    action?.ConnectedAction();
                }
            }

            public void PerformHighlightAction(GridTile gridTile)
            {
                if (_higlihtActions == null) return;

                foreach (var action in _higlihtActions)
                {
                    action?.HighlightAction();
                }
            }

            public void PerformDehighlightAction(GridTile gridTile)
            {
                foreach (var action in _dehighlightActions)
                {
                    action.DehighlightAction();
                }
            }
        }
    }
}
