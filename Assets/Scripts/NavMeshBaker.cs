using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshBaker : MonoBehaviour
{
    private NavMeshSurface _surface;
    private FloorGenerator _floorGenerator;

    void Start()
    {
        _surface = GetComponent<NavMeshSurface>();
        _floorGenerator = FindObjectOfType<FloorGenerator>();
        
        if (_surface == null)
        {
            Debug.LogError("NavMeshSurface component not found on " + gameObject.name);
            return;
        }

        if (_floorGenerator != null)
        {
            _floorGenerator.OnFloorGenerated += OnFloorReady;
        }
        else
        {
            // Pas de FloorGenerator, bake immédiatement
            _surface.BuildNavMesh();
        }
    }

    private void OnFloorReady(float minX, float minZ, float maxX, float maxZ)
    {
        // Attendre un peu que tous les NavMeshModifier soient prêts
        Invoke(nameof(BuildNavMesh), 0.1f);
    }

    private void BuildNavMesh()
    {
        _surface.BuildNavMesh();
        Debug.Log("NavMesh généré !");
    }

    void OnDestroy()
    {
        if (_floorGenerator != null)
            _floorGenerator.OnFloorGenerated -= OnFloorReady;
    }
}