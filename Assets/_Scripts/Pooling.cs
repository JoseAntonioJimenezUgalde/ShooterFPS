using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    [SerializeField] private GameObject[] zombies;
    [SerializeField] private int numberOfZombies;
    [SerializeField] private List<GameObject> zombieList;

   
    void Start()
    {
        AddZombieToList(numberOfZombies);
    }

    public void AddZombieToList(int zombiess)
    {
        for (int i = 0; i < zombiess; i++)
        {
            GameObject zombieInstantiate = Instantiate(zombies[i]);
            zombieInstantiate.SetActive(false);
            zombieList.Add(zombieInstantiate);
            zombieInstantiate.transform.parent = transform;
        }
    }
    public GameObject ReturnZombie()
    {
        int random = Random.Range(0, zombieList.Count);
        
        if (!zombieList[random].activeSelf)
        {
            zombieList[random].SetActive(true);
            return zombieList[random].gameObject;
        }
        else
        {
            for (int i = 0; i < zombieList.Count; i++)
            {
                if (!zombieList[i].activeSelf)
                {
                    zombieList[i].SetActive(true);
                    return zombieList[i].gameObject;
                }
            }
        }
        
        AddZombieToList(1);
        zombieList[zombieList.Count - 1].SetActive(true);
        return zombieList[zombieList.Count - 1];
        
    }

    IEnumerator QuitOfList()
    {
        yield return new WaitForSeconds(2);
        zombieList.RemoveAt(zombieList.Count - 1);
        
    }
}
