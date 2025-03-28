using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class BalanceView : MonoBehaviour
{
    public BalanceHud balanceHud;

    public void UpdateCashText(long prevValue, long newValue)
    {
        StartCoroutine(CoroutineCountTo(balanceHud.cashText, prevValue, newValue));
        PlayScaleIn(balanceHud.cashText.GetComponent<RectTransform>());
    }

    IEnumerator CoroutineCountTo(TextMeshProUGUI _text, long _prevValue, long _newValue)
    {
        long _value;
        float duration = 0.2f;
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            _value = (long)Mathf.Lerp(_prevValue, _newValue, progress);
            _text.text = _value.ToString();
            yield return null;
        }
        _value = _newValue;
        _text.text = _value.ToString();
    }

    void PlayScaleIn(RectTransform _rectTransform)
    {
        _rectTransform?.DOKill();
        _rectTransform?.DOScale(Vector3.one * 0.7f, 0.1f).SetEase(Ease.InOutQuad).OnComplete(() =>
        { 
            _rectTransform?.DOKill();
            _rectTransform?.DOScale(Vector3.one, 0.16f).SetEase(Ease.OutBack, 6f);
        });
    }
}
