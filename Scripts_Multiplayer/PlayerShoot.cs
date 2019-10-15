using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";
    private PlayerWeapon currentweapon;

    [SerializeField]
    private GameObject GetWeapon;

    [SerializeField]
    private GameObject GetHitEffect;


    [SerializeField]
    private PlayerWeapon weapon;

    //[SerializeField]
    //private GameObject weaponGFX;

    //[SerializeField]
    //private const string weaponLayerName = "Weapon";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    // Use this for initialization
    void Start()
    {
        if (cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!\n");
            this.enabled = false;
        }

        if (GetWeapon == null)
        {
            Debug.LogError("Donot get the weapon!\n");
           
        }

       // weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName);

    }
    void Update()
    {
        currentweapon = weapon;

        if (PauseMenu.IsOn)
            return;

        if(currentweapon.fireRate <= 0f)
        {

            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
            if (Input.GetButtonUp("Fire1"))
            {
                GetWeapon.GetComponent<ParticleSystem>().Stop();
            }
        }
        else
        {
           
            if (Input.GetButtonDown("Fire1"))
            {
                // it does not work now WHY?????????
                InvokeRepeating("Shoot", 0f, 1f / currentweapon.fireRate);
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
                GetWeapon.GetComponent<ParticleSystem>().Stop();
            }
        }

    }

    // is called on the server when a player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    // is called on all clients when we need to do a shoot effect
    [ClientRpc]
    void RpcDoShootEffect()
    {
        if (GetWeapon != null)
        {
            GetWeapon.GetComponent<ParticleSystem>().Play();
        }
    }

    // Is called on the server when we hit something
    // Take in the hit point and the normal of the surface

    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    // is called on all clients, we can spawn in cool effects
    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = (GameObject)Instantiate(GetHitEffect, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2f);
    }


    [Client]
    void Shoot()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        // We are shooting,call the OnShoot method on the server
        CmdOnShoot();

        //Debug.Log("Shoot!!!!\n");
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
             if (hit.collider.tag == PLAYER_TAG)
             {
                CmdPlayerShot(hit.collider.name, weapon.damage, transform.name);
             }
            // Debug.Log("we hit!\n");
            //  Debug.Log("we hit" + hit.collider.name +"\n");

            // We hit something, call the OnHit method on the server
            CmdOnHit(hit.point, hit.normal);
        }
    }

    [Command]
    void CmdPlayerShot(string _playerID, int _damage, string _sourceID)
    {
        Debug.Log(_playerID + "has been shot.\n");


        Player _player = GameManager.GetPlayer(_playerID);
        _player.RpcTakeDamage(_damage, _sourceID);
    }
}
