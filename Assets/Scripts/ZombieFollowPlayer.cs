using UnityEngine;
using UnityEngine.UI;

public class ZombieFollowPlayer : MonoBehaviour
{
    [Header("Configuración de Seguimiento")]
    [Tooltip("Velocidad de movimiento del zombie")]
    public float velocidad = 3f;
    
    [Tooltip("Distancia mínima al jugador antes de detenerse")]
    public float distanciaMinima = 2f;
    
    [Tooltip("Distancia a la que puede atacar")]
    public float distanciaAtaque = 1.5f;
    
    [Tooltip("Rotación suave (mayor = más lento)")]
    public float suavizadoRotacion = 5f;
    
    [Header("Configuración de Ataque")]
    [Tooltip("Daño por ataque (0.1 en la escala de la barra)")]
    public float damage = 10f;
    
    [Tooltip("Tiempo entre ataques en segundos")]
    public float attackCooldown = 1.5f;
    
    [Header("Referencias UI")]
    [Tooltip("Referencia a la barra de vida del jugador")]
    public Image healthBar;
    
    private Transform jugador;
    private Animator animator;
    private PlayerHealth playerHealth;
    private float lastAttackTime = 0f;
    private bool canAttack = true;
    
    private void Start()
    {
        // Buscar al jugador por su tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        
        if (playerObj != null)
        {
            jugador = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador. Asegúrate de que tenga el tag 'Player'");
        }
        
        // Intentar obtener el Animator si existe
        animator = GetComponent<Animator>();
        
        // Buscar la barra de vida si no está asignada
        if (healthBar == null)
        {
            GameObject healthBarObj = GameObject.FindGameObjectWithTag("HealthBar");
            if (healthBarObj != null)
            {
                healthBar = healthBarObj.GetComponent<Image>();
            }
        }
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
                animator.SetBool("Attacking", false);
            }
        }
        else
        {
            // Detener la animación de caminar
            if (animator != null)
            {
                animator.SetBool("Walking", false);
            }
            
            // Comprobar si está en rango de ataque
            if (distancia <= distanciaAtaque)
            {
                // Mirar hacia el jugador
                Vector3 direccion = (jugador.position - transform.position).normalized;
                direccion.y = 0;
                if (direccion != Vector3.zero)
                {
                    Quaternion rotacion = Quaternion.LookRotation(direccion);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * suavizadoRotacion);
                }
                
                // Atacar si puede
                if (canAttack)
                {
                    AttackPlayer();
                }
            }
        }
        
        // Actualizar cooldown del ataque
        if (!canAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            canAttack = true;
        }
    }
    
    private void AttackPlayer()
    {
        // Marcar que está atacando
        if (animator != null)
        {
            animator.SetBool("Attacking", true);
        }
        
        // Activar cooldown
        canAttack = false;
        lastAttackTime = Time.time;
        
        // Opción 1: Si el jugador tiene PlayerHealth
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
        // Opción 2: Modificar directamente la barra de vida
        else if (healthBar != null)
        {
            UpdateHealthBar();
        }
    }
    
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        
        // Reducir la escala en 0.1
        Vector3 currentScale = healthBar.rectTransform.localScale;
        currentScale.x = Mathf.Clamp(currentScale.x - 0.1f, 0f, 1f);
        healthBar.rectTransform.localScale = currentScale;
        
        Debug.Log($"Barra de vida actual: {currentScale.x * 100}%");
        
        // Cambiar color según la salud
        if (currentScale.x > 0.5f)
            healthBar.color = Color.green;
        else if (currentScale.x > 0.2f)
            healthBar.color = Color.yellow;
        else
            healthBar.color = Color.red;
    }
    
    // Método para reiniciar el ataque (puede ser llamado desde la animación)
    public void ResetAttack()
    {
        if (animator != null)
        {
            animator.SetBool("Attacking", false);
        }
    }
    
    // Visualizar los rangos en el editor
    private void OnDrawGizmosSelected()
    {
        // Rango de distancia mínima (amarillo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaMinima);
        
        // Rango de ataque (rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}