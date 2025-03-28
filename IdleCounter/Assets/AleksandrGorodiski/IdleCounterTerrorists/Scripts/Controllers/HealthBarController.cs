using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image healthBar;

    public void UpdateBar(float value)
    {
        healthBar.fillAmount = value;
        SetVisibility(value);
    }

    void SetVisibility(float value)
    {
        if (value <= 0f || value >= 1f) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}