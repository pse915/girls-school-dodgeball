using UnityEngine;

// Quaternius 여자 모델에 하복 입히기
// 사용법: Player/Rival 프리팹 루트에 붙이고, 각 Renderer 드래그 후 Apply 클릭
// Quaternius 명명: Body, Hair, Top, Bottom, Shoes 와 달라도 이름 포함으로 자동 탐색 지원
public class UniformSetup : MonoBehaviour
{
    public enum Role { Player2학년, Captain주장, Member부원 }

    [Header("역할 (주장은 완장+머리색으로 구분)")]
    public Role role = Role.Player2학년;

    [Header("색상 preset")]
    public UniformColors preset;

    [Header("부위 Renderer (비워두면 이름으로 자동탐색)")]
    public Renderer blouse;   // 상의 흰 부분
    public Renderer collar;   // 카라 체크
    public Renderer sleeveLining; // 소매 안감 체크
    public Renderer skirt;    // 치마 체크
    public Renderer pocketTrim;
    public Renderer emblem;
    public Renderer hair;
    public Renderer skin;
    public Renderer shoes;
    public Renderer socks;

    [Header("체크 텍스처 (없으면 자동생성)")]
    public Texture2D checkTexture;

    [ContextMenu("Apply Summer Uniform")]
    public void Apply()
    {
        if (!preset)
        {
            Debug.LogError("preset(UniformColors) 연결 필요. Assets/SchoolArt/에 생성됨.");
            return;
        }
        AutoFind();
        checkTexture = SummerCheckTextureGenerator.GetOrCreate(preset);

        Set(blouse, preset.blouseWhite, 0.15f);
        Set(skin, preset.skin, 0.4f);
        Set(shoes, preset.shoeBrown, 0.3f);
        Set(socks, preset.sockWhite, 0.2f);
        Set(pocketTrim, preset.pocketTrim, 0.2f);
        Set(emblem, preset.emblemBlue, 0.1f, true);

        // 체크 3부위: 카라/소매안감/치마
        SetCheck(collar);
        SetCheck(sleeveLining);
        SetCheck(skirt, 3f); // 치마는 플리츠라 반복 3배

        // 머리: 역할 구분
        Set(hair, role == Role.Captain주장 ? preset.hairCaptain : preset.hairPlayer, 0.5f);

        // 주장 완장: 왼쪽 팔에 빨간 밴드 (큐브 추가)
        var old = transform.Find("CaptainArmband");
        if (old) DestroyImmediate(old.gameObject);
        if (role == Role.Captain주장)
        {
            var band = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            band.name = "CaptainArmband";
            band.transform.SetParent(transform);
            band.transform.localPosition = new Vector3(0.28f, 1.25f, 0);
            band.transform.localScale = new Vector3(0.16f, 0.08f, 0.16f);
            Set(band.GetComponent<Renderer>(), preset.redBand, 0.2f);
        }

        // 부원은 교표 없음 (신입 구분)
        if (role == Role.Member부원 && emblem)
            emblem.gameObject.SetActive(false);

        Debug.Log($"하복 적용 완료: {role}");
    }

    void Set(Renderer r, Color c, float smooth, bool emission = false)
    {
        if (!r) return;
        var m = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        m.color = c;
        if (m.HasProperty("_Smoothness")) m.SetFloat("_Smoothness", smooth);
        if (emission)
        {
            m.EnableKeyword("_EMISSION");
            if (m.HasProperty("_EmissionColor")) m.SetColor("_EmissionColor", c * 0.4f);
        }
        r.sharedMaterial = m;
    }

    void SetCheck(Renderer r, float tiling = 1f)
    {
        if (!r) return;
        var m = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        m.mainTexture = checkTexture;
        m.color = Color.white;
        m.mainTextureScale = new Vector2(tiling, tiling);
        r.sharedMaterial = m;
    }

    void AutoFind()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            string n = r.gameObject.name.ToLower();
            if (!blouse && (n.Contains("blouse") || n.Contains("body") || n.Contains("top") || n.Contains("shirt"))) blouse = r;
            else if (!collar && n.Contains("collar")) collar = r;
            else if (!skirt && (n.Contains("skirt") || n.Contains("bottom"))) skirt = r;
            else if (!hair && n.Contains("hair")) hair = r;
            else if (!skin && (n.Contains("head") || n.Contains("skin") || n.Contains("hand"))) skin = r;
            else if (!shoes && (n.Contains("shoe") || n.Contains("foot"))) shoes = r;
        }
        // Quaternius 단일메시(Body 하나에 전부)인 경우: 머티리얼 슬롯으로 분리 불가 → 경고
        if (blouse && blouse == skirt)
            Debug.LogWarning("모델이 단일 메시입니다. Blender에서 상/하의를 분리하거나, 머티리얼 슬롯 2개로 나누세요. Docs/UNIFORM_GUIDE 참고.");
    }
}
