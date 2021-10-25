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
        TextData.Add(100, new string[] { "시체가 들어있는 바디백이다.", 
                                        "굳이 열어서 확인해보고 싶지는 않다."});

        TextData.Add(200, new string[] { "거대한 컨테이너다", 
                                         "굳게 잠겨있어서 열지는 못할거같다."});
    }
    
    public string GetTalk(int id, int textIndex)
    {
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
