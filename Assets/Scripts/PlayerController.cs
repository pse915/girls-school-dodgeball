using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float walkSpeed = 4.2f;
    public float sprintSpeed = 7.0f;
    public float jumpHeight = 1.4f;
    public float gravity = -18f;
    public float turnSmooth = 12f;

    [Header("Combat")]
    public Dodgeball ballPrefab;
    public Transform throwPoint;
    public int maxBalls = 1;
    public float throwCooldown = 0.45f;
    public float throwPower = 16f;
    public float dashThrowPower = 24f;
    public float dashTime = 0.18f;
    public float dashSpeed = 14f;

    [Header("Dodge")]
    public float rollDuration = 0.42f;
    public float rollSpeed = 8.5f;
    public float rollIFrames = 0.35f;

    [Header("Camera")]
    public Transform cam;
    public float lookSens = 2.2f;
    public float minPitch = -30f, maxPitch = 60f;
    public float camDist = 4.2f;
    public float minDist = 2f, maxDist = 8f;
    public LayerMask camBlock = ~0;

    CharacterController cc;
    Health health;
    Vector3 vel;
    float yaw, pitch;
    float lastThrow, rollStart = -99f, dashStart = -99f;
    Vector3 rollDir;
    int carriedBalls = 1;
    bool muted;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        health = GetComponent<Health>();
        if (cam == null && Camera.main) cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (health != null && health.IsDead)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
                FindObjectOfType<GameManager>()?.RespawnPlayer();
            return;
        }

        var kb = Keyboard.current;
        var ms = Mouse.current;

        // look
        if (ms != null)
        {
            yaw += ms.delta.ReadValue().x * lookSens * 0.1f;
            pitch -= ms.delta.ReadValue().y * lookSens * 0.1f;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            float wheel = ms.scroll.ReadValue().y * 0.002f;
            if (Mathf.Abs(wheel) > 0.001f) camDist = Mathf.Clamp(camDist - wheel, minDist, maxDist);
        }
        // 게임패드 우스틱 (InputSystem)
        var gp = Gamepad.current;
        if (gp != null)
        {
            Vector2 rs = gp.rightStick.ReadValue();
            yaw += rs.x * lookSens * 0.6f;
            pitch = Mathf.Clamp(pitch - rs.y * lookSens * 0.6f, minPitch, maxPitch);
        }
        if (kb != null && kb.escapeKey.wasPressedThisFrame)
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;

        // move input
        Vector2 inp = Vector2.zero;
        if (kb != null)
        {
            if (kb.wKey.isPressed) inp.y += 1;
            if (kb.sKey.isPressed) inp.y -= 1;
            if (kb.aKey.isPressed) inp.x -= 1;
            if (kb.dKey.isPressed) inp.x += 1;
        }
        bool sprint = kb != null && kb.leftShiftKey.isPressed;
        float speed = sprint ? sprintSpeed : walkSpeed;

        bool rolling = Time.time - rollStart < rollDuration;
        bool dashing = Time.time - dashStart < dashTime;

        Vector3 fwd = Quaternion.Euler(0, yaw, 0) * Vector3.forward;
        Vector3 right = Quaternion.Euler(0, yaw, 0) * Vector3.right;
        Vector3 wish = (fwd * inp.y + right * inp.x);
        if (wish.sqrMagnitude > 1f) wish.Normalize();

        if (kb != null && kb.cKey.wasPressedThisFrame && !rolling)
        {
            rollStart = Time.time;
            rollDir = wish.sqrMagnitude > 0.01f ? wish.normalized : transform.forward;
            if (health) health.GrantIFrame(rollIFrames);
        }

        if (dashing)
        {
            vel.x = transform.forward.x * dashSpeed;
            vel.z = transform.forward.z * dashSpeed;
        }
        else if (rolling)
        {
            vel.x = rollDir.x * rollSpeed;
            vel.z = rollDir.z * rollSpeed;
        }
        else
        {
            vel.x = wish.x * speed;
            vel.z = wish.z * speed;
        }

        // jump / gravity
        if (cc.isGrounded && vel.y < 0) vel.y = -2f;
        if (kb != null && kb.spaceKey.wasPressedThisFrame && cc.isGrounded && !rolling)
            vel.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        vel.y += gravity * Time.deltaTime;
        cc.Move(vel * Time.deltaTime);

        // face move dir
        if (wish.sqrMagnitude > 0.01f && !dashing)
        {
            Quaternion t = Quaternion.LookRotation(wish);
            transform.rotation = Quaternion.Slerp(transform.rotation, t, turnSmooth * Time.deltaTime);
        }

        // pick up ball / catch
        if (kb != null && kb.eKey.wasPressedThisFrame)
        {
            var rules = FindObjectOfType<MatchRules>();
            if (rules && rules.TryCatch(this)) { /* 캐치 성공 */ }
            else TryPickup();
        }
        if (gp != null && gp.buttonWest.wasPressedThisFrame)
        {
            var rules = FindObjectOfType<MatchRules>();
            if (!(rules && rules.TryCatch(this))) TryPickup();
        }

        // throw combo / dash throw
        if (ms != null && ms.leftButton.wasPressedThisFrame) Throw(throwPower);
        if (ms != null && ms.rightButton.wasPressedThisFrame)
        {
            if (Throw(dashThrowPower))
                dashStart = Time.time;
        }

        if (kb != null && kb.mKey.wasPressedThisFrame)
        {
            muted = !muted;
            AudioListener.volume = muted ? 0 : 1;
        }

        // camera (+충돌: 벽 뒤로 안 뚫리게)
        if (cam)
        {
            Vector3 target = transform.position + Vector3.up * 1.6f;
            Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
            Vector3 want = target - rot * Vector3.forward * camDist;
            if (Physics.Linecast(target, want, out RaycastHit hit, camBlock, QueryTriggerInteraction.Ignore))
                want = hit.point + hit.normal * 0.25f;
            cam.position = want;
            cam.rotation = rot;
        }
    }

    bool Throw(float power)
    {
        if (Time.time - lastThrow < throwCooldown) return false;
        if (carriedBalls <= 0) return false;
        if (ballPrefab == null) return false;
        lastThrow = Time.time;
        carriedBalls--;
        var b = Instantiate(ballPrefab, throwPoint ? throwPoint.position : transform.position + transform.forward + Vector3.up * 1.4f, Quaternion.identity);
        b.Launch(transform.forward + Vector3.up * 0.18f, power, gameObject);
        FindObjectOfType<WhistleAudio>()?.PlayThrow();
        return true;
    }

    void TryPickup()
    {
        var balls = FindObjectsOfType<Dodgeball>();
        float best = 2.6f;
        Dodgeball pick = null;
        foreach (var b in balls)
        {
            if (!b.IsLoose) continue;
            float d = Vector3.Distance(transform.position, b.transform.position);
            if (d < best) { best = d; pick = b; }
        }
        if (pick && carriedBalls < maxBalls)
        {
            carriedBalls++;
            Destroy(pick.gameObject);
            FindObjectOfType<WhistleAudio>()?.PlayPickup();
        }
    }

    public void RefillBall() => carriedBalls = maxBalls;

    void OnGUI()
    {
        if (carriedBalls <= 0)
        {
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter, fontSize = 14 };
            GUI.Label(new Rect(0, Screen.height - 60, Screen.width, 30), "E 근처 공 줍기", style);
        }
    }
}
