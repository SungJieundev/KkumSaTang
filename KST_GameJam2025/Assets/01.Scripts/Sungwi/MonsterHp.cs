using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHp : MonoBehaviour
{
    public int maxHp = 100;
    public int currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name} 피격: 현재 HP {currentHp}");

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
