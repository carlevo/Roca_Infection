using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; //Declaramos el prefab del zombie
    
   
    public float tiempoEntreSpawns = 10f; //Tiempo entre cada spawneo (momentaneamente)
    
    
    public Transform[] puntosDeSpawn;
    
   
    public int maxZombies = 0; //Maximo de zombies en la escena
    
    private int zombiesActuales = 0; //Zombies actuales en la escena
    
    private void Start()
    {
        // Verificar que tenemos el prefab y los puntos de spawn
        if (zombiePrefab == null)
        {
            Debug.LogError("¡No se ha asignado el prefab del zombie!");
            return;
        }
        
        if (puntosDeSpawn == null || puntosDeSpawn.Length == 0)
        {
            Debug.LogError("¡No se han asignado puntos de spawn!");
            return;
        }
        
        // Iniciar la corrutina de spawn
        StartCoroutine(SpawnZombiesCoroutine());
    }
    
    private IEnumerator SpawnZombiesCoroutine()
    {
        while (true)
        {
            // Esperar el tiempo definido
            yield return new WaitForSeconds(tiempoEntreSpawns);
            
            // Verificar si hay límite de zombies
            if (maxZombies > 0 && zombiesActuales >= maxZombies)
            {
                Debug.Log("Límite de zombies alcanzado");
                continue;
            }
            
            // Generar el zombie
            SpawnZombie();
        }
    }
    
    private void SpawnZombie()
    {
        // Seleccionar un punto de spawn aleatorio
        int indiceAleatorio = Random.Range(0, puntosDeSpawn.Length);
        Transform puntoSeleccionado = puntosDeSpawn[indiceAleatorio];
        
        // Instanciar el zombie en la posición y rotación del punto seleccionado
        GameObject nuevoZombie = Instantiate(
            zombiePrefab, 
            puntoSeleccionado.position, 
            puntoSeleccionado.rotation
        );
        
        zombiesActuales++;
        
        // Opcional: Agregar componente para rastrear cuando el zombie es destruido
        ZombieCounter counter = nuevoZombie.AddComponent<ZombieCounter>();
        counter.spawner = this;
        
        Debug.Log($"Zombie generado en el punto {indiceAleatorio + 1}. Total: {zombiesActuales}");
    }
    
    // Llamar esto cuando un zombie sea destruido
    public void ZombieDestruido()
    {
        zombiesActuales--;
        if (zombiesActuales < 0) zombiesActuales = 0;
    }
    
    // Método opcional para generar un zombie manualmente (útil para testing)
    [ContextMenu("Generar Zombie Ahora")]
    public void GenerarZombieManual()
    {
        SpawnZombie();
    }
}

// Clase auxiliar para rastrear zombies
public class ZombieCounter : MonoBehaviour
{
    public ZombieSpawner spawner;
    
    private void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.ZombieDestruido();
        }
    }
}