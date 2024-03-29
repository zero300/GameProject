using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerControlLoght : MonoBehaviour
{
    public Light upPlayerLight;

    public float expandSpeed = .1f;
    public float coolDown;
    [Header("SpotLightš€«×")]
    public float minlightAngle = 30;
    public float maxlightAngle = 150;

    [Header("SpotLightRange")]
    public float minLightRange;
    public float maxLightRange; 
    
    
    private bool isLighting;
    private float currentLightRange;
    private float currentLightAngle;
    private PlayerInput playerInput;

    void Awake()
    {
        playerInput=GetComponent<PlayerInput>();
        currentLightAngle = minlightAngle;
        currentLightRange = minLightRange;
        coolDown = (maxlightAngle - minlightAngle) / expandSpeed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.GetKeyDownLightControl() && !isLighting)
        {
            StartCoroutine(ExpandLight());
        }
    }

    IEnumerator ExpandLight()
    {
        float currentSpeed = expandSpeed;
        isLighting = true;
        while (currentLightAngle < maxlightAngle)
        {
            currentLightAngle += currentSpeed;
            upPlayerLight.spotAngle = currentLightAngle;
            currentSpeed *= 1.05f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        
        yield return new WaitForSecondsRealtime(2.0f);
        currentSpeed = expandSpeed;

        while (currentLightAngle > minlightAngle)
        {
            currentLightAngle -= currentSpeed;
            upPlayerLight.spotAngle = currentLightAngle;
            currentSpeed *= 1.1f;
            yield return new WaitForSecondsRealtime(0.01f);
        }
        isLighting = false;
        
    }
}
