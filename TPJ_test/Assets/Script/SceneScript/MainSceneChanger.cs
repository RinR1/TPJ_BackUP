using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneChanger : MonoBehaviour
{
    public static bool PauseActivated = false;

    //  필요 컴포넌트
    [SerializeField]
    private GameObject go_PauseMenu;
    [SerializeField]
    private Save_Load S_L;
    [SerializeField]
    private Inventory inv;

    void Update()
    {
        TryOpenPause();
    }

    private void TryOpenPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Inventory.InventoryActivated != true && ActionController.TextboxActivated != true)
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
