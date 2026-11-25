using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [Header("���� ����")]
    public float attackInterval = 1f;
    public float attackPower = 2f;
    public float attackRange = 2f; // �Ÿ��� ó��

    [Header("�Ѿ� ������")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            GameObject target = FindClosestMonster();
            if (target != null)
            {
                FaceTarget(target);
                Attack(target);
                attackTimer = 0f;
            }
        }
    }

    GameObject FindClosestMonster()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject monster in monsters)
        {
            float distance = Vector2.Distance(transform.position, monster.transform.position);
            if (distance <= attackRange && distance < minDistance)
            {
                closest = monster;
                minDistance = distance;
            }
        }

        return closest;
    }

    void FaceTarget(GameObject target)
    {
        if (target == null) return;

        bool isLeft = target.transform.position.x < transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isLeft ? 1 : -1);
        transform.localScale = scale;
    }

    void Attack(GameObject target)
    {
        GameObject bullet = PoolManager.Instance.BulletSpawn(firePoint);
        AudioManager.Instance.PlaySystem("Gacha");AudioManager.Instance.PlaySystem("LowBullet");
        if (bullet == null) return;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target, attackPower);
        }
    }

    // ���ݷ�, �ӵ�, ���� ���ÿ�
    public void SetAttackStats(float power, float interval, float range)
    {
        attackPower = power;
        attackInterval = Mathf.Max(0.1f, interval);
        attackRange = range;
    }
}
