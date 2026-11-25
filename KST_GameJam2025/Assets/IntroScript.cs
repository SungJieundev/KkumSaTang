using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
    
    public GameObject image1; // 처음 보여지는 이미지
    public GameObject image2; // 두 번째로 보여질 이미지

    private bool isFirstKeyPressed = false;
    private bool isSecondKeyPressed = false;

    void Start()
    {
        // 시작 시 image1 활성화, image2 비활성화
        image1.SetActive(true);
        image2.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (!isFirstKeyPressed)
            {
                // 첫 번째 키 입력: 이미지 전환
                isFirstKeyPressed = true;
                image1.SetActive(false);
                image2.SetActive(true);
            }
            else if (!isSecondKeyPressed)
            {
                // 두 번째 키 입력: 씬 전환
                isSecondKeyPressed = true;
                SceneManager.LoadScene("GameScene");
            }
        }
    }
    
}
