using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(InputClass))]
public class Controller2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float targetSpeed;
    [SerializeField] private float frictionAmount;
    [SerializeField] private float acceleration, decceleration;
    [Space]
    [SerializeField, Range(.5f, 2f)] private float accelPower;
    [SerializeField, Range(.5f, 2f)] private float stopPower;
    [SerializeField, Range(.5f, 2f)] private float turnPower;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCutMultiplier;

    [Header("Settings")]
    [SerializeField] private BaseCheck groundCheck;

    bool isGrounded => groundCheck.IsActive();

    public bool isJumping;

    float jumpCoutdown = 0.1f;
    float jumpTimer = 0f;

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
        // Movement
        Vector2 _moveAxis = Input.MoveAxis();
        float _targetSpeed = targetSpeed * _moveAxis.x;
        float _speedDif = _targetSpeed - Body2D.velocity.x;
        float _accelRate = ((Mathf.Abs(_targetSpeed) > 0.01f) ? acceleration : decceleration);
        float _velPower;

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

        // Friction
        if (isGrounded && Mathf.Abs(_moveAxis.x) < 0.01f)
        {
            float _amount = Mathf.Min(Mathf.Abs(Body2D.velocity.x), Mathf.Abs(frictionAmount));
            _amount *= Mathf.Sign(Body2D.velocity.x);
            Body2D.AddForce(Vector2.right * -_amount, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        // Jump
        bool _canJump = isGrounded && jumpTimer <= 0;
        bool _keyJump = Input.Jump();

        if (_keyJump && _canJump)
        {
            Body2D.velocity += Vector2.up * jumpForce;
            jumpTimer = jumpCoutdown;
            isJumping = true;
        }

        if (Body2D.velocity.y > 0 && !_keyJump && isJumping)
        {
            Move(Vector2.up * Body2D.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
            Debug.Log("test");
            isJumping = false;
        }

        if (jumpTimer >= 0)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    private void Move(Vector2 _velocity, ForceMode2D _forceMode = default(ForceMode2D)) => Body2D.AddForce(_velocity, _forceMode);
}
