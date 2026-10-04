using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("스폰")]
    public GameObject playerPrefab;
    public GameObject rivalPrefab;
    public Transform playerSpawn;
    public Transform[] rivalSpawns;
    public Dodgeball ballPrefab;
    public int extraBalls = 3;

    GameObject player;
    int rivalsAlive;

    void Start()
    {
        SpawnAll();
        // 운동장 바닥에 여분 공
        for (int i = 0; i < extraBalls; i++)
        {
            var p = playerSpawn.position + new Vector3(UnityEngine.Random.Range(-6, 6), 1, UnityEngine.Random.Range(-4, 4));
            Instantiate(ballPrefab, p, Quaternion.identity);
        }
    }

    void SpawnAll()
    {
        player = Instantiate(playerPrefab, playerSpawn.position, playerSpawn.rotation);
        player.GetComponent<Health>().OnDead += () => Debug.Log("패배 - R로 재시작");
        rivalsAlive = rivalSpawns.Length;
        foreach (var s in rivalSpawns)
            Instantiate(rivalPrefab, s.position, s.rotation);
    }

    public void OnRivalDown(RivalAI r)
    {
        rivalsAlive--;
        Debug.Log($"{r.rivalName} 다운 ({rivalsAlive} 남음)");
        if (rivalsAlive <= 0)
            FindObjectOfType<UIManager>()?.ShowWin();
    }

    public void RespawnPlayer()
    {
        var h = player.GetComponent<Health>();
        player.transform.position = playerSpawn.position;
        h.Revive();
        FindObjectOfType<UIManager>()?.ShowRespawn();
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
