using UnityEngine;

public class EnnemiGenerator : MonoBehaviour
{
    [Header("Prefabs des ennemis")]
    public GameObject slurpPrefab;
    public GameObject turtlePrefab;

    public Transform targetPoint;

    [Header("Vitesses")]
    public float slurpSpeed = 2.8f;
    public float turtleSpeed = 1.5f;

    [Header("Quantité d'ennemis")]
    public int nbSlurp = 2;
    public int nbTurtle = 2;

    private void Awake()
    {
        var floorGen = FindObjectOfType<FloorGenerator>();
        if (floorGen != null)
        {
            floorGen.OnFloorGenerated += SpawnEnnemis;
        }
    }

    public void SpawnEnnemis(float minX, float minZ, float maxX, float maxZ)
    {
        SpawnType(slurpPrefab, nbSlurp, slurpSpeed, minX, maxX, minZ, maxZ);
        SpawnType(turtlePrefab, nbTurtle, turtleSpeed, minX, maxX, minZ, maxZ);
    }

    void SpawnType(GameObject prefab, int count, float speed, float minX, float maxX, float minZ, float maxZ)
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            Vector3 rayStart = new Vector3(x, 50f, z);
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f))
            {
                Vector3 spawnPos = hit.point;
                var ennemi = Instantiate(prefab, spawnPos, Quaternion.identity);
                var nav = ennemi.GetComponent<AgentNavMesh>();
                if (nav != null)
                {
                    nav.target = targetPoint;
                    nav.agent.speed = speed;
                    nav.enPatrouille = true;
                    nav.centreZoneAttente = spawnPos;
                    nav.rayonZoneAttente = 5f;
                }
            }
        }
    }
}