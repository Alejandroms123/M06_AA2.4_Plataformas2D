using UnityEngine;

public class EnemyCrawler : Enemy
{
    [Space(20), Header("EnemyCrawler Components")]
    [SerializeField] EnemyGroundCheck _enemyGroundCheck;
    [SerializeField] EnemyWallCheck _enemyWallCheck;

    [Space(10), Header("Movement Properties")]
    [SerializeField, Range(0f, 10f)] private float _speed;
    [SerializeField, Range(-1f, 1f)] float _edgeOffset;
    [SerializeField, Range(-1f, 1f)] float _wallOffset;

    private float _facingDir;
    private float _dirToPoint;

    private bool _movingOnWall => Mathf.Abs(Vector2.Dot(transform.up, Vector2.up)) < 0.7f;
    private bool _canRotate;

    protected override void Start()
    {
        _facingDir = _sr.flipX ? -1f : 1f;

        base.Start();

        _isWaiting = true;
        StartCoroutine(RunAfterTime(_waitTime, () => _isWaiting = false));
        StartCoroutine(RunAfterTime(_waitTime, () => _canRotate = true));
    }

    private void OnEnable()
    {
        _canRotate = false;
        _isWaiting = true;
        StartCoroutine(RunAfterTime(_waitTime, () =>
        {
            _canRotate = true;
            _isWaiting = false;
        }));
    }

    protected override void UpdateIdle()
    {
        if (_isWaiting || !_canRotate) return;

        if (!_isStatic)
        {
            if (_movingOnWall)
            {
                _dirToPoint = Mathf.Sign(_currentMovePoint.position.y - transform.position.y);
                if (Mathf.Abs(transform.position.y - _currentMovePoint.position.y) < 0.05f)
                    AdvancePatrolPoint();
            }
            else
            {
                _dirToPoint = Mathf.Sign(_currentMovePoint.position.x - transform.position.x);
                if (Mathf.Abs(transform.position.x - _currentMovePoint.position.x) < 0.05f)
                    AdvancePatrolPoint();
            }

            _facingDir = _dirToPoint;
            FlipEnemy(_dirToPoint, _enemyGroundCheck.transform, _enemyWallCheck.transform);
        }

        if (!_enemyGroundCheck.isGrounded)
            RotateAtEdge();
        else if (_enemyWallCheck.wallInFront)
            RotateAtWall();
    }

    protected override void FixedIdle()
    {
        if (_isWaiting)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        _rb.linearVelocity = (Vector2)transform.right * _facingDir * _speed;
    }

    private void RotateAtEdge()
    {
        _rb.linearVelocity = Vector2.zero;

        float offsetDiff = _col.bounds.extents.x + _edgeOffset;
        transform.position += transform.right * _facingDir * offsetDiff;

        transform.Rotate(0f, 0f, -90f * _facingDir);

        _canRotate = false;
        StartCoroutine(RunAfterTime(0.2f, () => _canRotate = true));
    }

    private void RotateAtWall()
    {
        _rb.linearVelocity = Vector2.zero;

        float offsetDiff = _col.bounds.extents.y + _wallOffset;
        transform.position += transform.right * _facingDir * offsetDiff;

        transform.Rotate(0f, 0f, 90f * _facingDir);

        _canRotate = false;
        StartCoroutine(RunAfterTime(0.2f, () => _canRotate = true));
    }

    protected override void UpdateAnimator()
    {
        if (_anim == null) return;

        if (_rb.linearVelocity.magnitude > 0.1f)
        {
            _anim.Play("Enemy_Idle");
        }
        else
        {
            _anim.Play("Enemy_Walk");
        }
    }
}