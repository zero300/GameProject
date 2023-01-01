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
    //TODO �i��ݭn�P�_�C���O�_�Ȱ��ε��� 
    /// <summary>
    /// �^�ǬO�_�O��wcursor��
    /// </summary>
    /// <returns></returns>
    public bool CursorIsLock()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }
    /// <summary>
    /// ��^�ƹ���m�A��^�Ȧ��ù��Ŷ�
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
    /// ��^���ʪ���V
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
    /// ��^�O�_���U���D��
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
    /// ��^�O�_���U���D
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
    /// �ƹ��O�_���k�
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
    /// �ƹ��O�_�W�U�
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
    /// �O�_��������۩b�]��
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
    /// ����TAb����
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
    /// �椬����
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
    /// �����ϥ�(�O�����X�j)
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
