using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    private float rayRange = 3f; //레이 거리조절
    [SerializeField]
    private float getRange; // 아이템 습득가능 거리

    private bool pickupActivated = false; // 습등 가능유무 체크

    private RaycastHit hitinfo; // 충돌체 정보저장

    [SerializeField]
    private LayerMask layerMask; // 아이템 레이어 설정
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Image pickImage;
    [SerializeField]
    private Image pickItemImage;

    public Item item;
    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitinfo.transform != null)
            {
                inventory.AcquireItem(hitinfo.transform.GetComponent<ItemPickup>().item);
                Destroy(hitinfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    private void CheckItem()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitinfo, rayRange, layerMask))
        {
            if(hitinfo.transform.tag == "item")
            {
                item = hitinfo.transform.GetComponent<ItemPickup>().item;
                ItemInfoAppear(item);
            }
        }
        else
        {
            ItemInfoDisappear();
        }
    }

    private void ItemInfoAppear(Item _item)
    {
        pickupActivated = true;
        pickImage.gameObject.SetActive(true);
        pickItemImage.gameObject.SetActive(true);
        pickItemImage.sprite = _item.itemImage;
    }

    private void ItemInfoDisappear()
    {
        pickupActivated = false;
        pickImage.gameObject.SetActive(false);
        pickItemImage.gameObject.SetActive(false);
        pickItemImage.sprite = null;
    }
}
