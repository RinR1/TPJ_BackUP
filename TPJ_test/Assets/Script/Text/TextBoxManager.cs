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
        TextData.Add(5000, new string[] { "시체가 들어있는 바디백이다.",
                                        "굳이 열어서 확인해보고 싶지는 않다."});

        TextData.Add(6000, new string[] { "거대한 컨테이너다.",
                                         "굳게 잠겨있어서 열지는 못할거같다."});

        //퀘스트용 상호작용& 대사
        TextData.Add(10 + 1000, new string[] {"모래바람이 매우 거세지기 시작했다. 눈앞에 보이는 시청으로 잠시 피해야될거같다.",
                                             "그런데 시청앞을 로봇들이 막아서고있다. 일단 저것들부터 정리해야겠어"});

        TextData.Add(11+ 2000, new string[] {"로봇이 열쇠같은것을 떨어트렷다",
                                             "열쇠 끝에 시청정문이라 적혀져있었다.",
                                             "이열쇠를 이용해서 시청내부로 들어가봐야겠다."});
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
