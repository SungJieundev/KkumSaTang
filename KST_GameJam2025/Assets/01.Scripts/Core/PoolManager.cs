using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    
    public List<GameObject> heroList = new List<GameObject>();
    public List<GameObject> monsterList = new List<GameObject>();
    public Queue<GameObject> bulletQueue = new Queue<GameObject>();
    
    public Transform heroPooler = null;
    public Transform bulletPooler = null;
    public Transform monsterPooler = null;

    public GameObject bulletPrefab;
    //public GameObject heroPrefab;
    
    
    
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

    public bool IsInList(string name, List<GameObject> list)
    {
        bool isInlist = list.Contains(GameObject.Find(name));
        return isInlist;
    }

    public void MonsterSpawn(GameObject monsterObj, Transform spawnPos)
    {
        string monsterName = monsterObj.name;
        
        if (IsInList(monsterName, monsterList))
        {
            GameObject targetMonster = monsterList.Find(monster => monster.name == monsterName);
            monsterList.Remove(targetMonster);
            targetMonster.transform.SetParent(spawnPos);
            targetMonster.transform.position = spawnPos.position;
            targetMonster.SetActive(true);
        }
        else
        {
            GameObject targetMonster = Instantiate(Resources.Load<GameObject>(monsterName), spawnPos);
            targetMonster.transform.SetParent(spawnPos);
            targetMonster.transform.position = spawnPos.position;
        }
    }

    public void MonsterDeSpawn(GameObject monsterObj)
    {
        monsterObj.SetActive(false);
        monsterList.Add(monsterObj);
        monsterObj.transform.SetParent(monsterPooler);
    }

    public void HeroSpawn(string heroName, Transform spawnPos)
    {
        if (IsInList(heroName, heroList))
        {
            GameObject targetHero = heroList.Find(hero => hero.name == heroName);
            heroList.Remove(targetHero);
            targetHero.transform.SetParent(spawnPos);
            targetHero.transform.position = spawnPos.position;
            targetHero.SetActive(true);
        }
        else
        {
            GameObject targetHero = Instantiate(Resources.Load<GameObject>(heroName),spawnPos);
            //GameObject targetHero = Resources.Load<GameObject>(heroName);
            
            targetHero.gameObject.name = heroName;
            
            targetHero.transform.SetParent(spawnPos);
            targetHero.transform.rotation = Quaternion.identity;
            targetHero.transform.position = spawnPos.position;
            //targetHero.SetActive(true);
        }
    }
    
    public void HeroSpawn(GameObject heroObj, Transform spawnPos)
    {
        string heroName = heroObj.name;
        
        if (IsInList(heroName, heroList))
        {
            GameObject targetHero = heroList.Find(hero => hero.name == heroName);
            heroList.Remove(targetHero);
            targetHero.transform.SetParent(spawnPos);
            targetHero.transform.position = spawnPos.position;
            targetHero.SetActive(true);
        }
        else
        {
            GameObject targetHero = Instantiate(Resources.Load<GameObject>(heroName), spawnPos);
            targetHero.transform.SetParent(spawnPos);
            targetHero.transform.position = spawnPos.position;
        }
    }

    public void BulletDeSpawn(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.SetParent(bulletPooler);
        bulletQueue.Enqueue(bullet);
    }

    public GameObject BulletSpawn(Transform spawnPos)
    {
        GameObject bullet;

        if (bulletQueue.Count > 0)
        {
            bullet = bulletQueue.Dequeue();
            bullet.transform.SetParent(null);
            bullet.transform.position = spawnPos.position;
            bullet.SetActive(true);
        }
        else
        {
            bullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
        }

        return bullet;
    }

    /*public void BulletSpawn(Transform spawnPos)
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
    }*/


}
