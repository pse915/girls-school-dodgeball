using UnityEngine;

// 타격 먼지 + 던지기 궤적선. 외부 파티클 팩 없이 동작.
public class HitVFX : MonoBehaviour
{
    static HitVFX inst;
    void Awake() => inst = this;

    public static void Burst(Vector3 pos, Color color)
    {
        var go = new GameObject("HitBurst");
        go.transform.position = pos;
        var ps = go.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = color;
        main.startSize = 0.12f;
        main.startLifetime = 0.4f;
        main.startSpeed = 4f;
        ps.Emit(14);
        Object.Destroy(go, 1f);
    }

    public static void Trail(GameObject ball)
    {
        var tr = ball.AddComponent<TrailRenderer>();
        tr.time = 0.35f;
        tr.startWidth = 0.18f;
        tr.endWidth = 0.02f;
        tr.material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        tr.material.color = Color.white;
    }
}
