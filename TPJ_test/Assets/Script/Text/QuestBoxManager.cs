using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoxManager : MonoBehaviour
{
    public static bool Quest2Clear = false;

    public int questId;
    public int questActionIndex;
    public int quest3ObjCheck = 0;
    [SerializeField]
    private GameObject[] QuestItem;

    Dictionary<int, QuestBoxData> questList;

    private void Awake()
    {
        questList = new Dictionary<int, QuestBoxData>();
        GenerateQuestData();
    }

    private void Update()
    {
        quest2Check();
        Quest3Check();
    }



    void GenerateQuestData()
    {
        questList.Add(0, new QuestBoxData("퀘스트가 없습니다","새로운 퀘스트를 받으세요"
                                            , new int[] {0}));
        questList.Add(10, new QuestBoxData("시청으로 들어갈 방법을 찾아라", "일단 시청앞 로봇들을 처리해보자"
                                    , new int[] { 1000, 2000 }));
        questList.Add(20, new QuestBoxData("시청내부를 조사하라", "시청 내부에 들어가서 쓸만한 아이템을 찾아 보자(" + quest3ObjCheck +"/5)"
                                            , new int[] {2000, 3000}));
    }
    private void quest2Check()
    {
        if (Quest2Clear)
        {
            Robot.Quest2ItemActivated = true;
        }
    }

    public void Quest3Check()
    {
        if(quest3ObjCheck == 5)
        {
           MainSceneChanger.GameClearActivated = true;
        }
    }

    public int GetQuestIndex(int id)
    {
        return questId + questActionIndex;
    }

    public string QuestCheck(int id)
    {
        if (id == questList[questId].npcId[questActionIndex])
        {
            questActionIndex++;
        }

        ControlObject();
        if (questActionIndex == questList[questId].npcId.Length)
        {
            NextQuest();
        }

        return questList[questId].questName;
    }

    public string QuestContentsCheck(int id)
    {
        return questList[questId].questContents;
    }

    public string QuestCheck()
    {
        return questList[questId].questName;
    }

    public string QuestContentsCheck()
    {
        return questList[questId].questContents;
    }

    void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }

    void ControlObject()
    {
        switch (questId)
        {
            case 10:
                if(questActionIndex == 1)
                {
                    QuestItem[0].SetActive(true);
                }
                break;
            case 20:

                break;
        }
    }
}
