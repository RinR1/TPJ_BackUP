using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_ItemDrop : MonoBehaviour
{
    public static bool ItemDropActive = false;
    public static bool MonsterDead = false;

    [SerializeField]
    private int deadCount;
    private void Start()
    {
        deadCount = 0;
        ItemDropActive = false;
        MonsterDead = false;
    }

    private void Update()
    {
        if (!ItemDropActive)
        {
            if(deadCount != 7)
            {
                if (MonsterDead)
                {
                    deadCount++;
                    MonsterDead = false;
                }
            }
            else
            {
                ItemDropActive = true;
            }
        }
    }
}
