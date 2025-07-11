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

        //3초 뒤 자동 반환 예약
        Invoke("AutoDespawn", 3f);
    }

    void Update()
    {
        if (target == null)
        {
            AutoDespawn(); // 타겟이 사라지면 바로 반환
            return;
        }

        // 이동
        transform.position += direction * speed * Time.deltaTime;

        // 충돌 거리 판정
        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            MonsterHp hp = target.GetComponent<MonsterHp>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            CancelInvoke("AutoDespawn"); // 이미 명중했으므로 자동 반환 예약 취소
            PoolManager.Instance.BulletDeSpawn(gameObject);
        }
    }

    void AutoDespawn()
    {
        PoolManager.Instance.BulletDeSpawn(gameObject);
    }

    private void OnDisable()
    {
        CancelInvoke(); // 혹시 꺼질 때 예약 취소 처리
    }
}
