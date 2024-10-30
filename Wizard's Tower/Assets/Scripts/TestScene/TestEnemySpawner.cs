using System.Collections.Generic;
using DungenGenerator;
using Enemy;
using Factory;
using Player;
using UnityEngine;
using Zenject;

namespace TestScene
{
    public class TestEnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemyAIBase _enemyPrefab; // Префаб врага
        [SerializeField] private MeshRenderer _groundMesh; // Плоскость для деления
        [SerializeField] private int _gridSizeX = 5; // Количество комнат по X
        [SerializeField] private int _gridSizeZ = 5; // Количество комнат по Z

        private const string PlayerResourcePath = "Player";
        
        private List<Room> _rooms = new List<Room>();
        private DungeonFactory _dungeonFactory;
        private UIFactory _uiFactory;

        [Inject]
        private void Construct(DungeonFactory dungeonFactory, UIFactory uiFactory)
        {
            _uiFactory = uiFactory;
            _dungeonFactory = dungeonFactory;
        }

        private void Start()
        {
            _dungeonFactory.SpawnPlayer(Vector3.zero);
            _uiFactory.CreateUI();
            _uiFactory.CreateJoystick();
            _uiFactory.CreateSkillsWindow();
            _uiFactory.CreateStatsWindow();

            DivideMeshIntoRooms();
            SpawnEnemyInRandomRoom();
        }

        private void DivideMeshIntoRooms()
        {
            if (_groundMesh == null)
            {
                Debug.LogError("Плоскость не задана!");
                return;
            }

            for (int x = 0; x < _gridSizeX; x++)
            {
                for (int z = 0; z < _gridSizeZ; z++)
                {
                    RectInt roomRect = new RectInt(x, z, 1, 1);
                    Room newRoom = new Room(roomRect);

                    for (int i = roomRect.xMin; i <= roomRect.xMax; i++)
                    {
                        for (int j = roomRect.yMin; j <= roomRect.yMax; j++)
                        {
                            newRoom.AddPosition(new Vector2Int(i, j));
                        }
                    }

                    _rooms.Add(newRoom);
                }
            }
        }

        private void SpawnEnemyInRandomRoom()
        {
            if (_enemyPrefab == null || _rooms.Count == 0)
            {
                Debug.LogError("Нет доступных комнат или префаб врага не задан!");
                return;
            }

            // Выбираем случайную комнату
            Room randomRoom = _rooms[Random.Range(0, _rooms.Count)];
            Vector2Int roomCenter = randomRoom.GetCenter();

            // Позиция для спауна в центре комнаты
            Vector3 spawnPosition = new Vector3(roomCenter.x, 0, roomCenter.y);

            // Спавн врага
            SpawnEnemy(randomRoom, _enemyPrefab, spawnPosition, Quaternion.identity);
        }

        private void SpawnEnemy(Room room, EnemyAIBase enemyPrefab, Vector3 position, Quaternion rotation)
        {
            // Создаем врага на заданной позиции и задаем ротацию
            EnemyAIBase enemyAI = Instantiate(enemyPrefab, position, rotation);

            if (enemyAI != null)
            {
                enemyAI.SetRoom(room);
            }
        }
    }
}