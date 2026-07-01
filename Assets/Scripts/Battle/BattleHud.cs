using TMPro;
using UnityEngine;

public class BattleHud
{
    private readonly GameObject _battleTextHud;
    private readonly GameObject _statHud;
    private readonly GameObject _playerHud;
    private readonly GameObject _specialAttackButton;
    private readonly TMP_Text _battleText;
    private readonly TMP_Text _playerHealth;
    private readonly TMP_Text _enemyHealth;

    public BattleHud(GameObject battleTextHud, GameObject statHud, GameObject playerHud, GameObject specialAttackButton, TMP_Text battleText, TMP_Text playerHealth, TMP_Text enemyHealth)
    {
        _battleTextHud = battleTextHud;
        _statHud = statHud;
        _playerHud = playerHud;
        _specialAttackButton = specialAttackButton;
        _battleText = battleText;
        _playerHealth = playerHealth;
        _enemyHealth = enemyHealth;
    }

    public void SwitchHud(bool isBattleText = false, bool isStatHud = false)
    {
        _battleTextHud.SetActive(isBattleText);
        _statHud.SetActive(isStatHud);
    }

    public void SetText(string text) => _battleText.text = text;

    public void SetSpecialAttackButtonActive(bool value) => _specialAttackButton.SetActive(value);

    public void SetPlayerHudActive(bool value) => _playerHud.SetActive(value);

    public void UpdateHealth(BattleUnit playerUnit, BattleUnit enemyUnit)
    {
        _playerHealth.text = $"{playerUnit.currentHealth}/{playerUnit.stats.maxHealth}";
        _enemyHealth.text = $"{enemyUnit.currentHealth}/{enemyUnit.stats.maxHealth}";
    }
}
