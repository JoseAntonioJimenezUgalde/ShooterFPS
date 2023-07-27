
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float health = 100; // Salud actual del jugador.
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Camera _camera;
    [SerializeField] private float shakeTime = 1f; // Tiempo actual del efecto de sacudida de la cámara.
    [SerializeField] private float shakeDuration = 0.5f; // Duración total del efecto de sacudida de la cámara.
    [SerializeField] private Quaternion playerCameraOriginalRotation; // Rotación original de la cámara del jugador.
    [SerializeField] private CanvasGroup hitPanelRed; // Panel rojo para indicar que el jugador ha sido golpeado.
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private int activateWeaponIndex;
    [SerializeField] private GameObject activateWapon;
    [SerializeField] private PlayerInput _playerInput;
    
    
    void Start()
    {
        _camera = Camera.main; // Obtener la referencia de la cámara principal y almacenarla en "_camera".
        playerCameraOriginalRotation = _camera.transform.localRotation; // Guardar la rotación original de la cámara.
        _playerInput = GetComponent<PlayerInput>();
        WeaponSwitch(1);
    }

    void Update()
    {
        if (hitPanelRed.alpha > 0)
        {
            hitPanelRed.alpha -= Time.deltaTime; // Reducir gradualmente la transparencia del panel rojo para crear un efecto de desvanecimiento.
        }

        if (shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            CameraShake(); // Invocar el efecto de sacudida de la cámara.
        }
        else if (_camera.transform.rotation != playerCameraOriginalRotation)
        {
            _camera.transform.localRotation = playerCameraOriginalRotation; // Restablecer la rotación de la cámara a su valor original después de que termine el efecto de sacudida.
        }
        
        


        if (Input.GetAxis("Mouse ScrollWheel") != 0 || _playerInput.actions["ChangeWeapon"].WasPressedThisFrame())
        {
            WeaponSwitch(activateWeaponIndex +1);
        }
    }

    // Función para aplicar daño al jugador.
    public void Hit(float damage)
    {
        health -= damage; // Reducir la salud del jugador según el daño recibido.
        healthText.text = "HP = " + health; // Actualizar el texto de salud para mostrar la salud actual.

        if (health <= 0) // Verificar si la salud del jugador es menor o igual a cero.
        {
            GameManager.Instance.GameOver(); // Llamar al Game Manager utilizando Singleton para invocar la función de Game Over.
        }
        else
        {
            shakeTime = 0; // Restablecer el tiempo del efecto de sacudida para iniciar otra sacudida de la cámara.
            hitPanelRed.alpha = 1f; // Establecer el panel rojo como completamente visible cuando el jugador recibe un golpe.
        }
    }

    // Hacer un movimiento en el eje X de la cámara del jugador para dar la sensación de que está siendo atacado.
    public void CameraShake()
    {
        _camera.transform.localRotation = Quaternion.Euler(Random.Range(-2f, 2f), 0, 0); // Aplicar una rotación aleatoria en el eje X para simular la sacudida de la cámara.
    }

    public void WeaponSwitch(int weaponIndex)
    {
        int index = 0;
        int ammounthOfWeapons = weaponHolder.transform.childCount;

        if (weaponIndex > ammounthOfWeapons -1)
        {
            weaponIndex = 0;
        }

        foreach (Transform child in weaponHolder.transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }

            if (index == weaponIndex)
            {
                child.gameObject.SetActive(true);
                activateWapon = child.gameObject;
            }

            index++;
        }

        activateWeaponIndex = weaponIndex;
    }
}
