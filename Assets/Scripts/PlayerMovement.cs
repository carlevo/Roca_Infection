using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    [Header("Configuración Movimiento")]
    public float speed = 12f;
    public float gravity = -9.81f;

    [Header("Configuración Cámara")]
    public Transform playerCamera; // <--- Arrastra aquí tu Main Camera
    public float mouseSensitivity = 100f;
    
    float xRotation = 0f; // Para guardar la rotación vertical
    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Bloquea el cursor en el centro de la pantalla y lo oculta
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // ---------------------------------------------------------
        // 1. LÓGICA DE LA CÁMARA (RATÓN)
        // ---------------------------------------------------------
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación vertical (Mirar arriba/abajo) - Afecta solo a la cámara
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Límites para no romperse el cuello
        
        // Aplicamos la rotación a la cámara
        if(playerCamera != null)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // Rotación horizontal (Mirar izq/der) - Afecta a todo el cuerpo
        transform.Rotate(Vector3.up * mouseX);


        // ---------------------------------------------------------
        // 2. LÓGICA DE MOVIMIENTO (TECLADO)
        // ---------------------------------------------------------
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // CAMBIO IMPORTANTE:
        // Usamos transform.right y transform.forward para movernos 
        // relativo a hacia donde estamos mirando, no al mundo.
        Vector3 move = transform.right * h + transform.forward * v;

        if (move.sqrMagnitude > 0.01f)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);
        }

        // ---------------------------------------------------------
        // 3. GRAVEDAD
        // ---------------------------------------------------------
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f; // Un valor pequeño para asegurar contacto con el suelo

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}