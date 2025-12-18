using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Necesario para cambiar escenas

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Referencias")]
    public Image healthBar;
    
    [Header("Daño por Proximidad")]
    public float damageRange = 3f;
    public float damagePerSecond = 20f;
    
    [Header("Configuración de Game Over")]
    public string gameOverScene = "GameOver"; // Nombre de la escena de Game Over
    public float delayBeforeSceneChange = 0.5f; // Tiempo antes de cambiar escena
    
    private bool isDead = false;
    
    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
    
    void Update()
    {
        // Test con teclas
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10f);
            Debug.Log("Vida actual: " + currentHealth);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentHealth = maxHealth;
            UpdateHealthBar();
            Debug.Log("Vida restaurada");
        }
        
        // Solo verificar zombies si no está muerto
        if (!isDead)
        {
            CheckZombieProximity();
        }
    }
    
    void CheckZombieProximity()
    {
        // BUSCAR POR TAG (la forma más segura)
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        
        foreach (GameObject zombie in zombies)
        {
            if (zombie != null)
            {
                float distance = Vector3.Distance(transform.position, zombie.transform.position);
                
                if (distance < damageRange)
                {
                    // ¡Zombie cercano! Aplicar daño
                    TakeDamage(damagePerSecond * Time.deltaTime);
                    return; // Solo necesitamos uno cercano
                }
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        UpdateHealthBar();
        
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float healthPercentage = currentHealth / maxHealth;
            healthBar.rectTransform.localScale = new Vector3(healthPercentage, 1, 1);
            
            if (healthPercentage > 0.5f)
                healthBar.color = Color.green;
            else if (healthPercentage > 0.2f)
                healthBar.color = Color.yellow;
            else
                healthBar.color = Color.red;
        }
    }
    
    private void Die()
    {
        isDead = true;
        Debug.Log("¡Jugador derrotado!");
        
        // Desactivar controles del jugador (si los tiene)
        DisablePlayerControls();
        
        // Llamar a la función de cambio de escena con delay
        Invoke("LoadGameOverScene", delayBeforeSceneChange);
    }
    
   void DisablePlayerControls()
{
    // Desactivar scripts de movimiento
    MonoBehaviour[] components = GetComponents<MonoBehaviour>();
    foreach (MonoBehaviour component in components)
    {
        if (component != this)
        {
            component.enabled = false;
        }
    }
    
    // Si el jugador tiene Rigidbody, hacerlo cinemático
    Rigidbody rb = GetComponent<Rigidbody>();
    if (rb != null)
    {
        // Esta es la manera CORRECTA:
        rb.isKinematic = true;
        // Esto detendrá completamente el movimiento físico
    }
    
    // Opcional: Si tu jugador tiene un CharacterController
    CharacterController controller = GetComponent<CharacterController>();
    if (controller != null)
    {
        controller.enabled = false; // Este SÍ tiene .enabled
    }
}
    
    void LoadGameOverScene()
    {
        // Cambiar a la escena de Game Over
        SceneManager.LoadScene(gameOverScene);
    }
    
    // Opcional: Método para recargar la escena actual
    public void RestartGame()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo esté normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    // Opcional: Método para volver al menú principal
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Asegúrate de tener esta escena
    }
}