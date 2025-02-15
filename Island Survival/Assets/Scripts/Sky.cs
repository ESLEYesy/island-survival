using UnityEngine;
using System.Collections;

// This script is from the Unity 5 Tutorial - Realtime Global Illumination, Day/Night Cycle, Reflection Probes
public class Sky : MonoBehaviour {

    public Gradient nightDayColor;

    public float maxIntensity = 1f;
    public float minIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;


    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public float dayAtmosphereThickness = 1.2f;
    public float nightAtmosphereThickness = 1.4f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;

    public float skySpeed = 1;

    public float tRange;
    public float dot;
    public float i;

    Light mainLight;
    Skybox sky;
    Material skyMat;

    void Start ()
    {

        mainLight = GetComponent<Light>();
        skyMat = RenderSettings.skybox;
    }

    void Update ()
    {

        tRange = 1 - minPoint;
        dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minPoint) / tRange);
        i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        mainLight.intensity = i;

        tRange = 1 - minAmbientPoint;
        dot = Mathf.Clamp01 ((Vector3.Dot (mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        RenderSettings.ambientIntensity = i;

        mainLight.color = nightDayColor.Evaluate(dot);
        RenderSettings.ambientLight = mainLight.color;

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat ("_AtmosphereThickness", i);

        if (dot > 0)
        {
          transform.Rotate (dayRotateSpeed * Time.deltaTime * skySpeed);
        }

        else
        {
          transform.Rotate (nightRotateSpeed * Time.deltaTime * skySpeed);
        }

        //if (Input.GetKeyDown (KeyCode.Q)) skySpeed *= 0.5f;
        //if (Input.GetKeyDown (KeyCode.E)) skySpeed *= 2f;
    }
}
