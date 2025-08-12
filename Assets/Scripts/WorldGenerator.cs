using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject[] floorPrefabs; // Prefabs de sol
    public GameObject[] envPrefabs;   // Prefabs d'environnement (arbre, rocher, etc.)
    public GameObject[] smallEnvPrefabs; // Petits éléments (fleurs, champignons, etc.)
    public int width = 200;
    public int length = 100;
    public float spacing = 2f;
    [Range(0f, 1f)] public float envDensity = 0.5f; // % de cases avec un élément d'environnement
    [Range(0f, 1f)] public float smallEnvDensity = 0.2f; // Densité des petits éléments
    public Transform worldRoot; // Parent pour tous les objets générés

    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateWorld()
    {
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

                // Place un élément d'environnement aléatoirement
                if (envPrefabs.Length > 0 && Random.value < envDensity)
                {
                    GameObject env = envPrefabs[Random.Range(0, envPrefabs.Length)];
                    // Décale légèrement l'élément pour éviter qu'il soit exactement au centre
                    Vector3 envPos = pos + new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));
                    Instantiate(env, envPos, Quaternion.identity, worldRoot);
                }

                // Place un petit élément (fleur, champignon, etc.) aléatoirement
                if (smallEnvPrefabs.Length > 0 && Random.value < smallEnvDensity)
                {
                    GameObject small = smallEnvPrefabs[Random.Range(0, smallEnvPrefabs.Length)];
                    // Ajout d'un offset Y pour les petits éléments
                    Vector3 smallPos = pos + new Vector3(Random.Range(-0.4f, 0.4f), 0.5f, Random.Range(-0.4f, 0.4f));
                    Instantiate(small, smallPos, Quaternion.identity, worldRoot);
                }
            }
        }
    }
}
