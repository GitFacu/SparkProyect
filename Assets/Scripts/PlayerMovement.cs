using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float runSpeed = 9f;
    [SerializeField] float mouseSensitivity = 5f;
    [SerializeField] float jumpForce = 1.5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float heaviness = 2.5f;  // Factor para hacer la caída más rápida
    [SerializeField] float lowJumpMultiplier = 2f; // Para saltos más bajos si no se mantiene el botón de salto

    private CharacterController controller;
    private Vector3 velocity;
    private float yaw = 0f;

    private void OnEnable()
    {
        PlayerHealth.OnDead += OnDead;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        ChangeGravity();
    }

    private void OnDisable()
    {
        PlayerHealth.OnDead -= OnDead;
    }

    void Update()
    {
        // --- Rotación con el mouse ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // --- Movimiento con teclas ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.forward * vertical + transform.right * horizontal).normalized;

        // --- Detectar si está corriendo ---
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = moveDirection * currentSpeed;

        // --- Salto y gravedad ---
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f; // Ajuste para mantener al jugador pegado al suelo

            if (Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Aumentar la velocidad de caída si no se está saltando
        if (velocity.y < 0)
            velocity.y += Physics.gravity.y * heaviness * Time.deltaTime;
        // Modificar la velocidad de salto si no se mantiene el botón de salto
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
            velocity.y += Physics.gravity.y * lowJumpMultiplier * Time.deltaTime;
        else
            velocity.y += Physics.gravity.y * Time.deltaTime;

        // --- Movimiento final ---
        controller.Move((move + velocity) * Time.deltaTime);
    }

    private void ChangeGravity()
    {
        Physics.gravity = new Vector3(0, gravity, 0);
    }

    private void OnDead()
    {
        Cursor.lockState = CursorLockMode.None;
        this.enabled = false;
    }


}