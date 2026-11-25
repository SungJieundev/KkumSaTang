using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btn : MonoBehaviour
{
    /// <summary>
    /// 버튼에서 호출되는 함수 – TitleScene으로 이동
    /// </summary>
    public void ButtonToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
