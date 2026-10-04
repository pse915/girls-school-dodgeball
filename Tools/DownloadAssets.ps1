# CC0-only 자동 준비 (로그인 불필요)
# 실행: powershell -ExecutionPolicy Bypass -File Tools/DownloadAssets.ps1
# AssetStore·Mixamo는 미사용. 브라우저 6개 탭을 열어 수동 다운로드 위치만 안내 (itch.io 버튼 클릭 필요).
$out = "Assets/SchoolArt/CC0"
New-Item -ItemType Directory -Force -Path $out | Out-Null
New-Item -ItemType Directory -Force -Path "Assets/SchoolArt/Store" | Out-Null

$urls = @(
  "https://quaternius.com/packs/universalbasecharacters.html",
  "https://quaternius.com/packs/ultimatemodularwomen.html",
  "https://quaternius.com/packs/universalanimationlibrary.html",
  "https://kenney.nl/assets/city-kit-suburban",
  "https://kenney.nl/assets/furniture-kit",
  "https://kenney.nl/assets/city-kit-roads"
)
foreach ($u in $urls) { Start-Process $u }
Write-Host "6개 페이지에서 Download 클릭 후 $out 에 압축해제 하세요."
Write-Host "완료 확인: Unity 메뉴 School/Check Imports"
