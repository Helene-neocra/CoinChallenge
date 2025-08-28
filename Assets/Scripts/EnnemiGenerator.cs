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

    void Start()
    {
        SpawnEnnemis();
    }

    public void SpawnEnnemis()
    {
        // Plage de spawn temporaire (à adapter selon le floor plus tard)
        float minX = 10f, maxX = 30f, minZ = 10f, maxZ = 30f;

        // Slurp
        for (int i = 0; i < nbSlurp; i++)
        {
            Vector3 pos = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
            var slurp = Instantiate(slurpPrefab, pos, Quaternion.identity);
            var slurpNavMesh = slurp.GetComponent<AgentNavMesh>();
            if (slurpNavMesh != null)
            {
                slurpNavMesh.target = targetPoint;
                slurpNavMesh.agent.speed = slurpSpeed;
            }
        }

        // Turtle
        for (int i = 0; i < nbTurtle; i++)
        {
            Vector3 pos = new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ));
            var turtle = Instantiate(turtlePrefab, pos, Quaternion.identity);
            var turtleNavMesh = turtle.GetComponent<AgentNavMesh>();
            if (turtleNavMesh != null)
            {
                turtleNavMesh.target = targetPoint;
                turtleNavMesh.agent.speed = turtleSpeed;
            }
        }
    }
}