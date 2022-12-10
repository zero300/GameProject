using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class FirstCM : MonoBehaviour
{
    CinemachineRecomposer recomposer;
    public bool X_Invert;
    public bool Y_Invert;
    public float mouse_sensitive;
    private void Awake()
    {
        recomposer = GetComponent<CinemachineRecomposer>();
    }
    private void LateUpdate()
    {
        CameraContorol();
    }
    public void CameraContorol()
    {
        //������w�~���
        if (Cursor.lockState != CursorLockMode.Locked) return;
        //��V
        float Angle_hor = -Input.GetAxis("Mouse X");
        if(X_Invert) recomposer.m_Pan -= Angle_hor * mouse_sensitive;
        else recomposer.m_Pan += Angle_hor * mouse_sensitive;
        if (recomposer.m_Pan < -180 || recomposer.m_Pan > 180)
            recomposer.m_Pan = -recomposer.m_Pan;
        recomposer.m_Pan = Mathf.Clamp(recomposer.m_Pan, -180, 180);
        //����
        float Angle_ver = -Input.GetAxis("Mouse Y");
        if(Y_Invert) recomposer.m_Tilt -= Angle_ver * mouse_sensitive;
        else recomposer.m_Tilt += Angle_ver * mouse_sensitive;
        recomposer.m_Tilt = Mathf.Clamp(recomposer.m_Tilt, -40f, 89.9f);
    }
}
