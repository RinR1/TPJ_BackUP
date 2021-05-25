using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool InventoryActivated = false; //인벤토리 on&off용

    //필요 컴포넌트
    [SerializeField]
    private GameObject go_Inventory;
    [SerializeField]
    private GameObject go_SlotParent;

    [SerializeField]
    private PlayerController playercon;
    [SerializeField]
    private GunController guncon;
    [SerializeField]
    private Status status;

    private ItemSlot[] slots; //아이템 슬롯

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
                playercon.isActive = false;
                guncon.isActive = false;
                status.isActive = false;
                OpenInventory();
            }
            else
            {
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playercon.isActive = true;
                guncon.isActive = true;
                status.isActive = true;
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
}
