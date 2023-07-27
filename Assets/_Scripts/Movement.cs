using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    PlayerInput _playerInput;
    CharacterController _controller;
    [SerializeField] float moveSpeed = 5f; // Velocidad de movimiento del personaje.
    private Vector3 velocity; // Vector de velocidad del personaje en el eje Y (gravedad y salto).
    [SerializeField] private float gravity = -9.81f; // Valor de la gravedad para aplicar en caídas.
    [SerializeField] private bool isGrounded; // Indica si el personaje está en el suelo.
    [SerializeField] private Transform groundCheck; // Transform que define la posición para comprobar si el personaje está en el suelo.
    private float groundDistance = 0.5f; // Distancia máxima para comprobar si el personaje está en el suelo.
    [SerializeField] private LayerMask groundMask; // Máscara de capas para comprobar si el personaje está en el suelo.
    [SerializeField] private float jumpHeight = 2; // Altura del salto del personaje.
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 15f;
    [SerializeField]private float currentSpeed; // Velocidad actual del personaje (cambia entre moveSpeed y runSpeed).
    [SerializeField]private bool isRunning; // Indica si el jugador está corriendo.
    [SerializeField]private float runTimer = 10f; // Duración del tiempo de carrera en segundos.



    void Start()
    {
        _playerInput = GetComponent<PlayerInput>(); // Obtener el componente PlayerInput para leer la entrada del jugador.
        _controller = GetComponent<CharacterController>(); // Obtener el componente CharacterController para mover el personaje.
        currentSpeed = moveSpeed; // Establecer la velocidad inicial al valor de moveSpeed.

    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Comprobar si el personaje está en el suelo mediante un Raycast esférico.

        // Si el personaje está en el suelo, reiniciar la velocidad vertical (eje Y) a un valor negativo pequeño para mantenerlo en el suelo.
        velocity.y = isGrounded && velocity.y < 0 ? -2 : velocity.y;

        // Si el jugador pulsa el botón de salto y está en el suelo, aplicar una fuerza hacia arriba para realizar el salto.
        if (_playerInput.actions["Jump"].WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        Vector2 movement = _playerInput.actions["Movement"].ReadValue<Vector2>(); // Leer la entrada de movimiento (eje X e Y) del jugador.
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y); // Crear un vector de dirección para el movimiento del personaje (ignorando el eje Y para el salto).
        moveDirection = transform.TransformDirection(moveDirection); // Convertir el movimiento relativo del jugador a movimiento global.
        
        // Cambiar la velocidad actual según si el jugador está corriendo o no.
        float speed = isRunning ? runSpeed : moveSpeed;


        // Mover el personaje en la dirección calculada por el vector de movimiento y la velocidad de movimiento, ajustado por el tiempo.
        _controller.Move(moveDirection * speed * Time.deltaTime);

        // Aplicar la gravedad al vector de velocidad (eje Y) para simular la caída y el salto.
        velocity.y += gravity * Time.deltaTime;

        // Mover el personaje verticalmente según el vector de velocidad (gravedad y salto), ajustado por el tiempo.
        _controller.Move(velocity * Time.deltaTime);
        
        

        if (_playerInput.actions["RunSpeed"].WasPressedThisFrame() && isGrounded)
        {
            isRunning = true;
            currentSpeed = runSpeed;

            // Iniciar el temporizador para detener el modo de carrera después del tiempo establecido.
            StartCoroutine(StopRunningAfterDelay(runTimer));
        }
    }
    
    // Corrutina para detener el modo de carrera después del tiempo especificado.
    private System.Collections.IEnumerator StopRunningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isRunning = false;
        currentSpeed = moveSpeed;
    }
}
