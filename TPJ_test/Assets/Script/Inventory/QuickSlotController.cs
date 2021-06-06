using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField]
    private ItemSlot[] quickSlots;
    [SerializeField]
    private Transform tf_Parent; // 퀵슬롯의 부모 오브젝트

    private int selectedSlot;
    [SerializeField]
    private GameObject go_SelectImage;

    private void Start()
    {
        quickSlots = tf_Parent.GetComponentsInChildren<ItemSlot>();
        selectedSlot = 0;
        go_SelectImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TryInputNum();
    }

    private void TryInputNum()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeSlot(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeSlot(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeSlot(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            ChangeSlot(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            ChangeSlot(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            ChangeSlot(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            ChangeSlot(6);
    }

    private void ChangeSlot(int _num)
    {
        SelectedSlot(_num);
    }

    private void SelectedSlot(int _num)
    {
        selectedSlot = _num;

        quickSlots[selectedSlot].Useditem();
        go_SelectImage.transform.position = quickSlots[selectedSlot].transform.position;
        go_SelectImage.SetActive(true);
    }
}
