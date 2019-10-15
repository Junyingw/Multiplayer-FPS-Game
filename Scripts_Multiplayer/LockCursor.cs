using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursor : MonoBehaviour
{

    public bool isLock;
    // Use this for initialization
    void Start()
    {
        LockState();
    }

    public void LockState()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isLock = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isLock = true;
        }


        if (isLock)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (!isLock)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
