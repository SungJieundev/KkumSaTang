using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    public float attackInterval = 1f;    // 1초에 한 번 공격
    public int attackPower = 2;          // 공격력

    [Header("총알 프리팹")]
    public GameObject bulletPrefab;      // 발사할 총알 프리팹
    public Transform firePoint;          // 총알 발사 위치

    private List<GameObject> monstersInRange = new List<GameObject>();  // 공격 범위 내 몬스터 리스트
    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval && monstersInRange.Count > 0)
        {
            FaceTarget(monstersInRange[0]);  // 타워가 타겟을 바라보도록 좌우 플립 처리
            Attack();
            attackTimer = 0f;
        }
    }

    // 타워 방향 좌우 반전 처리
    void FaceTarget(GameObject target)
    {
        if (target == null) return;

        bool isLeft = target.transform.position.x < transform.position.x;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isLeft ? 1 : -1);
        transform.localScale = scale;
    }

    void Attack()
    {
        GameObject target = monstersInRange[0];
        if (target == null) return;

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 총알에 타겟과 공격력 정보 전달
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(target, attackPower);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster") && !monstersInRange.Contains(other.gameObject))
            monstersInRange.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
            monstersInRange.Remove(other.gameObject);
    }
}
