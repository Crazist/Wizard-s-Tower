using System.Collections.Generic;
using UnityEngine;

namespace DungenGenerator
{
    public class RandomWalkService
    {
        public HashSet<Vector2Int> GeneratePathBetweenPoints(Vector2Int start, Vector2Int end)
        {
            HashSet<Vector2Int> pathPositions = new HashSet<Vector2Int>();
            Vector2Int currentPos = start;

            while (currentPos != end)
            {
                if (currentPos.x < end.x)
                {
                    currentPos += Vector2Int.right;
                }
                else if (currentPos.x > end.x)
                {
                    currentPos += Vector2Int.left;
                }
                else if (currentPos.y < end.y)
                {
                    currentPos += Vector2Int.up;
                }
                else if (currentPos.y > end.y)
                {
                    currentPos += Vector2Int.down;
                }

                pathPositions.Add(currentPos);
            }

            return pathPositions;
        }

       public HashSet<Vector2Int> GenerateRoomShape(Vector2Int roomCenter, int roomWalkLength, int walkIterations,
            bool resetEachWalkIteration, RectInt roomBounds, HashSet<Vector2Int> existingRoomPositions)
        { 
            HashSet<Vector2Int> roomShape = new HashSet<Vector2Int>();
            Vector2Int currentPos = roomCenter;

            for (int i = 0; i < walkIterations; i++)
            {
                if (resetEachWalkIteration)
                {
                    currentPos = roomCenter;
                }

                currentPos = PerformRandomWalk(currentPos, roomWalkLength, roomShape, roomBounds, existingRoomPositions);
            }

            return roomShape;
        }

        private Vector2Int PerformRandomWalk(Vector2Int startPos, int walkLength, HashSet<Vector2Int> roomShape,
            RectInt roomBounds, HashSet<Vector2Int> existingRoomPositions)
        {
            Vector2Int currentPos = startPos;
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            for (int i = 0; i < walkLength; i++)
            {
                directions = ShuffleDirections(directions);

                Vector2Int nextPos = FindNextValidPosition(currentPos, directions, roomBounds);

                if (nextPos != currentPos)
                {
                    if (!existingRoomPositions.Contains(nextPos))
                    {
                        roomShape.Add(nextPos);
                    }

                    currentPos = nextPos;
                }
            }

            return currentPos;
        }

        private Vector2Int FindNextValidPosition(Vector2Int currentPos, Vector2Int[] directions,RectInt roomBounds)
        {
            foreach (var direction in directions)
            {
                Vector2Int newPos = currentPos + direction;

                if (roomBounds.Contains(newPos))
                {
                    return newPos;
                }
            }

            return currentPos;
        }

        private Vector2Int[] ShuffleDirections(Vector2Int[] directions)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                int randomIndex = Random.Range(i, directions.Length);
                (directions[i], directions[randomIndex]) = (directions[randomIndex], directions[i]);
            }

            return directions;
        }
    }
}