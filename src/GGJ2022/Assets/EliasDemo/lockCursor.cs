using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockCursor : MonoBehaviour
{
    void OnGUI()
    {
        //Press this button to lock the Cursor
        if (GUI.Button(new Rect(0, 0, 250, 50), "Lock Cursor - 'esc' to get cursor back"))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }
}
