using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoding : MonoBehaviour
{
    [SerializeField]
    private string loadingScene;
    [SerializeField]
    private GameObject textbar;
    [SerializeField]
    private GameObject player;

    private IEnumerator ChangeScene()
    {
        var targetScene = SceneManager.GetSceneByName(loadingScene);
        player.transform.position = new Vector3(1.79f, 1.85f, -62f);
        if (!targetScene.isLoaded)
        {
            var op = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);

            while (!op.isDone)
            {
                yield return null;
            }
        }
    }

    private IEnumerator UnLoadScene()
    {
        var targetScene = SceneManager.GetSceneByName(loadingScene);
        if (targetScene.isLoaded)
        {
            var op = SceneManager.UnloadSceneAsync(loadingScene);

            while (!op.isDone)
            {
                yield return null;
            }
        }
    }

    private void OnTriggerEnter(Collider other) // 문에 접근시
    {
        textbar.SetActive(true);
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            player.transform.position = new Vector3(1.79f, 1.85f, -62f);
            StartCoroutine(ChangeScene());
        }
    }

    private void OnTriggerExit(Collider other) // 문에서 벗어날시
    {
        if (other.CompareTag("Player"))
        {
            textbar.SetActive(false);
        }
    }
}
