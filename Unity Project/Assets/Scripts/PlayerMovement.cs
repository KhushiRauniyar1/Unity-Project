using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed     = 4f;
    public float sprintSpeed   = 8f;
    public float gravity       = -15f;
    public float rotationSpeed = 10f;

    [Header("Animator")]
    public Animator animator;

    CharacterController _cc;
    Vector3             _velocity;
    float               _currentSpeed;
    Transform           _cam;

    static readonly int _hashSpeed = Animator.StringToHash("Speed");

    void Awake()
    {
        _cc  = GetComponent<CharacterController>();
        _cam = Camera.main.transform;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Move();
        Gravity();
        Animate();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = _cam.forward;
        Vector3 right   = _cam.right;
        forward.y = 0f;
        right.y   = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 dir    = (forward * v + right * h);
        bool    moving = dir.magnitude > 0.1f;

        float target = Input.GetKey(KeyCode.LeftShift)
                     ? sprintSpeed : walkSpeed;

        if (moving)
        {
            dir.Normalize();

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                rotationSpeed * Time.deltaTime);

            _currentSpeed = Mathf.Lerp(
                _currentSpeed, target, Time.deltaTime * 10f);

            _cc.Move(dir * _currentSpeed * Time.deltaTime);
        }
        else
        {
            _currentSpeed = Mathf.Lerp(
                _currentSpeed, 0f, Time.deltaTime * 15f);

            if (_currentSpeed < 0.05f) _currentSpeed = 0f;
        }
    }

    void Gravity()
    {
        if (_cc.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(new Vector3(0f, _velocity.y, 0f) * Time.deltaTime);
    }

    void Animate()
    {
        if (animator == null) return;
        float norm = _currentSpeed / sprintSpeed;
        animator.SetFloat(_hashSpeed, norm, 0.1f, Time.deltaTime);
    }
}