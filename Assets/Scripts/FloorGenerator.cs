using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public GameObject[] floorPrefabs;
    public int worldSize = 1;
    private float spacing = 4f;

    void Start()
    {
        GameObject prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
        spacing = prefab.GetComponentInChildren<RefPointFloor>().getDistance() * 2;
        GenerateFloor();

    }

    void GenerateFloor()
    {
        for (int x = 0; x <= worldSize; x++)
        {
            for (int z = 0; z <= worldSize; z++)
            {

                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                Debug.Log(spacing);
                GameObject prefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
                Instantiate(prefab, position, Quaternion.identity);
            }
        }
    }
}