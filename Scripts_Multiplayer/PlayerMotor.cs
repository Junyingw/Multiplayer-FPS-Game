using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {
    [Header("KeyCode Binds")]
    public KeyCode _sprint;


    [SerializeField]
    private Camera tempCam;

    [Header("Speed Setting")]
    public float walkSpeed = 3;
    public float runSpeed = 6;
    [HideInInspector]
    public float speed;

    [Header("Rotation Settings")]
    public float mouseSensitivity =100;
    private float mouseX, mouseY;
    private Transform cam;
    public Vector2 lookMinMax = new Vector2(-60,60);

  
    void Start()
    {
       cam = tempCam.transform;

      //Remove later
     /*   Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    void Update()
    {
        //if (PauseMenu.IsOn)
        //{
        //    if (Cursor.lockState != CursorLockMode.None)
        //    {
        //        Cursor.lockState = CursorLockMode.None;
        //    }
        //    return;
        //}
            

        //if(Cursor.lockState!=CursorLockMode.Locked)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}

        Actions();
        Locomotion();
        Rotation();
    }
    void Actions()
    {
        #region Sprint
        if (Input.GetKey(_sprint))
            speed = runSpeed;
        else
            speed = walkSpeed;
        #endregion
    }
    void Locomotion()
    {
        var x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(x, 0, z);
    }
    void Rotation()
    {
         mouseY += Input.GetAxis("MouseX") * mouseSensitivity * Time.deltaTime;
         mouseX -= Input.GetAxis("MouseY") * mouseSensitivity * Time.deltaTime;
         mouseX = Mathf.Clamp(mouseX, lookMinMax.x, lookMinMax.y);

         transform.rotation = Quaternion.Euler(0, mouseY, 0);
         cam.rotation = Quaternion.Euler(mouseX, mouseY, 0);
    }
}
