using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBoxManager : MonoBehaviour
{

    public int questId;
    public int questActionIndex;
    public int quest1ObjCheck = 0;
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
        Quest2Check();
    }

    void GenerateQuestData()
    {
        questList.Add(0, new QuestBoxData("퀘스트가 없습니다","새로운 퀘스트를 받으세요"
                                            , new int[] {0}));
        questList.Add(10, new QuestBoxData("시청내부를 조사해보자", "시청 내부에 들어가서 쓸만한 아이템을 찾아 보자(" + quest1ObjCheck +"/5)"
                                            , new int[] {1000, 2000}));
    }

    public void Quest2Check()
    {
        if(quest1ObjCheck == 5)
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
