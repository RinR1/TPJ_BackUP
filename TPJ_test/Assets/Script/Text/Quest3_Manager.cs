using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest3_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private ActionController acManager;
    [SerializeField]
    private QuestBoxManager questManager;

    private void Start()
    {
        Player = GameObject.Find("Player");
        acManager = Player.GetComponentInChildren<ActionController>();
        questManager = Player.GetComponentInChildren<QuestBoxManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            acManager.Quest3Action();
        }
    }
}
