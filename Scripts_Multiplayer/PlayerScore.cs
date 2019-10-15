using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class PlayerScore : MonoBehaviour {

    Player player;

	// Use this for initialization
	void Start () 
    {
        player = GetComponent<Player>();
        StartCoroutine(SyncScoreLoop());
	}

    void OnDestroy()
    {
        if (player != null)
        SyncNow();
    }

    IEnumerator SyncScoreLoop()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);

            SyncNow();
           
        }
       
    }

    void SyncNow()
    {
        if (UserAccountManager.IsLoggedIn)
        {
            UserAccountManager.instance.GetUserData(OnDataRecieved);
        }
    }

    void OnDataRecieved(string data)
    {
        if (player.kills == 0 && player.deaths == 0)
            return;

        int kills = DataTranslator.DataToKills(data);
        int deaths = DataTranslator.DataToDeaths(data);

        int newKills = player.kills + kills;
        int newDeaths = player.deaths + deaths;

        string newData = DataTranslator.ValuesToData(newKills, newDeaths);

        Debug.Log("Syncing:" + newData);

        UserAccountManager.instance.SendUserData(newData); 

        player.kills = 0;
        player.deaths = 0;

    }
}
