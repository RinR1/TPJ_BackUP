using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Add item", menuName = "AddItem/Item")]

public class Item : ScriptableObject
{
    public string itemName; // 아이템 이름
    [TextArea]
    public string itemInfo; // 아이템에 대한 설명
    public ItemType itemType; // 아이템 종류
    public Sprite itemImage; // 아이템 이미지
    public GameObject itemPrefab; // 아이템 프리팹

    public string weaponType; // 무기유형

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        Questitem,
        Keyitem,
        ETC
    }

}
