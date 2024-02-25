using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager Main;

    [SerializeField]
    private CinemachineVirtualCamera camera;

    [SerializeField]
    private Transform water;

    [SerializeField]
    private ParticleSystem speedStripes;

    [SerializeField]
    private ParticleSystem speedStars;

    [SerializeField]
    private ParticleSystem splash;

    [SerializeField]
    private ParticleSystem clouds;

    [SerializeField]
    private ParticleSystem stars;
    [SerializeField]
    private ParticleSystem asteroids;

    [SerializeField]
    private IslandLauncher island;

    private CinemachineBasicMultiChannelPerlin cameraNoise;
    private CinemachineDollyCart cameraDolly;

    private double currentHeight = 0.0;
    private double targetHeight = 0.0;
    private float lerpStarted = 0.0f;
    private double lerpStartHeight = 0.0;
    private float heightLerpDuration = 1.0f;


    private float rumbleTime = 0.0f;
    private float rumbleDuration = 1.0f;
    private float rumbleIntensity = 3.0f;

    private float origWaterLevel;

    [SerializeField]
    private Material skyboxMaterial;

    [SerializeField]
    private List<SkyboxHeightThreshold> skyboxHeightThresholds;

    private Material waterMaterial;

    [SerializeField]
    private Volume volume;

    private VolumeProfile profile;

    private Bloom bloom;

    [SerializeField]
    private AudioSource music;

    void Awake() {
        Main = this;
    }

    private double testHeight = 500000;

    // Start is called before the first frame update
    void Start()
    {
        cameraNoise = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cameraDolly = camera.GetComponent<CinemachineDollyCart>();
        origWaterLevel = water.transform.position.y;
        waterMaterial = water.GetComponent<Renderer>().material;
        clouds.Stop();
        speedStripes.Stop();
        speedStars.Stop();
        stars.Stop();

        profile = volume.profile;
        if (!profile) throw new NullReferenceException(nameof(VolumeProfile));
        if (!profile.TryGet(out bloom)) throw new NullReferenceException(nameof(bloom));
        bloom.intensity.Override(0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //testHeight = testHeight + Time.deltaTime * 10000000.0;
        //SetHeight(testHeight);

        var heightT = Math.Clamp((Time.time - lerpStarted) / heightLerpDuration, 0.0f, 1.0f);
        var heightDifference = targetHeight - lerpStartHeight;
        currentHeight = lerpStartHeight + heightT * heightDifference;

        if (rumbleTime > Time.time) {
            var t = (rumbleTime - Time.time) / rumbleDuration;
            cameraNoise.m_AmplitudeGain = rumbleIntensity * t;
            cameraNoise.m_FrequencyGain = rumbleIntensity * t;
            if (currentHeight < 12) {
                splash.Play();
            }
        } else {
            cameraNoise.m_AmplitudeGain = 0.0f;
            cameraNoise.m_FrequencyGain = 0.0f;
            splash.Stop();
        }

        handleSky();

        var targetWaterLevel = -currentHeight + 1;
        if (targetWaterLevel > 0) targetWaterLevel = 0;
        if (targetWaterLevel < -2000) targetWaterLevel = -2000;
        water.transform.position = new Vector3(water.transform.position.x, origWaterLevel + (float)targetWaterLevel, water.transform.position.z);

        var cameraOrbitSpeed = (currentHeight - 5) * 5;
        if (cameraOrbitSpeed < 0) cameraOrbitSpeed = 0;
        if (cameraOrbitSpeed > 10) cameraOrbitSpeed = 10;
        cameraDolly.m_Speed = (float)cameraOrbitSpeed;

        if (cameraOrbitSpeed > 0 && !music.isPlaying) {
            music.Play();
        }

        if (targetHeight != nextTargetHeight) {
            targetHeight = nextTargetHeight;
            lerpStarted = Time.time;
            lerpStartHeight = currentHeight;
        }
    }

    private double nextTargetHeight;

    public void SetHeight(double height) {
        if (height == nextTargetHeight) {
            return;
        }
        if (height < 12) {
            Rumble();
        }
        nextTargetHeight = height;
    }

    public void Rumble() {
        if (rumbleTime > Time.time) return;
        rumbleTime = Time.time + rumbleDuration;
    }

    private void handleSky() {
        var i = nextSkyboxThreshold();
        if (i >= skyboxHeightThresholds.Count) {
            var config = skyboxHeightThresholds.Last();
            configureSkyBox(config.SkyColor, config.GroundColor, config.Thickness, config.Exposure, config.WaterAlpha);
            island.Launch();
            enableBloom();
            return;
        }

        var currentConfig = skyboxHeightThresholds[i - 1];
        var nextConfig = skyboxHeightThresholds[i];
        float t = (float)(currentHeight - currentConfig.Threshold) / (float)(nextConfig.Threshold - currentConfig.Threshold);
        var skyColor = Color.Lerp(currentConfig.SkyColor, nextConfig.SkyColor, t);
        var groundColor = Color.Lerp(currentConfig.GroundColor, nextConfig.GroundColor, t);
        var thickness = Mathf.Lerp(currentConfig.Thickness, nextConfig.Thickness, t);
        var exposure = Mathf.Lerp(currentConfig.Exposure, nextConfig.Exposure, t);
        var waterAlpha = Mathf.Lerp(currentConfig.WaterAlpha, nextConfig.WaterAlpha, t);
        configureSkyBox(skyColor, groundColor, thickness, exposure, waterAlpha);

        if (currentConfig.SpawnClouds) {
            if (clouds.isStopped) {
                clouds.Play();
            }
        } else {
            clouds.Stop();
        }
        
        if (currentConfig.ShowSpeedStripes) {
            if (speedStripes.isStopped) {
                speedStripes.Play();
            }
        } else {
            speedStripes.Stop();
        }
        
        if (currentConfig.SpawnSpeedStars) {
            if (speedStars.isStopped) {
                speedStars.Play();
            }
        } else {
            speedStars.Stop();
        }

        if (currentConfig.ShowStars) {
            if (stars.isStopped) {
                stars.Play();
            }
        } else {
            stars.Stop();
        }

        if (currentConfig.ShowAsteroids) {
            if (asteroids.isStopped) {
                asteroids.Play();
            }
        } else {
            asteroids.Stop();
        }

        if (currentConfig.EnableObject != null) {
            currentConfig.EnableObject.gameObject.SetActive(true);
            currentConfig.EnableObject.SetPosition(t);
        }

        if (rumbleTime < Time.time) {
            var rumbleAmount = Mathf.Lerp(currentConfig.Rumble, nextConfig.Rumble, t);
            if (currentConfig.Rumble < 0.001) {
                rumbleAmount = 0.0f;
            }
            cameraNoise.m_AmplitudeGain = rumbleAmount;
            cameraNoise.m_FrequencyGain = rumbleAmount;
        }
    }

    private int nextSkyboxThreshold() {
        var i = 0;
        while(i < skyboxHeightThresholds.Count) {
            if (currentHeight < skyboxHeightThresholds[i].Threshold) {
                return i;
            }
            i++;
        }
        return i;
    }

    private void configureSkyBox(Color skyColor, Color groundColor, float thickness, float exposure, float waterAlpha) {
        skyboxMaterial.SetColor("_SkyTint", skyColor);
        skyboxMaterial.SetColor("_GroundColor", groundColor);
        skyboxMaterial.SetFloat("_AtmosphereThickness", thickness);
        skyboxMaterial.SetFloat("_Exposure", exposure);
        waterMaterial.SetFloat("_Alpha", waterAlpha);
    }
    
    private void enableBloom() {
        bloom.intensity.Override(20.0f);
    }
}

[Serializable]
public struct SkyboxHeightThreshold {
    public float Threshold;
    public Color SkyColor;
    public Color GroundColor;
    public float Thickness;
    public float Exposure;
    public float WaterAlpha;
    public bool SpawnClouds;
    public bool ShowSpeedStripes;
    public bool SpawnSpeedStars;
    public bool ShowStars;
    public bool ShowAsteroids;
    public Mover EnableObject;
    public float Rumble;
}
