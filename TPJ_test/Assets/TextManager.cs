using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    public Text talkText;
    public GameObject scanObject;

    public void ScanAction(GameObject scanObj)
    {
        scanObject = scanObj;

        talkText.text = scanObject.name;
    }
}
