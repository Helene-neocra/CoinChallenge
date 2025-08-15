using UnityEngine;

public class CoinController : MonoBehaviour
{
    [Header("Coin Settings")]
    public GameObject coinPrefab;
    public int coinCount = 50;
    public float coinHeight = 1.5f;
    
    [Header("World Bounds")]
    public float minX = 0f;
    public float maxX = 80f;
    public float minZ = 0f;
    public float maxZ = 80f;
    
    public static int score = 0;
    
    void Start()
    {
        SpawnCoins();
    }
    
    void SpawnCoins()
    {
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
                coin.AddComponent<CoinTrigger>();
            }
        }
        
        Debug.Log($"{coinCount} coins générés");
    }
    
    // Méthode statique pour ramasser une coin
    public static void CollectCoin()
    {
        score++;
        Debug.Log($"Coin collectée ! Score: {score}");
    }
    
    // Méthode statique pour obtenir le score
    public static int GetScore()
    {
        return score;
    }
    
    // Réinitialiser le score
    public static void ResetScore()
    {
        score = 0;
    }
    
    // Classe interne pour gérer les collisions
    public class CoinTrigger : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CoinController.CollectCoin();
                Destroy(gameObject);
            }
        }
    }
}
