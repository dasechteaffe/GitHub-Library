using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    private Button button;

    void Start()
    {
        // AudioSource hinzufügen, falls noch nicht vorhanden
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Button-Komponente abrufen
        button = GetComponent<Button>();

        // Listener für den Klick-Event hinzufügen
        button.onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("Kein Klick-Sound zugewiesen!");
        }
    }
}