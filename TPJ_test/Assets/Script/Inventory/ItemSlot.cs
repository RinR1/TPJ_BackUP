using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 아이템 갯수
    public Image itemImage; // 아이템 이미지
    public GameObject sp_click; // 선택시 이미지

    [SerializeField]
    private Text t_Count; // 아이템 갯수 표시

    private ItemEffectDataBase itemData;
    private SlotInfo slotInfo;

    public bool slotClear = false;

    void Start()
    {
        itemData = FindObjectOfType<ItemEffectDataBase>();
        slotInfo = FindObjectOfType<SlotInfo>();
    }

    // 이미지 투명도 조절
    private void Setcolor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    // 아이템 획득
    public void Additem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if(item.itemType != Item.ItemType.Equipment)
        {
            t_Count.text = itemCount.ToString();
        }
        else
        {
            t_Count.text = "0";
        }

        Setcolor(1);
    }

    // 아이템 갯수 조절
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        t_Count.text = "X" + itemCount.ToString();
        t_Count.gameObject.SetActive(true);

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    // 슬롯 초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        sp_click.SetActive(false);
        Setcolor(0);

        t_Count.text = "0";
        t_Count.gameObject.SetActive(false);

        slotClear = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (item != null)
            {
                sp_click.SetActive(true);
                slotInfo.ShowTooltip(item);
            }
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                itemData.UseItem(item);

                if (item.itemType == Item.ItemType.Used)
                {
                    SetSlotCount(-1);
                    if (itemCount <= 0)
                    {
                        slotInfo.HideTooltip();
                    }
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        sp_click.SetActive(false);
    }
}
