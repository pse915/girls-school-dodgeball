# 빌드 (WebGL = Walden처럼 Pages 배포)
1. Unity 2022.3.20f1 설치 (Hub에서 지정 버전)
2. `School/Build Full Game` → `School/Build Animator Controller` → `School/Bake Summer Check Texture` 순서 실행
3. File > Build Profiles > WebGL > Switch Platform → Player Settings:
   - Color Space: Linear, API: WebGL2
   - Resolution: 1280x720, Fullscreen: Windowed
4. 로컬 확인: `Tools/BuildWebGL.ps1` 실행 또는 File > Build And Run
5. 배포: main 푸시하면 `.github/workflows/webgl.yml`이 빌드→Pages 배포 (Secrets에 UNITY_LICENSE 필요)

# 데스크탑 빌드
- Windows: File > Build Profiles > Windows > Build `Builds/GirlsSchool.exe`
