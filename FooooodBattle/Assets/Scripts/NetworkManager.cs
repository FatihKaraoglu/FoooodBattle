using Assets.Scripts;
using SharedLibrary.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SharedLibrary;

public class NetworkManager : MonoBehaviour
{

    [SerializeField]
    public TMP_InputField Username;
    [SerializeField]
    public TMP_InputField Password;
    [SerializeField]
    public TMP_InputField Email;
    [SerializeField]
    public GameObject loginCanvas;
    // Start is called before the first frame update
    public static NetworkManager Instance;
    public static string Token;
    public static string User;

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
    }

    public static async void Register()
    {
        string username = NetworkManager.Instance.Username.text; 
        string password = NetworkManager.Instance.Password.text;
        string email = NetworkManager.Instance.Email.text; 
        var RegisterDTO = new RegisterDTO()
        {
            Username = username,
            Password = password,
            Email = email,
        };
        var player = await HttpClient.Post<UserDTO>("https://localhost:7125/api/account/register", RegisterDTO);
        Token = player.Token;
        User = player.Username;
        checkLogin();
        Debug.Log(player.Username + " Your Token = " + player.Token);
    }

    public static async void Login()
    {
        string username = NetworkManager.Instance.Username.text;
        string password = NetworkManager.Instance.Password.text;
        var loginDTO = new LoginDTO() { Username = username, Password = password };
        var player = await HttpClient.Post<UserDTO>("https://localhost:7125/api/account/login", loginDTO);

        
        Token = player.Token;
        User = player.Username;
        checkLogin();
        Debug.Log(player.Username + " " + player.Token);

    }

    public static void checkLogin()
    {
        if(User != null && Token != null)
        {
            NetworkManager.Instance.loginCanvas.gameObject.SetActive(false);
        }
    }

    public static async void newArenaSession(List<GameObject> ShopUnits, List<UnitSlot> Units)
    {
        List<ShopUnit> unitsShop = new List<ShopUnit>();
        for(int i = 0; i < ShopUnits.Count; i++)
        {
            var unit = ShopUnits[i].GetComponent<Unit>();
            ShopUnit shopUnit = new ShopUnit
            {
                UnitType = unit.Id,
                Name = unit.Name,
                Attack = unit.Attack,
                Health = unit.Health,
                Energy = unit.Energy,
                partLevelCount = unit.partLevelCount,
                partLevelMax = unit.partLevelMax,
                Level = unit.Level
            };
            unitsShop.Add(shopUnit);
        }

        List<UnitObject> boughtUnits = new List<UnitObject>();
        for (int i = 0; i < Units.Count; i++)
        {
            var shopSlotUnit = Units[i].Unit;
            var unit = shopSlotUnit.GetComponent<Unit>();
            UnitObject boughtUnit = new UnitObject
            {
                UnitType = unit.Id,
                Name = unit.Name,
                Attack = unit.Attack,
                Health = unit.Health,
                Energy = unit.Energy,
                partLevelCount = unit.partLevelCount,
                partLevelMax = unit.partLevelMax,
                Level = unit.Level
            };
            boughtUnits.Add(boughtUnit);
        }

        ArenaDTO arenaDTO = new ArenaDTO
        {
            Turn = 10,
            Wins = 0,
            Health = 10,
            Money = 10,
            ShopUnits = unitsShop,
            UnitsBought = boughtUnits,
            Username = User,
            Token = Token
        };

        await HttpClient.Post<ArenaDTO>("https://localhost:7125/api/arena", arenaDTO);

    }
}
