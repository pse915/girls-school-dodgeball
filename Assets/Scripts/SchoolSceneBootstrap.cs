using UnityEngine;
using UnityEngine.AI;

// 원클릭 풀게임 배선. Menu: School/Build Full Game
// 씬 파일 없이도 Play 가능: 월드+조명+플레이어+라이벌3+공+UI+오디오+NavMesh+룰 전부 연결
public class SchoolSceneBootstrap : MonoBehaviour
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("School/Build Full Game")]
    static void Build()
    {
        // 1. 월드
        var wb = FindObjectOfType<SchoolWorldBuilder>() ?? new GameObject("SchoolWorldBuilder").AddComponent<SchoolWorldBuilder>();
        wb.Build();
        var light = FindObjectOfType<LightingSetup>() ?? new GameObject("Lighting").AddComponent<LightingSetup>();
        light.Apply();

        // 2. NavMesh
        var nav = FindObjectOfType<NavMeshSetup>() ?? new GameObject("NavMesh").AddComponent<NavMeshSetup>();
        try { nav.Bake(); } catch { Debug.LogWarning("NavMesh 수동 Bake 필요: Window > AI > Navigation"); }

        // 3. 오디오/UI/룰/스포너
        if (!FindObjectOfType<WhistleAudio>()) new GameObject("Audio").AddComponent<WhistleAudio>();
        if (!FindObjectOfType<UIManager>()) new GameObject("UI").AddComponent<UIManager>();
        if (!FindObjectOfType<HitVFX>()) new GameObject("VFX").AddComponent<HitVFX>();
        if (!FindObjectOfType<MatchRules>()) new GameObject("Rules").AddComponent<MatchRules>();
        GameUIBuilder _ = FindObjectOfType<GameUIBuilder>();
        if (!_) UnityEditor.EditorApplication.ExecuteMenuItem("School/Build Game UI");
       

        // 4. 공 프리팹 (Quaternius 없을 때 빨간 피구공)
        var ball = FindObjectOfType<Dodgeball>();
        Dodgeball ballPrefab = null;
        if (!ball)
        {
            var bg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bg.name = "Dodgeball_Prefab_Source";
            bg.transform.localScale = Vector3.one * 0.45f;
            bg.GetComponent<Renderer>().material.color = new Color(0.85f, 0.2f, 0.25f);
            var rb = bg.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            ballPrefab = bg.AddComponent<Dodgeball>();
            ballPrefab.damage = 34f;
        }
        else ballPrefab = ball;

        // 5. 플레이어 (캡슐+UniformSetup, Quaternius 오면 교체)
        var pc = FindObjectOfType<PlayerController>();
        if (!pc)
        {
            var p = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            p.name = "Player_2학년 (Quaternius Women으로 교체)";
            p.transform.position = GameObject.Find("PlayerSpawn")?.transform.position ?? new Vector3(0, 0.5f, 15);
            p.AddComponent<Health>();
            var ctrl = p.AddComponent<PlayerController>();
            p.AddComponent<Animator>();
            p.AddComponent<CharacterAnimator>();
            var uni = p.AddComponent<UniformSetup>();
            uni.role = UniformSetup.Role.Player2학년;
            var tp = new GameObject("ThrowPoint").transform;
            tp.SetParent(p.transform); tp.localPosition = new Vector3(0, 1.4f, 0.6f);
            ctrl.throwPoint = tp;
            ctrl.ballPrefab = ballPrefab;
            var cc = p.GetComponent<CharacterController>();
            // CharacterController는 CapsuleCollider와 충돌하므로 Primitive collider 제거
            foreach (var col in p.GetComponents<Collider>()) if (!(col is CharacterController)) Object.DestroyImmediate(col);
            var ag = p.AddComponent<NavMeshAgent>(); ag.enabled = false; // 플레이어는 수동
        }

        // 6. 라이벌 3명
        var gm = FindObjectOfType<GameManager>() ?? new GameObject("GameManager").AddComponent<GameManager>();
        string[] names = { "Rival Spawn 1", "Rival Spawn 2", "Rival Spawn 3" };
        string[] roles = { "피구부 주장", "부원 A", "부원 B" };
        for (int i = 0; i < 3; i++)
        {
            if (FindObjectOfType<RivalAI>() != null && GameObject.Find(roles[i])) continue;
            var s = GameObject.Find(names[i]);
            var r = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            r.name = roles[i];
            r.transform.position = s ? s.transform.position : new Vector3(i * 4 - 4, 0.5f, -10);
            r.transform.localScale = new Vector3(1.05f, 1.1f, 1.05f);
            r.GetComponent<Renderer>().material.color = new Color(0.75f, 0.25f, 0.3f);
            r.AddComponent<Health>().maxHP = 80f;
            var ai = r.AddComponent<RivalAI>();
            ai.rivalName = roles[i];
            ai.ballPrefab = ballPrefab;
            r.AddComponent<Animator>();
            r.AddComponent<CharacterAnimator>();
            var uni = r.AddComponent<UniformSetup>();
            uni.role = i == 0 ? UniformSetup.Role.Captain주장 : UniformSetup.Role.Member부원;
            foreach (var col in r.GetComponents<Collider>()) if (!(col is CharacterController)) Object.DestroyImmediate(col);
            r.AddComponent<CharacterController>();
        }

        // 7. GameManager 배선
        var player = FindObjectOfType<PlayerController>();
        var rivals = GameObject.FindGameObjectsWithTag("Untagged");
        gm.playerPrefab = player?.gameObject;
        var spawner = FindObjectOfType<BallSpawner>() ?? new GameObject("BallSpawner").AddComponent<BallSpawner>();
        spawner.ballPrefab = ballPrefab;

        Debug.Log("풀게임 배선 완료. Play 누르면 바로 피구 시작. Quaternius 받으면 Capsule만 교체하세요.");
    }
#endif
}
