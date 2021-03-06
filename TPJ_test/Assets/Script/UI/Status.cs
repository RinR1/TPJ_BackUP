using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{

    [SerializeField]
    private int hp; // 최대체력
    [SerializeField] private int currentHp; //현재 체력

    [SerializeField]
    private int sp; // 최대기력
    private int currentSp; // 현재 기력

    [SerializeField]
    private int spIncreaseSpeed; // 기력회복 증가량

    [SerializeField]
    private int spIncreaseDelay; // 기력회복 딜레이
    private int currentSpIncreaseDelay;

    [SerializeField]
    private int water; // 최대갈증
    [SerializeField]
    private int currentWater; // 현재갈증

    [SerializeField]
    private int waterDecreaseSpeed;
    private int currentWaterDecreaseSpeed;

    [SerializeField]
    private int mre; // 최대허기
    [SerializeField]
    private int currentMRE; // 현재 허기

   [SerializeField]
    private int mreDecreaseSpeed;
    private int currentMreDecreaseSpeed;

    [SerializeField]
    private Image[] statusGauge;

    private const int HP = 0, SP = 1, Water = 2, MRE = 3;

    private bool spUsed;

    public bool isActive = true;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = hp;
        currentSp = sp;
        currentWater = water;
        currentMRE = mre;
    }

    // Update is called once per frame
    void Update()
    {   
        GaugeUpdate();
        
        if (isActive)
        {
            Hungry();
            Thirsty();
            SpRechargingTime();
            SpRecover();
            
        }
    }

    private void SpRechargingTime()
    {
        if (spUsed)
        {
            if (currentSpIncreaseDelay < spIncreaseDelay)
            {
                currentSpIncreaseDelay++;
            }
            else
            {
                spUsed = false;
            }
        }
    }

    private void SpRecover()
    {
        if (!spUsed && currentSp < sp)
        {
            currentSp += spIncreaseSpeed;
        }
    }

    private void Hungry()
    {
        if (currentMRE > 0)
        {
            if (currentMreDecreaseSpeed < mreDecreaseSpeed)
            {
                currentMreDecreaseSpeed++;
            }
            else
            {
                currentMRE--;
                currentMreDecreaseSpeed = 0;
            }
        }
        else
            Debug.Log("허기가 0이 되었다");
    }

    private void Thirsty()
    {
        if (currentWater > 0)
        {
            if (currentWaterDecreaseSpeed < waterDecreaseSpeed)
            {
                currentWaterDecreaseSpeed++;
            }
            else
            {
                currentWater--;
                currentWaterDecreaseSpeed = 0;
            }
        }
        else
            Debug.Log("갈증이 0이 되었다");
    }

    private void GaugeUpdate()
    {
        statusGauge[HP].fillAmount = (float)currentHp / hp;
        statusGauge[SP].fillAmount = (float)currentSp / sp;
        statusGauge[Water].fillAmount = (float)currentWater / water;
        statusGauge[MRE].fillAmount = (float)currentMRE / mre;
    }

    public void IncreaseHP(int _count)
    {
        if(currentHp + _count < hp)
        {
            currentHp += _count;
        }
        else
        {
            currentHp = hp;
        }
    }

    public void DecreaseHP(int _count)
    {
        if(currentHp - _count < 0)
        {
            currentHp = 0;
        }
        else
        {
            currentHp -= _count;
        }
        

        if(currentHp <= 0)
        {
            Debug.Log("캐릭터 사망");
        }
    }

    public void IncreaseMRE(int _count)
    {
        if (currentMRE + _count < mre)
        {
            currentMRE += _count;
        }
        else
        {
            currentMRE = mre;
        }
    }

    public void IncreaseWater(int _count)
    {
        if (currentWater + _count < water)
        {
            currentWater += _count;
        }
        else
        {
            currentWater = water;
        }
    }

    public void DecreaseSp(int _count)
    {
        spUsed = true;
        currentSpIncreaseDelay = 0;

        if(currentSp - _count > 0)
        {
            currentSp -= _count;
        }
        else
        {
            currentSp = 0;
        }
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }
}
