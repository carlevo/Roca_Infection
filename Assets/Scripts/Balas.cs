using UnityEngine;

public class Balas : MonoBehaviour
{
    public float vel = 50f;
    public float tiempoVida = 3f; // Destruir tras 3 segundos

    void Start()
    {
        // Programamos su muerte automática para que no se acumulen
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        // CORRECCIÓN IMPORTANTE:
        // Translate(Vector3.forward...) mueve el objeto hacia SU propio frente
        // independientemente de hacia donde mire el jugador.
        transform.Translate(Vector3.forward * vel * Time.deltaTime);
    }

    // AÑADIDO: Lógica de impacto
    void OnTriggerEnter(Collider other)
    {
        // Si la bala toca un Zombie (asegúrate que el zombie tenga el tag "Zombie")
        if (other.CompareTag("Zombie"))
        {
            Destroy(other.gameObject); // Destruye al zombie
            Destroy(gameObject);       // Destruye la bala
        }
        // Si choca contra el escenario (ponle tag "Escenario" o "Suelo" a las paredes si quieres)
        else if (!other.CompareTag("Player") && !other.CompareTag("Energia")) 
        {
            Destroy(gameObject);
        }
    }
}