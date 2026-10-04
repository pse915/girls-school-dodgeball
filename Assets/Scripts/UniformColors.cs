using UnityEngine;

// 이미지 하복 분석값 (여고 여름교복)
// 상의: 퓨어화이트 블라우스, 카라/소매안감 적+남 체크, 주머니 적색 파이핑+하늘색 교표
// 하의: 적(버건디)/남(네이비) 타탄체크 플리츠스커트
[CreateAssetMenu(menuName = "School/Summer Uniform", fileName = "SummerUniform_Habok")]
public class UniformColors : ScriptableObject
{
    [Header("블라우스")]
    public Color blouseWhite = new Color(0.96f, 0.97f, 0.99f);
    public Color buttonPearl = new Color(0.92f, 0.93f, 0.95f);

    [Header("체크 (카라+치마) - 이미지 실측")]
    public Color navyBase = new Color(0.13f, 0.16f, 0.30f);   // #21294D
    public Color redBand = new Color(0.62f, 0.13f, 0.20f);    // #9E2133
    public Color darkRed = new Color(0.42f, 0.10f, 0.16f);
    public Color thinWhite = new Color(0.88f, 0.88f, 0.90f);
    public Color thinBlue = new Color(0.35f, 0.45f, 0.65f);

    [Header("주머니")]
    public Color pocketTrim = new Color(0.45f, 0.12f, 0.16f);
    public Color emblemBlue = new Color(0.42f, 0.70f, 0.84f);

    [Header("피부/머리/신발")]
    public Color skin = new Color(1f, 0.87f, 0.78f);
    public Color hairPlayer = new Color(0.16f, 0.12f, 0.10f); // 흑갈색 긴머리
    public Color hairCaptain = new Color(0.35f, 0.20f, 0.10f); // 주장 염색 구분
    public Color shoeBrown = new Color(0.35f, 0.22f, 0.15f);
    public Color sockWhite = new Color(0.95f, 0.95f, 0.96f);
}
