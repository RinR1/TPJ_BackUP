using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text txt_ItemName;
    [SerializeField]
    private Text txt_ItemInfo;
    [SerializeField]
    private Image img_ItemImage;

    public void ShowTooltip(Item _item)
    {
        go_Base.gameObject.SetActive(true);

        txt_ItemName.text = _item.itemName;
        txt_ItemInfo.text = _item.itemInfo;
        img_ItemImage.sprite = _item.itemImage;
    }

    public void HideTooltip()
    {
        go_Base.gameObject.SetActive(false);
    }
}
