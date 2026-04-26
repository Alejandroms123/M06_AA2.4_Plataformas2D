using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private void Update()
    {
        float current = StaminaManager.instance.stamina;
        float max = StaminaManager.instance.maxStamina;

        _fillImage.fillAmount = current / max;
    }
}