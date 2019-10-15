using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour
{

    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;


    [SyncVar]
    public string username = "Loading...";


    public int kills;
    public int deaths;


    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath;


    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject respawnEffect;

    private bool firstSetup = true;

    public float GetHealthPct()
    {
        return (float)currentHealth / maxHealth;
    }


    public void SetupPlayer()
    {
        if(isLocalPlayer)
        {
            //Switch camera
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
       
        SetDefaults();
    }


    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        // Enable the components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }


        // Disable game objects 
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }


        // Enable the colliders
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }

        // switch camera
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
        }

        // create spwan effect. It does not work????
        GameObject respawnPlayer = (GameObject)Instantiate(respawnEffect, transform.position, Quaternion.identity);

        Destroy(respawnPlayer, 10f);

    }

    //void Update()
    //{
    //    if (!isLocalPlayer)
    //        return;
    //    if(Input.GetKeyDown(KeyCode.K))
    //    {
    //        RpcTakeDamage(9999999);
    //    }
    //}

    [ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourceID)
    {
        if (isDead)
            return;

        currentHealth -= _amount;

        Debug.Log(transform.name + "now has" + currentHealth + "health\n");
        if (currentHealth <= 0)
        {

            Die(_sourceID);
        }
    }

    

    private void Die(string _sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);

        if(sourcePlayer!=null)
        {
            sourcePlayer.kills++;
        }

        GameManager.instance.onPlayerKilledCallback.Invoke(username,sourcePlayer.username);

        deaths++;

        // Disable components 
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        // Disable game objects 
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        // Disable the collider
        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = false;
        }

        // deathEffect does not work????????????? WHY
        deathEffect.GetComponent<ParticleSystem>().Play();

        // Spawn a death collider
        GameObject deathPlayer = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(deathPlayer, 3f);

        // switch camera
        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }

        Debug.Log(transform.name + "is dead!\n");

        StartCoroutine(Respawn());

        //call respawn method
    }


    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);


        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        ////Switch camera
        //GameManager.instance.SetSceneCameraActive(false);
        //GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);

        SetupPlayer();

        Debug.Log(transform.name + "respawned!\n");
    }

}
