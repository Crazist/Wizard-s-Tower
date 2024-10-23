using DungenGenerator;
using Factory;
using UnityEngine;
using Zenject;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _smoothSpeed = 0.125f; 
    [SerializeField] private Vector3 _offset;  
   
    private DungeonFactory _dungeonFactory;

    [Inject]
    public void Construct(DungeonFactory dungeonFactory) => _dungeonFactory = dungeonFactory;

    private void LateUpdate()
    {
        if(_dungeonFactory.Player == null) return;
        
        Vector3 desiredPosition = _dungeonFactory.Player.transform.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition; 

        transform.LookAt(_dungeonFactory.Player.transform);
    }
}