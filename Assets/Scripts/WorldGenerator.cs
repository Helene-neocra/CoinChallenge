using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] floorPrefabs;
    public GameObject[] environmentPrefabs;
    public GameObject[] smallEnvironmentPrefabs;
    public GameObject[] platformPrefabs;
    
    [Header("World Settings")]
    public int worldSize = 30;
    public float spacing = 2f;
    public Transform worldRoot;
    
    [Header("Object Counts")]
    public int environmentCount = 50;
    public int smallEnvironmentCount = 100;
    public int platformCount = 15;
    
    [Header("Placement Settings")]
    public float minDistanceBetweenObjects = 2f;
    public float raycastHeight = 50f;
    public LayerMask groundLayerMask = -1;
    
    private List<Vector3> placedPositions = new List<Vector3>();
    private List<Transform> generatedPlatforms = new List<Transform>();
    
    void Start()
    {
        GenerateWorld();
    }
    
    public void GenerateWorld()
    {
        ClearWorld();
        
        // 1. Générer le sol
        GenerateFloor();
        
        // 2. Placer les objets avec raycast
        StartCoroutine(PlaceObjectsWithDelay());
    }
    
    void ClearWorld()
    {
        if (worldRoot == null)
        {
            GameObject rootObj = GameObject.Find("WorldRoot");
            if (rootObj == null)
            {
                rootObj = new GameObject("WorldRoot");
            }
            worldRoot = rootObj.transform;
        }
        
        // Supprimer tous les enfants
        for (int i = worldRoot.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(worldRoot.GetChild(i).gameObject);
        }
        
        placedPositions.Clear();
        generatedPlatforms.Clear();
    }
    
    void GenerateFloor()
    {
        if (floorPrefabs.Length == 0) return;
        
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                GameObject floorPrefab = floorPrefabs[Random.Range(0, floorPrefabs.Length)];
                GameObject floor = Instantiate(floorPrefab, position, Quaternion.identity, worldRoot);
                floor.isStatic = true;
            }
        }
        
        Debug.Log($"Sol généré: {worldSize * worldSize} tuiles");
    }
    
    IEnumerator PlaceObjectsWithDelay()
    {
        // Attendre que le sol soit bien en place
        yield return new WaitForSeconds(0.5f);
        
        // Placer les objets d'environnement
        PlaceObjects(environmentPrefabs, environmentCount, "Environment");
        yield return new WaitForSeconds(0.1f);
        
        // Placer les petits objets
        PlaceObjects(smallEnvironmentPrefabs, smallEnvironmentCount, "SmallEnvironment");
        yield return new WaitForSeconds(0.1f);
        
        // Placer les plateformes
        PlaceObjects(platformPrefabs, platformCount, "Platform");
        
        Debug.Log($"Génération terminée! {worldRoot.childCount} objets créés");
    }
    
    void PlaceObjects(GameObject[] prefabs, int count, string objectType)
    {
        if (prefabs.Length == 0)
        {
            Debug.LogWarning($"Aucun prefab assigné pour {objectType}");
            return;
        }
        
        int placed = 0;
        int attempts = 0;
        int maxAttempts = count * 10; // Limite pour éviter les boucles infinies
        
        while (placed < count && attempts < maxAttempts)
        {
            // Position aléatoire dans le monde
            float x = Random.Range(0, worldSize * spacing);
            float z = Random.Range(0, worldSize * spacing);
            Vector3 rayStart = new Vector3(x, raycastHeight, z);
            
            // Raycast vers le bas pour trouver le sol
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, raycastHeight * 2, groundLayerMask))
            {
                // Vérifier la distance avec les autres objets
                if (IsPositionValid(hit.point))
                {
                    // Choisir un prefab aléatoire
                    GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
                    
                    // Rotation aléatoire sur Y
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    
                    // Instancier l'objet
                    GameObject obj = Instantiate(prefab, hit.point, rotation, worldRoot);
                    
                    // Ajouter à la liste des positions
                    placedPositions.Add(hit.point);
                    
                    // Si c'est une plateforme, l'ajouter à la liste
                    if (objectType == "Platform")
                    {
                        generatedPlatforms.Add(obj.transform);
                    }
                    
                    placed++;
                }
            }
            
            attempts++;
        }
        
        Debug.Log($"{objectType}: {placed}/{count} objets placés en {attempts} tentatives");
    }
    
    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 placedPos in placedPositions)
        {
            if (Vector3.Distance(position, placedPos) < minDistanceBetweenObjects)
            {
                return false;
            }
        }
        return true;
    }
    
    public List<Transform> GetGeneratedPlatforms()
    {
        return generatedPlatforms;
    }
    
    // Méthode publique pour régénérer
    [ContextMenu("Regenerate World")]
    public void RegenerateWorld()
    {
        StopAllCoroutines();
        GenerateWorld();
    }
}
