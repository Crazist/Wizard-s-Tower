using System.Collections.Generic;
using UnityEngine;

namespace DungenGenerator
{
    public class Room
    {
        public HashSet<Vector2Int> WallPositions { get; set; }
        public HashSet<Vector2Int> RoomPositions { get; private set; }
        public Vector2Int ExitPoints { get;  set; }
        public RectInt RoomRect { get; private set; }
        public Vector2Int EntryPoint { get; set; }

        public Room(RectInt roomRect)
        {
            RoomRect = roomRect;
            RoomPositions = new HashSet<Vector2Int>();
            WallPositions = new HashSet<Vector2Int>();
            EntryPoint = Vector2Int.zero;
            ExitPoints = Vector2Int.zero;
        }

        public void AddPosition(Vector2Int position) => 
            RoomPositions.Add(position);

        public void AddPositions(IEnumerable<Vector2Int> positions)
        {
            foreach (var position in positions)
            {
                RoomPositions.Add(position);
            }
        }

        public Vector2Int GetCenter() => 
            new(Mathf.RoundToInt(RoomRect.center.x), Mathf.RoundToInt(RoomRect.center.y));

        public List<Vector2Int> GetEdges()
        {
            List<Vector2Int> edges = new List<Vector2Int>();

            foreach (var pos in RoomPositions)
            {
                if (IsEdgePosition(pos))
                {
                    edges.Add(pos);
                }
            }

            return edges;
        }

        public float DistanceTo(Vector2Int position)
        {
            Vector2Int roomCenter = GetCenter();
            return Vector2Int.Distance(roomCenter, position);
        }

        private bool IsEdgePosition(Vector2Int pos)
        {
            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            foreach (var dir in directions)
            {
                Vector2Int neighborPos = pos + dir;
                if (!RoomPositions.Contains(neighborPos))
                {
                    return true;
                }
            }

            return false;
        }
    }
}