using UnityEngine;


public class Balas : MonoBehaviour
{
    public float vel = 50f;
    public float tiempoVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * vel * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {             
            Destroy(other.gameObject);
            Destroy(gameObject);
            
            // Llama al GameManager para añadir puntos
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(100);
            }
        }
        else if (!other.CompareTag("Player")) 
        {
            Destroy(gameObject);
        }
    }
}