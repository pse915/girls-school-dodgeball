using UnityEngine;
using UnityEngine.AI;

// 운동장 NavMesh 자동 설정. Menu: School/Bake NavMesh
public class NavMeshSetup : MonoBehaviour
{
    [ContextMenu("Bake NavMesh")]
    public void Bake()
    {
#if UNITY_2022_1_OR_NEWER
        // AI.Navigation 패키지의 NavMeshSurface가 있으면 사용, 없으면 기본 베이크 안내
        var surf = GetComponent<Unity.AI.Navigation.NavMeshSurface>();
        if (!surf) surf = gameObject.AddComponent<Unity.AI.Navigation.NavMeshSurface>();
        surf.collectObjects = Unity.AI.Navigation.CollectObjects.All;
        surf.BuildNavMesh();
        Debug.Log("NavMesh baked.");
#else
        Debug.LogWarning("NavMesh 베이크는 Unity 에디터 Navigation 창에서 Bake를 누르세요.");
#endif
    }
}
