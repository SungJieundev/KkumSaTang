using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomGacha : MonoBehaviour
{
    private int _coinGachaCost = 0;
    private int _specialGacha1Cost = 0;
    private int _specialGacha2Cost = 0;
    
    public List<string> coinGachaList = new List<string>();
    public List<string> specialGacha1List = new List<string>();
    public List<string> specialGacha2List = new List<string>();

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
    }

    public void CoinGachaButtonClick()
    {
        //영웅 랜덤 뽑기 구현완료
        Debug.Log(RandomGachaSystem(coinGachaList));
        
        //뽑힌 영웅 랜덤한 위치에 생성 
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
