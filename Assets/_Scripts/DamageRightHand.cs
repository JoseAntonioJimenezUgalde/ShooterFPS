using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRightHand : MonoBehaviour
{
    public GameObject player; // Referencia al GameObject del jugador.
    [SerializeField] private float damage = 20f; // Cantidad de daño que inflige esta mano.
    [SerializeField] private GameObject parent;



    void Start()
    {
        player = GetComponentInParent<Enemy>().player;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) // Verificar si el objeto con el que colisionó tiene la etiqueta "Player".
        {
            player.GetComponent<PlayerManager>().Hit(damage);
        }
    }
    
}
