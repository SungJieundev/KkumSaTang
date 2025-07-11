using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private GameObject target;
    private float damage;

    public void SetTarget(GameObject targetObj, float dmg)
    {
        target = targetObj;
        damage = dmg;

        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
        }

        //3�� �� �ڵ� ��ȯ ����
        Invoke("AutoDespawn", 3f);
    }

    void Update()
    {
        if (target == null)
        {
            AutoDespawn(); // Ÿ���� ������� �ٷ� ��ȯ
            return;
        }

        // �̵�
        transform.position += direction * speed * Time.deltaTime;

        // �浹 �Ÿ� ����
        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            MonsterHp hp = target.GetComponent<MonsterHp>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            CancelInvoke("AutoDespawn"); // �̹� ���������Ƿ� �ڵ� ��ȯ ���� ���
            PoolManager.Instance.BulletDeSpawn(gameObject);
        }
    }

    void AutoDespawn()
    {
        PoolManager.Instance.BulletDeSpawn(gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke(); // Ȥ�� ���� �� ���� ��� ó��
    }
}
