using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(InputClass))]
public class Controller2D : MonoBehaviour
{
    // Variables
    [Header("Movement")]
    [SerializeField] private float targetSpeed;
    [SerializeField] private float acceleration, decceleration;
    [Space]
    [SerializeField, Range(.5f, 2f)] private float accelPower;
    [SerializeField, Range(.5f, 2f)] private float stopPower;
    [SerializeField, Range(.5f, 2f)] private float turnPower;
    [SerializeField, Range(.1f, 10f)] private float frictionPower;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCutMultiplier;

    [Header("Gravity")]
    [SerializeField] private float gravityScale;
    [SerializeField] private float fallGravityMultiplier;

    [Header("Extra Settings")]
    [SerializeField] private BaseCheck groundCheck;

    // Bools
    bool isGrounded => groundCheck.IsActive();
    bool isJumping;
    bool lockHoldPressJumps;

    // Floats
    float jumpCoutdown = 0.1f;
    float jumpTimer = 0f;

    // Components
    Rigidbody2D Body2D;
    CapsuleCollider2D BoxCol2D;
    InputClass Input;

    private void Start()
    {
        Body2D = GetComponent<Rigidbody2D>();
        BoxCol2D = GetComponent<CapsuleCollider2D>();
        Input = GetComponent<InputClass>();
    }

    private void FixedUpdate()
    {
        Movement();
        Friction();
    }

    private void Update()
    {
        Jump();
        JumpGravity();
    }

    private void Movement()
    {
        // Movement
        Vector2 _moveAxis = Input.MoveAxis();

        float _targetSpeed, _speedDif, _accelRate, _velPower;

        _targetSpeed = targetSpeed * _moveAxis.x;
        _speedDif = _targetSpeed - Body2D.velocity.x;
        _accelRate = ((Mathf.Abs(_targetSpeed) > 0.01f) ? acceleration : decceleration);

        if (Mathf.Abs(_targetSpeed) < 0.01f)
        {
            _velPower = stopPower;
        }
        else if (Mathf.Abs(Body2D.velocity.x) > 0 && (Mathf.Sign(_targetSpeed) != Mathf.Sign(Body2D.velocity.x)))
        {
            _velPower = turnPower;
        }
        else
        {
            _velPower = accelPower;
        }

        Vector2 _currentVelocity = new Vector2(Mathf.Pow(Mathf.Abs(_speedDif) * _accelRate, _velPower) * Mathf.Sign(_speedDif), 0);
        Move(_currentVelocity);
    }

    private void Friction()
    {
        // Friction
        float _moveAxisX = Input.MoveAxis().x;

        if (isGrounded && Mathf.Abs(_moveAxisX) < 0.01f)
        {
            float _amount = Mathf.Min(Mathf.Abs(Body2D.velocity.x), Mathf.Abs(10f));
            _amount *= Mathf.Sign(Body2D.velocity.x);
            Body2D.AddForce(Vector2.right * -_amount * frictionPower, ForceMode2D.Impulse);
        }
    }

    private void Jump()
    {
        // Jump
        bool _canJump, _keyJump, _canCutJump, _startJump, _canUpdateJumpTimer;

        _canJump = isGrounded && jumpTimer <= 0;
        _keyJump = Input.Jump();
        _canCutJump = Body2D.velocity.y > 0 && !_keyJump && isJumping;
        _startJump = _keyJump && _canJump && !lockHoldPressJumps;
        _canUpdateJumpTimer = jumpTimer >= 0;

        if (_startJump)
        {
            Body2D.velocity += Vector2.up * jumpForce;
            jumpTimer = jumpCoutdown;

            lockHoldPressJumps = true;
            isJumping = true;
        }

        if (_canCutJump)
        {
            Move(Vector2.up * Body2D.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            isJumping = false;
        }

        if (_canUpdateJumpTimer)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (!_keyJump && isGrounded)
        {
            lockHoldPressJumps = false;
        }
    }

    private void JumpGravity()
    {
        // Jump Gravity
        if (Body2D.velocity.y < 0)
        {
            Body2D.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else
        {
            Body2D.gravityScale = gravityScale;
        }
    }

    private void Move(Vector2 _velocity, ForceMode2D _forceMode = default(ForceMode2D)) => Body2D.AddForce(_velocity, _forceMode);
}
