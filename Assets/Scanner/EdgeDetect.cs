using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetect : MonoBehaviour
{

    public Transform player;
    [Header("√‰Ωt¿À¥˙")]
    public Material edgeDetectMaterial;
    [Range(0.0f, 1.0f)]
    public float edgesOnly = 0.0f;
    public float maxDistance = 20.0f;
    public Color edgeColor = Color.black;
    public Color backgroundColor = Color.white;
    public float sampleDistance = 1.0f;  
    public float sensitivityDepth = 1.0f;  
    public float sensitivityNormals = 1.0f;
    void OnEnable()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
    }

    [ImageEffectOpaque]  
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (edgeDetectMaterial != null)
        {
            edgeDetectMaterial.SetVector("_ScanCenterPos", player.position);
            edgeDetectMaterial.SetFloat("_EdgeOnly", edgesOnly);
            edgeDetectMaterial.SetColor("_EdgeColor", edgeColor);
            edgeDetectMaterial.SetColor("_BackgroundColor", backgroundColor);
            edgeDetectMaterial.SetFloat("_SampleDistance", sampleDistance);
            edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(sensitivityNormals, sensitivityDepth, 0.0f, 0.0f));
            edgeDetectMaterial.SetFloat("_MaxDistance", maxDistance);

            RaycastCornerBlit(src, dest, edgeDetectMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
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
}
