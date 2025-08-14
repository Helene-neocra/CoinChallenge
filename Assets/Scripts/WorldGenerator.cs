using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] floorPrefabs; // Prefabs de sol
    public GameObject[] envPrefabs;   // Prefabs d'environnement (arbre, rocher, etc.)
    public GameObject[] smallEnvPrefabs; // Petits éléments (fleurs, champignons, etc.)
    public GameObject[] platformPrefabs; // Prefabs de plateformes
    public int width = 200;
    public int length = 100;
    public float spacing = 2f;
    [Range(0f, 1f)] public float envDensity = 0.5f; // % de cases avec un élément d'environnement
    [Range(0f, 1f)] public float smallEnvDensity = 0.2f; // Densité des petits éléments
    [Range(0f, 1f)] public float platformDensity = 0.05f; // Densité des plateformes
    public Transform worldRoot; // Parent pour tous les objets générés

    private List<Transform> generatedPlatforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Transform> GetGeneratedPlatforms()
    {
        return generatedPlatforms;
    }

    public void GenerateWorld()
    {
        generatedPlatforms.Clear();
        // Supprime les anciens objets générés
        if (worldRoot != null)
        {
            for (int i = worldRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(worldRoot.GetChild(i).gameObject);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                // Place un sol à chaque case
                GameObject floor = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
                Vector3 pos = new Vector3(x * spacing, 0, z * spacing);
                var floorObj = Instantiate(floor, pos, Quaternion.identity, worldRoot);

                // Place une plateforme aléatoirement
                if (platformPrefabs.Length > 0 && Random.value < platformDensity)
                {
                    GameObject platform = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
                    // Position de base avec décalage aléatoire
                    Vector3 basePos = pos + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
                    
                    // Raycast vers le bas pour trouver la surface exacte du sol
                    Vector3 rayStart = basePos + Vector3.up * 5f;
                    RaycastHit hit;
                    
                    if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f))
                    {
                        // Place la plateforme à 0.8f au-dessus de la surface détectée (plus accessible)
                        Vector3 platformPos = hit.point + Vector3.up * 0.8f;
                        GameObject platformObj = Instantiate(platform, platformPos, Quaternion.identity, worldRoot);
                        generatedPlatforms.Add(platformObj.transform);
                    }
                    else
                    {
                        // Fallback si le raycast échoue
                        Vector3 platformPos = basePos + Vector3.up * 0.8f;
                        GameObject platformObj = Instantiate(platform, platformPos, Quaternion.identity, worldRoot);
                        generatedPlatforms.Add(platformObj.transform);
                    }
                }

                // Place un élément d'environnement aléatoirement
                if (envPrefabs.Length > 0 && Random.value < envDensity)
                {
                    GameObject env = envPrefabs[Random.Range(0, envPrefabs.Length)];
                    // Position de base avec décalage aléatoire
                    Vector3 basePos = pos + new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));
                    
                    // Raycast vers le bas pour trouver la surface exacte du sol
                    Vector3 rayStart = basePos + Vector3.up * 5f;
                    RaycastHit hit;
                    
                    if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f))
                    {
                        // Place l'élément d'environnement sur la surface détectée
                        Vector3 envPos = hit.point + Vector3.up * 0.05f; // Petit offset pour éviter les conflits
                        GameObject envObj = Instantiate(env, envPos, Quaternion.identity, worldRoot);
                    }
                    else
                    {
                        // Fallback si le raycast échoue
                        Vector3 envPos = basePos + Vector3.up * 0.1f;
                        Instantiate(env, envPos, Quaternion.identity, worldRoot);
                    }
                }

                // Place un petit élément (fleur, champignon, etc.) aléatoirement
                if (smallEnvPrefabs.Length > 0 && Random.value < smallEnvDensity)
                {
                    GameObject small = smallEnvPrefabs[Random.Range(0, smallEnvPrefabs.Length)];
                    // Position de base avec décalage aléatoire
                    Vector3 basePos = pos + new Vector3(Random.Range(-0.4f, 0.4f), 0, Random.Range(-0.4f, 0.4f));
                    
                    // Raycast vers le bas pour trouver la surface exacte du sol
                    Vector3 rayStart = basePos + Vector3.up * 5f; // Commence 5 unités au-dessus
                    RaycastHit hit;
                    
                    if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f))
                    {
                        // Place l'objet sur la surface détectée + un petit offset
                        Vector3 smallPos = hit.point + Vector3.up * 0.1f;
                        GameObject smallObj = Instantiate(small, smallPos, Quaternion.identity, worldRoot);
                    }
                    else
                    {
                        // Fallback si le raycast échoue
                        Vector3 smallPos = basePos + Vector3.up * 0.5f;
                        Instantiate(small, smallPos, Quaternion.identity, worldRoot);
                    }
                }
            }
        }

        // Note: NavMesh baking pour génération procédurale nécessite une configuration manuelle
        // Pour l'instant, on laisse le NavMesh se gérer manuellement dans Unity
        Debug.Log("World generation completed!");
    }
    
    // Méthode pour baker manuellement le NavMesh si besoin
    public void BakeNavMeshManually()
    {
        // Cette méthode peut être appelée manuellement depuis un autre script
        Debug.Log("Use Window > AI > Navigation to bake NavMesh manually");
    }
}
