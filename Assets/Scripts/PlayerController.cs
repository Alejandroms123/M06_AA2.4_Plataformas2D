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
        OnGround, OnWall
    }
    private PlayerState _currentPlayerState;

    public bool _isGrounded;
    public bool _isOnWall;
    public float _moveXInput;
    private float _moveYInput;

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

        _moveXInput = _playerInput.xInput;
        _moveYInput = _playerInput.yInput;

        if (_moveXInput != 0)
        {
            lastMoveDir = Mathf.Sign(_moveXInput);
            _playerMovement.FlipSr(_sr, lastMoveDir);
        }

        _isGrounded = _playerCheck.IsGrounded(_col, _rb);
        _isOnWall = _playerCheck.IsOnWall(_col, _rb, lastMoveDir);

        StaminaManager.instance.RegenStamina(_isGrounded);
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
            case PlayerState.OnWall:

                _playerMovement.MoveX(_rb, _moveXInput);
                _playerJump.WallSlide(_rb);
                _playerJump.Climb(_rb, _moveYInput);
                _playerJump.HandleJump(_rb, !_isGrounded && _isOnWall, lastMoveDir);
                break;

            case PlayerState.OnGround:

                _playerMovement.MoveX(_rb, _moveXInput);
                _playerJump.HandleJump(_rb, !_isGrounded && _isOnWall, lastMoveDir);
                _playerJump.JumpCancel(_rb, _playerInput.jumpHeld);
                break;
        }
    }

    private void UpdatePlayerState()
    {
        if (_isOnWall && !_isGrounded)
        {
            _currentPlayerState = PlayerState.OnWall;
            return;
        }

        _currentPlayerState = PlayerState.OnGround;
    }

    private void UpdateAnimatior()
    {
        _anim.SetFloat("VelocityX", Mathf.Abs(_moveXInput));
        _anim.SetFloat("VelocityY", _rb.linearVelocityY);
        _anim.SetBool("IsGrounded", _isGrounded);
        _anim.SetBool("IsWallSliding", _currentPlayerState == PlayerState.OnWall);
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