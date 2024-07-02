using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float amplitude = 0.1f; // Maximale Amplitude der Bewegung
    public float frequency = 1f; // Frequenz der Bewegung

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; // Ursprüngliche Position speichern
    }

    void Update()
    {
        // Berechne die neue Position
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}