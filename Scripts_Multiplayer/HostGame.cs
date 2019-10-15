using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private NetworkManager networkManager;

    void Start()
    {

        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

    }

    public void GetRoomName(string _name)
    {
        roomName = _name;
    }

    public void CreateRoom()
    {

        if (!string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Creating Room: " + roomName + " with room for " + roomSize + " players.");
            // Create room
            networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);

        }
     }
}

