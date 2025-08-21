using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface _surface;

    void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
        
        if (_surface != null)
        {
            // Bake au lancement
            _surface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface component not found on " + gameObject.name);
        }
    }

    public void RebuildNavMesh()
    {
        if (_surface != null)
        {
            _surface.BuildNavMesh(); // Recalcule le NavMesh
        }
        else
        {
            Debug.LogError("NavMeshSurface is not initialized");
        }
    }
}