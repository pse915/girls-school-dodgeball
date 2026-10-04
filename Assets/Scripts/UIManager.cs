using UnityEngine;

public class UIManager : MonoBehaviour
{
    PlayerController player;
    Health playerHealth;
    bool showHelp = true;
    string msg = "";

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player) playerHealth = player.GetComponent<Health>();
    }

    public void ShowWin() => msg = "승리! 피구부 제패! (R: 다시 시작)";
    public void ShowLose(string s) => msg = s;
    public void ShowRespawn() => msg = "";

    void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
            if (player) playerHealth = player.GetComponent<Health>();
        }
        if (Input.GetKeyDown(KeyCode.F1)) showHelp = !showHelp;
        if (Input.GetKeyDown(KeyCode.R)) FindObjectOfType<GameManager>()?.Restart();
        if (playerHealth && playerHealth.IsDead) msg = "졌어요… R로 다시 일어나기";
    }

    void OnGUI()
    {
        // 플레이어 체력
        if (playerHealth)
        {
            GUI.Box(new Rect(20, 20, 240, 24), "");
            GUI.Box(new Rect(20, 20, 240 * (playerHealth.hp / playerHealth.maxHP), 24), $"2학년 {Mathf.CeilToInt(playerHealth.hp)}");
        }
        // 라이벌 체력
        var rivals = FindObjectsOfType<RivalAI>();
        for (int i = 0; i < rivals.Length; i++)
        {
            var h = rivals[i].GetComponent<Health>();
            GUI.Box(new Rect(Screen.width - 260, 20 + i * 30, 240, 24), "");
            GUI.Box(new Rect(Screen.width - 260, 20 + i * 30, 240 * (h.hp / h.maxHP), 24), rivals[i].rivalName);
        }
        // 중앙 메시지
        if (!string.IsNullOrEmpty(msg))
        {
            var s = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 28 };
            GUI.Label(new Rect(0, Screen.height / 2 - 60, Screen.width, 60), msg, s);
        }
        // 조작법
        if (showHelp)
        {
            GUI.Box(new Rect(20, Screen.height - 150, 420, 130), "WASD 이동 / Shift 달리기 / Space 점프 / C 구르기(무적)\nE 공줍기 / 좌클릭 던지기 / 우클릭 대시던지기\n마우스 시점, 휠 줌 / R 재시작 / M 음소거 / F1 도움말");
        }
        // 크로스헤어
        GUI.Label(new Rect(Screen.width / 2 - 4, Screen.height / 2 - 10, 20, 20), "+");
    }
}
