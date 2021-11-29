using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunChange : MonoBehaviour
{
    [SerializeField] private float realtimeAndSecond; // 게임상 100초 = 현실 1초

    [SerializeField] private float fogDensityCal; // Fog 증감량

    [SerializeField] private float nightFogDensity; // 밤상태의 Fog밀도
    private float dayFogDensity; // 낮 상태의 Fog밀도
    private float currentFogDensity; // 현재 Fog밀도 계산

    private bool nightCheck = false; //밤낮 확인용
    // Start is called before the first frame update
    void Start()
    {
        dayFogDensity = RenderSettings.fogDensity;

    }

    // Update is called once per frame
    void Update()
    {
        if(Inventory.InventoryActivated != true && ActionController.TextboxActivated != true && MainSceneChanger.PauseActivated != true && MainSceneChanger.GameClearActivated != true && Status.PlayerDead != true)
        {
            transform.Rotate(Vector3.right, 0.07f * realtimeAndSecond * Time.deltaTime);

            if(transform.eulerAngles.x >= 170)
            {
                nightCheck = true;
            }
            else if(transform.eulerAngles.x <= 340)
            {
                nightCheck = false;
            }

            if (nightCheck)
            {
                if (currentFogDensity <= nightFogDensity)
                {
                    currentFogDensity += 0.07f * fogDensityCal * Time.deltaTime;
                    RenderSettings.fogDensity = currentFogDensity;
                }

            }
            else
            {
                if (currentFogDensity >= dayFogDensity)
                {
                    currentFogDensity -= 0.07f * fogDensityCal * Time.deltaTime;
                    RenderSettings.fogDensity = currentFogDensity;
                }
            }
        }
    }
}
