#if AI
using UnityEngine;
using Unity.AI.Navigation;

/// <summary>
/// 
/// </summary>
public class JNavigationManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;

    public static NavigationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void RefreshNavMesh()
    {
        navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
    }
}
#endif