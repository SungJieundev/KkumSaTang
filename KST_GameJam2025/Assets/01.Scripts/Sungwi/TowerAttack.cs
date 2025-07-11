using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [Header("���� ����")]
    public float attackInterval = 1f;    // 1�ʿ� �� �� ����
    public int attackPower = 2;          // ���ݷ�

    [Header("�Ѿ� ������")]
    public GameObject bulletPrefab;      // �߻��� �Ѿ� ������
    public Transform firePoint;          // �Ѿ� �߻� ��ġ

    private List<GameObject> monstersInRange = new List<GameObject>();  // ���� ���� �� ���� ����Ʈ
    private float attackTimer;

    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval && monstersInRange.Count > 0)
        {
            FaceTarget(monstersInRange[0]);  // Ÿ���� Ÿ���� �ٶ󺸵��� �¿� �ø� ó��
            Attack();
            attackTimer = 0f;
        }
    }

    // Ÿ�� ���� �¿� ���� ó��
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

        // �Ѿ� ����
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // �Ѿ˿� Ÿ�ٰ� ���ݷ� ���� ����
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
