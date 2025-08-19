using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    public int score;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreUI.IncrementScore(score);
            Destroy(gameObject);
        }
    }
}