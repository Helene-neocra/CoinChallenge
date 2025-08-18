using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public event System.Action<float, float, float, float> OnFloorGenerated;
    public GameObject[] floorPrefabs;
    public int worldSize = 2;
    private float spacing = 4f;

    void Start()
    {
        GameObject prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
        spacing = prefab.GetComponentInChildren<RefPointFloor>().getDistance() * 2;
        GenerateFloor();

    }

    void GenerateFloor()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {

                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                Debug.Log(spacing);
                GameObject prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
                Instantiate(prefab, position, Quaternion.identity);
            }
        }

        var minX = -spacing / 2;
        var minZ = -spacing / 2;
        var maxX = worldSize * spacing - spacing / 2;
        var maxZ = worldSize * spacing - spacing / 2;
        OnFloorGenerated?.Invoke(minX, minZ, maxX, maxZ);
    }
}