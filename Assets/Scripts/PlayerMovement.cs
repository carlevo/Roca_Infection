using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = -9.81f;

    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");  // A / D
        float v = -Input.GetAxisRaw("Vertical");   // W / S

        Vector3 move = new Vector3(h, 0f, v);

        if (move.sqrMagnitude > 0.01f)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(move);
        }

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -1f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
