using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot ds_instance;

    public ItemSlot dragSlot;

    [SerializeField]
    private Image itemImage;

    // Start is called before the first frame update
    void Start()
    {
        ds_instance = this;
    }

    public void DragSlotImage(Image _itemImage)
    {
        itemImage.sprite = _itemImage.sprite;
        SetImageColor(1);
    }

    public void SetImageColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
}
