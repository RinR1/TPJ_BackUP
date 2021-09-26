using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public Vector3 playerpos;
}


public class Save_Load : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVEDATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.text";

    private PlayerController thePlayer;


    void Start()
    {
        SAVEDATA_DIRECTORY = Application.dataPath + "/Saves/";

        if (!Directory.Exists(SAVEDATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVEDATA_DIRECTORY);
        }
    }

    public void SaveData()
    {
        thePlayer = FindObjectOfType<PlayerController>();

        saveData.playerpos = thePlayer.transform.position;

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVEDATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log(json);
    }

    public void LoadData()
    {
        if(File.Exists(SAVEDATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVEDATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            thePlayer.transform.position = saveData.playerpos;
        }
        else
        {

        }
    }
}
