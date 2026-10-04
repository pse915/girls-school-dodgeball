using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class Dodgeball : MonoBehaviour
{
    public float damage = 34f;
    public float life = 4f;
    public bool IsLoose { get; private set; }
    GameObject owner;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    public void Launch(Vector3 dir, float power, GameObject from)
    {
        owner = from;
        IsLoose = false;
        rb.velocity = dir.normalized * power;
        HitVFX.Trail(gameObject);
        Destroy(gameObject, life);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject == owner) return;
        var h = c.gameObject.GetComponent<Health>();
        if (h && !IsLoose)
        {
            h.Damage(damage, transform.position);
            HitVFX.Burst(c.contacts[0].point, Color.white);
            FindObjectOfType<WhistleAudio>()?.PlayHit();
            // 맞고 떨어진 공은 주울 수 있게
            BecomeLoose();
        }
        else if (c.contacts.Length > 0 && c.contacts[0].normal.y > 0.5f)
        {
            // 바닥에 떨어지면 줍기 가능
            BecomeLoose();
        }
    }

    void BecomeLoose()
    {
        IsLoose = true;
        owner = null;
        rb.velocity *= 0.2f;
        // 잠시 후 자동 삭제 방지: GameManager가 리스폰
        CancelInvoke();
        Invoke(nameof(Despawn), 12f);
    }

    void Despawn() => Destroy(gameObject);
}
