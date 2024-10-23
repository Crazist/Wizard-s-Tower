using System.Collections.Generic;
using UnityEngine;

namespace DungenGenerator
{
    public class BspService
    {
        public List<RectInt> GenerateRooms(RectInt space, int iterations, int minRoomSize, int maxRoomSize)
        {
            List<RectInt> rooms = new List<RectInt>();
            SplitSpace(space, iterations, rooms, minRoomSize, maxRoomSize);
            return rooms;
        }

        private void SplitSpace(RectInt space, int iterations, List<RectInt> rooms, int minRoomSize, int maxRoomSize)
        {
            if (iterations <= 0 || space.width < minRoomSize || space.height < minRoomSize)
            {
                rooms.Add(space);
                return;
            }

            bool splitHorizontally = Random.value > 0.5f;
            if (space.width > space.height) splitHorizontally = false;
            if (space.height > space.width) splitHorizontally = true;

            if (splitHorizontally)
            {
                int splitY = Random.Range(minRoomSize, space.height - minRoomSize);

                RectInt top = new RectInt(space.x, space.y, space.width, splitY);
                RectInt bottom = new RectInt(space.x, space.y + splitY, space.width, space.height - splitY);

                if (IsRoomValid(top, minRoomSize) && IsRoomValid(bottom, minRoomSize))
                {
                    if (IsRoomWithinMaxSize(top, maxRoomSize)) rooms.Add(top);
                    else SplitSpace(top, iterations - 1, rooms, minRoomSize, maxRoomSize);

                    if (IsRoomWithinMaxSize(bottom, maxRoomSize)) rooms.Add(bottom);
                    else SplitSpace(bottom, iterations - 1, rooms, minRoomSize, maxRoomSize);
                }
                else
                {
                    rooms.Add(space);
                }
            }
            else
            {
                int splitX = Random.Range(minRoomSize, space.width - minRoomSize);

                RectInt left = new RectInt(space.x, space.y, splitX, space.height);
                RectInt right = new RectInt(space.x + splitX, space.y, space.width - splitX, space.height);

                if (IsRoomValid(left, minRoomSize) && IsRoomValid(right, minRoomSize))
                {
                    if (IsRoomWithinMaxSize(left, maxRoomSize)) rooms.Add(left);
                    else SplitSpace(left, iterations - 1, rooms, minRoomSize, maxRoomSize);

                    if (IsRoomWithinMaxSize(right, maxRoomSize)) rooms.Add(right);
                    else SplitSpace(right, iterations - 1, rooms, minRoomSize, maxRoomSize);
                }
                else
                {
                    rooms.Add(space);
                }
            }
        }

        private bool IsRoomValid(RectInt room, int minRoomSize) => 
            room.width >= minRoomSize && room.height >= minRoomSize;

        private bool IsRoomWithinMaxSize(RectInt room, int maxRoomSize) => 
            room.width <= maxRoomSize && room.height <= maxRoomSize;
    }
}
