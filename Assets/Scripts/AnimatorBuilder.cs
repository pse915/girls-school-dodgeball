using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif

// Menu: School/Build Animator Controller
// Quaternius AnimLib 클립을 받아서 Player.controller 생성. 클립 없어도 파라미터만 생성.
public static class AnimatorBuilder
{
#if UNITY_EDITOR
    [MenuItem("School/Build Animator Controller")]
    static void Build()
    {
        System.IO.Directory.CreateDirectory("Assets/Animator");
        var ctrl = AnimatorController.CreateAnimatorControllerAtPath("Assets/Animator/Player.controller");
        var root = ctrl.layers[0].stateMachine;
        var idle = root.AddState("Idle");
        var run = root.AddState("Run");
        var jump = root.AddState("Jump");
        var throwS = root.AddState("Throw");
        var dodge = root.AddState("Dodge");
        var hit = root.AddState("Hit");

        ctrl.AddParameter("Speed", AnimatorControllerParameterType.Float);
        ctrl.AddParameter("Grounded", AnimatorControllerParameterType.Bool);
        ctrl.AddParameter("Throw", AnimatorControllerParameterType.Trigger);
        ctrl.AddParameter("Dodge", AnimatorControllerParameterType.Trigger);
        ctrl.AddParameter("Hit", AnimatorControllerParameterType.Trigger);

        // 클립 자동 연결: Quaternius Universal Animation Library 이름 기준 (Mixamo 불필요)
        foreach (var guid in AssetDatabase.FindAssets("t:AnimationClip"))
        {
            string p = AssetDatabase.GUIDToAssetPath(guid);
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(p);
            string n = clip.name.ToLower();
            if (n.Contains("idle") && !idle.motion) idle.motion = clip;
            else if ((n.Contains("run") || n.Contains("walk")) && !run.motion) run.motion = clip;
            else if (n.Contains("jump") && !jump.motion) jump.motion = clip;
            else if (n.Contains("throw") && !throwS.motion) throwS.motion = clip;
            else if ((n.Contains("dodge") || n.Contains("roll")) && !dodge.motion) dodge.motion = clip;
            else if (n.Contains("hit") && !hit.motion) hit.motion = clip;
        }

        var toRun = idle.AddTransition(run);
        toRun.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Greater, 0.15f, "Speed");
        var toIdle = run.AddTransition(idle);
        toIdle.AddCondition(UnityEditor.Animations.AnimatorConditionMode.Less, 0.15f, "Speed");
        var toThrow = root.AddAnyStateTransition(throwS);
        toThrow.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "Throw");
        toThrow.duration = 0.05f;
        var toDodge = root.AddAnyStateTransition(dodge);
        toDodge.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "Dodge");
        toDodge.duration = 0.05f;

        AssetDatabase.SaveAssets();
        Debug.Log("Animator 생성: Assets/Animator/Player.controller. Quaternius AnimLib 클립 넣고 Rebuild하면 자동 연결.");
    }
#endif
}
