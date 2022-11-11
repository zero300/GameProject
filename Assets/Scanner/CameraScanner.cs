using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScanner : MonoBehaviour
{
    
    public float coolDownOffset = 2.0f;
    public Transform player;
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
    private float coolDown;
    private float coolDownCount;

    [Header("邊緣檢測")]
    public Material edgeDetectMaterial;
    [Range(0.0f, 1.0f)]
    public float edgesOnly = 0.0f;
    
    public Color edgeColor = Color.black;
    public Color backgroundColor = Color.white;
    public float sampleDistance = 1.0f;
    public float sensitivityDepth = 1.0f;
    public float sensitivityNormals = 1.0f;

    private RenderTexture edgeTexture;
    private float maxDistance ;
    private float currentDistance;
    private void OnEnable()
    {
        if (startScanRange > maxScanRange)
            throw new Exception("CameraScanner.cs : startScanRange 不能大於 maxScanRange");
        if(scanMat == null || edgeDetectMaterial == null)
            throw new Exception("CameraScanner.cs : scanMat 或 edgeDetectMaterial 不能為 null ");

        scanRadius = startScanRange;
        Camera.main.depthTextureMode = DepthTextureMode.DepthNormals;
        currentDistance = startScanRange;
        maxDistance = maxScanRange;
        coolDown = 10;
    }

    void Update()
    {
        if(coolDownCount >= 0)
        {
            coolDownCount -= Time.deltaTime;

        }
        if (Input.GetKeyDown(KeyCode.J) && coolDownCount <= 0)
        {
            coolDownCount = coolDown;
            centerPos = player.position;
            if(scanRadius <= startScanRange)
            {
                Scan();
            }
        }
    }
    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source , RenderTexture destination)
    {
        edgeTexture = source;
        if (edgeDetectMaterial != null)
        {
            edgeDetectMaterial.SetVector("_ScanCenterPos", player.position);
            edgeDetectMaterial.SetFloat("_EdgeOnly", edgesOnly);
            edgeDetectMaterial.SetColor("_EdgeColor", edgeColor);
            edgeDetectMaterial.SetColor("_BackgroundColor", backgroundColor);
            edgeDetectMaterial.SetFloat("_SampleDistance", sampleDistance);
            edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(sensitivityNormals, sensitivityDepth, 0.0f, 0.0f));
            edgeDetectMaterial.SetFloat("_MaxDistance", currentDistance);
            RaycastCornerBlit(source, edgeTexture, edgeDetectMaterial);
        }
        if (scanMat != null && isInScan)
        {
            scanMat.SetVector("_ScanCenterPos", centerPos);
            scanMat.SetFloat("_ScanRadius", scanRadius);
            scanMat.SetFloat("_ScanWidth", scanWidth);
            scanMat.SetColor("_HeadColor", headColor);
            scanMat.SetColor("_TrailColor", trailColor);

            RaycastCornerBlit(edgeTexture, destination, scanMat);   
        }
        else
        {
            Graphics.Blit(source, destination);
        }

    }

    // Scan
    void RaycastCornerBlit(RenderTexture source , RenderTexture dest , Material mat)
    {
        float CameraFar = Camera.main.farClipPlane;
        float CameraFov = Camera.main.fieldOfView;
        float CameraAspect = Camera.main.aspect;

        float fovWHalf = CameraFov * 0.5f;

        Vector3 toRight = Camera.main.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * CameraAspect;
        Vector3 toTop = Camera.main.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) ;

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
        scanHandler = ScanCoroutine() ;
        StartCoroutine(scanHandler);
    }
    private IEnumerator ScanCoroutine()
    {

        isInScan = true;
        scanRadius = startScanRange;
        while (scanRadius < maxScanRange)
        {
            scanRadius += scanSpeed;
            currentDistance += scanSpeed;
            yield return new WaitForSecondsRealtime(.01f);
        }
        isInScan = false;
        scanRadius = startScanRange;
        StartCoroutine(EdgeFade());
    }
    private IEnumerator EdgeFade()
    {
        yield return new WaitForSecondsRealtime(coolDownOffset);
        while (currentDistance > startScanRange)
        {
            currentDistance -= scanSpeed;
            yield return new WaitForSecondsRealtime(.01f);
        }
        currentDistance = startScanRange;
    }
}
