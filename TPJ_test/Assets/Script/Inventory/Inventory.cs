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

    public ItemSlot[] slots; //아이템 슬롯

    [SerializeField]
    private PlayerController playCon;
    [SerializeField]
    private GunController gunCon;
    [SerializeField]
    private Status stat;
    [SerializeField]
    private SlotInfo slotInfo;

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
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryActivated = !InventoryActivated;

            if (InventoryActivated)
            {
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                playCon.isActive = false;
                gunCon.isActive = false;
                stat.isActive = false;
                OpenInventory();
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                slotInfo.HideTooltip();
                playCon.isActive = true;
                gunCon.isActive = true;
                stat.isActive = true;
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
                    Debug.Log("이거 되긴하냐?");
                }
            }
        }
    }
}
