#if AI
using UnityEngine;
using Unity.AI.Navigation;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class JRandomWalk : MonoBehaviour
{
    [SerializeField] private float walkRadius = 10f;
    [SerializeField] private Vector2 idleTimeRange = new Vector2(1f, 3f);

    private NavMeshAgent agent;
    private Vector3 spawnPoint;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
    }

    private void Update()
    {

    }

    private void SetRandomDestination()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? spawnPoint : transform.position, wanderRadius);
    }
}
#endif