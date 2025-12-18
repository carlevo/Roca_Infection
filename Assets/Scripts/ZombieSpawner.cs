using UnityEngine;
using UnityEngine.UI; // Necesario para el Texto de la UI
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Configuración Principal")]
    public GameObject zombiePrefab;      // Tu prefab del zombie
    public Transform[] puntosDeSpawn;    // Tus puntos vacíos (SpawnPoints)

    [Header("Configuración de Oleadas")]
    public Text textoOleada;             // Asigna aquí el objeto Text del Canvas
    public int oleadaActual = 1;         // Empieza en la 1
    public int zombiesPorOleada = 5;     // Cantidad inicial
    public float tiempoEntreZombies = 1f; // Velocidad a la que salen dentro de la oleada
    public float tiempoEntreOleadas = 4f; // Tiempo de descanso al terminar una oleada

    private void Start()
    {
        // Verificaciones de seguridad
        if (zombiePrefab == null)
        {
            Debug.LogError("¡Falta el Prefab del Zombie!");
            return;
        }
        if (puntosDeSpawn == null || puntosDeSpawn.Length == 0)
        {
            Debug.LogError("¡No has puesto los Puntos de Spawn!");
            return;
        }

        // Arrancamos el bucle infinito de oleadas
        StartCoroutine(RutinaOleadas());
    }

    private IEnumerator RutinaOleadas()
    {
        // Bucle infinito: Mantiene el juego corriendo oleada tras oleada
        while (true)
        {
            // --- FASE 1: INICIO DE OLEADA ---
            if (textoOleada != null) 
                textoOleada.text = "Oleada " + oleadaActual;
            
            Debug.Log("Iniciando Oleada " + oleadaActual + " con " + zombiesPorOleada + " zombies.");

            // --- FASE 2: SPAWN DE ENEMIGOS ---
            for (int i = 0; i < zombiesPorOleada; i++)
            {
                SpawnZombie();
                // Esperamos un poco entre cada zombie para que no salgan todos pegados
                yield return new WaitForSeconds(tiempoEntreZombies);
            }

            // --- FASE 3: ESPERAR A QUE EL JUGADOR LIMPIE LA SALA ---
            // Esta línea espera hasta que el número de objetos con tag "Zombie" sea 0
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Zombie").Length == 0);

            // --- FASE 4: PREPARAR SIGUIENTE NIVEL ---
            if (textoOleada != null) 
                textoOleada.text = "¡Oleada " + oleadaActual + " Completada!";

            Debug.Log("Oleada terminada. Descansando...");
            
            yield return new WaitForSeconds(tiempoEntreOleadas);

            // Aumentamos dificultad
            oleadaActual++;           // Siguiente nivel
            zombiesPorOleada += 5;    // 5 zombies más que antes
        }
    }

    private void SpawnZombie()
    {
        // Lógica original para elegir posición aleatoria
        int indiceAleatorio = Random.Range(0, puntosDeSpawn.Length);
        Transform puntoSeleccionado = puntosDeSpawn[indiceAleatorio];

        Instantiate(zombiePrefab, puntoSeleccionado.position, puntoSeleccionado.rotation);
    }
}