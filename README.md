# 여고 피구 액션 (Walden-like, Unity)

Walden 구조를 여학교 낮 학원물로 옮긴 3인칭 피구 액션 프로토타입입니다.
Unity 에디터 없이 파일만으로 스캐폴드되어 있고, 여는 즉시 실행 가능합니다.

##コン셉트
- 장소: 여고 운동장 + 본관 앞 + 교실 1개 (그레이박스로 먼저 동작, 무료 에셋으로 교체)
- 주인공: 2학년 여학생, 적: 피구부 주장 + 부원
- 전투: Walden 검술을 비폭력 피구로 치환
  - 좌클릭: 던지기 콤보 / 우클릭: 대시 던지기
  - C: 구르기 (무적 0.35s) / E: 공 줍기·들기 / Space: 점프 / Shift: 달리기 / R: 재시작 / F1: 조작법 / M: 음소거

## 열기
1. Unity Hub → Unity 2022.3 LTS (URP 템플릿) 로 새 프로젝트 생성
2. 이 폴더의 `Assets/`, `ProjectSettings/`, `Packages/` 를 새 프로젝트에 덮어쓰기
   (또는 이 폴더 자체를 Unity Hub → Open 으로 열기)
3. 무료 에셋 임포트 (아래 표, `Docs/ASSETS_GIRLS_SCHOOL.md` 참고)
4. 메뉴 순서대로 실행 → Play (Docs/BUILD.md 참고)
   - `School/Build Full Game` (씬·프리팹·카메라·룰·UI·오디오·NavMesh·조명 일괄 배선)
   - `School/Build Animator Controller` (Mixamo/Quaternius 클립 자동 연결)
   - `School/Bake Summer Check Texture` + `School/Create Summer Uniform Preset` (하복)
   - `School/Build Game UI` (타이틀·일시정지)
   - `School/Bake NavMesh` (AI 이동)

## 조작 (Walden과 동일 키)
WASD 이동, 마우스 시점, 휠 줌, Shift 달리기, Space 점프, C 구르기, E 공줍기, 좌클릭 던지기, 우클릭 대시던지기, R 부활, M 음소거, F1 도움말

## 폴더
```
Assets/
  Scripts/      Player, Rival, Dodgeball, Health, GameManager, UI, Audio, WorldBuilder
  Scenes/       SchoolDay_Outfield (스크립트로 자동 생성, 에셋 없어도 동작)
  Prefabs/      Ball, Rival, Player 교체용 가이드
  SchoolArt/    무료 에셋 다운로드 후 넣는 곳 (CC0는 커밋 가능, AssetStore는 제외)
Docs/
  ASSETS_GIRLS_SCHOOL.md  여학교 전용 무료 에셋 + 라이선스 정리
Tools/
  DownloadAssets.ps1      CC0 에셋 자동 다운로드
```
