using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArenaManager : MonoBehaviour


{
    public static ArenaManager Instance;
    public List<GameObject> ShopCardsList;
    public List<GameObject> ShopSlots;
    public List<GameObject> CurrentShopUnits;
    public List<UnitSlot> CurrentBoughtUnits;

    public GameObject MoneyStat;
    public GameObject HealthStat;
    public GameObject WinStat;
    public GameObject TurnStat;

    public GameObject selectedCard;
    [SerializeField]
    public int UnitCosts;
    [SerializeField]
    public int RollCosts;
    [SerializeField]
    public int SellCost;
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
        StartOfTurn();
    }

    public void rollUnits()
    {
        if (checkMoney())
        {
            instantiateUnits();
            calculateMoney(-RollCosts);
        }
    }

    public void instantiateUnits()
    {
        clearCurrentShopUnits();
        for (int i = 0; i < ShopCardsList.Count; i++)
        {
            GameObject shopCard = Instantiate(ShopCardsList[randomNumber()], new Vector3(0, 0, 0), Quaternion.identity);
            setAttributes(shopCard);
            shopCard.transform.SetParent(ShopSlots[i].GetComponent<RectTransform>(), false);
            shopCard.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            CurrentShopUnits.Add(shopCard);
        }   
    }

    public void StartOfTurn() {
        instantiateUnits();
        NetworkManager.newArenaSession(ArenaManager.Instance.CurrentShopUnits, ArenaManager.Instance.CurrentBoughtUnits);
    }

    public void setAttributes(GameObject shopCard)
    {
        Unit unit = shopCard.GetComponent<Unit>();
        TextMeshProUGUI unitName =  shopCard.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        unitName.text = unit.Name;
        GameObject frontBox = shopCard.transform.GetChild(1).GetChild(1).gameObject;
        TextMeshProUGUI levelCount = frontBox.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        levelCount.text = unit.partLevelCount.ToString() + "/" + unit.partLevelMax.ToString();
        TextMeshProUGUI energy = frontBox.transform.GetChild(3).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        energy.text = unit.Energy.ToString();
        TextMeshProUGUI attack = frontBox.transform.GetChild(4).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        attack.text = unit.Attack.ToString();
        TextMeshProUGUI health = frontBox.transform.GetChild(5).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        health.text = unit.Health.ToString();

        
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
        if (boughtUnit.GetComponent<Unit>().bought == false)
        {
            CurrentBoughtUnits.Add(new UnitSlot(slot, boughtUnit));
            CurrentShopUnits.Remove(boughtUnit);
            boughtUnit.GetComponent<Unit>().bought = true;
            calculateMoney(-UnitCosts);
        }
    }

    public void calculateMoney(int cost)
    {
        int stat = Int16.Parse(MoneyStat.GetComponent<TextMeshProUGUI>().text) + cost;
        MoneyStat.GetComponent<TextMeshProUGUI>().text = stat.ToString();
        MoneyStat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = stat.ToString();
    }

    public bool checkMoney()
    {
        int stat = Int16.Parse(MoneyStat.GetComponent<TextMeshProUGUI>().text) - UnitCosts;
        if (stat < 0)
        {
            Debug.Log("Not enough Money");
            return false;
        } else
        {
            return true;
        }
    }

    public void Sell()
    {
        if(selectedCard != null) {
            if (selectedCard.GetComponent<Unit>().bought)
            {
                var toRemove = CurrentBoughtUnits.Where(x => x.Unit == selectedCard).FirstOrDefault();
                CurrentBoughtUnits.Remove(toRemove);
                Destroy(selectedCard);
                selectedCard = null;
                calculateMoney(SellCost);
            } else
            {
                Debug.Log("Cant sell Shop Unit!");
            }
        } else
        {
            Debug.Log("Select a Unit to sell!");
        }
        
    }

    public void toBattle()
    {
        LevelManager.Instance.loadScene("Battle");
    }
}
