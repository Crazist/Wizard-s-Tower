using System.Collections.Generic;
using Factory;
using UnityEngine;
using Zenject;

namespace DungenGenerator
{
    public class RoomService
    {
        private RandomWalkService _randomWalkService;
        private BspService _bspService;
        private DungeonFactory _dungeonFactory;

        [Inject]
        private void Construct(RandomWalkService randomWalkService, BspService bspService, DungeonFactory dungeonFactory)
        {
            _dungeonFactory = dungeonFactory;
            _randomWalkService = randomWalkService;
            _bspService = bspService;
        }

        public List<Room> GenerateRooms(RectInt dungeonSpace, int iterations, float shrinkPercentage,
            int roomWalkLength, int walkIterations, bool resetEachWalkIteration,
            GameObject parent, int minRoomArea)
        {
            List<RectInt> rooms = _bspService.GenerateRooms(dungeonSpace, iterations, minRoomArea);
            List<Room> roomList = new List<Room>();

            foreach (var roomRect in rooms)
            {
                var baseBounds = ReduceRoomBounds(roomRect, 0.05f);

                RectInt reducedRoomRect = ReduceRoomBounds(roomRect, shrinkPercentage);

                Room room = new Room(reducedRoomRect);

                AddInitialRoomPositions(room, reducedRoomRect);

                HashSet<Vector2Int> roomShape = _randomWalkService.GenerateRoomShape(
                    room.GetCenter(), roomWalkLength, walkIterations, resetEachWalkIteration, baseBounds,
                    room.RoomPositions);

                room.AddPositions(roomShape);
                roomList.Add(room);
            }

            SpawnRoomFloor(roomList, parent);
            return roomList;
        }

        private RectInt ReduceRoomBounds(RectInt roomRect, float shrinkPercentage)
        {
            int newWidth = Mathf.FloorToInt(roomRect.width * (1 - shrinkPercentage));
            int newHeight = Mathf.FloorToInt(roomRect.height * (1 - shrinkPercentage));

            int offsetX = (roomRect.width - newWidth) / 2;
            int offsetY = (roomRect.height - newHeight) / 2;

            return new RectInt(roomRect.xMin + offsetX, roomRect.yMin + offsetY, newWidth, newHeight);
        }

        private void AddInitialRoomPositions(Room room, RectInt roomRect)
        {
            for (int x = roomRect.xMin; x < roomRect.xMax; x++)
            {
                for (int y = roomRect.yMin; y < roomRect.yMax; y++)
                {
                    Vector2Int position = new Vector2Int(x, y);
                    room.AddPosition(position);
                }
            }
        }

        private void SpawnRoomFloor(List<Room> rooms, GameObject parent)
        {
            foreach (var room in rooms)
            {
                foreach (var pos in room.RoomPositions)
                {
                    Vector3 floorPosition = new Vector3(pos.x, 0f, pos.y);
                    _dungeonFactory.SpawnFloor(floorPosition, parent.transform);
                }
            }
        }
    }
}