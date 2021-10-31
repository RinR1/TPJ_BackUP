using System.Collections;
using System.Collections.Generic;

public class QuestBoxData
{
    public string questName;
    public string questContents;
    public int[] npcId;

    public QuestBoxData(string name, string contents, int[] npc)
    {
        questName = name;
        questContents = contents;
        npcId = npc;
    }
}
