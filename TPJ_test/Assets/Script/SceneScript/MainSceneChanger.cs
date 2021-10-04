using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneChanger : MonoBehaviour
{
    public static bool PauseActivated = false;

    [SerializeField]
    private GameObject go_PauseMenu;
    [SerializeField]
    private PlayerController playCon;
    [SerializeField]
    private GunController gunCon;
    [SerializeField]
    private Status stat;
    [SerializeField]
    private Save_Load S_L;

    void Update()
    {
        TryOpenPause();
    }

    private void TryOpenPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseActivated = !PauseActivated;

            if (PauseActivated)
            {
                Time.timeScale = 0;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                playCon.isActive = false;
                gunCon.isActive = false;
                stat.isActive = false;
                go_PauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playCon.isActive = true;
                gunCon.isActive = true;
                stat.isActive = true;
                go_PauseMenu.SetActive(false);
            }
        }
    }

    public void ResumeButton()
    {
        PauseActivated = !PauseActivated;

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playCon.isActive = true;
        gunCon.isActive = true;
        stat.isActive = true;
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
        playCon.isActive = true;
        gunCon.isActive = true;
        stat.isActive = true;
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
