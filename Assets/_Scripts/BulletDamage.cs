using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [SerializeField] private float damage = 20; // Daño de la bala
    [SerializeField] private ParticleSystem particle; // Particle de sangre
    [SerializeField] private AudioClip shootAudioClip;
    [SerializeField] private AudioSource weaponAudioSource;

    void Start()
    {
        Destroy(gameObject, 2.5f); //Eliminar el bullet después de ser instanciado esperará 2.5 segundos
        weaponAudioSource = GetComponent<AudioSource>();
        weaponAudioSource.Play();

    }
    
    //Si collisiona la bala con un Enemigo se eliminara la bala, instanciara sangre y le hará daño al enemigo
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Hit(damage);
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
