using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts._GameLogic.Logic.Grid
{
    public class PathfindingPriorityQueue
    {
        private class QueueItem
        {
            public Vector2Int Position;
            public readonly int Priority;

            public QueueItem(Vector2Int position, int priority)
            {
                Position = position;
                Priority = priority;
            }
        }

        private readonly List<QueueItem> queue = new List<QueueItem>();

        public int Count => queue.Count;

        public void Enqueue(Vector2Int item, int priority)
        {
            var queueItem = new QueueItem(item, priority);
            int index = queue.BinarySearch(queueItem, Comparer<QueueItem>.Create((x, y) => x.Priority.CompareTo(y.Priority)));

            if (index < 0)
                index = ~index;

            queue.Insert(index, queueItem);
        }

        public Vector2Int Dequeue()
        {
            if (Count == 0) throw new InvalidOperationException("The queue is empty.");
        
            var item = queue[0];
            queue.RemoveAt(0);
            return item.Position;
        }

        public bool Contains(Vector2Int item)
        {
            return queue.Any(queueItem => queueItem.Position.Equals(item));
        }

        public void UpdatePriority(Vector2Int item, int newPriority)
        {
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].Position.Equals(item))
                {
                    queue.RemoveAt(i);
                    Enqueue(item, newPriority);
                    return;
                }
            }

            throw new InvalidOperationException("The item does not exist in the queue.");
        }
    }
}