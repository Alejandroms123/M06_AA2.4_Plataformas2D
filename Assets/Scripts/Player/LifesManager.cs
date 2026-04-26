using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class LifesManager : MonoBehaviour
{
    public static LifesManager Instance { get; private set; }

    public int currentLifes;
    [SerializeField] private int maxLifes;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _invulnerabilityDuration = 1f;
    Vector3 spawnLocation;

    public UnityEvent ZeroLifeRemaining;
    public UnityEvent OneLifeRemaining;
    public UnityEvent TwoLifeRemaining;
    public UnityEvent ThreeLifeRemaining;
    public bool _isInvulnerable = false;
    private bool _isDead = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);

        maxLifes = 3;
        currentLifes = maxLifes;
    }

    private void Start()
    {
        spawnLocation = transform.position;
    }

    private void Update()
    {
        switch (currentLifes)
        {
            case 0: ZeroLifeRemaining.Invoke(); break;
            case 1: OneLifeRemaining.Invoke(); break;
            case 2: TwoLifeRemaining.Invoke(); break;
            case 3: ThreeLifeRemaining.Invoke(); break;
        }

        if (currentLifes <= 0 && !_isDead)
        {
            _isDead = true;
            StartCoroutine(ReloadScene());
        }
    }

    public void getHurt(int damage)
    {
        currentLifes -= damage;
        StartCoroutine(InvulnerabilityCoroutine());
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(_invulnerabilityDuration);
        _isInvulnerable = false;
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}