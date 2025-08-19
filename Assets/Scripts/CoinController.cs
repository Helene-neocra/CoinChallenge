using UnityEngine;

public class CoinController : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public int coinCount = 50;
    public float coinHeight = 1.5f;
    
    void Awake()
    {
        var myFloorGenerator = FindObjectOfType<FloorGenerator>();
        myFloorGenerator.OnFloorGenerated += SpawnCoins;
        
    }
    
    void SpawnCoins(float minX, float minZ, float maxX, float maxZ)
    {
        ClearCoins(); 
        
        for (int i = 0; i < coinCount; i++)
        {
            // Position aléatoire
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            
            // Raycast pour trouver le sol
            Vector3 rayStart = new Vector3(x, 50f, z);
            
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 100f))
            {
                // Placer le coin au-dessus du sol
                Vector3 coinPosition = hit.point + Vector3.up * coinHeight;
                GameObject coin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
                
                // Ajouter le script de collision directement sur chaque coin
                // coin.AddComponent<CoinTrigger>();
            }
        }
       
        
        Debug.Log($"{coinCount} coins générés");
    }
    
    void ClearCoins()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }
    
  
    
    
}  

// Classe interne pour gérer les collisions