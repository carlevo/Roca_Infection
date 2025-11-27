using UnityEngine;

public class ZombieFollowPlayer : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    [Tooltip("Velocidad de movimiento del zombie")]
    public float velocidad = 3f;
    
    [Tooltip("Distancia mínima al jugador antes de detenerse")]
    public float distanciaMinima = 2f;
    
    [Tooltip("Rotación suave (mayor = más lento)")]
    public float suavizadoRotacion = 5f;
    
    private Transform jugador;
    private Animator animator;
    
    private void Start()
    {
        // Buscar al jugador por su tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador. Asegúrate de que tenga el tag 'Player'");
        }
        
        // Intentar obtener el Animator si existe
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        if (jugador == null) return;
        
        // Calcular distancia al jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);
        
        // Si está lejos del jugador, moverse hacia él
        if (distancia > distanciaMinima)
        {
            // Calcular dirección hacia el jugador
            Vector3 direccion = (jugador.position - transform.position).normalized;
            
            // Mover el zombie
            transform.position += direccion * velocidad * Time.deltaTime;
            
            // Rotar hacia el jugador suavemente
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionObjetivo, suavizadoRotacion * Time.deltaTime);
            
            // Activar animación de caminar si existe
            if (animator != null)
            {
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            // Detener animación si está cerca
            if (animator != null)
            {
                animator.SetBool("Walking", false);
            }
        }
    }
    
    // Visualizar el rango en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaMinima);
    }
}