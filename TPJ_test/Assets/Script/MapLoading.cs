using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoading : MonoBehaviour
{
    [SerializeField]
    private string loadingScene;
    [SerializeField]
    private string ownScene;
    [SerializeField] private GameObject textbar;
    [SerializeField] private FadeChange fadeChange;

    [SerializeField] private bool sceneChange = false;
    public IEnumerator ChangeScene()
    {
        var targetScene = SceneManager.GetSceneByName(loadingScene);
        if (!targetScene.isLoaded)
        {
            var op = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Additive);

            while (!op.isDone)
            {
                yield return null;
            }

            yield return new WaitForSeconds(2f);

        }
    }

    public IEnumerator UnLoadScene()
    {
        var targetScene = SceneManager.GetSceneByName(loadingScene);

        if (targetScene.isLoaded)
        {
            var currentScene = SceneManager.GetSceneByName(ownScene);
            SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), currentScene);

            var op = SceneManager.UnloadSceneAsync(loadingScene);
            while (!op.isDone)
            {
                yield return null;
            }
        }
    }

    private void OnTriggerStay(Collider other) // 문에 접근시
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!sceneChange)
            {
                sceneChange = true;
                other.transform.position = new Vector3(4.3f, 2.8f, -319f);
            }
            else
            {
                sceneChange = false;
                other.transform.position = new Vector3(1.9f, 1.5f, -57.8f);
            }
            StartCoroutine(ChangeScene());
        }
    }

    private void OnTriggerExit(Collider other) // 문에서 벗어날시
    {
        if (other.CompareTag("Player"))
        {
            var dir = Vector3.Angle(transform.forward, other.transform.position - transform.position);
            if(dir < 90)
            {
                StartCoroutine(UnLoadScene());
            }
            else
            {

            }
        }
    }
}
