using System;
using System.Collections;
using UnityEngine;

public class OnlyScan : MonoBehaviour
{

    public float coolDownOffset = 2.0f;
    public Transform player;
    private PlayerInput playerInput;
    [Header("全局掃描")]
    public Material scanMat;
    public float startScanRange = 0;
    public float maxScanRange = 20;
    public float scanWidth = 3;
    public float scanSpeed = 1;
    public Color headColor;
    public Color trailColor;

    private bool isInScan = false;
    private Vector3 centerPos;
    private float scanRadius;
    private IEnumerator scanHandler = null;
    private const float increaseFixedNumber = 2;
    private void OnEnable()
    {
        if (startScanRange > maxScanRange)
            throw new Exception("CameraScanner.cs : startScanRange 不能大於 maxScanRange");
        if (scanMat == null )
            throw new Exception("CameraScanner.cs : scanMat 不能為 null ");

        scanRadius = startScanRange;
        Camera.main.depthTextureMode = DepthTextureMode.DepthNormals;
        playerInput = player.GetComponent<PlayerInput>();
        EventManager.AddEvents<UpdateEvent>(IncreaseRadius);
    }
    void Update()
    {
        if (playerInput.GetKeyDownLightControl() && !isInScan)
        {
            centerPos = player.position;
            if (scanRadius <= startScanRange)
            {
                Scan();
            }
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (scanMat != null && isInScan)
        {
            scanMat.SetVector("_ScanCenterPos", centerPos);
            scanMat.SetFloat("_ScanRadius", scanRadius);
            scanMat.SetFloat("_ScanWidth", scanWidth);
            scanMat.SetColor("_HeadColor", headColor);
            scanMat.SetColor("_TrailColor", trailColor);

            RaycastCornerBlit(source, destination, scanMat);
        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }

    // Scan
    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        float CameraFar = Camera.main.farClipPlane;
        float CameraFov = Camera.main.fieldOfView;
        float CameraAspect = Camera.main.aspect;

        float fovWHalf = CameraFov * 0.5f;

        Vector3 toRight = Camera.main.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * CameraAspect;
        Vector3 toTop = Camera.main.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = Camera.main.transform.forward - toRight + toTop;
        float CameraScale = topLeft.magnitude * CameraFar;

        topLeft.Normalize();
        topLeft *= CameraScale;

        Vector3 topRight = Camera.main.transform.forward + toRight + toTop;
        topRight.Normalize();
        topRight *= CameraScale;

        Vector3 bottomLeft = Camera.main.transform.forward - toRight - toTop;
        bottomLeft.Normalize();
        bottomLeft *= CameraScale;

        Vector3 bottomRight = Camera.main.transform.forward + toRight - toTop;
        bottomRight.Normalize();
        bottomRight *= CameraScale;

        RenderTexture.active = dest;
        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }
    void CheckAndBlock()
    {
        if (scanHandler != null)
            StopCoroutine(scanHandler);
    }
    void Scan()
    {
        CheckAndBlock();
        scanHandler = ScanCoroutine();
        StartCoroutine(scanHandler);
    }
    private IEnumerator ScanCoroutine()
    {

        isInScan = true;
        scanRadius = startScanRange;
        while (scanRadius < maxScanRange)
        {
            scanRadius += scanSpeed;
            yield return new WaitForSecondsRealtime(.01f);
        }
        isInScan = false;
        scanRadius = startScanRange;
    }

    /// <summary>
    /// 提升範圍
    /// </summary>
    /// <param name="evt"></param>
    public void IncreaseRadius(GameEvent evt)
    {
        if (evt is not UpdateEvent) return;
        if ((evt as UpdateEvent).updateType != UpdateType.ScanRange) return;
        Debug.Log("提升探測距離");
        //TODO : 可以播放個 升級動畫 聲音之類的
        maxScanRange += increaseFixedNumber;
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<UpdateEvent>(IncreaseRadius);
    }
}
