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

    void Start()
    {
        SpawnEnnemis();
    }

    public void SpawnEnnemis()
    {
        Vector3 slurpPos = new Vector3(15f, 0f, 15f);
        Vector3 turtlePos = new Vector3(20f, 0f, 15f);

        // Instancie Slurp
        var slurp = Instantiate(slurpPrefab, slurpPos, Quaternion.identity);
        var slurpNavMesh = slurp.GetComponent<AgentNavMesh>();
        if (slurpNavMesh != null)
        {
            slurpNavMesh.target = targetPoint;
            slurpNavMesh.agent.speed = slurpSpeed;
        }

        // Instancie Turtle
        var turtle = Instantiate(turtlePrefab, turtlePos, Quaternion.identity);
        var turtleNavMesh = turtle.GetComponent<AgentNavMesh>();
        if (turtleNavMesh != null)
        {
            turtleNavMesh.target = targetPoint;
            turtleNavMesh.agent.speed = turtleSpeed;
        }
    }
}