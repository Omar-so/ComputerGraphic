using UnityEngine;
using UnityEngine.InputSystem; // 👈 replaces UnityEngine.Input

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Vector3 playerVelocity;
    [SerializeField] bool groundedPlayer;
    [SerializeField] float playerSpeed;
    [SerializeField] float gravityValue;
    [SerializeField] GameObject activeChar;
    [SerializeField] float moveHorizontal;
    [SerializeField] float moveVertical;
    [SerializeField] float speed = 4;
    [SerializeField] float rotateSpeed = 4;
    [SerializeField] float jumpHeight = 1.2f;
    [SerializeField] bool isJumping;

    private Keyboard kb; // cached keyboard reference
    private Animator anim;

    void Start()
    {
        playerSpeed = 4;
        gravityValue = -20;

        kb = Keyboard.current;                          // cache once
        anim = activeChar.GetComponent<Animator>();     // cache once
    }

    void Update()
    {
        kb = Keyboard.current; // safety refresh (handles device reconnect)

        // ── Grounded check ────────────────────────────────────────────
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            isJumping = false; // ✅ reset jump flag on landing
        }

        // ── Rotation (Horizontal axis: A/D or Left/Right arrows) ──────
        float horizontal = 0f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) horizontal =  1f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  horizontal = -1f;
        transform.Rotate(0, horizontal * rotateSpeed, 0);

        // ── Forward movement (Vertical axis: W/S or Up/Down arrows) ───
        float vertical = 0f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)   vertical =  1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) vertical = -1f;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * vertical;
        controller.SimpleMove(forward * curSpeed);

        // ── Jump ───────────────────────────────────────────────────────
        if (kb.spaceKey.wasPressedThisFrame && groundedPlayer)
        {
            isJumping = true;
            anim.Play("Jump");
            playerVelocity.y += 10;
        }

        // ── Gravity ────────────────────────────────────────────────────
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // ── Animations ────────────────────────────────────────────────
        // ✅ Checks both WASD and Arrow keys
        bool isMoving = kb.wKey.isPressed    || kb.sKey.isPressed ||
                kb.aKey.isPressed    || kb.dKey.isPressed ||
                kb.upArrowKey.isPressed   || kb.downArrowKey.isPressed ||
                kb.leftArrowKey.isPressed || kb.rightArrowKey.isPressed;
        if (isMoving)
        {
            controller.minMoveDistance = 0.001f;
            if (!isJumping)
                anim.Play("Standard Run");
        }
        else
        {
            controller.minMoveDistance = 0.901f;
            if (!isJumping)
                anim.Play("Idle");
        }
    }
}