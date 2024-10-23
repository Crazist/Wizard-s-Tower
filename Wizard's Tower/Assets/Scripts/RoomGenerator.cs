using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private int _walkLength = 20;  // Количество шагов random walk
    [SerializeField] private int _repeats = 5;  // Количество повторений генерации комнат

    private HashSet<Vector2Int> _floorPositions = new HashSet<Vector2Int>();

    private const float FloorHeight = 0f;

    private void Start()
    {
        GenerateRoom();
    }

    private void GenerateRoom()
    {
        for (int i = 0; i < _repeats; i++)
        {
            Vector2Int startPosition = new Vector2Int(0, 0);  // Начальная точка генерации
            _floorPositions.Add(startPosition);

            Vector2Int currentPosition = startPosition;
            for (int j = 0; j < _walkLength; j++)
            {
                currentPosition = RandomWalkStep(currentPosition);
                _floorPositions.Add(currentPosition);
            }
        }

        GenerateFloors();
        GenerateWalls();
    }

    private Vector2Int RandomWalkStep(Vector2Int currentPos)
    {
        Vector2Int newPos = currentPos;
        for (int attempt = 0; attempt < 10; attempt++)
        {
            Vector2Int direction = GetRandomDirection();
            newPos = currentPos + direction;

            if (!_floorPositions.Contains(newPos))
            {
                return newPos;
            }
        }
        return currentPos;
    }

    private Vector2Int GetRandomDirection()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0: return Vector2Int.up;
            case 1: return Vector2Int.down;
            case 2: return Vector2Int.left;
            case 3: return Vector2Int.right;
            default: return Vector2Int.zero;
        }
    }

    private void GenerateFloors()
    {
        foreach (var position in _floorPositions)
        {
            Vector3 floorPosition = new Vector3(position.x, FloorHeight, position.y);
            Instantiate(_floorPrefab, floorPosition, Quaternion.identity);
        }
    }

    private void GenerateWalls()
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (var pos in _floorPositions)
        {
            foreach (var dir in directions)
            {
                Vector2Int neighborPos = pos + dir;

                if (!_floorPositions.Contains(neighborPos))
                {
                    Vector3 wallPosition = new Vector3(pos.x + dir.x * 0.5f, 0, pos.y + dir.y * 0.5f);
                    Quaternion wallRotation = Quaternion.identity;

                    if (dir == Vector2Int.up)
                    {
                        wallRotation = Quaternion.Euler(0, 180, 0);
                    }
                    else if (dir == Vector2Int.down)
                    {
                        wallRotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (dir == Vector2Int.left)
                    {
                        wallRotation = Quaternion.Euler(0, 90, 0);
                    }
                    else if (dir == Vector2Int.right)
                    {
                        wallRotation = Quaternion.Euler(0, -90, 0);
                    }

                    Instantiate(_wallPrefab, wallPosition, wallRotation);
                }
            }
        }
    }
}
