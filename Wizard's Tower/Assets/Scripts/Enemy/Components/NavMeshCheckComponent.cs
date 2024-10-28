using Panda;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Components
{
    public class NavMeshCheckComponent : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        [Task]
        public void CheckNavMeshAgent()
        {
            if (_navMeshAgent != null && _navMeshAgent.isActiveAndEnabled)
            {
                Task.current.Succeed();
            }
            else
            {
                Task.current.Fail();
            }
        }
    }
}