using UnityEngine;

public class Mouse : MonoBehaviour
{
    private void Start(){
        LockCursor();
    }
    
    void Update(){
        if (Input.GetKey(KeyCode.B))
        {
            UnlockCursor();
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            LockCursor();
        }
    }

    private void UnlockCursor(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LockCursor(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}