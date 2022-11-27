using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSlot
{
    public GameObject Slot;
    public GameObject Unit;

    public UnitSlot(GameObject Slot, GameObject Unit)
    {
        this.Slot = Slot;
        this.Unit = Unit;
    }
}
