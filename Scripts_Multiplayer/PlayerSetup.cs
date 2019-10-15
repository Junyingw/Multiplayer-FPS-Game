
using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent(typeof(Player))]

public class PlayerSetup : NetworkBehaviour
{
    //[SerializeField]
    //Behaviour[] componentsToDisable;
    //void Start()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        for (int i = 0; i < componentsToDisable.Length; i++)
    //        {
    //            componentsToDisable[i].enabled = false;
    //        }
    //    }
    //}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    //[SerializeField]
    //string DonotDrawName = "DoNotDraw";

    //[SerializeField]
    //GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPrefab;
    public GameObject playerUIInstance;


    void Start()
    {
        //Disable components that should only be active on the player that we control
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();

            /*  for (int i = 0; i < componentsToDisable.Length; i++){
                  componentsToDisable[i].enabled = false;
              }*/
        }
        else
        {
            //we are the local palyer, disable the scence camera
            //sceneCamera = Camera.main;
            //if (sceneCamera != null)
            //{
            //    sceneCamera.gameObject.SetActive(false);
            //}
            // Disable player graphics for local player
            // SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(DonotDrawName));

            //Create PlayerUI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            // Configure PlayerUI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
                Debug.LogError("No PlayerUI component on PlayerUI prefab");
            ui.SetPlayer(GetComponent<Player>());

            GetComponent<Player>().SetupPlayer();

            string _username = "Loading...";

            if (UserAccountManager.IsLoggedIn)
                _username = UserAccountManager.playerUsername;
            else
                _username = transform.name;

            CmdSetUsername(transform.name,_username);

        }
    }

    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        Player player = GameManager.GetPlayer(playerID);
        if(player != null)
        {
            Debug.Log(username + " has joined!");
            player.username = username;
        }
    }

    //void SetLayerRecursively(GameObject obj, int newLayer)
    //{
    //    obj.layer = newLayer;

    //    foreach(Transform child in obj.transform)
    //    {
    //        SetLayerRecursively(child.gameObject, newLayer);
          
    //    }
    //}


    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    void RegisterPlayer()
    {
        string _ID = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    //When we are destroyed 
    void OnDisable()
    {
        Destroy(playerUIInstance);
        if(isLocalPlayer)
        GameManager.instance.SetSceneCameraActive(true);

        // Re enable the scene camera
        //if (sceneCamera != null)
        //{
        //    sceneCamera.gameObject.SetActive(true);
        //}

        GameManager.UnRegisterPlayer(transform.name);
    }

}
