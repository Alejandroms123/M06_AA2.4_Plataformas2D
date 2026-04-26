using TMPro;
using UnityEngine;
using System.Collections;

public class WorldHintUI : MonoBehaviour
{
    public static WorldHintUI Instance;

    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        Instance = this;
        _panel.SetActive(false);
    }

    public void ShowHint(string message)
    {
        StopAllCoroutines();

        _text.text = message;
        _panel.SetActive(true);

        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        _animator.Play("Hint_In", 0, 0f);

        yield return WaitForAnimation("Hint_In");

        _animator.Play("Hint_Idle", 0, 0f);
    }

    public void HideHint()
    {
        StopAllCoroutines();
        StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        _animator.Play("Hint_Out", 0, 0f);

        yield return WaitForAnimation("Hint_Out");

        _panel.SetActive(false);
    }

    private IEnumerator WaitForAnimation(string animName)
    {
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            yield return null;

        while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;
    }
}