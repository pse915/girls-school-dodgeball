using UnityEngine;

// 운동장에 공이 마르면 자동 리필. GameManager와 별도 동작.
public class BallSpawner : MonoBehaviour
{
    public Dodgeball ballPrefab;
    public int keepLoose = 3;
    public Vector2 area = new Vector2(20, 12);
    public float interval = 3f;
    float next;

    void Update()
    {
        if (!ballPrefab) return;
        if (Time.time < next) return;
        next = Time.time + interval;
        int loose = 0;
        foreach (var b in FindObjectsOfType<Dodgeball>())
            if (b.IsLoose) loose++;
        if (loose < keepLoose)
        {
            Vector3 p = transform.position + new Vector3(Random.Range(-area.x / 2, area.x / 2), 1, Random.Range(-area.y / 2, area.y / 2));
            Instantiate(ballPrefab, p, Quaternion.identity);
        }
    }
}
