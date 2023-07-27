using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    public float mouseSensibilityX = 100f; // Sensibilidad del mouse en el eje X para rotación horizontal.
    public float mouseSensibilityY = 100f; // Sensibilidad del mouse en el eje Y para rotación vertical.
    public Transform playerBody; // Referencia al objeto del cuerpo del jugador (usualmente el padre de este script).
    [SerializeField] private PlayerInput playerInput; // Referencia al componente PlayerInput para capturar la entrada del mouse.
    private float xRotation; // Variable para almacenar la rotación vertical (eje X).

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Oculta el cursor del mouse y lo mantiene centrado en la pantalla.
    }

    void Update()
    {
        Vector2 movement = playerInput.actions["Look"].ReadValue<Vector2>(); // Lee la entrada del mouse en los ejes X e Y.

        float sensitivityX = mouseSensibilityX * Time.deltaTime; // Sensibilidad ajustada con el tiempo para suavizar la rotación.
        float sensitivityY = mouseSensibilityY * Time.deltaTime; // Sensibilidad ajustada con el tiempo para suavizar la rotación.

        
        //Rotacion en Y Horizontal
        playerBody.Rotate(Vector3.up * movement.x * sensitivityX); // Rota el objeto del cuerpo del jugador horizontalmente (eje Y) según la entrada del mouse.

        //Rotacion en X Vertical

        xRotation -= movement.y * sensitivityY; // Calcula la rotación vertical (eje X) basada en la entrada del mouse.
        xRotation = Mathf.Clamp(xRotation, -90, 90); // Limita la rotación vertical para que no se gire más allá de 90 grados hacia arriba o hacia abajo.
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0); // Aplica la rotación vertical (eje X) al objeto que contiene este script para mirar hacia arriba y hacia abajo.
    }
}
