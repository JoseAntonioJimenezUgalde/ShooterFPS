using UnityEngine;
using TMPro;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{

    public static GameManager instance; // Instancia del GameManager para el patrón Singleton.
    [SerializeField] public int enemiesAlive; // Contador de enemigos vivos.
    [SerializeField] private int round; // Número actual de la ronda.
    [SerializeField] private GameObject[] spawnsPoints; // Puntos de aparición de los enemigos.
    [SerializeField] private GameObject enemyPrefab; // Prefab del enemigo.
    [SerializeField] private TextMeshProUGUI roundText; // Texto para mostrar el número de la ronda actual.
    [SerializeField] private GameObject GameOverPanel; // Panel para mostrar el mensaje de "Game Over".
    [SerializeField] private TextMeshProUGUI roundsSurvivedText; // Texto para mostrar la cantidad de rondas sobrevividas.
    [SerializeField] private GameObject PausePanel; // Panel de pausa.
    [SerializeField] private PlayerInput _playerInput; // Referencia al componente PlayerInput para recibir input del jugador.
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private Pooling _pooling;

    // Property para acceder a la instancia del Singleton.
    public static GameManager Instance
    {
        get
        {
            // Comprobar si la instancia es nula (no se ha creado aún) o si fue destruida.
            if (instance == null)
            {
                // Buscar un GameManager existente en la escena.
                instance = FindObjectOfType<GameManager>();

                // Si no se encontró ningún GameManager, crear uno nuevo.
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }

                // Asegurarse de que el GameManager persista entre escenas.
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    void Update()
    {
        // Verificar si no hay enemigos vivos en la escena.
        if (enemiesAlive == 0)
        {
            round++; // Incrementar el número de ronda.
            roundText.text = "Round = " + round; // Actualizar el texto que muestra el número de la ronda.
            NextWave(round); // Llamar a la función para iniciar la siguiente oleada de enemigos.
        }

        // Verificar si se presionó el botón de pausa.
        if (_playerInput.actions["Pause"].WasPressedThisFrame())
        {
            Pause(); // Llamar a la función para pausar el juego.
        }
    }

    // Función para iniciar la siguiente oleada de enemigos.
    public void NextWave(int round)
    {
        for (int i = 0; i < this.round; i++)
        { 
            // Seleccionar un punto de aparición aleatorio para el enemigo.
            int randomPos = Random.Range(0, spawnsPoints.Length);

            // Instanciar un nuevo enemigo en el punto de aparición seleccionado.
           // Instantiate(enemyPrefab, spawnsPoints[randomPos].transform.position, Quaternion.identity);
           GameObject zombie = _pooling.ReturnZombie();
           zombie.transform.position = spawnsPoints[randomPos].transform.position;

           zombie.GetComponent<Enemy>().enabled = true;
           zombie.GetComponent<NavMeshAgent>().enabled = true;
           zombie.GetComponent<CapsuleCollider>().enabled = true;

            // Incrementar el contador de enemigos vivos.
            enemiesAlive++;
        }
    }

    // Función para mostrar el mensaje de "Game Over".
    public void GameOver()
    {
        GameOverPanel.SetActive(true); // Activar el panel de "Game Over".
        Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor del mouse.
        roundsSurvivedText.text = round.ToString(); // Mostrar la cantidad de rondas sobrevividas en el panel.
        Time.timeScale = 0; // Pausar el tiempo del juego para detener las actualizaciones.
        AudioListener.volume = 0;
    }

    // Función para reiniciar el juego cargando una escena específica.
    public void RestarGame(int scene)
    {
        Time.timeScale = 1; // Restablecer el tiempo del juego a su valor normal.
        SceneManager.LoadScene(scene); // Cargar la escena especificada.
        AudioListener.volume = 1;

    }

    public void MainMenu()
    {
        fadeAnimator.SetTrigger("FadeIn");
        Time.timeScale = 1;
        AudioListener.volume = 1;
        Invoke("LoadTimeMenu",1f);
    }

    public void LoadTimeMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Función para continuar el juego después de pausarlo.
    public void Continue()
    {
        Time.timeScale = 1; // Restablecer el tiempo del juego a su valor normal.
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor del mouse.
        PausePanel.SetActive(false); // Desactivar el panel de pausa.
        AudioListener.volume = 1;

    }

    // Función para pausar el juego.
    public void Pause()
    {
        Time.timeScale = 0; // Pausar el tiempo del juego para detener las actualizaciones.
        PausePanel.SetActive(true); // Activar el panel de pausa.
        Cursor.lockState = CursorLockMode.None; // Desbloquear el cursor del mouse.
        AudioListener.volume = 0;
    }
}
