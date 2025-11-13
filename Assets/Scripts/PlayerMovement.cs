using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; //Velocidad de movimiento del jugador

    CharacterController controller;
    Camera mainCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>(); //Coger el componente CharacterController del objeto al que se asigna este script
        mainCam = Camera.main; //Pillar la camara principal
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); //Eje horizontal (A,D o flechas)
        float v = Input.GetAxis("Vertical"); //Eje vertical (W,S o flechas)

        Vector3 input = new Vector3(h, 0f, v);
        if (input.magnitude > 0.01f)
        {
            // Dirección según la cámara
            Vector3 camForward = mainCam.transform.forward;
            Vector3 camRight   = mainCam.transform.right;
            camForward.y = 0;
            camRight.y   = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 move = camForward * v + camRight * h;
            controller.SimpleMove(move.normalized * speed);

            // Opcional: hacer que el player mire hacia donde se mueve
            transform.rotation = Quaternion.LookRotation(move);
        }
        
    }
}
