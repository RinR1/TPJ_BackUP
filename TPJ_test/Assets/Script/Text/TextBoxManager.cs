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
        TextData.Add(10 + 1000, new string[] {"모래바람이 매우 거세지기 시작했다.",
                                             "마침 눈앞에 시청이 보이니 저기서 물건을 찾아봐야겠다."});

        TextData.Add(11+ 2000, new string[] {"여기서 얻을만한 아이템은 다얻은거같다.",
                                             "오늘은 여기서 쉬고 내일 다시 움직여야 할거같다."});
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
