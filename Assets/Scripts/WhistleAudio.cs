using UnityEngine;

// Walden의 WebAudio 생성음악 방식을 Unity로: 파일 없이 호루라기/타격/픽업 합성
public class WhistleAudio : MonoBehaviour
{
    AudioSource src;

    void Awake()
    {
        src = gameObject.AddComponent<AudioSource>();
    }

    AudioClip Tone(float freq, float dur, float vol = 0.4f)
    {
        int sr = 22050, n = (int)(sr * dur);
        float[] d = new float[n];
        for (int i = 0; i < n; i++)
        {
            float t = (float)i / sr;
            float env = 1f - (float)i / n;
            d[i] = Mathf.Sin(2 * Mathf.PI * freq * t) * env * vol;
        }
        var c = AudioClip.Create("tone", n, 1, sr, false);
        c.SetData(d, 0);
        return c;
    }

    public void PlayThrow() => src.PlayOneShot(Tone(620, 0.12f));
    public void PlayHit() => src.PlayOneShot(Tone(160, 0.18f, 0.6f));
    public void PlayPickup() => src.PlayOneShot(Tone(880, 0.08f));
    public void PlayWhistle() => src.PlayOneShot(Tone(2100, 0.4f, 0.5f));
    public void PlayStep() => src.PlayOneShot(Tone(220 + Random.Range(-20, 20), 0.05f, 0.15f));

    // 낮 BGM: 밝은 펜타토닉 루프 (파일 없이)
    float bgmNext;
    readonly float[] penta = { 523, 587, 659, 784, 880 };
    int bgmIdx;
    void Update()
    {
        if (AudioListener.volume < 0.01f) return;
        if (Time.time > bgmNext)
        {
            bgmNext = Time.time + 0.42f;
            src.PlayOneShot(Tone(penta[bgmIdx++ % penta.Length], 0.35f, 0.08f));
        }
    }
}
