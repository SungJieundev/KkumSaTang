using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemove : MonoBehaviour
{
    private bool hasPressed = false;

    void Update()
    {
        // UI 클릭 중이면 키 입력 무시
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        if (!hasPressed && Input.anyKeyDown)
        {
            hasPressed = true;
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // 다음 씬 이름 또는 인덱스 입력
        SceneManager.LoadScene("Intro"); // 또는 SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
