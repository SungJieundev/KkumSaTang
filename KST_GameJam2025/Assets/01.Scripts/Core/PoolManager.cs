using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    
    public List<GameObject> heroList = new List<GameObject>();
    public Queue<GameObject> bulletQueue = new Queue<GameObject>();
    
    public Transform heroPooler = null;
    public Transform bulletPooler = null;

    public GameObject bulletPrefab;
    public GameObject heroPrefab;
    
    
    
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void HeroDeSpawn(GameObject heroObject)
    {
        heroObject.SetActive(false);
        heroList.Add(heroObject);
        heroObject.transform.SetParent(heroPooler);
    }

    public bool IsInHeroList(string heroName)
    {
        return heroList.Contains(GameObject.Find(heroName));
    }

    public void HeroSpawn(string heroName, Transform spawnPos)
    {
        
    }
    
    public void BulletDeSpawn(Transform pooler, Queue<GameObject> pooling, GameObject bullet)
    {
        bullet.SetActive(false);
        pooling.Enqueue(bullet);
        bullet.transform.SetParent(pooler);
    }

    public void BulletSpawn(Transform spawnPos)
    {
        if (bulletPooler.transform.childCount > 0)
        {
            GameObject bullet = bulletQueue.Dequeue();
            bullet.transform.SetParent(null);
            bullet.transform.position = spawnPos.position;
            
            bullet.SetActive(true);
        }
        else
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
        }
    }
    
    
}
