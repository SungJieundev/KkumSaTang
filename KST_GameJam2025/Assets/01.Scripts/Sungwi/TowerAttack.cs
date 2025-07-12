using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public float attackInterval = 1f;
    public float attackPower = 2f;
    public float attackRange = 2f; //  추가: 공격 범위

    [Header("총알 프리팹")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    private List<GameObject> monstersInRange = new List<GameObject>();
    private float attackTimer;

    private CircleCollider2D rangeCollider; //  추가: 콜라이더 참조

    void Awake()
    {
        rangeCollider = GetComponent<CircleCollider2D>();
        if (rangeCollider != null)
        {
            rangeCollider.radius = attackRange; //  콜라이더 반영
        }
    }

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval && monstersInRange.Count > 0)
        {
            FaceTarget(monstersInRange[0]);
            Attack();
            attackTimer = 0f;
        }
    }

    void FaceTarget(GameObject target)
    {
        if (target == null) return;

        bool isLeft = target.transform.position.x < transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isLeft ? 1 : -1);
        transform.localScale = scale;
    }

    // 기존 버전 (공격력, 속도만)
    public void SetAttackStats(float power, float interval)
    {
        attackPower = power;
        attackInterval = Mathf.Max(0.1f, interval);
    }

    // 확장 버전 (공격력, 속도, 범위까지)
    public void SetAttackStats(float power, float interval, float range)
    {
        attackPower = power;
        attackInterval = Mathf.Max(0.1f, interval);
        attackRange = range;

        if (rangeCollider != null)
        {
            rangeCollider.radius = attackRange;
        }
    }

    void Attack()
    {
        GameObject target = monstersInRange[0];
        if (target == null) return;

        GameObject bullet = PoolManager.Instance.BulletSpawn(firePoint);
        if (bullet == null) return;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target, attackPower);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster") && !monstersInRange.Contains(other.gameObject))
        {
            monstersInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            monstersInRange.Remove(other.gameObject);
        }
    }
}
