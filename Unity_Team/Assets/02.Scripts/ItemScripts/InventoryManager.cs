using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public enum inventoryType
    {
        BackPack,
        FeildBackPack,
        Storage,
        Inen_Quick_Slot,
        Quick_Slot,
        Equipment_Slot
    }

    public inventoryType inventype;

}
