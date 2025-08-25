using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public event System.Action<float, float, float, float> OnFloorGenerated;
    public event System.Action<Vector3> OnPlatformGenerated; // Nouvel événement pour la position de la plateforme
    public GameObject[] floorPrefabs;
    public GameObject platformPrefab; // Prefab spécifique pour la plateforme, assignable depuis Unity
    public int worldSize = 2;
    private float _spacing = 4f;

    void Start()
    {
        GameObject prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
        _spacing = prefab.GetComponentInChildren<RefPointFloor>().getDistance() * 2;

        // S'abonner à l'événement pour générer la plateforme après le sol
        OnFloorGenerated += GeneratePlatformAfterFloor;

        GenerateFloor();
    }

    void GenerateFloor()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                Vector3 position = new Vector3(x * _spacing, 0, z * _spacing);
                GameObject prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
                Instantiate(prefab, position, Quaternion.identity);
            }
        }

        var minX = -_spacing / 2;
        var minZ = -_spacing / 2;
        var maxX = worldSize * _spacing - _spacing / 2;
        var maxZ = worldSize * _spacing - _spacing / 2;
        OnFloorGenerated?.Invoke(minX, minZ, maxX, maxZ);
    }

    void GeneratePlatformAfterFloor(float minX, float minZ, float maxX, float maxZ)
    {
        // Vérifier qu'un prefab de plateforme est assigné
        if (platformPrefab == null)
        {
            Debug.LogWarning("Aucun prefab de plateforme assigné !");
            return;
        }

        // Placer la plateforme juste à côté du bord nord du terrain
        float centerX = (minX + maxX) / 2f;
        Vector3 platformPosition = new Vector3(centerX, 0, maxZ + _spacing);

        Instantiate(platformPrefab, platformPosition, Quaternion.identity);
        
        // Notifier que la plateforme a été générée avec sa position
        OnPlatformGenerated?.Invoke(platformPosition);
    }
}