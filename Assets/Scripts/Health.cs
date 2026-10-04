using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHP = 100f;
    public float hp;
    public bool IsDead => hp <= 0;
    float iframeUntil;
    public event Action<float> OnDamaged;
    public event Action OnDead;

    void Awake() => hp = maxHP;

    public void GrantIFrame(float sec) => iframeUntil = Mathf.Max(iframeUntil, Time.time + sec);
    public bool IsInvulnerable() => Time.time < iframeUntil;

    public void Damage(float amount, Vector3 from)
    {
        if (IsDead || IsInvulnerable()) return;
        hp = Mathf.Max(0, hp - amount);
        OnDamaged?.Invoke(hp / maxHP);
        // 넉백
        var cc = GetComponent<CharacterController>();
        if (cc)
        {
            Vector3 dir = (transform.position - from).normalized;
            cc.Move(dir * 0.6f);
        }
        // 쓰러짐: 크기 찌그러짐 대신 비활성화는 GameManager가 처리
        if (IsDead)
        {
            OnDead?.Invoke();
            // 가벼운 dissolve 대신 눕히기
            transform.rotation *= Quaternion.Euler(70, 0, 0);
        }
    }

    public void Revive()
    {
        hp = maxHP;
        transform.rotation = Quaternion.identity;
        var pc = GetComponent<PlayerController>();
        if (pc) pc.RefillBall();
    }
}
