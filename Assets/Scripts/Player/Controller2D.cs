using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(InputClass))]
public class Controller2D : MonoBehaviour
{
    // Variables
    [Header("Movement")]
    [SerializeField, Range(.5f, 15f)] private float targetSpeed;
    [SerializeField, Range(.5f, 30f)] private float acceleration;
    [SerializeField, Range(.1f, 10f)] private float decceleration;
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
    bool facingRight;

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
        UpdateMovement();
    }

    private void Update()
    {
        UpdateJump();
    }

    private void UpdateMovement()
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
        Body2D.AddForce(_currentVelocity);

        // Friction
        if (isGrounded && Mathf.Abs(_moveAxis.x) < 0.01f)
        {
            float _amount = Mathf.Min(Mathf.Abs(Body2D.velocity.x), Mathf.Abs(10f));
            _amount *= Mathf.Sign(Body2D.velocity.x);
            Body2D.AddForce(Vector2.right * -_amount * frictionPower, ForceMode2D.Impulse);
        }

        // Turn
        if (_moveAxis.x != 0)
        {
            Turn(_moveAxis.x < 0);
        }
    }

    private void UpdateJump()
    {
        // Jump
        bool _canJump, _keyJump, _cutJump, _startJump, _jumpTimer, _resetHoldPress;

        _canJump = isGrounded && jumpTimer <= 0;
        _keyJump = Input.Jump();
        _cutJump = Body2D.velocity.y > 0 && !_keyJump && isJumping;
        _startJump = _keyJump && _canJump && !lockHoldPressJumps;
        _jumpTimer = jumpTimer >= 0;
        _resetHoldPress = !_keyJump && isGrounded;

        if (_startJump)
        {
            Body2D.velocity += Vector2.up * jumpForce;
            jumpTimer = jumpCoutdown;

            lockHoldPressJumps = true;
            isJumping = true;
        }

        if (_cutJump)
        {
            Body2D.AddForce(Vector2.up * Body2D.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            isJumping = false;
        }

        if (_jumpTimer)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (_resetHoldPress)
        {
            lockHoldPressJumps = false;
        }

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

    private void Turn(bool _isRight)
    {
        if (_isRight != facingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            facingRight = !facingRight;
        }
    }

    private void Drag(float amount)
    {
        Vector2 force = amount * Body2D.velocity.normalized;
        force.x = Mathf.Min(Mathf.Abs(Body2D.velocity.x), Mathf.Abs(force.x));
        force.y = Mathf.Min(Mathf.Abs(Body2D.velocity.y), Mathf.Abs(force.y));
        force.x *= Mathf.Sign(Body2D.velocity.x);
        force.y *= Mathf.Sign(Body2D.velocity.y);

        Body2D.AddForce(-force, ForceMode2D.Impulse);
    }
}
