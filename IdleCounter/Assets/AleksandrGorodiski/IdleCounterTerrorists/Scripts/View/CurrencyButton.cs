using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyButton : GameElement
{
    [SerializeField]
    public Button _button;
    [SerializeField]
    public TextMeshProUGUI _buttonText;
    [SerializeField]
    public CurrencyModel currencyModel;

    public string GetTextSprite(int _spriteID)
    {
        return "<sprite=" + _spriteID + ">";
    }

    string GetNewLine()
    {
        return "\n";
    }

    public void SetButtonInteraction(bool _status)
    {
        if(_button) _button.interactable = _status;
    }

    public void SetButtonIconText(int _spriteID)
    {
        _buttonText.text = GetTextSprite(_spriteID);
    }

    public void SetButtonText(string _textAction)
    {
        _buttonText.text = _textAction;
    }
    public void SetButtonText(string _textSprite, string _textCost)
    {
        string _text = _textSprite + " " + _textCost;
        _buttonText.text = _text;
    }
    public void SetButtonText(string _textAction, string _textSprite,  string _textCost)
    {
        string _text = _textAction + GetNewLine() + _textSprite + " " + _textCost;
        _buttonText.text = _text;
    }

    public void SetButtonVisibility(bool _visibility)
    {
        if (gameObject.activeInHierarchy != _visibility)
        {
            gameObject.SetActive(_visibility);
        }
    }
}
