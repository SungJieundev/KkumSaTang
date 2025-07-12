using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Skill_Straw : MonoBehaviour
{
    [Header("스킬 보정값")]
    public float attackPowerBonus = 5f;
    [Range(0f, 100f)] public float intervalReductionPercent = 50f;  // 0~100%

    private class TowerOriginalStat
    {
        public TowerAttack tower;
        public float originalPower;
        public float originalInterval;
    }

    private List<TowerOriginalStat> modifiedTowers = new List<TowerOriginalStat>();

    void OnEnable()
    {
        GameObject[] heroes = GameObject.FindGameObjectsWithTag("Hero");

        foreach (GameObject hero in heroes)
        {
            TowerAttack tower = hero.GetComponent<TowerAttack>();
            if (tower == null) continue;

            TowerOriginalStat stat = new TowerOriginalStat
            {
                tower = tower,
                originalPower = tower.attackPower,
                originalInterval = tower.attackInterval
            };

            modifiedTowers.Add(stat);

            float newPower = stat.originalPower + attackPowerBonus;

            // 간격 퍼센트 감소 적용
            float newInterval = Mathf.Max(0.1f, stat.originalInterval * (1f - intervalReductionPercent / 100f));

            tower.SetAttackStats(newPower, newInterval);
        }

        Debug.Log("Tower_Skill_Straw 활성화됨: Hero 대상 공격력 증가, 간격 퍼센트 감소 적용");
    }

    void OnDisable()
    {
        foreach (var stat in modifiedTowers)
        {
            if (stat.tower != null)
            {
                stat.tower.SetAttackStats(stat.originalPower, stat.originalInterval);
            }
        }

        modifiedTowers.Clear();

        Debug.Log("Tower_Skill_Straw 비활성화됨: Hero 대상 원래 수치로 복구");
    }
}
