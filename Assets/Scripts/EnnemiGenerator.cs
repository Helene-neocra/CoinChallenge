using UnityEngine;

public class EnnemiGenerator : MonoBehaviour
{
    [Header("Prefabs des ennemis")]
    public GameObject slurpPrefab;
    public GameObject turtlePrefab;

    [Header("Point de spawn")]
    public Transform spawnPoint;  // Si vide → utilisera la position de ce GameObject
    public Transform targetPoint;  
    
    public float offsetX = 2f; // Décalage entre Slurp et Turtle

    void Start()
    {
        SpawnEnnemis();
    }

    public void SpawnEnnemis()
    {
        if (slurpPrefab == null || turtlePrefab == null)
        {
            Debug.LogError("⚠️ Assigne Slurp et Turtle dans l’inspector !");
            return;
        }

        // Point de base pour le spawn
        Vector3 basePos = spawnPoint ? spawnPoint.position : transform.position;

        // Instancie Slurp à la position de base
        var slurp = Instantiate(slurpPrefab, basePos, Quaternion.identity);
        // Assigne la cible à Slurp
        if (targetPoint != null)
        {
            var slurpNavMesh = slurp.GetComponent<AgentNavMesh>();
            if (slurpNavMesh != null)
            {
                slurpNavMesh.target = targetPoint;
            }
        }

        // Instancie Turtle à côté (en X)
        var turtle = Instantiate(turtlePrefab, basePos, Quaternion.identity);
        if (targetPoint != null)
        {
            var turtleNavMesh = turtle.GetComponent<AgentNavMesh>();
            if (turtleNavMesh != null)
            {
                Vector3 turtlePos = basePos + new Vector3(offsetX, 0, 0);
                turtleNavMesh.target = targetPoint;
            }
        }
        
        
    }
}