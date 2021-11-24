using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxManager : MonoBehaviour
{
    Dictionary<int, string[]> TextData;

    // Start is called before the first frame update
    void Start()
    {
        TextData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        // 일반 상호작용&대화 대사
        TextData.Add(4000, new string[] {"단단히 잠겨있군…"});

        TextData.Add(5000, new string[] { "시체가 들어있는 바디백이군.",
                                        "굳이 열어서 확인해보고 싶지는 않아."});

        TextData.Add(6000, new string[] { "거대한 컨테이너군.",
                                         "굳게 잠겨있어서 열지는 못할거같군."});

        //퀘스트용 상호작용& 대사
        TextData.Add(10 + 100, new string[] {"모래 폭풍이 불 것 같군",
                                              "어딘가 몸을 피하는게 좋겠어"});

        TextData.Add(11+ 200, new string[] {"시청 열쇠인가 본데."});

        TextData.Add(20 + 300, new string[] {"무엇…드릴…요…",
                                             "희미한, 말소리가 들리는군"});

        TextData.Add(30 + 400, new string[] {"제법 익숙한 외형이로군.",
                                             "… 그날이 기억나기도 해",
                                             "아주 오래전의…"});
    }
    
    public string GetTalk(int id, int textIndex)
    {
        if (!TextData.ContainsKey(id))
        {
            if (!TextData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 100, textIndex);
            }
            else
            {
                return GetTalk(id - id % 10, textIndex);
            }
        }

        if(textIndex == TextData[id].Length)
        {
            return null;
        }
        else
        {
            return TextData[id][textIndex];
        }  
    }
}
