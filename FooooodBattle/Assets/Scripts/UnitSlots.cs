using SharedLibrary;
using SharedLibrary.DTOs;
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
    public async void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Debug.Log(dropped);
        Transform parentAfterDrag = transform;
        DraggableShopCard draggableCard = dropped.GetComponent<DraggableShopCard>();
        if (ArenaManager.Instance.checkMoney())
        {
            Unit unit = dropped.gameObject.GetComponent<Unit>();
            bool success = await NetworkManager.buy(ArenaManager.Instance.getMoney(), new UserDTO
            {
                Username = NetworkManager.User,
                Token = NetworkManager.Token
            },
            new UnitObject
            {
                Health = unit.Health,
                Attack = unit.Attack,
                Energy = unit.Energy,
                UnitType = unit.UnitType,
                Level = unit.Level,
                partLevelCount = unit.partLevelCount,
                partLevelMax = unit.partLevelMax,
                Name = unit.name,
                //Id = unit.Id
            }
            );
            Debug.Log("Transfrom = " + transform + " /// " + parentAfterDrag);
            if (success == true)
            {
                draggableCard.parentAfterDrag = parentAfterDrag;
            }
           
                //mergeUnits(dropped);
                ArenaManager.Instance.Buy(gameObject, dropped);
            
        } 
    }

    public void mergeUnits(GameObject dropped)
    {
        UnitSlot unitSlot = ArenaManager.Instance.CurrentBoughtUnits.Where(x => x.Slot == gameObject).FirstOrDefault(); 
        if(unitSlot != null && unitSlot.Unit.GetComponent<Unit>().UnitType == dropped.GetComponent<Unit>().UnitType)
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
