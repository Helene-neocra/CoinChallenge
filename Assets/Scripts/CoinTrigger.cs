using UnityEngine;
using UnityEngine.Audio;

public class CoinTrigger : MonoBehaviour
{
    public int score;
    SimpleAudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<SimpleAudioManager>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreUI.IncrementScore(score);
            audioManager.PlayCoinSound();
            Destroy(gameObject);
        }
    }
}