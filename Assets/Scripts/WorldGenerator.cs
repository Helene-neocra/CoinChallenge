using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] environmentPrefabs;
    public GameObject[] smallEnvironmentPrefabs;
    public GameObject[] upEnvironmentPrefabs;
    public GameObject[] WinPrefabs;

    [Header("World Settings")]
    public Transform worldRoot;

    [Header("Object Counts")]
    public int environmentCount = 50;
    public int smallEnvironmentCount = 100;
    public int upEnvironmentCount = 50;
    private int WinCount = 1;

    [Header("Height Offsets")]
    public float environmentHeightOffset;
    public float smallEnvironmentHeightOffset;
    public float upEnvironmentHeightOffset;
    public float WinHeightOffset;

    void Awake()
    {
       var myFloorGenerator = FindObjectOfType<FloorGenerator>();
       myFloorGenerator.OnFloorGenerated += GenerateWorld;
       
       if (!worldRoot)
       {
           GameObject rootObj = GameObject.Find("WorldRoot") ?? new GameObject("WorldRoot");
           worldRoot = rootObj.transform;
       }
    }

    public void GenerateWorld(float minX, float minZ, float maxX, float maxZ)
    {
        ClearWorld();

        PlaceObjects(environmentPrefabs, environmentCount, "Environment", environmentHeightOffset, minX, maxX, minZ, maxZ);
        PlaceObjects(smallEnvironmentPrefabs, smallEnvironmentCount, "SmallEnvironment", smallEnvironmentHeightOffset,  minX, maxX, minZ, maxZ);
        PlaceObjects(upEnvironmentPrefabs, upEnvironmentCount, "UpEnvironment", upEnvironmentHeightOffset,  minX, maxX, minZ, maxZ);
        PlaceObjects(WinPrefabs, WinCount, "Win", WinHeightOffset, minX, maxX, minZ, maxZ);
    }

    void ClearWorld()
    {
        Transform envParent = worldRoot.Find("Environment");
        Transform smallEnvParent = worldRoot.Find("SmallEnvironment");
        Transform upEnvParent = worldRoot.Find("UpEnvironment");
        Transform winParent = worldRoot.Find("Win");

        if (envParent) DestroyImmediate(envParent.gameObject);
        if (smallEnvParent) DestroyImmediate(smallEnvParent.gameObject);
        if (upEnvParent) DestroyImmediate(upEnvParent.gameObject);
        if(winParent) DestroyImmediate(winParent.gameObject);

    } 

    void PlaceObjects(GameObject[] prefabs, int count, string parentName, float heightOffset, float minX, float maxX,
        float minZ, float maxZ)
    {
        if (prefabs.Length == 0) return;

        GameObject parent = new GameObject(parentName);
        parent.transform.SetParent(worldRoot);

        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            Vector3 rayStart = new Vector3(x, 50f, z);

            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f))
            {
                Vector3 spawnPos = hit.point + Vector3.up * heightOffset;
                GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
                Instantiate(prefab, spawnPos, rot, parent.transform);
            }
        }
        Debug.Log($"{parentName}: {count} objets générés");
    }
}
