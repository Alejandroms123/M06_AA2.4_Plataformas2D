using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public static PlayerStamina instance;

    [Header("Porperties")]
    [field: SerializeField] public float maxStamina { get; private set; }

    public float stamina { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        stamina = maxStamina;
    }
}