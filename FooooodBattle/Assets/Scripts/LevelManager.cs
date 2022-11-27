using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    void Awake(){
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else{
            Destroy(gameObject);
        }
    }

    public void loadScene(string SceneName){
        SceneManager.LoadScene(SceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode){
        SceneManager.SetActiveScene(scene);
        //NetworkManager.newArenaSession(ArenaManager.Instance.CurrentShopUnits, ArenaManager.Instance.CurrentBoughtUnits);
    }

    
}
