using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour
{
    [SerializeField]
    private Save_Load S_L;

    public void NewGameButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadGameButton()
    {
        S_L.LoadData();
    }

    public void OptionButton()
    {

    }

    public void QuitGameButton()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); // 어플리케이션 종료
        #endif
    }
}
