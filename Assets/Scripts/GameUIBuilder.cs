using UnityEngine;

// Canvas 타이틀/일시정지/승패. Menu: School/Build Game UI
// UIManager(OnGUI HUD)와 함께 동작, 충돌 없음.
public class GameUIBuilder : MonoBehaviour
{
    public Canvas canvas;
    public GameObject titlePanel;
    public GameObject pausePanel;
    bool started;

    void Awake()
    {
        Time.timeScale = 0f; // 타이틀에서 시작
    }

    void Update()
    {
        if (!started && (Input.anyKeyDown))
        {
            started = true;
            titlePanel?.SetActive(false);
            Time.timeScale = 1f;
            FindObjectOfType<WhistleAudio>()?.PlayWhistle();
        }
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) && started)
        {
            bool paused = Time.timeScale > 0.01f;
            Time.timeScale = paused ? 0f : 1f;
            if (pausePanel) pausePanel.SetActive(paused);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("School/Build Game UI")]
    static void Build()
    {
        var c = FindObjectOfType<Canvas>();
        if (!c)
        {
            var go = new GameObject("GameCanvas");
            c = go.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            go.AddComponent<UnityEngine.UI.CanvasScaler>();
            go.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
        var b = c.GetComponent<GameUIBuilder>() ?? c.gameObject.AddComponent<GameUIBuilder>();

        GameObject Panel(string n, string txt)
        {
            var t = c.transform.Find(n);
            if (t) return t.gameObject;
            var p = new GameObject(n);
            p.transform.SetParent(c.transform, false);
            var img = p.AddComponent<UnityEngine.UI.Image>();
            img.color = new Color(0.1f, 0.15f, 0.3f, 0.75f);
            var rt = p.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
            var label = new GameObject("Text").AddComponent<UnityEngine.UI.Text>();
            label.transform.SetParent(p.transform, false);
            label.text = txt;
            label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            label.fontSize = 30;
            label.alignment = TextAnchor.MiddleCenter;
            label.color = Color.white;
            var lrt = label.GetComponent<RectTransform>();
            lrt.anchorMin = Vector2.zero; lrt.anchorMax = Vector2.one;
            lrt.offsetMin = Vector2.zero; lrt.offsetMax = Vector2.zero;
            return p;
        }

        b.titlePanel = Panel("Title", "○○여자고등학교 피구 대결\n아무 키나 눌러 시작 (클릭=마우스 잠금)");
        b.pausePanel = Panel("Pause", "일시정지 (P/ESC로 계속)");
        b.pausePanel.SetActive(false);
        b.canvas = c;
        UnityEditor.EditorUtility.SetDirty(c);
        Debug.Log("Game UI 생성.");
    }
#endif
}
