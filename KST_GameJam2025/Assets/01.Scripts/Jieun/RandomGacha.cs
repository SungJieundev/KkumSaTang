using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Random = UnityEngine.Random;

public class RandomGacha : MonoBehaviour
{
    private int _coinGachaCost = 0;
    private int _specialGacha1Cost = 0;
    private int _specialGacha2Cost = 0;
    
    public List<string> coinGachaList = new List<string>();
    public List<string> specialGacha1List = new List<string>();
    public List<string> specialGacha2List = new List<string>();
    
    public GameObject coinGachaPanel;
    public TMP_Text coinGachaPriceText;
    
    [Header("스폰 지점 부모 - 빈 오브젝트")]
    [SerializeField] private Transform spawnPointParent;   // “SpawnPoint” 오브젝트 drag

    [Header("히어로 프리팹")]
    [SerializeField] private GameObject heroPrefab;        //  Hero 프리팹 drag

    private List<Transform> _allSlots   = new List<Transform>();   // 16칸 전부
    private System.Random  _rng         = new System.Random();     // C# 난수

    private void Awake()
    {
        // 코인 룰렛 요소 추가
        coinGachaList.Add("Low_WhiteBread");
        coinGachaList.Add("Low_Baguette");
        coinGachaList.Add("Low_RollBread");
        coinGachaList.Add("Low_Croissant");
        
        //스페셜가챠 1 요소 추가
        specialGacha1List.Add("Boom");
        specialGacha1List.Add("return");
        specialGacha1List.Add("3");
        specialGacha1List.Add("4");
        
        //스페셜가챠 2 요소 추가
        specialGacha2List.Add("5");
        specialGacha2List.Add("6");
        specialGacha2List.Add("7");
        specialGacha2List.Add("8");
        
        // SpawnPoint 아래 자식(A1~D4) 전부 수집
        foreach (Transform child in spawnPointParent)
            _allSlots.Add(child);
    }
    

    /// <summary>
    /// 비어 있는 칸 중 하나를 골라 Hero를 배치한다
    /// </summary>
    public void SpawnHero()
    {
        // 1) 아직 비어 있는 슬롯만 필터
        List<Transform> emptySlots = _allSlots.FindAll(slot => slot.childCount == 0);

        if (emptySlots.Count == 0)
        {
            Debug.LogWarning("빈 슬롯이 없습니다!");   // 다 차면 경고만
            return;
        }

        // 2) 랜덤으로 하나 선택
        int pick = _rng.Next(emptySlots.Count);
        Transform targetSlot = emptySlots[pick];

        // 3) 프리팹 인스턴스 → 슬롯의 자식으로
        GameObject hero = Instantiate(heroPrefab, targetSlot.position, Quaternion.identity, targetSlot);
        hero.name = $"Hero_{targetSlot.name}";   // 디버깅 편하게 이름 붙이기
    }

    
    

    

    public void CoinGachaButtonClick()
    {
        //영웅 랜덤 뽑기
        string gachaResult = RandomGachaSystem(coinGachaList);
        //Debug.Log(RandomGachaSystem(coinGachaList));
        
        //잠시 버튼 위에 보였다 사라지기
        PopUpPricePanel(coinGachaPanel, coinGachaPriceText, gachaResult);
        
        //뽑힌 영웅 랜덤한 위치에 생성
        
    }

    
    
    public void PopUpPricePanel(GameObject panel,TMP_Text tmp, string text)
    {
        tmp.text = text;
        panel.SetActive(true);

        Sequence seq = DOTween.Sequence();

        seq.Append(panel.transform.DOScale(new Vector3(0.6f, 0.6f), 0.4f))
            //.Join(tmp.transform.DOScale(new Vector3(1.05f, 1.05f), 0.4f))
            .Append(panel.transform.DOScale(new Vector3(0.5f, 0.5f), 0.2f))
            //.Join(tmp.transform.DOScale(new Vector3(1f, 1f), 0.2f));
            .AppendCallback(() => panel.SetActive(false));
    }

    public void HeroLoad()
    {
        
    }

    public void HeroSpawn(string heroName)
    {
        
        GameObject heroPrefab = Resources.Load<GameObject>(heroName);
    }
    
    public void SpecialGacha1ButtonClick()
    {
        Debug.Log(RandomGachaSystem(specialGacha1List));
    }
    public void SpecialGacha2ButtonClick()
    {
        Debug.Log(RandomGachaSystem(specialGacha2List));
    }
    
    //public void 

    private string RandomGachaSystem(List<string> targetList)
    {
        string result = "";
        
        Debug.Log($"list.count = {targetList.Count}");
        
        result = targetList[Random.Range(0, targetList.Count)];
        
        return result;
    }
}
