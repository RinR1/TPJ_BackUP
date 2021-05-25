using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName; // 아이템의 이름
    [Tooltip("HP, Water, MRE 만 가능합니다")]
    public string[] itemPart; // 아이템의 효과 (복수 효과 가능)
    public int[] itemNum; // 아이템 효과수치값
}

public class ItemEffectDataBase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffect;

    private const string HP = "HP", Water = "Water", MRE = "MRE";

    [SerializeField]
    private Status status;
    [SerializeField]
    private GunController gunController;

    public void UseItem(Item _item)
    {
        if(_item.itemType == Item.ItemType.Equipment)
        {

        }

        else if(_item.itemType == Item.ItemType.Used)
        {
            for (int i = 0; i < itemEffect.Length; i++)
            {
                if(itemEffect[i].itemName == _item.itemName)
                {
                    for(int j=0; j< itemEffect[i].itemPart.Length; j++)
                    {
                        switch (itemEffect[i].itemPart[j])
                        {
                            case HP:
                                status.IncreaseHP(itemEffect[i].itemNum[j]);
                                break;
                            case Water:
                                status.IncreaseWater(itemEffect[i].itemNum[j]);
                                break;
                            case MRE:
                                status.IncreaseMRE(itemEffect[i].itemNum[j]);
                                break;
                            default:
                                Debug.Log("적용되지 않았어 ㅅㅂ");
                                break;
                        }
                    }
                    return;
                }
            }
        }
    }
}
