using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// 캡슐 플레이스홀더를 Quaternius 여자 모델로 교체 (CC0-only)
// Menu: School/Replace Capsules with Quaternius Women
// 전제: Assets/SchoolArt/CC0/ 에 Quaternius FBX 있음 (Universal Base / Modular Women / Animated Women + AnimLib)
public class QuaterniusReplacer : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("School/Replace Capsules with Quaternius Women")]
    static void Replace()
    {
        var preset = AssetDatabase.LoadAssetAtPath<UniformColors>("Assets/SchoolArt/SummerUniform_Habok.asset");
        var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animator/Player.controller");
        if (!preset) Debug.LogWarning("하복 preset 없음. School/Create Summer Uniform Preset 먼저.");

        var models = FindQuaterniusModels();
        if (models.Length == 0)
        {
            Debug.LogError("Quaternius FBX 없음. https://quaternius.com/packs/universalbasecharacters.html + ultimatemodularwomen.html 받아서 Assets/SchoolArt/CC0/에 넣으세요.");
            return;
        }
        Debug.Log($"Quaternius 후보 {models.Length}개: {string.Join(", ", System.Array.ConvertAll(models, m => m.name))}");

        ReplaceOne("Player_2학년 (Quaternius Women으로 교체)", models[0], UniformSetup.Role.Player2학년, preset, controller, true);
        string[] rivals = { "피구부 주장", "부원 A", "부원 B" };
        var roles = new[] { UniformSetup.Role.Captain주장, UniformSetup.Role.Member부원, UniformSetup.Role.Member부원 };
        for (int i = 0; i < rivals.Length; i++)
        {
            var m = models[(i + 1) % models.Length]; // 주장/부원 다른 바리에이션
            ReplaceOne(rivals[i], m, roles[i], preset, controller, false);
        }
        Debug.Log("교체 완료. 캡슐은 삭제되고 Quaternius+하복+Animator가 연결됨.");
    }

    static GameObject[] FindQuaterniusModels()
    {
        var list = new System.Collections.Generic.List<GameObject>();
        foreach (var guid in AssetDatabase.FindAssets("t:Model"))
        {
            string p = AssetDatabase.GUIDToAssetPath(guid);
            if (!p.Contains("SchoolArt/CC0")) continue;
            string n = p.ToLower();
            // 부분 파츠·남성·동물 제외, 전신 여성만
            if (n.Contains("hairstyle") || n.Contains("hair_") || n.Contains("eyebrow")) continue;
            if (n.Contains("_head.fbx") || n.Contains("_legs.fbx") || n.Contains("_feet.fbx") || n.Contains("_body.fbx")) continue;
            if (n.Contains("unreal engine")) continue; // Unity용만
            if ((n.Contains("man") || n.Contains("male")) && !n.Contains("female")) continue;
            if (n.Contains("zombie") || n.Contains("skeleton") || n.Contains("animal")) continue;
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(p);
            if (go) list.Add(go);
        }
        // 여고 체형 우선: Teen/Casual/Formal/Superhero_Female > Humanoid Rigs > Base
        list.Sort((a, b) =>
        {
            int Score(GameObject g)
            {
                string path = AssetDatabase.GetAssetPath(g).ToLower() + " " + g.name.ToLower();
                int s = 0;
                if (path.Contains("female") || path.Contains("girl") || path.Contains("casual") || path.Contains("formal") || path.Contains("teen")) s -= 10;
                if (path.Contains("humanoid rigs")) s -= 5;
                if (path.Contains("base characters") && path.Contains("unity")) s -= 4;
                if (path.Contains("all together")) s += 20; // 올인원 제외
                return s;
            }
            return Score(a).CompareTo(Score(b));
        });
        return list.ToArray();
    }

    static void ReplaceOne(string oldName, GameObject modelPrefab, UniformSetup.Role role, UniformColors preset, RuntimeAnimatorController controller, bool isPlayer)
    {
        var old = GameObject.Find(oldName);
        // 이름 바뀌었을 경우 PlayerController/RivalAI로 찾기
        if (!old)
        {
            if (isPlayer) { var pc = FindObjectOfType<PlayerController>(); if (pc) old = pc.gameObject; }
            else
            {
                foreach (var r in FindObjectsOfType<RivalAI>())
                    if (r.rivalName == oldName) { old = r.gameObject; break; }
            }
        }
        if (!old) { Debug.LogWarning($"{oldName} 없음. Build Full Game 먼저."); return; }

        Vector3 pos = old.transform.position;
        Quaternion rot = old.transform.rotation;

        // 기존 컴포넌트 값 백업
        var oldHealth = old.GetComponent<Health>();
        float hp = oldHealth ? oldHealth.maxHP : 100f;
        var oldPC = old.GetComponent<PlayerController>();
        Dodgeball ballPrefab = oldPC ? oldPC.ballPrefab : FindObjectOfType<BallSpawner>()?.ballPrefab;
        var oldAI = old.GetComponent<RivalAI>();
        string rivalName = oldAI ? oldAI.rivalName : oldName;

        var fresh = (GameObject)PrefabUtility.InstantiatePrefab(modelPrefab);
        // FBX 직접 로드된 경우 Instantiate
        if (!fresh) fresh = Instantiate(modelPrefab);
        fresh.name = isPlayer ? "Player_2학년" : rivalName;
        fresh.transform.position = pos;
        fresh.transform.rotation = rot;
        // 여고 체형: 주장만 1.07배
        if (role == UniformSetup.Role.Captain주장) fresh.transform.localScale *= 1.07f;

        // 휴머노이드 아바타 + 컨트롤러 설정
        var anim = fresh.GetComponent<Animator>() ?? fresh.AddComponent<Animator>();
        if (controller) anim.runtimeAnimatorController = controller;
        anim.applyRootMotion = false;

        // 물리/이동
        var cc = fresh.GetComponent<CharacterController>() ?? fresh.AddComponent<CharacterController>();
        cc.height = 1.6f; cc.radius = 0.3f; cc.center = new Vector3(0, 0.8f, 0);
        var h = fresh.GetComponent<Health>() ?? fresh.AddComponent<Health>();
        h.maxHP = isPlayer ? 100f : 80f; h.hp = h.maxHP;

        if (isPlayer)
        {
            var pc2 = fresh.GetComponent<PlayerController>() ?? fresh.AddComponent<PlayerController>();
            pc2.ballPrefab = ballPrefab;
            pc2.walkSpeed = 4.2f; pc2.sprintSpeed = 7f;
            var tp = fresh.transform.Find("ThrowPoint");
            if (!tp) { var t = new GameObject("ThrowPoint").transform; t.SetParent(fresh.transform); t.localPosition = new Vector3(0, 1.4f, 0.6f); tp = t; }
            pc2.throwPoint = tp as Transform;
            if (oldPC) { pc2.throwPower = oldPC.throwPower; pc2.dashThrowPower = oldPC.dashThrowPower; }
        }
        else
        {
            var nav = fresh.AddComponent<UnityEngine.AI.NavMeshAgent>();
            nav.speed = 4.5f; nav.angularSpeed = 360;
            var ai2 = fresh.GetComponent<RivalAI>() ?? fresh.AddComponent<RivalAI>();
            ai2.rivalName = rivalName;
            ai2.ballPrefab = ballPrefab;
            var tp = fresh.transform.Find("ThrowPoint");
            if (!tp) { var t = new GameObject("ThrowPoint").transform; t.SetParent(fresh.transform); t.localPosition = new Vector3(0, 1.4f, 0.6f); tp = t; }
            ai2.throwPoint = tp as Transform;
        }

        fresh.AddComponent<CharacterAnimator>();
        var uni = fresh.GetComponent<UniformSetup>() ?? fresh.AddComponent<UniformSetup>();
        uni.role = role;
        uni.preset = preset;
        uni.Apply();

        // 카메라가 구 캡슐을 보고 있으면 새 모델로
        foreach (var p in FindObjectsOfType<PlayerController>())
            if (p.gameObject == old) break;
        Undo.RegisterCreatedObjectUndo(fresh, "Replace with Quaternius");
        Undo.DestroyObjectImmediate(old);
    }
#endif
}
