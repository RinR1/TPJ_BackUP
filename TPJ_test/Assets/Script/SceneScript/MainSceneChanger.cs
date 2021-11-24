using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneChanger : MonoBehaviour
{
    public static bool PauseActivated = false;
    public static bool GameClearActivated = false;
    public static bool CityhallActivated = false;

    //  필요 컴포넌트
    [SerializeField]
    private GameObject go_PauseMenu;
    [SerializeField]
    private GameObject go_GameOver;
    [SerializeField]
    private GameObject go_cityhall;
    [SerializeField]
    private Save_Load S_L;
    [SerializeField]
    private Inventory inv;

    private void Start()
    {
        PauseActivated = false;
        GameClearActivated = false;
        CityhallActivated = false;
    }

    void Update()
    {
        TryOpenPause();
        TryGameOver();
        TryGameClear();
        TryCityhallActivated();
    }

    private void TryOpenPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Inventory.InventoryActivated != true && ActionController.TextboxActivated != true && GameClearActivated != true && Status.PlayerDead != true)
        {
            PauseActivated = !PauseActivated;

            if (PauseActivated)
            {
                Time.timeScale = 0;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                go_PauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                go_PauseMenu.SetActive(false);
            }
        }
    }

    private void TryCityhallActivated()
    {
        if (CityhallActivated)
        {
            go_cityhall.SetActive(true);
            CityhallActivated = !CityhallActivated;
        }
    }

    private void TryGameOver()
    {
        if(Status.PlayerDead == true)
        {
            Time.timeScale = 0;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            go_GameOver.SetActive(true);
        }
    }

    private void TryGameClear()
    {
        if (GameClearActivated)
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    public void ResumeButton()
    {
        PauseActivated = !PauseActivated;

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        go_PauseMenu.SetActive(false);
    }

    public void SaveButton()
    {
        S_L.SaveData();
    }

    public void OptionButton()
    {

    }

    public void ReturnTitleButton()
    {
        PauseActivated = !PauseActivated;

        Time.timeScale = 1;
        go_PauseMenu.SetActive(false);

        SceneManager.LoadScene("TitleScene");
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
