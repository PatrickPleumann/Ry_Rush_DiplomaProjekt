using UnityEngine;
using UnityEngine.UI;

public class UI_Healthbar : MonoBehaviour
{
    [SerializeField] private CentralizedValues values;
    [SerializeField] private RectTransform healthbarFill;
    private float healthValueIntoNormalizedFillAmount;

    private void Start()
    {
        if (values.PlayerMaxHealth > 0)
            healthValueIntoNormalizedFillAmount = healthbarFill.localScale.x / values.PlayerMaxHealth;

        UpdateHealthbarUI(values.PlayerCurrentHealth);
    }

    private void OnEnable()
    {
        values.PlayerCurrentHealth_OnValueChanged.AddListener(UpdateHealthbarUI);
    }

    private void OnDisable()
    {
        values.PlayerCurrentHealth_OnValueChanged.RemoveListener(UpdateHealthbarUI);
    }

    private void UpdateHealthbarUI(float _playerCurrentHealth)
    {
        healthbarFill.localScale = new Vector3(_playerCurrentHealth * healthValueIntoNormalizedFillAmount, healthbarFill.localScale.y, 0f);
    }
}
