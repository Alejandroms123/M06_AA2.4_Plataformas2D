using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager instance;

    [Header("Porperties")]
    [field: SerializeField] public float maxStamina { get; private set; }
    [SerializeField, Range(0f, 10f)] private float _staminaLoseSpeed;
    [SerializeField, Range(0f, 10f)] private float _staminaRegenSpeed;
    [SerializeField, Range(0f, 10f)] private float _staminaRegenCooldown;

    public float stamina { get; private set; }

    private float _staminaRegenTimer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    private void Start() { stamina = maxStamina; }

    public void LoseStamina()
    {
        _staminaRegenTimer = 0f;
        stamina = Mathf.Lerp(stamina, stamina - 1f, _staminaLoseSpeed);
    }

    public void RegenStamina(bool isGrounded)
    {
        _staminaRegenTimer = Mathf.Min(_staminaRegenCooldown, _staminaRegenTimer + Time.deltaTime);

        if (_staminaRegenTimer >= _staminaRegenCooldown)
            stamina = Mathf.Lerp(stamina, maxStamina, _staminaRegenSpeed);
    }
}