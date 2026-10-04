# 여학교 전용 에셋 리스트 - CC0 ONLY (로그인 없이 받기)

전부 CC0. 상업·수정·크레딧 생략 가능. AssetStore·Mixamo·Adobe 로그인 일절 불필요.
`School/Open CC0 Downloads` 메뉴로 6개 탭 한 번에 열기 → `Assets/SchoolArt/CC0/`에 압축해제.

## 1. 필수 코어 (Package Manager, 로그인 불필요)
- Unity Starter Assets - Third Person Controller (Companion License, 무료)
- FinalCharacterController (MIT, https://github.com/spaderdabomb/FinalCharacterController) - 선택

## 2. 여학생 캐릭터 (CC0)
- Quaternius Universal Base Characters + Ultimate Modular Women Pack (CC0)
  - https://quaternius.com/packs/universalbasecharacters.html
  - https://quaternius.com/packs/ultimatemodularwomen.html
  - 여자 베이스 + 긴머리 + 치마 모듈. 머티리얼 색만 남색/흰색으로 바꾸면 여고 교복 완성.
  - 리깅·휴머노이드 호환, Universal Animation Library와 리타겟 바로 됨.
- Quaternius Animated Women Pack (CC0)
  - https://quaternius.com/packs/animatedwomen.html
  - 피구부 주장/부원 적으로 스케일 1.05~1.1로 키우면 위압감. 체력바는 `Health.cs` 공용.
- Quaternius Universal Animation Library 1+2 (CC0, Mixamo 대체)
  - https://quaternius.com/packs/universalanimationlibrary.html
  - 달리기/점프/던지기/피격/쓰러짐 120종. `AnimatorBuilder`가 이름으로 자동 연결.
  - Mixamo 미사용: 로그인·리깅 불필요, 휴머노이드 리타겟 바로 됨.

제외: AssetStore 여고생·Mixamo·Ready Player Me (로그인/EULA 필요).

## 3. 여고 건물·교실 (CC0)
- Kenney City Kit Suburban + City Kit Roads (CC0, School assets 대체)
  - https://kenney.nl/assets/city-kit-suburban + https://kenney.nl/assets/city-kit-roads
  - 모듈러 본관/창문/복도/정문/펜스. 본관 흰색 머티리얼로 여고 표현.
- Kenney Furniture Kit (CC0)
  - https://kenney.nl/assets/furniture-kit
  - 책상/의자/칠판/사물함 140종. 교실 1개 분량 충분.
- 여고 간판/표지판: TextMeshPro로 `○○여자고등학교` 직접 제작 (폰트: Noto Sans KR, OFL).

## 4. 운동장·자연 (낮)
- Kenney Mini Forest + Nature Kit (CC0) — 운동장 주변 벚나무/잔디 대용
- Quaternius Stylized Nature MegaKit (CC0) — 저사양에서도 horizon까지 잔디
- Kenney Skyboxes (CC0) — 파란 낮 하늘. Directional Light 5500K + Fog 밝게.

## 5. 피구·스포츠 소품 (전투 교체)
- Kenney Sports Kit — `sports` 검색 (CC0): 피구공, 콘, 골대, 휘슬
  - `Dodgeball.cs`에 공 프리팹 연결. 빨강/파랑 2색으로 아군/적 구분.
- Kenney Particle Pack (CC0): 타격 먼지, 구르기 먼지
- Unity URP Decal: 운동장 흰선은 Decal Projector로 (에셋 불필요)

## 6. 오디오 (파일 없이 시작 가능)
- 코드는 `WhistleAudio.cs`가 호루라기/타격/발소리를 사인파 합성으로 생성 (Walden WebAudio 방식의 Unity판)
- 교체용 CC0: Kenney Audio Packs, BBC Rewind SFX

## 7. UI
- Kenney Input Prompts + UI Pack (CC0): WASD/마우스 아이콘, 체력바 프레임
- 코드 `UIManager.cs`는 기본 Unity UI만으로 동작 (에셋 없어도 됨)

## 라이선스 커밋 규칙
- `Assets/SchoolArt/CC0/` — Kenney/Quaternius는 그대로 커밋 OK
- `Assets/SchoolArt/Store/` — AssetStore/Mixamo/ReadyPlayerMe는 `.gitignore`에 추가, 각자 다운로드
- 유료 에셋은 절대 커밋 금지

## 권장 다운로드 순서
1. Starter Assets (Unity)
2. Quaternius 4종 (Base Women, Animated Women, Anim Lib 1+2, Nature)
3. Kenney 5종 (Furniture, Suburban, Roads, Sports, Skyboxes)
4. School assets (AssetStore)
5. Mixamo 모션 6개 (Throw, Dodge, Hit, Defeat, Win, Idle)
