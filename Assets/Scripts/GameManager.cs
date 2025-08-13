using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject coinPrefab;
    public int coinCount = 50;
    public float minX = 0f;
    public float maxX = 100f;
    public float minZ = 0f;
    public float maxZ = 100f;
    public Transform coinParent;

    private bool gameEnded = false;

    void OnEnable()
    {
        Timer.OnTimeUp += EndGame;
    }

    void OnDisable()
    {
        Timer.OnTimeUp -= EndGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Récupère les bornes du monde depuis WorldGenerator si possible
        WorldGenerator wg = FindObjectOfType<WorldGenerator>();
        if (wg != null)
        {
            minX = 0f;
            maxX = (wg.width - 1) * wg.spacing;
            minZ = 0f;
            maxZ = (wg.length - 1) * wg.spacing;

            // Attendre que la génération soit terminée puis générer les coins
            StartCoroutine(GenerateCoinsAfterWorld(wg));
        }
        else
        {
            // Génère les coins aléatoirement dans le monde à la hauteur 1.84
            CoinController.SpawnRandomCoins(coinCount, coinPrefab, minX, maxX, minZ, maxZ, coinParent);
        }
    }

    IEnumerator GenerateCoinsAfterWorld(WorldGenerator wg)
    {
        yield return new WaitForEndOfFrame(); // Attendre que la génération soit finie
        var platforms = wg.GetGeneratedPlatforms();
        CoinController.SpawnRandomCoins(coinCount, coinPrefab, minX, maxX, minZ, maxZ, coinParent, 1.84f, platforms);
    }

    // Update is called once per frame
    void Update()
    {
        // Vérification si le jeu est terminé
        if (gameEnded)
        {
            // Désactive les contrôles du joueur
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.enabled = false;
            }
        }
    }

    void EndGame()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            Debug.Log($"Temps écoulé ! Score final : {CoinController.score}");

            // Arrête le timer
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.StopTimer();
            }

            // Ici on pourra ajouter le changement de scène plus tard
            ShowGameOverUI();
        }
    }

    void ShowGameOverUI()
    {
        // Pour l'instant, juste un message dans la console
        Debug.Log("Game Over! Press R to restart (not implemented yet)");
        // Plus tard : afficher UI de fin de partie, bouton restart, etc.
    }
}
