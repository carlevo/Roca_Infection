using UnityEngine;
using UnityEngine.UI;

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
        
        // Verificar zombies cercanos
        CheckZombieProximity();
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
        
        if (currentHealth <= 0)
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
        Debug.Log("¡Jugador derrotado!");
        Time.timeScale = 0f;
    }
}