using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float rotationSpeed = 90f; // degrés par seconde
    public static int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    public static void SpawnRandomCoins(int count, GameObject coinPrefab, float minX, float maxX, float minZ, float maxZ, Transform parent = null, float offsetY = 1.84f, List<Transform> platformList = null)
    {
        int coinsOnPlatforms = 0;
        int coinsOnGround = count;
        if (platformList != null && platformList.Count > 0)
        {
            coinsOnPlatforms = Mathf.Min(count / 3, platformList.Count);
            coinsOnGround = count - coinsOnPlatforms;
            // Instancie un tiers des pièces sur les plateformes
            var chosenPlatforms = new List<Transform>(platformList);
            for (int i = 0; i < coinsOnPlatforms; i++)
            {
                if (chosenPlatforms.Count == 0) break;
                int idx = Random.Range(0, chosenPlatforms.Count);
                Transform plat = chosenPlatforms[idx];
                chosenPlatforms.RemoveAt(idx);
                // Place le coin exactement au centre de la plateforme, à la hauteur du dessus du collider
                float y = plat.position.y;
                Collider platCol = plat.GetComponentInChildren<Collider>(); // Prend aussi les colliders enfants
                if (platCol != null)
                {
                    y = platCol.bounds.center.y + platCol.bounds.extents.y;
                    // Ajoute la moitié de la hauteur du coin si besoin (ex: 0.5f)
                    y += 0.5f;
                }
                else
                {
                    y += 1f; // fallback si pas de collider
                }
                Vector3 pos = new Vector3(platCol != null ? platCol.bounds.center.x : plat.position.x, y, platCol != null ? platCol.bounds.center.z : plat.position.z);
                Object.Instantiate(coinPrefab, pos, Quaternion.identity, parent);
            }
        }
        // Instancie le reste des pièces au sol
        for (int i = 0; i < coinsOnGround; i++)
        {
            float x = Random.Range(minX, maxX);
            float z = Random.Range(minZ, maxZ);
            Vector3 pos = new Vector3(x, offsetY, z);
            Object.Instantiate(coinPrefab, pos, Quaternion.identity, parent);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            score++;
            // Ici tu peux ajouter un effet, un son, etc.
            gameObject.SetActive(false); // Désactive le coin
        }
    }
}
