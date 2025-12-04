using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bala; // Aquí arrastras el prefab de la bala
    public Transform firePoint;     // Aquí arrastras el objeto FirePoint
    public float fireRate = 0.5f;   // Tiempo entre disparos
    private float nextFire = 0.0f;

    void Update()
    {
        // Detectar clic izquierdo (Fire1 es por defecto clic izq o Ctrl)
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Crear la bala en la posición y rotación del FirePoint
        Instantiate(bala, firePoint.position, firePoint.rotation);
        
        // Opcional: Aquí podrías reproducir un sonido de disparo
    }
}