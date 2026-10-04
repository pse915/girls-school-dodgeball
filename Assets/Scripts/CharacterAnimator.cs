using UnityEngine;

// Animator가 없어도 T포즈로 미끄러지지 않게: 상하 바운스+기울기+팔 흔들 흉내 (임시)
// AnimatorController 연결되면 자동으로 해당 파라미터 구동으로 전환
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    Animator anim;
    CharacterController cc;
    PlayerController player;
    RivalAI rival;
    Vector3 lastPos;
    float speed01;

    void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        player = GetComponent<PlayerController>();
        rival = GetComponent<RivalAI>();
        lastPos = transform.position;
    }

    void Update()
    {
        float dt = Mathf.Max(Time.deltaTime, 0.0001f);
        float spd = (transform.position - lastPos).magnitude / dt;
        lastPos = transform.position;
        speed01 = Mathf.Lerp(speed01, Mathf.Clamp01(spd / 7f), 8 * dt);

        if (anim.runtimeAnimatorController)
        {
            anim.SetFloat("Speed", speed01);
            anim.SetBool("Grounded", cc ? cc.isGrounded : true);
        }
        else
        {
            // 폴백: 몸통 바운스 (자식 모델이 있으면)
            var visual = transform.childCount > 0 ? transform.GetChild(0) : transform;
            visual.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(Time.time * 10f)) * 0.06f * speed01, 0);
            visual.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * 10f) * 2f * speed01);
        }
    }

    public void PlayThrow() { if (anim.runtimeAnimatorController) anim.SetTrigger("Throw"); }
    public void PlayDodge() { if (anim.runtimeAnimatorController) anim.SetTrigger("Dodge"); }
    public void PlayHit() { if (anim.runtimeAnimatorController) anim.SetTrigger("Hit"); }
}
