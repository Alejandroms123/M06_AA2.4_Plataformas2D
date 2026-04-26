using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Components")]
    [SerializeField] protected SpriteRenderer _sr;
    [SerializeField] protected Collider2D _col;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected AudioSource _deathSound;
    [SerializeField] protected ParticleSystem _deathParticles;
    [SerializeField] protected LayerMask _playerLyr;

    [Space(10), Header("Patrol Properties")]
    [SerializeField, Range(0f, 10f)] protected float _waitTime;
    [SerializeField] protected Transform[] _movePoints;

    protected Transform _currentMovePoint => _isStatic ? null : _movePoints[_currentMovePointIndex];
    protected int _currentMovePointIndex;

    protected float _vel;
    protected bool _isStatic;
    protected bool _isWaiting;

    protected Vector2 _startPos;
    protected Vector2 _patrolTarget => _isStatic ? _startPos : _currentMovePoint.position;

    private EnemyStates _state;
    protected enum EnemyStates
    {
        Idle,
        Chase,
        Attack,
        Return
    }

    protected EnemyStates State
    {
        get => _state;
        set { OnEnterState(value); _state = value; }
    }

    protected virtual void Start()
    {
        _isStatic = _movePoints == null || _movePoints.Length == 0;

        if (_sr == null) _sr = GetComponent<SpriteRenderer>();
        if (_col == null) _col = GetComponent<Collider2D>();
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();

        State = EnemyStates.Idle;
        _startPos = transform.position;
    }

    protected void Update()
    {
        switch (_state)
        {
            case EnemyStates.Idle: UpdateIdle(); break;
            case EnemyStates.Chase: UpdateChase(); break;
            case EnemyStates.Attack: UpdateAttack(); break;
            case EnemyStates.Return: UpdateReturn(); break;
        }

        UpdateAnimator();
    }

    protected void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyStates.Idle: FixedIdle(); break;
            case EnemyStates.Chase: FixedChase(); break;
            case EnemyStates.Attack: FixedAttack(); break;
            case EnemyStates.Return: FixedReturn(); break;
        }
    }

    public void Die()
    {
        StopAllCoroutines();

        if (_deathParticles != null)
        {
            _deathParticles.transform.parent = null;
            _deathParticles.Play();
            Destroy(_deathParticles.gameObject, 2f);
        }

        if (_deathSound != null)
        {
            AudioSource.PlayClipAtPoint(_deathSound.clip, transform.position);
        }

        gameObject.SetActive(false);
    }

    protected virtual void OnEnterState(EnemyStates state) { }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateChase() { }
    protected virtual void UpdateAttack() { }
    protected virtual void UpdateReturn() { }

    protected virtual void FixedIdle() { }
    protected virtual void FixedChase() { }
    protected virtual void FixedAttack() { }
    protected virtual void FixedReturn() { }

    protected void FlipEnemy(float dir, params Transform[] transforms)
    {
        _sr.flipX = dir < 0f;
        foreach (var t in transforms)
        {
            Vector3 pos = t.localPosition;
            pos.x = Mathf.Abs(pos.x) * Mathf.Sign(dir);
            t.localPosition = pos;
        }
    }

    protected void Decelerate(float time)
    {
        float deceleration = _vel / time;
        _rb.linearVelocity = Vector2.MoveTowards(_rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
    }

    protected void SetFarthestMovePoint()
    {
        if (_isStatic) return;

        float maxDist = float.MinValue;
        for (int i = 0; i < _movePoints.Length; i++)
        {
            if (_movePoints[i] == null) continue;
            float dist = Mathf.Abs(transform.position.x - _movePoints[i].position.x);
            if (dist > maxDist) { maxDist = dist; _currentMovePointIndex = i; }
        }
    }

    protected void AdvancePatrolPoint(System.Action onComplete = null)
    {
        _isWaiting = true;
        StartCoroutine(RunAfterTime(_waitTime, () =>
        {
            int next = _currentMovePointIndex;
            do
            {
                next = (next + 1) % _movePoints.Length;
            } while (_movePoints[next] == null && next != _currentMovePointIndex);

            if (_movePoints[next] == null)
            {
                _isStatic = true;
                _isWaiting = false;
                return;
            }

            _currentMovePointIndex = next;
            _isWaiting = false;
            onComplete?.Invoke();
        }));
    }

    protected IEnumerator RunAfterTime(float time, System.Action onComplete)
    {
        yield return new WaitForSeconds(time);
        onComplete?.Invoke();
    }

    public void FreezeUntilGrounded()
    {
        StartCoroutine(FreezeCoroutine());
    }

    private IEnumerator FreezeCoroutine()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.1f);
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    protected virtual void UpdateAnimator() { }
}