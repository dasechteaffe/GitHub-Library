using UnityEngine;

public class SimpleVerticalMover : MonoBehaviour
{
    public float speed = 5.0f;          // Geschwindigkeit der Bewegung in Einheiten pro Sekunde
    public float distance = 10.0f;      // Gesamte Bewegungsdistanz

    private float initialY;             // Startposition in Y-Richtung

    void Start()
    {
        initialY = transform.position.y;  // Speichere die Startposition
    }

    void Update()
    {
        // Berechne die neue Y-Position mit einer PingPong-Funktion für eine gleichmäßige Hin- und Herbewegung
        float newY = initialY + Mathf.PingPong(Time.time * speed, distance) - (distance / 2);

        // Setze die neue Position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}

