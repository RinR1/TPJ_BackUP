using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoxManager : MonoBehaviour
{
    public static bool Quest2Clear = false;

    public int questId;
    public int questActionIndex;

    Dictionary<int, QuestBoxData> questList;

    private void Awake()
    {
        Quest2Clear = false;
        questList = new Dictionary<int, QuestBoxData>();
        GenerateQuestData();
    }

    private void Update()
    {
        quest2Check();
    }

    void GenerateQuestData()
    {
        questList.Add(0, new QuestBoxData("퀘스트가 없습니다","새로운 퀘스트를 받으세요"
                                            , new int[] {0}));
        questList.Add(10, new QuestBoxData("시청으로 들어갈 방법을 찾아라", "시청 앞 광장을 탐색해보자"
                                    , new int[] { 100, 200 }));
        questList.Add(20, new QuestBoxData("시청내부를 조사하라", "시청 내부에 들어가서 쓸만한 아이템을 찾아 보자"
                                            , new int[] {200, 300}));
        questList.Add(30, new QuestBoxData("시청 2층을 조사하라", "시청 2층에서 들린 소리의 원인을 찾아 보자"
                                            , new int[] { 300, 400 }));
    }

    private void quest2Check()
    {
        if (Quest2Clear)
        {
            Robot.Quest2ItemActivated = true;
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

    private void NextQuest()
    {
        questId += 10;
        questActionIndex = 0;
    }
}
