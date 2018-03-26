using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	void Start () 
    {
        Init();
        CF.ConnectServer();

        UIRoot.ShowPage(Page.GameMain);
	}
	
	void Update () 
    {
		
	}

    private void Init()
    {
        ConfigeManager.Instance.Init();
        NetworkManager.Instance.Init();
    }

    void OnApplicationQuit()
    {
        NetworkManager.Instance.Disconnect();
    }  
}
