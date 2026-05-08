using UnityEngine;
using UnityEngine.AI;  // for NavMesh walking

public class PlayerController : MonoBehaviour
{
    // ── Inspector Fields ──────────────────────────────────────────────────
    [Header("Player Settings")]
    [SerializeField] private float walkSpeed     = 3f;
    [SerializeField] private float runSpeed      = 6f;
    [SerializeField] private float rotateSpeed   = 8f;
    [SerializeField] private float interactRange = 2f;   // how close to switch

    [Header("References")]
    [SerializeField] private Transform      switchTransform;   // drag Switch 3D object
    [SerializeField] private SwitchController switchController; // drag SwitchController
    [SerializeField] private Camera          playerCamera;
    [SerializeField] private Animator        animator;         // drag player animator

    [Header("UI")]
    [SerializeField] private GameObject interactPrompt; // "Press E to switch" UI text

    // ── Private ───────────────────────────────────────────────────────────
    private CharacterController charController;
    private NavMeshAgent         navAgent;
    private Vector3              moveDirection;
    private bool                 isNearSwitch  = false;
    private bool                 isWalkingTo   = false;
    private float                currentSpeed  = 0f;

    // Animator parameter names
    private const string ANIM_SPEED    = "Speed";
    private const string ANIM_INTERACT = "Interact";

    // ── Start ─────────────────────────────────────────────────────────────
    void Start()
    {
        charController = GetComponent<CharacterController>();
        navAgent       = GetComponent<NavMeshAgent>();

        // hide interact prompt at start
        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        // set nav agent speed
        if (navAgent != null)
        {
            navAgent.speed        = walkSpeed;
            navAgent.stoppingDistance = interactRange - 0.5f;
        }
    }

    // ── Update — runs every frame ─────────────────────────────────────────
    void Update()
    {
        HandleMovement();
        CheckSwitchDistance();
        HandleInteract();
        HandleClickToWalk();
    }

    // ── WASD / Arrow key movement ─────────────────────────────────────────
    private void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // cancel nav agent if player moves manually
        if ((h != 0 || v != 0) && navAgent != null)
        {
            navAgent.ResetPath();
            isWalkingTo = false;
        }

        // get camera forward direction
        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight   = playerCamera.transform.right;
        camForward.y = 0f;
        camRight.y   = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // combine input with camera direction
        moveDirection = (camForward * v + camRight * h);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        currentSpeed   = moveDirection.magnitude > 0.1f
                         ? (isRunning ? runSpeed : walkSpeed)
                         : 0f;

        // rotate player to face movement direction
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation   = Quaternion.Lerp(
                transform.rotation, targetRot,
                rotateSpeed * Time.deltaTime);
        }

        // apply gravity
        if (!charController.isGrounded)
            moveDirection.y -= 9.8f * Time.deltaTime;

        // move the character
        if (charController != null)
            charController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // update animator speed
        if (animator != null)
            animator.SetFloat(ANIM_SPEED, currentSpeed);
    }

    // ── Click on switch to auto walk to it ───────────────────────────────
    private void HandleClickToWalk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                // if player clicked the switch object
                if (hit.transform == switchTransform)
                {
                    WalkToSwitch();
                }
            }
        }
    }

    // ── Auto walk to switch using NavMesh ─────────────────────────────────
    public void WalkToSwitch()
    {
        if (navAgent != null && switchTransform != null)
        {
            navAgent.SetDestination(switchTransform.position);
            isWalkingTo = true;
        }
    }

    // ── Check distance to switch ──────────────────────────────────────────
    private void CheckSwitchDistance()
    {
        if (switchTransform == null) return;

        float distance = Vector3.Distance(
            transform.position,
            switchTransform.position);

        isNearSwitch = distance <= interactRange;

        // show or hide interact prompt
        if (interactPrompt != null)
            interactPrompt.SetActive(isNearSwitch);

        // auto interact when nav walk reaches switch
        if (isWalkingTo && isNearSwitch)
        {
            isWalkingTo = false;
            InteractWithSwitch();
        }
    }

    // ── Press E to interact with switch ───────────────────────────────────
    private void HandleInteract()
    {
        if (isNearSwitch && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithSwitch();
        }
    }

    // ── Actually toggle the switch ────────────────────────────────────────
    private void InteractWithSwitch()
    {
        // play interact animation
        if (animator != null)
            animator.SetTrigger(ANIM_INTERACT);

        // wait a tiny bit for animation then toggle
        Invoke("ToggleSwitchDelayed", 0.3f);
    }

    // ── Delayed toggle so animation plays first ───────────────────────────
    private void ToggleSwitchDelayed()
    {
        if (switchController != null)
            switchController.OnSwitchPressed();
    }
}
