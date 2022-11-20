using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArenaManager : MonoBehaviour


{
    public static ArenaManager Instance;
    public List<GameObject> ShopCardsList;
    public List<GameObject> ShopSlots;
    public List<GameObject> CurrentShopUnits;
    // UnitSlot - Unit 
    public List<Tuple<GameObject, GameObject>> CurrentUnits;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ShopCardsList = new List<GameObject>(Resources.LoadAll<GameObject>("ShopCards"));
    }

    private void Start()
    {
        rollUnits();
    }

    public void rollUnits()
    {
        clearCurrentShopUnits();
        for (int i = 0; i < ShopCardsList.Count; i++)
        {
            GameObject shopCard = Instantiate(ShopCardsList[randomNumber()], new Vector3(0, 0, 0), Quaternion.identity);
            shopCard.transform.SetParent(ShopSlots[i].GetComponent<RectTransform>(), false);
            shopCard.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            CurrentShopUnits.Add(shopCard);
        }
    }

    private void clearCurrentShopUnits()
    {
        foreach(GameObject currentShopUnit in CurrentShopUnits)
        {
            Destroy(currentShopUnit);
        }
        CurrentShopUnits.Clear();
    }

    private int randomNumber()
    {
        int upperRange = ShopCardsList.Count;
        return (int) Random.Range(0.0f, (float) upperRange);
    }

    public void Buy(GameObject slot, GameObject boughtUnit)
    {
        Tuple<GameObject, GameObject> tmp = new Tuple<GameObject, GameObject>(slot, boughtUnit);
        if (CurrentUnits.Any())
        {
            bool checkIsSlotIsFilled = CurrentUnits.Any(x => x.Item1 == slot);
            if (!checkIsSlotIsFilled)
            {
                CurrentUnits.Add(tmp);
                CurrentShopUnits.Remove(boughtUnit);
            }
            else
            {
                Debug.Log("Slot is already filled");
            }

        } else
        {
            CurrentUnits.Add(tmp);
            CurrentShopUnits.Remove(boughtUnit);

        }
       
        
    }


}
