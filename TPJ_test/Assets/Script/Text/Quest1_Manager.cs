using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1_Manager : MonoBehaviour
{
    [SerializeField]
    private ActionController acMAnager;
    [SerializeField]
    private QuestBoxManager questManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            questManager.questId = 10;
            acMAnager.quest1Check = true;
            acMAnager.Quest1Action();
        }
    }
}
