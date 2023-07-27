using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput; // Referencia al componente PlayerInput para capturar la entrada del jugador.
    [SerializeField] private Animator playerAnimator; //Referencia al animator
    [SerializeField] private Transform gunSpawnPoint; //Referencia del cañón del arma
    public GameObject bulletPrefab; //Bullet que será disparado
    public float shootForce; //Fuerza a la que saldrá disparada la bala
    public float timeBetweenShots = 0.2f; // Tiempo entre disparos
    private float shotTimer = 0f; // Temporizador para controlar el tiempo entre disparos.
    private Camera _camera; // Referencia a la cámara principal.
    private bool canShoot = true; // Booleano para determinar si se puede disparar.

    [SerializeField] private WeaponSway _weaponSway;
    [SerializeField] private float swaySensitivity;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private float currentAmmo;
    [SerializeField] private float maxAmmo;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool isReloading;
    [SerializeField] private float reserveAmmo;
    [SerializeField] private TextMeshProUGUI currentAmmoText;
    [SerializeField] private TextMeshProUGUI reservAmmoText;
    [SerializeField] private Look _look;
    [SerializeField] private float lookX;
    [SerializeField] private float lookY;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private string weaponType;

    

     


    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main; // Obtener la cámara principal de la escena.
        _playerInput = GetComponentInParent<PlayerInput>();
        _weaponSway = GetComponentInParent<WeaponSway>();
        swaySensitivity = _weaponSway.swaySensitivity;
        currentAmmoText.text = currentAmmo.ToString();
        reservAmmoText.text = reserveAmmo.ToString();
        _look = GetComponentInParent<Look>();
        lookX = _look.mouseSensibilityX;
        lookY = _look.mouseSensibilityY;

    }

    // Update is called once per frame
    void Update()
    {
        if (playerAnimator.GetBool("isAiming"))
        {
            playerAnimator.SetBool("isAiming", false);
            _weaponSway.swaySensitivity = swaySensitivity;
            _look.mouseSensibilityX = lookX;
            _look.mouseSensibilityY = lookY;
            crossHair.SetActive(true);

        }
        
        if (playerAnimator.GetBool("isShooting"))
        {
            playerAnimator.SetBool("isShooting", false);
        }

        if (reserveAmmo <= 0 && currentAmmo <= 0)
        {
            Debug.Log("No hay balas");
            return;
        }
        
        if (currentAmmo <= 0 && !isReloading)
        {
            Debug.Log("No tienes balas");
            StartCoroutine(Reload(reloadTime));
            return;
        }

        if (isReloading)
        {
            return;
        }

        if (_playerInput.actions["ReloadAmmo"].WasPressedThisFrame() && reserveAmmo > 0)
        {
            Debug.Log("Recarga manual de balas");
            StartCoroutine(Reload(reloadTime));
            return;
        }

        // Actualizar el temporizador entre disparos.
        shotTimer += Time.deltaTime;
        
        //Llamar a la funcion de Shoot
        if (_playerInput.actions["Shoot"].IsPressed() && canShoot && isAutomatic && shotTimer >= timeBetweenShots) Shoot();
        
        //Llamar a la funcion de Shoot
        if (_playerInput.actions["Shoot"].WasPressedThisFrame() && canShoot && !isAutomatic && shotTimer >= timeBetweenShots) Shoot();
        
        
       

        if (_playerInput.actions["Aim"].IsPressed())
        {
            Aim();
        }

    }
    void OnEnable()
    {
        playerAnimator.SetTrigger(weaponType);
    }

    // Función para realizar el disparo.
    private void Shoot()
    {
        currentAmmo--;
        currentAmmoText.text = currentAmmo.ToString();

        playerAnimator.SetBool("isShooting", true);
        shotTimer = 0f; // Reiniciar el temporizador.
        canShoot = false; // Desactivar la posibilidad de disparar hasta que pase el tiempo entre disparos.

        float cameraX = Screen.width / 2;
        float cameraY = Screen.height / 2;
        
        var ray = _camera.ScreenPointToRay(new Vector3(cameraX, cameraY, 0));
        GameObject b = Instantiate(bulletPrefab, gunSpawnPoint.position, Quaternion.identity);
        b.GetComponent<Rigidbody>().AddForce(ray.direction * shootForce, ForceMode.Impulse);

        // Invocar una función para habilitar el disparo nuevamente después del tiempo entre disparos.
        Invoke("EnableShoot", timeBetweenShots);
    }

    // Función para habilitar el disparo nuevamente.
    private void EnableShoot()
    {
        canShoot = true;
    }

    private void Aim()
    {
        playerAnimator.SetBool("isAiming", true);
        _weaponSway.swaySensitivity = swaySensitivity / 4;
        _look.mouseSensibilityX = lookX / 3f;
        _look.mouseSensibilityY = lookY/ 3f;
        crossHair.SetActive(false);
    }

    public IEnumerator Reload(float rt)
    {
        isReloading = true;
        playerAnimator.SetBool("isReloading", true);
        crossHair.SetActive(false);

        yield return new WaitForSeconds(rt);
        playerAnimator.SetBool("isReloading", false);
        crossHair.SetActive(true);


        float missingAmmo = maxAmmo - currentAmmo;
        if (reserveAmmo >= missingAmmo)
        {
            currentAmmo += missingAmmo;
            reserveAmmo -= missingAmmo;
        }
        else
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        }
        
        currentAmmoText.text = currentAmmo.ToString();
        reservAmmoText.text = reserveAmmo.ToString();
        
        isReloading = false;
    }
}
