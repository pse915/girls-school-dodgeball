using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class RivalAI : MonoBehaviour
{
    [Header("여고 피구부 설정")]
    public string rivalName = "피구부 주장";
    public float chaseRange = 25f;
    public float attackRange = 12f;
    public float throwCooldown = 2.2f;
    public float throwPower = 14f;
    public Dodgeball ballPrefab;
    public Transform throwPoint;

    Transform player;
    NavMeshAgent agent;
    Health health;
    float lastThrow;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
        health.OnDead += () => { agent.isStopped = true; FindObjectOfType<GameManager>()?.OnRivalDown(this); };
    }

    void Start()
    {
        var p = FindObjectOfType<PlayerController>();
        if (p) player = p.transform;
    }

    void Update()
    {
        if (health.IsDead || !player) return;
        float d = Vector3.Distance(transform.position, player.position);
        if (d < chaseRange)
        {
            // 적당한 거리 유지: 너무 가까우면 살짝 후퇴
            Vector3 target = d > attackRange ? player.position
                : transform.position + (transform.position - player.position).normalized * 4f;
            agent.SetDestination(target);

            if (d < attackRange + 3f)
            {
                transform.LookAt(player.position);
                if (Time.time - lastThrow > throwCooldown && ballPrefab)
                {
                    lastThrow = Time.time;
                    var b = Instantiate(ballPrefab, throwPoint ? throwPoint.position : transform.position + Vector3.up * 1.4f + transform.forward, Quaternion.identity);
                    Vector3 dir = (player.position + Vector3.up * 1.2f - b.transform.position).normalized;
                    b.Launch(dir, throwPower, gameObject);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
