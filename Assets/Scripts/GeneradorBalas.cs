using UnityEngine;

public class GeneradorBalas : MonoBehaviour
{
    public GameObject prefabBalas;
    public Camera camaraPrincipal; // Referencia a tu cámara para saber dónde es el centro
    public Transform puntoDisparo; // Referencia al objeto vacío en la punta del arma

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerarBalas();
        }
    }

    private void GenerarBalas()
    {
        // 1. Encontrar el centro exacto de la pantalla
        Ray rayoCentro = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        Vector3 puntoObjetivo;

        // 2. Lanzar un rayo invisible para ver qué estamos mirando
        if (Physics.Raycast(rayoCentro, out hit))
        {
            // Si miramos una pared o un zombie, ese es nuestro objetivo
            puntoObjetivo = hit.point;
        }
        else
        {
            // Si miramos al cielo y no chocamos con nada, inventamos un punto muy lejano
            puntoObjetivo = rayoCentro.GetPoint(75); 
        }

        // 3. Calcular la dirección desde el arma hasta ese punto objetivo
        Vector3 direccion = puntoObjetivo - puntoDisparo.position;

        // 4. Calcular la rotación necesaria para mirar ahí
        Quaternion rotacionHaciaElObjetivo = Quaternion.LookRotation(direccion);

        // 5. Instanciar la bala con esa rotación calculada
        Instantiate(prefabBalas, puntoDisparo.position, rotacionHaciaElObjetivo);
    }
}