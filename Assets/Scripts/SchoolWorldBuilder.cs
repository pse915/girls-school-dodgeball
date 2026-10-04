using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// 에셋 없이도 Play 되게: 운동장+본관+펜스+나무+스폰을 박스/캡슐 그레이박스로 생성
// 무료 에셋 받으면 SchoolArt 프리팹으로 교체용 (위치만 유지)
public class SchoolWorldBuilder : MonoBehaviour
{
    [Header("여고 운동장 크기")]
    public Vector2 fieldSize = new Vector2(100, 70);

    public void Build()
    {
        // 바닥
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Outfield_Ground";
        ground.transform.localScale = new Vector3(fieldSize.x / 10f, 1, fieldSize.y / 10f);
        ground.GetComponent<Renderer>().material.color = new Color(0.45f, 0.72f, 0.42f);

        // 운동장 흰선 (얇은 박스)
        Line(0, 0, fieldSize.x, 0.3f);
        Line(0, 0, 0.3f, fieldSize.y);

        // 본관 (여고) - Kenney City Kit Suburban으로 교체용 그레이박스
        var main = GameObject.CreatePrimitive(PrimitiveType.Cube);
        main.name = "GirlsHigh_MainBuilding (City Kit Suburban으로 교체)";
        main.transform.position = new Vector3(0, 6, -fieldSize.y / 2 - 12);
        main.transform.localScale = new Vector3(60, 12, 8);
        main.GetComponent<Renderer>().material.color = new Color(0.93f, 0.90f, 0.84f);

        var sign = new GameObject("SchoolSign_○○여자고등학교");
        sign.transform.position = main.transform.position + new Vector3(0, 7, 4.2f);
        var text = sign.AddComponent<TextMesh>();
        text.text = "○○여자고등학교";
        text.fontSize = 48;
        text.characterSize = 0.35f;
        text.color = new Color(0.2f, 0.25f, 0.45f);

        // 교실 1개
        var room = GameObject.CreatePrimitive(PrimitiveType.Cube);
        room.name = "Classroom_2-3 (Furniture Kit로 교체)";
        room.transform.position = new Vector3(-35, 2, -20);
        room.transform.localScale = new Vector3(12, 4, 8);
        room.GetComponent<Renderer>().material.color = new Color(0.96f, 0.95f, 0.90f);

        // 펜스 4면
        Fence(new Vector3(0, 1, fieldSize.y / 2), new Vector3(fieldSize.x, 2, 0.4f));
        Fence(new Vector3(0, 1, -fieldSize.y / 2), new Vector3(fieldSize.x, 2, 0.4f));
        Fence(new Vector3(fieldSize.x / 2, 1, 0), new Vector3(0.4f, 2, fieldSize.y));
        Fence(new Vector3(-fieldSize.x / 2, 1, 0), new Vector3(0.4f, 2, fieldSize.y));

        // 벚나무 대용 (기둥+구)
        for (int i = 0; i < 8; i++)
        {
            float x = -fieldSize.x / 2 + i * (fieldSize.x / 7f);
            Tree(new Vector3(x, 0, fieldSize.y / 2 + 5));
        }

        // 조명: 밝은 낮
        var sun = new GameObject("Sun_Day").AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.4f;
        sun.color = new Color(1f, 0.96f, 0.88f);
        sun.transform.rotation = Quaternion.Euler(50, -30, 0);
        RenderSettings.ambientIntensity = 0.9f;

        // 스폰
        Spawn("PlayerSpawn", new Vector3(0, 0.5f, 15));
        var gm = FindObjectOfType<GameManager>();
        Debug.Log("여고 운동장 그레이박스 완성. GameManager rivalSpawns 3곳 자동 생성");
        Spawn("RivalSpawn 1", new Vector3(0, 0.5f, -10));
        Spawn("Rival Spawn 2", new Vector3(-8, 0.5f, -12));
        Spawn("Rival Spawn 3", new Vector3(8, 0.5f, -12));
    }

    void Line(float x, float z, float w, float d)
    {
        var l = GameObject.CreatePrimitive(PrimitiveType.Cube);
        l.name = "FieldLine";
        l.transform.position = new Vector3(x, 0.02f, z);
        l.transform.localScale = new Vector3(w, 0.04f, d);
        l.GetComponent<Renderer>().material.color = Color.white;
    }

    void Fence(Vector3 pos, Vector3 scale)
    {
        var f = GameObject.CreatePrimitive(PrimitiveType.Cube);
        f.name = "Fence (City Kit으로 교체)";
        f.transform.position = pos;
        f.transform.localScale = scale;
        f.GetComponent<Renderer>().material.color = new Color(0.35f, 0.45f, 0.55f);
    }

    void Tree(Vector3 pos)
    {
        var t = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        t.name = "Tree (Mini Forest로 교체)";
        t.transform.position = pos + Vector3.up * 1.5f;
        t.GetComponent<Renderer>().material.color = new Color(0.4f, 0.28f, 0.18f);
        var c = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        c.transform.position = pos + Vector3.up * 4f;
        c.transform.localScale = Vector3.one * 3f;
        c.GetComponent<Renderer>().material.color = new Color(0.95f, 0.75f, 0.82f); // 벚꽃빛
    }

    void Spawn(string n, Vector3 p)
    {
        var s = new GameObject(n);
        s.transform.position = p;
    }

#if UNITY_EDITOR
    [MenuItem("School/Build Girls School Greybox")]
    static void MenuBuild()
    {
        var b = FindObjectOfType<SchoolWorldBuilder>();
        if (!b)
        {
            var go = new GameObject("SchoolWorldBuilder");
            b = go.AddComponent<SchoolWorldBuilder>();
        }
        b.Build();
    }
#endif
}
