using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(InputClass))]
public class Controller2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private PhysicsMaterial2D physicsMat;

    [Header("Jump")]
    [SerializeField] private float jumpForce;

    Rigidbody2D Body2D;
    BoxCollider2D BoxCol2D;
    CircleCollider2D CircleCol2D;
    InputClass Input;

    private void Start()
    {
        Body2D = GetComponent<Rigidbody2D>();
        BoxCol2D = GetComponent<BoxCollider2D>();
        CircleCol2D = GetComponent<CircleCollider2D>();
        Input = GetComponent<InputClass>();

        CircleCol2D.sharedMaterial = physicsMat;
    }

    private void Update()
    {
        Vector2 _moveAxis = Input.MoveAxis();
        Vector2 _currentVelocity = new Vector2(moveSpeed * _moveAxis.x, 0);

        if (_moveAxis.x != 0)
        {
            Move(_currentVelocity, ForceMode2D.Force);
            SetDrag(0);
            SetFriction(0);
        }
        else // if (isGrounded)
        {
            // Friction
            SetDrag(9999);
            SetFriction(9999);
        }
    }

    private void Move(Vector2 _velocity, ForceMode2D _forceMode) => Body2D.AddForce(_velocity, _forceMode);
    private void SetDrag(float _drag) => Body2D.drag = _drag;
    private void SetFriction(float _friction) => physicsMat.friction = _friction;
}
