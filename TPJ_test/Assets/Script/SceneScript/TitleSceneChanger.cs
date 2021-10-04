using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChanger : MonoBehaviour
{
    public static TitleSceneChanger ts_mine;

    [SerializeField]
    private Save_Load theSaveNRoad;

    private void Awake()
    {
        if (ts_mine == null)
        {
            ts_mine = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public void NewGameButton()
    {
        StartCoroutine(NewLoadCoroutine());
    }

    IEnumerator NewLoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");

        while (!operation.isDone)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    public void LoadGameButton()
    {
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {

        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");

        while (!operation.isDone)
        {
            yield return null;
        }

        theSaveNRoad = FindObjectOfType<Save_Load>();
        theSaveNRoad.LoadData();
        Destroy(gameObject);
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
