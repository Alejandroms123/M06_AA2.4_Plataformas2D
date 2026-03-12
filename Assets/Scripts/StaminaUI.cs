using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private StaminaManager _stamina;

    private void Awake()
    {
        if (_slider == null) _slider = GetComponent<Slider>();
    }

    private void Start()
    {
        // Config inicial
        _slider.minValue = 0f;
        _slider.maxValue = _stamina.maxStamina;
        _slider.value = _stamina.stamina;
    }

    private void Update()
    {
        // Simple (vale para demo): actualiza cada frame
        _slider.value = _stamina.stamina;
    }
}