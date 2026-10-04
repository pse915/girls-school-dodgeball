using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// CC0-ONLY: 로그인 없이 받는 6종. AssetStore·Mixamo 미사용.
// Menu: School/Open CC0 Downloads → 압축해제 위치: Assets/SchoolArt/CC0/
public static class StoreImportHelper
{
    const string Q_BASE = "https://quaternius.com/packs/universalbasecharacters.html";
    const string Q_WOMEN = "https://quaternius.com/packs/ultimatemodularwomen.html";
    const string Q_ANIM = "https://quaternius.com/packs/universalanimationlibrary.html";
    const string K_SUBURBAN = "https://kenney.nl/assets/city-kit-suburban";
    const string K_FURN = "https://kenney.nl/assets/furniture-kit";
    const string K_SPORTS = "https://kenney.nl/assets?q=sports";

#if UNITY_EDITOR
    [MenuItem("School/Open CC0 Downloads")]
    static void OpenAll()
    {
        Application.OpenURL(Q_BASE);
        Application.OpenURL(Q_WOMEN);
        Application.OpenURL(Q_ANIM);
        Application.OpenURL(K_SUBURBAN);
        Application.OpenURL(K_FURN);
        Application.OpenURL(K_SPORTS);
        Debug.Log("CC0 6종 탭 열림. 받아서 Assets/SchoolArt/CC0/에 넣으세요. 로그인 불필요.");
    }

    // 구 메뉴 호환 (기존 가이드에서 호출 시 CC0로 리다이렉트)
    [MenuItem("School/Open Asset Downloads")]
    static void OpenLegacy() => OpenAll();

    [MenuItem("School/Check Imports")]
    static void Check()
    {
        bool hasQ = AssetDatabase.FindAssets("t:Model", new[] { "Assets/SchoolArt/CC0" }).Length > 0;
        bool hasAnim = AssetDatabase.FindAssets("t:AnimationClip", new[] { "Assets" }).Length > 0;
        Debug.Log($"[Check CC0-only] 캐릭터: {(hasQ ? "OK" : "없음→Open CC0 Downloads")} / 애니메이션(Quaternius AnimLib): {(hasAnim ? "OK" : "없음→Universal Animation Library 받기")}");
        if (!hasQ || !hasAnim)
            EditorUtility.DisplayDialog("Import 체크 (CC0-only)", $"캐릭터: {(hasQ ? "OK" : "없음")}\n애니메이션: {(hasAnim ? "OK" : "없음")}\n\n없으면 School/Open CC0 Downloads 실행\n(AssetStore·Mixamo 불필요)", "확인");
    }
#endif
}
