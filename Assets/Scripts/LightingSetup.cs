using UnityEngine;

// 밝은 낮 학원물 조명: 파란하늘+따뜻한 태양+부드러운 그림자 (URP)
public class LightingSetup : MonoBehaviour
{
    [ContextMenu("Apply Day Look")]
    public void Apply()
    {
        RenderSettings.skybox = null;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
        RenderSettings.ambientIntensity = 1.0f;
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.75f, 0.85f, 0.95f);
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.0025f;

        var sun = FindObjectOfType<Light>();
        if (!sun) sun = new GameObject("Sun_Day").AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.5f;
        sun.color = new Color(1f, 0.96f, 0.88f);
        sun.shadowStrength = 0.7f;
        sun.transform.rotation = Quaternion.Euler(50, -30, 0);

        var cam = Camera.main;
        if (cam && !cam.GetComponent<AudioListener>()) cam.gameObject.AddComponent<AudioListener>();
        Debug.Log("낮 룩 적용.");
    }
}
