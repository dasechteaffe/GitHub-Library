using UnityEngine;

public class SimpleHorizontalMover : MonoBehaviour
{
    public float speed = 5.0f;          // Geschwindigkeit der Bewegung in Einheiten pro Sekunde
    public float distance = 10.0f;      // Gesamte Bewegungsdistanz

    private float initialX;             // Startposition in X-Richtung

    void Start()
    {
        initialX = transform.position.x;  // Speichere die Startposition
    }

    void Update()
    {
        // Berechne die neue X-Position mit einer PingPong-Funktion für eine gleichmäßige Hin- und Herbewegung
        float newX = initialX + Mathf.PingPong(Time.time * speed, distance) - (distance / 2);

        // Setze die neue Position
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}

