using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RandomGacha : MonoBehaviour
{
    private int _coinGachaCost = 0;
    private int _highGachaCost = 0;
    private int _specialGachaCost = 0;
    
    public List<string> lowGachaList = new List<string>(); 
    public List<string> highGachaList = new List<string>();
    public List<string> specialGachaList = new List<string>();
    
    public GameObject lowGachaPanel;
    public TMP_Text lowGachaPriceText;
    
    public GameObject highGachaPanel;
    public TMP_Text highGachaPriceText;
    
    public GameObject specialGachaPanel;
    public TMP_Text specialGachaPriceText;
    
    [Header("스폰 지점 부모 - 빈 오브젝트")]
    [SerializeField] private Transform spawnPointParent;   // “SpawnPoint” 오브젝트 drag

    // [Header("히어로 프리팹")]
    // [SerializeField] private GameObject heroPrefab;        //  Hero 프리팹 drag

    private List<Transform> _allSlots   = new List<Transform>();   // 16칸 전부
    private System.Random  _rng         = new System.Random();     // C# 난수

    public static RandomGacha Instance;

    public UnityEvent onBangEvent; 

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        
        // 하위등급 요소추가
        lowGachaList.Add("Low_WhiteBread");
        lowGachaList.Add("Low_Baguette");
        lowGachaList.Add("Low_RollBread");
        lowGachaList.Add("Low_Croissant");
        
        // 상위등급 요소추가
        highGachaList.Add("High_Chestnut Loaf");
        highGachaList.Add("High_Choco_Shell_Bread");
        highGachaList.Add("High_Custard_Cream_Bun");
        highGachaList.Add("High_Melon Pan");
        highGachaList.Add("High_Strawberry_Muffin Variant");
        highGachaList.Add("High_Sweet_Red_Bean_Bun");
        
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        
        
        //최상위 등급 요소 추가
        specialGachaList.Add("5");
        specialGachaList.Add("6");
        specialGachaList.Add("7");
        specialGachaList.Add("8");
        
        // SpawnPoint 아래 자식(A1~D4) 전부 수집
        foreach (Transform child in spawnPointParent)
            _allSlots.Add(child);
    }
    

    /// <summary>
    /// 비어 있는 칸 중 하나를 골라 Hero를 배치한다
    /// </summary>
    public void SpawnHero(string heroName)
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
        //GameObject hero = Instantiate(heroPrefab, targetSlot.position, Quaternion.identity, targetSlot);
        PoolManager.Instance.HeroSpawn(heroName, targetSlot.transform);
    }
    
    public void SpawnHero(GameObject hero)
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
        //GameObject hero = Instantiate(heroPrefab, targetSlot.position, Quaternion.identity, targetSlot);
        PoolManager.Instance.HeroSpawn(hero, targetSlot.transform);
    }

    
    private string RandomGachaSystem(List<string> targetList)
    {
        string result = "";
        
        //Debug.Log($"list.count = {targetList.Count}");
        
        result = targetList[Random.Range(0, targetList.Count)];
        
        return result;
    }

    

    public void LowGachaButtonClick()
    {
        //영웅 랜덤 뽑기
        string gachaResult = RandomGachaSystem(lowGachaList);
        //Debug.Log(RandomGachaSystem(coinGachaList));

        if (IsBang(gachaResult) == false)
        {
            //잠시 버튼 위에 보였다 사라지기
            PopUpPricePanel(lowGachaPanel, lowGachaPriceText, gachaResult);
        
            //뽑힌 영웅 랜덤한 위치에 생성
            SpawnHero(gachaResult);
        }
        else
        {
            // 꽝이벤트 발행
            onBangEvent.Invoke();
        }
    }
    
    public void HighGachaButtonClick()
    {
        Debug.Log(RandomGachaSystem(highGachaList));
    }
    public void SpecialGacha2ButtonClick()
    {
        Debug.Log(RandomGachaSystem(specialGachaList));
    }
    
    

    public bool IsBang(string result)
    {
        if (result == "Bang")
        {
            return true;
        }
        else
        {
            return false;
        }
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
    
    
    
    //public void 

    
}
