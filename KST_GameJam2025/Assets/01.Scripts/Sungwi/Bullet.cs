using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private GameObject target;
    private int damage;

    public void SetTarget(GameObject targetObj, int dmg)
    {
        target = targetObj;
        damage = dmg;

        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // x축 위주로 이동
        transform.position += new Vector3(direction.x, direction.y, 0f) * speed * Time.deltaTime;

        // 충돌 판정
        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            MonsterHp hp = target.GetComponent<MonsterHp>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
