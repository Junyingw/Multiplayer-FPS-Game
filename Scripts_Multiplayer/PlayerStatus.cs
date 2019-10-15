﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStatus : MonoBehaviour {

    public Text killCount;
    public Text deathCount;

	// Use this for initialization
	void Start () {
        if(UserAccountManager.IsLoggedIn)
            UserAccountManager.instance.GetUserData(OnReceivedData);
		
	}
	
	void OnReceivedData(string data)
    {
        killCount.text = DataTranslator.DataToKills(data).ToString() +" KILLS";
        deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
    }
}
