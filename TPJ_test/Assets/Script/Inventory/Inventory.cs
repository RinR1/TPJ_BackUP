using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool InventoryActivated = false;

    [SerializeField]
    private GameObject go_Inventory;
    [SerializeField]
    private GameObject go_SlotParent;
    [SerializeField]
    private QuestBoxManager qbManager;
    [SerializeField]
    private ActionController acMAnager;
    [SerializeField]
    private GameObject q1Check;
    public ItemSlot[] slots; //아이템 슬롯

    //  필요 컴포넌트
    [SerializeField]
    private SlotInfo slotInfo;

    public ItemSlot[] GetSlots() { return slots; }

    [SerializeField]
    private Item[] items;

    public void LoadtoInv(int _arrayNum, string _itemName, int _itemNum)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                slots[_arrayNum].Additem(items[i], _itemNum);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = go_SlotParent.GetComponentsInChildren<ItemSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I) && MainSceneChanger.PauseActivated != true && ActionController.TextboxActivated != true && MainSceneChanger.GameClearActivated != true)
        {
            InventoryActivated = !InventoryActivated;

            if (InventoryActivated)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                OpenInventory();
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                slotInfo.HideTooltip();
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        go_Inventory.SetActive(true);
    }

    private void CloseInventory()
    {
        go_Inventory.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)
        {
            if (Item.ItemType.Keyitem == _item.itemType)
            {
                MainSceneChanger.CityhallActivated = true;
                acMAnager.Quest2Action();
                if(q1Check != null)
                {
                    Destroy(q1Check);
                }
            }

            else
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (slots[i].item != null)
                    {
                        if (slots[i].item.itemName == _item.itemName)
                        {
                            slots[i].SetSlotCount(_count);
                            return;
                        }

                    }
                }
            }
        }


        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].Additem(_item, _count);
                return;
            }
        }
    }

    public void SlotRearrange()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                if (slots[i].slotClear == true && slots[i+1].item != null)
                {
                    slots[i].Additem(slots[i + 1].item, 1);
                    slots[i].SetSlotCount(slots[i + 1].itemCount - 1);
                    slots[i + 1].ClearSlot();

                    slots[i].slotClear = false;
                }
            }
        }
    }
}
