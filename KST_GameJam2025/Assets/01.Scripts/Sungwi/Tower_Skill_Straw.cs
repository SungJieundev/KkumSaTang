using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Skill_Straw : MonoBehaviour
{
    [Header("스킬 보정값")]
    public float attackPowerBonus = 5f;
    [Range(0f, 100f)] public float intervalReductionPercent = 50f;
    public float rangeMultiplier = 1.25f; // 공격 범위 증가 배율

    private class TowerOriginalStat
    {
        public TowerAttack tower;
        public float originalPower;
        public float originalInterval;
        public float originalRange;
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
                originalInterval = tower.attackInterval,
                originalRange = tower.attackRange
            };

            modifiedTowers.Add(stat);

            float newPower = stat.originalPower + attackPowerBonus;
            float newInterval = Mathf.Max(0.1f, stat.originalInterval * (1f - intervalReductionPercent / 100f));
            float newRange = stat.originalRange * rangeMultiplier;

            tower.SetAttackStats(newPower, newInterval, newRange);
        }

        Debug.Log("Tower_Skill_Straw 활성화됨: 공격력 증가, 속도 향상, 범위 증가");
    }

    void OnDisable()
    {
        foreach (var stat in modifiedTowers)
        {
            if (stat.tower != null)
            {
                stat.tower.SetAttackStats(stat.originalPower, stat.originalInterval, stat.originalRange);
            }
        }

        modifiedTowers.Clear();

        Debug.Log("Tower_Skill_Straw 비활성화됨: 원래 스탯으로 복원됨");
    }
}
