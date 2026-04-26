using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    public static StaminaManager instance;

    [Header("Porperties")]
    [field: SerializeField] public float maxStamina { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float _staminaLoseSpeed { get; private set; }
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

    public void LoseStamina(float amount)
    {
        _staminaRegenTimer = 0f;
        stamina = Mathf.Max(0f, stamina - amount);
    }

    public void RegenStamina(bool canRegen)
    {
        _staminaRegenTimer = Mathf.Min(_staminaRegenCooldown, _staminaRegenTimer + Time.deltaTime);

        if (_staminaRegenTimer >= _staminaRegenCooldown && canRegen)
        {
            stamina += _staminaRegenSpeed * Time.deltaTime;
            stamina = Mathf.Min(stamina, maxStamina);
        }
    }
}