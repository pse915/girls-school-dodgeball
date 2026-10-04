using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

// 이미지 하복 타탄체크를 코드로 굽기: 네이비 바탕 + 버건디 굵은밴드 + 흰/하늘 가는선
public static class SummerCheckTextureGenerator
{
    const int S = 512;

    public static Texture2D GetOrCreate(UniformColors p)
    {
        if (!p) return null;
        var tex = new Texture2D(S, S, TextureFormat.RGBA32, true);
        tex.wrapMode = TextureWrapMode.Repeat;

        for (int y = 0; y < S; y++)
            for (int x = 0; x < S; x++)
            {
                float u = (float)x / S * 4f; // 4칸 반복
                float v = (float)y / S * 4f;
                tex.SetPixel(x, y, Tartan(u, v, p));
            }
        tex.Apply();
        return tex;
    }

    static Color Tartan(float u, float v, UniformColors p)
    {
        // 기본 네이비
        Color c = p.navyBase;
        // 버건디 굵은 밴드 (주기 1.0, 폭 0.28)
        bool bx = Band(u, 0.28f), by = Band(v, 0.28f);
        bool bxDark = Band(u + 0.5f, 0.12f), byDark = Band(v + 0.5f, 0.12f);
        if (bx || by) c = p.redBand;
        if (bxDark || byDark) c = Color.Lerp(c, p.darkRed, 0.6f);
        if (bx && by) c = Color.Lerp(c, p.redBand * 1.15f, 0.5f); // 교차점 밝게
        // 가는 흰선 / 하늘선
        if (Thin(u, 0.02f) || Thin(v, 0.02f)) c = p.thinWhite;
        if (Thin(u + 0.25f, 0.012f) || Thin(v + 0.25f, 0.012f)) c = p.thinBlue;
        return c;
    }

    static bool Band(float t, float w)
    {
        float f = t - Mathf.Floor(t);
        return f < w;
    }
    static bool Thin(float t, float w)
    {
        float f = t - Mathf.Floor(t);
        return f < w;
    }

#if UNITY_EDITOR
    [MenuItem("School/Bake Summer Check Texture")]
    static void Bake()
    {
        var preset = AssetDatabase.LoadAssetAtPath<UniformColors>("Assets/SchoolArt/SummerUniform_Habok.asset");
        if (!preset) { Debug.LogError("preset 없음. 먼저 School/Create Summer Uniform Preset 실행"); return; }
        var tex = GetOrCreate(preset);
        byte[] png = UnityEngine.ImageConversion.EncodeToPNG(tex);
        Directory.CreateDirectory("Assets/SchoolArt");
        File.WriteAllBytes("Assets/SchoolArt/SummerCheck_Tartan.png", png);
        AssetDatabase.Refresh();
        Debug.Log("Baked: Assets/SchoolArt/SummerCheck_Tartan.png");
    }

    [MenuItem("School/Create Summer Uniform Preset")]
    static void CreatePreset()
    {
        Directory.CreateDirectory("Assets/SchoolArt");
        var p = ScriptableObject.CreateInstance<UniformColors>();
        AssetDatabase.CreateAsset(p, "Assets/SchoolArt/SummerUniform_Habok.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Preset 생성: 드래그해서 UniformSetup.preset에 연결");
    }
#endif
}
