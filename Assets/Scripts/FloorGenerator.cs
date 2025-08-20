using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public event System.Action<float, float, float, float> OnFloorGenerated;
    public GameObject[] floorPrefabs;
    public int worldSize = 2;
    private float _spacing = 4f;

    void Start()
    {
        GameObject prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
        _spacing = prefab.GetComponentInChildren<RefPointFloor>().getDistance() * 2;
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
}