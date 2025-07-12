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
        highGachaList.Add("High_Strawberry_Muffin");
        highGachaList.Add("High_Sweet_Red_Bean_Bun");
        
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        highGachaList.Add("Bang");
        
        
        //최상위 등급 요소 추가
        specialGachaList.Add("Bang");
        specialGachaList.Add("Bang");
        specialGachaList.Add("Bang");
        specialGachaList.Add("Bang");
        
        
        specialGachaList.Add("Low_WhiteBread");
        specialGachaList.Add("Low_Baguette");
        specialGachaList.Add("Low_RollBread");
        specialGachaList.Add("Low_Croissant");
        
        specialGachaList.Add("Low_WhiteBread");
        specialGachaList.Add("Low_Baguette");
        specialGachaList.Add("Low_RollBread");
        specialGachaList.Add("Low_Croissant");
        
        specialGachaList.Add("Low_WhiteBread");
        specialGachaList.Add("Low_Baguette");
        specialGachaList.Add("Low_RollBread");
        specialGachaList.Add("Low_Croissant");
        
        
        
        specialGachaList.Add("High_Chestnut Loaf");
        specialGachaList.Add("High_Choco_Shell_Bread");
        specialGachaList.Add("High_Custard_Cream_Bun");
        specialGachaList.Add("High_Melon Pan");
        specialGachaList.Add("High_Strawberry_Muffin");
        specialGachaList.Add("High_Sweet_Red_Bean_Bun");
        
        specialGachaList.Add("High_Chestnut Loaf");
        specialGachaList.Add("High_Choco_Shell_Bread");
        specialGachaList.Add("High_Custard_Cream_Bun");
        specialGachaList.Add("High_Melon Pan");
        specialGachaList.Add("High_Strawberry_Muffin");
        specialGachaList.Add("High_Sweet_Red_Bean_Bun");
        specialGachaList.Add("High_Chestnut Loaf");
        
        specialGachaList.Add("High_Choco_Shell_Bread");
        specialGachaList.Add("High_Custard_Cream_Bun");
        specialGachaList.Add("High_Melon Pan");
        specialGachaList.Add("High_Strawberry_Muffin");
        specialGachaList.Add("High_Sweet_Red_Bean_Bun");
        
        specialGachaList.Add("High_Chestnut Loaf");
        specialGachaList.Add("High_Choco_Shell_Bread");
        specialGachaList.Add("High_Custard_Cream_Bun");
        specialGachaList.Add("High_Melon Pan");
        specialGachaList.Add("High_Strawberry_Muffin");
        specialGachaList.Add("High_Sweet_Red_Bean_Bun");
        
        
        
        specialGachaList.Add("Special_Strawberry_Cake");
        specialGachaList.Add("Special_Chocolate_Roll_Bread");
        specialGachaList.Add("Special_Melon_Croissant");
        specialGachaList.Add("Special_Red_Bean_Baguette");
        
        
        
        //Special_Strawberry_Cake
        //Special_Chocolate_Roll_Bread
        //Special_Melon_Croissant
        //Special_Red_Bean_Baguette
        
        specialGachaList.Add("");
        
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
        
        //잠시 버튼 위에 보였다 사라지기
        PopUpPricePanel(lowGachaPanel, lowGachaPriceText, heroNameKorean(gachaResult));
        
        //뽑힌 영웅 랜덤한 위치에 생성
        SpawnHero(gachaResult);
    }
    public void HighGachaButtonClick()
    {
        string gachaResult = RandomGachaSystem(highGachaList);
        PopUpPricePanel(highGachaPanel, highGachaPriceText, heroNameKorean(gachaResult));
        if(gachaResult != "Bang") SpawnHero(gachaResult);
        
    }
    public void SpecialGachaButtonClick()
    {
        string gachaResult = RandomGachaSystem(specialGachaList);
        PopUpPricePanel(specialGachaPanel, specialGachaPriceText, heroNameKorean(gachaResult));
        if(gachaResult != "Bang") SpawnHero(gachaResult);
    }

    // public string heroNameKorean(string engHeroName)
    // {
    //     if (engHeroName == "Low_WhiteBread") return "식빵";
    //     else if (engHeroName == "Low_Baguette") return "바게트";
    //     else if (engHeroName == "Low_RollBread") return "롤빵";
    //     else if (engHeroName == "Low_Croissant") return "크루아상";
    //     
    //     else if (engHeroName == "High_Chestnut Loaf") return "밤식빵";
    //     else if (engHeroName == "High_Choco_Shell_Bread") return "초코소라빵";
    //     else if (engHeroName == "High_Custard_Cream_Bun") return "슈크림빵";
    //     else if (engHeroName == "High_Melon Pan") return "메론빵";
    //     else if (engHeroName == "High_Strawberry_Muffin Variant") return "딸기머핀";
    //     else if (engHeroName == "High_Sweet_Red_Bean_Bun") return "단팥빵";
    //     
    //     else if (engHeroName == "Special_Strawberry_Cake") return "딸기케이크";
    //     else if (engHeroName == "Special_Chocolate_Roll_Bread") return "초코롤빵";
    //     else if (engHeroName == "Special_Melon_Croissant") return "메론크루아상";
    //     else if (engHeroName == "Special_Red_Bean_Baguette") return "팥바게트";
    //     
    //     else if (engHeroName == "Bang") return "꽝";
    //
    //     return "";
    //     //Special_Strawberry_Cake
    //     //Special_Chocolate_Roll_Bread
    //     //Special_Melon_Croissant
    //     //Special_Red_Bean_Baguette
    //
    //     //     lowGachaList.Add("Low_WhiteBread");
    //     // lowGachaList.Add("Low_Baguette");
    //     // lowGachaList.Add("Low_RollBread");
    //     // lowGachaList.Add("Low_Croissant");
    //
    //     // (High_Chestnut Loaf);
    //     // specialGachaList.Add("High_Choco_Shell_Bread");
    //     // specialGachaList.Add("High_Custard_Cream_Bun");
    //     // specialGachaList.Add("High_Melon Pan");
    //     // specialGachaList.Add("High_Strawberry_Muffin Variant");
    //     // specialGachaList.Add("High_Sweet_Red_Bean_Bun");
    // }
    
    private static readonly Dictionary<string, string> heroNameMap = new Dictionary<string, string>()
    {
        // Low
        { "Low_WhiteBread", "식빵" },
        { "Low_Baguette", "바게트" },
        { "Low_RollBread", "롤빵" },
        { "Low_Croissant", "크루아상" },

        // High
        { "High_Chestnut Loaf", "밤식빵" },
        { "High_Choco_Shell_Bread", "초코소라빵" },
        { "High_Custard_Cream_Bun", "슈크림빵" },
        { "High_Melon Pan", "메론빵" },
        { "High_Strawberry_Muffin", "딸기머핀" },
        { "High_Sweet_Red_Bean_Bun", "단팥빵" },

        // Special
        { "Special_Strawberry_Cake", "딸기케이크" },
        { "Special_Chocolate_Roll_Bread", "초코롤빵" },
        { "Special_Melon_Croissant", "메론크루아상" },
        { "Special_Red_Bean_Baguette", "팥바게트" },

        // 꽝
        { "Bang", "꽝" },
    };

    public string heroNameKorean(string engHeroName)
    {
        if (heroNameMap.TryGetValue(engHeroName, out var koreanName))
            return koreanName;

        return ""; // 매칭되는 이름이 없을 때 빈 문자열
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
