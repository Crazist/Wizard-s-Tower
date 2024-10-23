using System.Collections.Generic;
using UnityEngine;

namespace DungenGenerator
{
    public class BspService
    {
        public List<RectInt> GenerateRooms(RectInt space, int iterations, int minRoomSize)
        {
            List<RectInt> rooms = new List<RectInt>();
            SplitSpace(space, iterations, rooms, minRoomSize);
            return rooms;
        }

        private void SplitSpace(RectInt space, int iterations, List<RectInt> rooms, int minRoomSize)
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

                if (top.width >= minRoomSize && top.height >= minRoomSize &&
                    bottom.width >= minRoomSize && bottom.height >= minRoomSize)
                {
                    SplitSpace(top, iterations - 1, rooms, minRoomSize);
                    SplitSpace(bottom, iterations - 1, rooms, minRoomSize);
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

                if (left.width >= minRoomSize && left.height >= minRoomSize &&
                    right.width >= minRoomSize && right.height >= minRoomSize)
                {
                    SplitSpace(left, iterations - 1, rooms, minRoomSize);
                    SplitSpace(right, iterations - 1, rooms, minRoomSize);
                }
                else
                {
                    rooms.Add(space);
                }
            }
        }
    }
}
