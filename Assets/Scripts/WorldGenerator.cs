using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] environmentPrefabs;
    public GameObject[] smallEnvironmentPrefabs;
    public GameObject[] upEnvironmentPrefabs;

    [Header("World Settings")]
    public Transform worldRoot;

    [Header("Object Counts")]
    public int environmentCount = 50;
    public int smallEnvironmentCount = 100;
    public int upEnvironmentCount = 50;

    [Header("Height Offsets")]
    public float environmentHeightOffset;
    public float smallEnvironmentHeightOffset;
    public float upEnvironmentHeightOffset;

    [Header("World Bounds")]
    public float minX = 0f;
    public float maxX = 80f;
    public float minZ = 0f;
    public float maxZ = 80f;

    void Start()
    {
        if (!worldRoot)
        {
            GameObject rootObj = GameObject.Find("WorldRoot") ?? new GameObject("WorldRoot");
            worldRoot = rootObj.transform;
        }

        GenerateWorld();
    }

    public void GenerateWorld()
    {
        ClearWorld();

        PlaceObjects(environmentPrefabs, environmentCount, "Environment", environmentHeightOffset);
        PlaceObjects(smallEnvironmentPrefabs, smallEnvironmentCount, "SmallEnvironment", smallEnvironmentHeightOffset);
        PlaceObjects(upEnvironmentPrefabs, upEnvironmentCount, "UpEnvironment", upEnvironmentHeightOffset);
    }

    void ClearWorld()
    {
        Transform envParent = worldRoot.Find("Environment");
        Transform smallEnvParent = worldRoot.Find("SmallEnvironment");

        if (envParent) DestroyImmediate(envParent.gameObject);
        if (smallEnvParent) DestroyImmediate(smallEnvParent.gameObject);
    }

    void PlaceObjects(GameObject[] prefabs, int count, string parentName, float heightOffset)
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

    [ContextMenu("Regenerate World")]
    public void RegenerateWorld()
    {
        GenerateWorld();
    }
} 