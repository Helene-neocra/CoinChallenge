using UnityEngine;
using UnityEngine.Audio;

public class SimpleAudioManager : MonoBehaviour
{
    public AudioMixer audioMixer; // Assigner GameAudioMixer dans l'inspecteur
    public AudioSource musicSource; // Assigner, groupe Music
    public AudioSource sfxSource;   // Assigner, groupe SFX
    public AudioClip backgroundMusic;
    public AudioClip jumpSound;
    public AudioClip coinSound;

    void Start()
    {
        // Initialisation du volume à 0 dB (volume normal)
        if (audioMixer != null)
            audioMixer.SetFloat("MusicVolume", 0f); // Vérifie le nom du paramètre exposé !

        // Lance la musique d'ambiance en boucle
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlayCoinSound()
    {
        PlaySound(coinSound);
    }

    public void PlayJumpSound()
    {
        PlaySound(jumpSound);
    }

    private void PlaySound(AudioClip sound)
    {
        if (sound != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(sound);
        }
    }
}