using UnityEngine;

// 피구 룰: 라운드 타이머 + 아웃 + 캐치 + 점수. GameManager와 함께 동작.
public class MatchRules : MonoBehaviour
{
    [Header("룰")]
    public float roundTime = 180f;
    public int outsToWin = 3;
    public float catchWindow = 0.45f; // 날아오는 공 앞에서 E 누르면 캐치

    public int playerOuts { get; private set; }
    public int rivalOuts { get; private set; }
    public float timeLeft { get; private set; }
    public bool IsOver { get; private set; }

    void Start()
    {
        timeLeft = roundTime;
        FindObjectOfType<WhistleAudio>()?.PlayWhistle();
    }

    void Update()
    {
        if (IsOver) return;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0 || playerOuts >= outsToWin || rivalOuts >= outsToWin)
            Finish();
    }

    public void OnRivalOut(RivalAI r)
    {
        rivalOuts++;
        Debug.Log($"아웃! {r.rivalName} ({rivalOuts}/{outsToWin})");
    }

    public void OnPlayerOut() => playerOuts++;

    void Finish()
    {
        IsOver = true;
        var ui = FindObjectOfType<UIManager>();
        bool win = rivalOuts > playerOuts;
        if (win) ui?.ShowWin();
        else ui?.ShowLose("시간 종료 / 패배… R로 재시작");
        FindObjectOfType<WhistleAudio>()?.PlayWhistle();
    }

    // 캐치 시도: 플레이어 앞에서 날아오는 공이 있으면 잡기
    public bool TryCatch(PlayerController p)
    {
        foreach (var b in FindObjectsOfType<Dodgeball>())
        {
            if (b.IsLoose) continue;
            float d = Vector3.Distance(p.transform.position, b.transform.position);
            if (d < 2.2f)
            {
                // 소유자가 적이던 공을 제거하고 손에 보충
                Object.Destroy(b.gameObject);
                p.RefillBall();
                FindObjectOfType<WhistleAudio>()?.PlayPickup();
                return true;
            }
        }
        return false;
    }
}
