using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject player; // Referencia al GameObject del jugador.
    private NavMeshAgent _navMeshAgent; // Componente NavMeshAgent utilizado para el movimiento.
    [SerializeField] private Animator _animator; // Componente Animator utilizado para animaciones.

    public UnityEvent attack; // Evento UnityEvent invocado cuando el enemigo ataca al jugador.
    public UnityEvent exit; // Evento UnityEvent invocado cuando el enemigo deja de estar en contacto con el jugador.

    [SerializeField] private float health = 100f; // Salud del enemigo.
    [SerializeField] private Slider _sliderHealth; //Slider de la vida 
    [SerializeField] private AudioSource enemyAudioSource;
    [SerializeField] private AudioClip[] growAudioClips;

    
    
    
    void Start()
    {
        _sliderHealth.maxValue = health; // Asignar los parametros del slider al health
        _sliderHealth.value = health;
        enemyAudioSource = GetComponent<AudioSource>(); 
        player = GameObject.FindGameObjectWithTag("Player"); // Encontrar el objeto del jugador mediante la etiqueta "Player".
        _navMeshAgent = GetComponent<NavMeshAgent>(); // Obtener el componente NavMeshAgent del enemigo para el movimiento.
    }

    void Update()
    {
        if (!enemyAudioSource.isPlaying)
        {
            enemyAudioSource.clip = growAudioClips[Random.Range(0, growAudioClips.Length)];
            enemyAudioSource.Play();
        }
        _sliderHealth.transform.LookAt(player.transform); //Funcion para que el slider siempre mire a la cámara
        _navMeshAgent.SetDestination(player.transform.position); // Establecer la posición del objetivo del NavMeshAgent al jugador.
        _animator.SetBool("isRunning", _navMeshAgent.velocity.magnitude > 1); // Configurar la animación de correr en función de la velocidad del NavMeshAgent.
    }
    
    // Función llamada cuando se produce una colisión con otro objeto.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Verificar si el objeto con el que se colisionó tiene la etiqueta "Player".
        {
            _animator.SetTrigger("isAttacking"); // Establecer el disparador de la animación de ataque.
            attack.Invoke(); // Invocar el evento "attack" cuando el enemigo ataca al jugador.
        }
    }

// Función llamada cuando se deja de producir una colisión con otro objeto.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Verificar si el objeto con el que se dejó de colisionar tiene la etiqueta "Player".
        {
            exit.Invoke(); // Invocar el evento "exit" cuando el enemigo deja de estar en contacto con el jugador.
        }
    }

    void OnEnable()
    {
        health = 100f;
        _sliderHealth.maxValue = health; // Asignar los parametros del slider al health
        _sliderHealth.value = health;
    }

    // Función para reducir la salud del enemigo cuando recibe daño.
    public void Hit(float damage)
    {
        health -= damage; // Reducir la salud del enemigo según el daño recibido.
        _sliderHealth.value = health; //Actualizar el slider de vida

        if (health <= 0) // Verificar si la salud del enemigo es menor o igual a cero.
        {
            _animator.SetTrigger("isDead"); // Establecer el disparador de la animación de muerte.

        }
    }
    
}
