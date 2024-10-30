using DungenGenerator;
using Factory;
using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothSpeed = 0.125f;
    [SerializeField] private float _inertia = 0.2f; // Параметр инерции
    [SerializeField] private float _rotationSmoothSpeed = 5f; // Скорость плавного поворота камеры

    private DungeonFactory _dungeonFactory;
    private Vector3 _velocity = Vector3.zero; // Вектор для хранения текущей скорости камеры

    [Inject]
    public void Construct(DungeonFactory dungeonFactory) => 
        _dungeonFactory = dungeonFactory;

    private void LateUpdate()
    {
        if (_dungeonFactory.Player == null) return;

        // Плавное следование с инерцией
        Vector3 desiredPosition = _dungeonFactory.Player.transform.position + _offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _inertia);
        transform.position = smoothedPosition;

        // Плавный поворот камеры в сторону игрока
        Quaternion targetRotation = Quaternion.LookRotation(_dungeonFactory.Player.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSmoothSpeed * Time.deltaTime);
    }
}