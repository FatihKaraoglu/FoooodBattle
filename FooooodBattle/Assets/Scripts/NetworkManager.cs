using Assets.Scripts;
using SharedLibrary.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
}
