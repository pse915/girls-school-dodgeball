# 하복 입히기 가이드 (이미지 실측)

사진: 흰 반팔 블라우스 + 적/남 체크 세일러 카라 + 적/남 체크 플리츠스커트 하복.

## 1. 30초 적용
1. Quaternius 다운로드: `Universal Base Characters` + `Ultimate Modular Women` → `Assets/SchoolArt/CC0/`에
2. Unity 메뉴 `School/Create Summer Uniform Preset` → `Assets/SchoolArt/SummerUniform_Habok.asset` 생성
3. Unity 메뉴 `School/Bake Summer Check Texture` → `SummerCheck_Tartan.png` 생성
4. Player 프리팹(여자 베이스)에 `UniformSetup` 추가 → preset 연결 → Role=`Player2학년` → 우클릭 `Apply Summer Uniform`
5. Rival 주장: Role=`Captain주장` (갈색머리+빨간 완장), 부원: Role=`Member부원` (교표 숨김)

## 2. 모델 분리 필요 시 (Quaternius 단일메시인 경우)
- Blender: 상의/치마를 머티리얼 슬롯 분리 (Slot1 흰, Slot2 체크) 후 FBX 재출력
- 또는 Modular Women의 Skirt 파츠를 그대로 사용 (이미 분리됨, 권장)

## 3. 색상 대조표
| 부위 | 색 | HEX |
|---|---|---|
| 블라우스 | 퓨어화이트 | #F5F8FC |
| 카라/치마 바탕 | 딥네이비 | #21294D |
| 체크 굵은밴드 | 버건디레드 | #9E2133 |
| 가는선 | 오프화이트/스틸블루 | #E0E0E6 / #5973A6 |
| 주머니 파이핑 | 다크레드 | #731E29 |
| 교표 자수 | 스카이블루 | #6BB3D6 |

## 4. Player/Rival 구분 (낮 운동장에서 식별)
- Player: 흑갈색 긴머리 + 교표 있음
- 주장: 밝은 염색머리 + 왼쪽팔 빨간 완장 + 치마 타일링 3x
- 부원: 교표 없음, 머리 묶음 파츠 사용 권장
