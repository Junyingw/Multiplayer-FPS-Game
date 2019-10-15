using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerAnimController: MonoBehaviour {
    public Animator anim;
    PlayerMotor pm;

    //Gun aim
    [Header("Gun Position")]
    public Transform targetDirection;
    public Vector3 boneOffset;
    public Transform targetBone;

	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animator>();
        pm = GetComponent<PlayerMotor>();
	}
	
	// Update is called once per frame
	void Update () {
        if (PauseMenu.IsOn)
            return;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        if(pm.speed == pm.runSpeed){
            x = Mathf.Clamp(x, -1, 1);
            y = Mathf.Clamp(y, -1, 1);
        }
        else{
            x = Mathf.Clamp(x, -0.5f, 0.5f);
            y = Mathf.Clamp(y, -0.5f, 0.5f);
        }
        Move(x, y);

        //Aiming using keycode--"Space"
        //if(Input.GetKeyDown(KeyCode.Space)){
        //    //Debug.Log("Mouse Pressed");
        //    anim.SetBool("isAiming", true);
        //    anim.SetLayerWeight(1, 1);
        //}
        //if(Input.GetKeyUp(KeyCode.Space))
        //{
        //    anim.SetBool("isAiming", false);
        //    anim.SetLayerWeight(1, 0);
        //}
	}
    private void LateUpdate()
    {
        //Gun Aim 
        targetBone.LookAt(targetDirection);
        targetBone.rotation = targetBone.rotation * Quaternion.Euler(boneOffset);
    }
    private void Move(float x, float y)
    {
        anim.SetFloat("Direction", x);
        anim.SetFloat("Speed", y);
    }
}
