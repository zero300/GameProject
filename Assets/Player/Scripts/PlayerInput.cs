using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    //TODO 可能需要判斷遊戲是否暫停或結束 
    /// <summary>
    /// 回傳是否是鎖定cursor的
    /// </summary>
    /// <returns></returns>
    public bool CursorIsLock()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
    /// <summary>
    /// 返回滑鼠位置，返回值位於螢幕空間
    /// </summary>
    /// <returns></returns>
    public Vector3? GetMousePosition()
    {
        if (CursorIsLock())
        {
            return Input.mousePosition;
        }
        return null;
    }
    /// <summary>
    /// 返回移動的方向
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMove()
    {
        if (CursorIsLock())
        {
            Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            return Vector3.ClampMagnitude(direction, 1);
        }
        return Vector3.zero;
    }
    /// <summary>
    /// 返回是否按下跳躍鍵
    /// </summary>
    /// <returns></returns>
    public bool GetKeyDownJump()
    {

        if (CursorIsLock())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 返回是否按下跳躍
    /// </summary>
    /// <returns></returns>
    public bool GetKeyHeldJump()
    {
        if (CursorIsLock())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 滑鼠是否左右橫移
    /// </summary>
    /// <returns></returns>
    public float GetMouseX()
    {
        if (CursorIsLock())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }
    /// <summary>
    /// 滑鼠是否上下橫移
    /// </summary>
    /// <returns></returns>
    public float GetMouseY()
    {
        if (CursorIsLock())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }
    /// <summary>
    /// 是否有持續按著奔跑鍵
    /// </summary>
    /// <returns></returns>
    public bool GetKeyHeldRun()
    {
        if (CursorIsLock())
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 先用TAb按鍵
    /// </summary>
    /// <returns></returns>
    public bool GetKeyDownDash()
    {
        if (CursorIsLock())
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 交互按鍵
    /// </summary>
    /// <returns></returns>
    public bool GetKeyDownInteract()
    {
        if (CursorIsLock())
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 探測使用(燈光的擴大)
    /// </summary>
    /// <returns></returns>
    public bool GetKeyDownLightControl() {
        {
            if (CursorIsLock())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
