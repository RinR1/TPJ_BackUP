using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeChange : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Image img;
    [SerializeField] MapLoading map;
    private float currenttime;
    private float f_time = 0.3f;

    public void FadeOut()
    {
        StartCoroutine(FadeOutFlow());
    }

    IEnumerator FadeOutFlow()
    {
        currenttime = 0f;
        img.gameObject.SetActive(true);
        Color alpha = img.color;
        while(alpha.a < 1f)
        {
            currenttime += Time.deltaTime * f_time;
            alpha.a = Mathf.Lerp(0, 1, currenttime);
            img.color = alpha;

            yield return null;
        }

        yield return null;
    }

    public void FadeIn()
    {
        currenttime = 0f;
        StartCoroutine(FadeInFlow());
    }

    IEnumerator FadeInFlow()
    {
        currenttime = 0f;
        Color alpha = img.color;
        while (alpha.a > 0f)
        {
            currenttime += Time.deltaTime * f_time;
            alpha.a = Mathf.Lerp(1, 0, currenttime);
            img.color = alpha;

            yield return null;
        }
        img.gameObject.SetActive(false);

        yield return null;
    }
}
