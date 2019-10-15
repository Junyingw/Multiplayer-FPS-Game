using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.\n");
        }else
        {
            instance = this;
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null)
            return;
        sceneCamera.SetActive(isActive);
     }


    #region Player tracking

    private const string PLAYRT_ID = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player)
    {
        string _playerID = PLAYRT_ID + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));

    //    GUILayout.BeginVertical();

    //    foreach (string _playerID in players.Keys)
    //    {
    //        GUILayout.Label(_playerID + "-" + players[_playerID].transform.name);
    //    }

    //    GUILayout.EndVertical();

    //    GUILayout.EndArea();
    //}
    #endregion
}

