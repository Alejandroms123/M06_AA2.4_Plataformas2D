using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Components")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private PlayerCheck _playerCheck;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerJump _playerJump;

    [Space(10)]

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Animator _anim;
    [SerializeField] private Collider2D _col;
    [SerializeField] private Rigidbody2D _rb;

    public bool hasMoved { get; private set; } = false;
    public float lastMoveDir { get; private set; }

    private enum PlayerState
    {
        BaseState, Dashing, WallSliding
    }
    private PlayerState _currentPlayerState;

    private bool _isGrounded;
    private bool _isOnWall;
    private float _moveInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        _playerInput.GatherInput();

        _moveInput = _playerInput.moveInput;
        if (_moveInput != 0)
        {
            lastMoveDir = Mathf.Sign(_moveInput);
            _playerMovement.FlipSr(_sr, lastMoveDir);
        }

        _isGrounded = _playerCheck.IsGrounded(_col, _rb);
        _isOnWall = _playerCheck.IsOnWall(_col, _rb, lastMoveDir);

        _playerJump.UpdateJump(_playerInput.jumpPressed, _isGrounded);

        UpdatePlayerState();
        UpdateAnimatior();

        if (_rb.linearVelocity != Vector2.zero && hasMoved != true)
        {
            hasMoved = true;
        }
    }

    private void FixedUpdate()
    {
        switch (_currentPlayerState)
        {
            case PlayerState.WallSliding:

                _playerMovement.Move(_rb, _moveInput);
                _playerJump.WallSlide(_rb);
                _playerJump.HandleJump(_rb, !_isGrounded && _isOnWall, lastMoveDir);
                break;

            case PlayerState.BaseState:

                _playerMovement.Move(_rb, _moveInput);
                _playerJump.HandleJump(_rb, !_isGrounded && _isOnWall, lastMoveDir);
                _playerJump.JumpCancel(_rb, _playerInput.jumpHeld);
                break;
        }
    }

    private void UpdatePlayerState()
    {
        if (_isOnWall && !_isGrounded && _rb.linearVelocity.y < 0f)
        {
            _currentPlayerState = PlayerState.WallSliding;
            return;
        }

        _currentPlayerState = PlayerState.BaseState;
    }

    private void UpdateAnimatior()
    {
        _anim.SetFloat("VelocityX", Mathf.Abs(_moveInput));
        _anim.SetFloat("VelocityY", _rb.linearVelocityY);
        _anim.SetBool("IsGrounded", _isGrounded);
        _anim.SetBool("IsWallSliding", _currentPlayerState == PlayerState.WallSliding);
    }

    private void OnDrawGizmos()
    {
        if (_col == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.down * 0.12f, _col.bounds.size * 0.8f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + Vector3.right * lastMoveDir * 0.06f, _col.bounds.size * 0.8f);
    }
}