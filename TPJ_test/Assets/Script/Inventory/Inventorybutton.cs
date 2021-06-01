using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventorybutton : MonoBehaviour
{

    [SerializeField]
    private Inventory Inv;

    public void ClickRearrange()
    {
        Inv.SlotRearrange();
    }
}
