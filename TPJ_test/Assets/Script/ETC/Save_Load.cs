using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{

    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNum = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNum = new List<int>();
}



public class Save_Load : MonoBehaviour
{
    private SaveData saveData = new SaveData();
    public bool indoorCheck = false;

    private string SAVEDATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.text";

    private PlayerController thePlayer;
    private Inventory theInventory;


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
        theInventory = FindObjectOfType<Inventory>();

        saveData.playerPos = thePlayer.transform.position;
        saveData.playerRot = thePlayer.transform.eulerAngles;

        saveData.invenArrayNum.Clear();
        saveData.invenItemName.Clear();
        saveData.invenItemNum.Clear();

        ItemSlot[] _slots = theInventory.GetSlots();

        for(int i = 0; i < _slots.Length; i++)
        {

            if (_slots[i].item != null)
            {
                saveData.invenArrayNum.Add(i);
                saveData.invenItemName.Add(_slots[i].item.itemName);
                saveData.invenItemNum.Add(_slots[i].itemCount);
            }
        }


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

            thePlayer = FindObjectOfType<PlayerController>();
            theInventory = FindObjectOfType<Inventory>();


            thePlayer.transform.position = saveData.playerPos;
            thePlayer.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInventory.LoadtoInv(saveData.invenArrayNum[i], saveData.invenItemName[i],saveData.invenItemNum[i]);
            }
        }
        else
        {

        }
    }
}
