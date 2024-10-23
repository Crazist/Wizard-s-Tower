using System.Collections.Generic;
using System.Linq;
using Factory;
using UnityEngine;
using Zenject;

namespace DungenGenerator
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _dungeonParent;
        [SerializeField] private SpawnableItemsHolder _itemsHolder;
        [SerializeField] [Range(0.0f, 1f)] private float _shrinkPercentage = 0.2f;
        [SerializeField] private RectInt _dungeonSpace;
        [SerializeField] private int _iterations = 4;
        [SerializeField] private int _roomWalkLength = 5;
        [SerializeField] private int _walkIterations = 10;
        [SerializeField] private int _minRoomSize = 5;
        [SerializeField] private bool _resetEachWalkIteration = true;

        private RoomService _roomService;
        private CorridorService _corridorService;
        private WallService _wallService;
        private DungeonFactory _dungeonFactory;
        private ItemSpawner _itemSpawner;

        [Inject]
        public void Construct(BspService bspService, RoomService roomService, CorridorService corridorService,
            WallService wallService, DungeonFactory dungeonFactory, ItemSpawner itemSpawner)
        {
            _itemSpawner = itemSpawner;
            _roomService = roomService;
            _corridorService = corridorService;
            _wallService = wallService;
            _dungeonFactory = dungeonFactory;
        }

        private void Start() => GenerateDungeon();

        public void GenerateDungeon()
        {
            ClearDungeon();

            GameObject dungeonContainer = new GameObject("Dungeon");
            dungeonContainer.transform.SetParent(_dungeonParent);

            List<Room> rooms = _roomService.GenerateRooms(_dungeonSpace, _iterations, _shrinkPercentage,
                _roomWalkLength, _walkIterations, _resetEachWalkIteration, dungeonContainer, _minRoomSize);

            Vector2Int initialPosition = rooms[0].GetCenter();
            rooms = rooms.OrderBy(room => room.DistanceTo(initialPosition)).ToList();

            HashSet<Vector2Int> corridorPositions = _corridorService.ConnectAllRoomsInOrder(rooms,
                dungeonContainer);

            _wallService.GenerateWalls(rooms, corridorPositions, dungeonContainer, _itemsHolder);
            
            _itemSpawner.SpawnItemsInRooms(rooms, _dungeonParent, _itemsHolder);

            SpawnPlayer(rooms[0]);
        }

        public void ClearDungeon()
        {
            foreach (Transform child in _dungeonParent)
            {
                DestroyImmediate(child.gameObject);
            }
            
            _itemSpawner.Clear();
        }

        private void SpawnPlayer(Room firstRoom)
        {
            Vector3 spawnPosition = new Vector3(Mathf.RoundToInt(firstRoom.RoomRect.center.x), 0,
                Mathf.RoundToInt(firstRoom.RoomRect.center.y));
            _dungeonFactory.SpawnPlayer(spawnPosition);
        }
    }
}