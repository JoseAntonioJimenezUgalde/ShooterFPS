using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dead : MonoBehaviour
{
    public UnityEvent isDead; // Evento UnityEvent que se invocará cuando el objeto esté muerto.

    [SerializeField] private GameObject parentGameObject; // Referencia al GameObject padre de este script.

    public void ActivateEvent()
    {
        isDead.Invoke(); // Activa el evento "isDead", en el primer frame de la animacion.
    }
    
    public void IsDead()
    {
        GameManager.Instance.enemiesAlive--; // Restar un enemigo del round para la siguiente Horda
        parentGameObject.gameObject.SetActive(false);
        //Destroy(parentGameObject); // Destruye el GameObject padre de este script, es decir, el objeto al que está adjunto este script.
    }
}
