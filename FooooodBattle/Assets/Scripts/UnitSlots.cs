using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class UnitSlots : MonoBehaviour, IDropHandler
{
    //dropped == boughtUnit
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Debug.Log(dropped);
        DraggableShopCard draggableCard = dropped.GetComponent<DraggableShopCard>();
        if (ArenaManager.Instance.checkMoney())
        {
            draggableCard.parentAfterDrag = transform;
            //ArenaManager.Instance.Buy(gameObject, dropped);
            mergeUnits(dropped);
            ArenaManager.Instance.Buy(gameObject, dropped);
            
        } 
    }

    public void mergeUnits(GameObject dropped)
    {
        UnitSlot unitSlot = ArenaManager.Instance.CurrentBoughtUnits.Where(x => x.Slot == gameObject).FirstOrDefault(); 
        if(unitSlot != null && unitSlot.Unit.GetComponent<Unit>().Id == dropped.GetComponent<Unit>().Id)
        {
            Unit unit = unitSlot.Unit.GetComponent<Unit>();
            var toRemove = ArenaManager.Instance.CurrentBoughtUnits.Where(x => x.Unit == dropped).FirstOrDefault();
            ArenaManager.Instance.CurrentBoughtUnits.Remove(toRemove);
            Destroy(dropped);

            unit.partLevelCount += 1;
            unit.Attack += 1;
            unit.Health += 1;
            unit.Energy += 1;
            ArenaManager.Instance.setAttributes(unitSlot.Unit);
            //Partcile effect add here!
        } else
        {
            Debug.Log("Nothing to merge");
            return;
        }
        
    }
}
