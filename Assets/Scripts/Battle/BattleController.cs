using System;
using System.Collections;
using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Won, Lost }

public class BattleController : MonoBehaviour
{
    public Flowchart flowchart;
    public BattleState battleState;
    public GameObject statHUD;
    public GameObject battleTextHUD;
    public GameObject playerHUD;
    public GameObject specialAttackButton;
    public CharacterStats playerStats;
    public CharacterStats enemyStats;
    public BattleUnit playerUnit;
    public BattleUnit enemyUnit;
    public TMP_Text battleText;
    public TMP_Text playerHealth;
    public TMP_Text enemyHealth;
    public float moveSpeed = 12f;
    public float stoppingDistance = 1.5f;
    public AudioSource audioSource;
    public AudioClip playerAttackSound;
    public AudioClip enemyAttackSound;
    public AudioClip playerDefenseSound;
    public AudioClip playerSpecialSound;
    private static readonly WaitForSeconds _waitForSeconds_5 = new(.5f);
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);
    private bool isPlayerDefending;
    private bool isSpecialAttackUsed = false;
    private BattleHud battleHud;
    private BattleAudio battleAudio;

    void Awake()
    {
        battleHud = new BattleHud(battleTextHUD, statHUD, playerHUD, specialAttackButton, battleText, playerHealth, enemyHealth);
        battleAudio = new BattleAudio(audioSource, playerAttackSound, enemyAttackSound, playerDefenseSound, playerSpecialSound);
    }

    void Start()
    {
        battleState = BattleState.Start;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        battleHud.SetSpecialAttackButtonActive(false);
        playerUnit.Setup();
        enemyUnit.Setup();
        battleHud.UpdateHealth(playerUnit, enemyUnit);

        battleHud.SwitchHud(true, false);
        battleHud.SetText("Kill the " + enemyStats.charName + "!");

        yield return _waitForSeconds_5;
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        battleHud.SwitchHud(true, false);
        battleState = BattleState.PlayerTurn;
        battleHud.SetText("Choose an Action!");
        yield return _waitForSeconds_5;

        if (playerUnit.currentHealth <= 5 && !isSpecialAttackUsed)
        {
            battleHud.SetSpecialAttackButtonActive(true);
        }

        battleHud.SwitchHud(false, true);
    }

    private IEnumerator EnemyTurn()
    {
        battleHud.SwitchHud(true, false);
        battleState = BattleState.EnemyTurn;
        battleHud.SetText("Watch out of the attack!");

        yield return _waitForSeconds_5;

        Vector3 startPos = enemyUnit.transform.position;
        Vector3 dir = (startPos - playerUnit.transform.position).normalized;
        Vector3 attackPos = playerUnit.transform.position + dir * stoppingDistance;

        yield return enemyUnit.MoveTo(attackPos, moveSpeed);

        enemyUnit.PlayAnimation("ShamanAttack");
        battleAudio.PlayEnemyAttack();

        int damage = UnityEngine.Random.Range(enemyUnit.stats.minAttackDamage, enemyUnit.stats.maxAttackDamage + 1);
        if (isPlayerDefending)
        {
            damage = Mathf.Max(0, Mathf.RoundToInt(damage * 0.5f) - 1);
            isPlayerDefending = false;
        }

        bool isDead = playerUnit.TakeDamage(damage);
        battleHud.UpdateHealth(playerUnit, enemyUnit);
        yield return _waitForSeconds1;

        yield return enemyUnit.MoveTo(startPos, moveSpeed);

        if (isDead)
        {
            battleState = BattleState.Lost;
            EndBattle();
        }
        else
        {
            battleState = BattleState.PlayerTurn;
            StartCoroutine(PlayerTurn());
        }
    }

    public void OnAttackButton()
    {
        if (battleState != BattleState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerAttack("You attack!", () => UnityEngine.Random.Range(playerStats.minAttackDamage, playerStats.maxAttackDamage + 1), battleAudio.PlayPlayerAttack));
    }

    public void OnDefenseButton()
    {
        if (battleState != BattleState.PlayerTurn)
        {
            return;
        }

        StartCoroutine(PlayerDefense());
    }

    public void OnSpecialAttackButton()
    {
        if (battleState != BattleState.PlayerTurn || isSpecialAttackUsed)
        {
            return;
        }

        isSpecialAttackUsed = true;
        StartCoroutine(PlayerAttack("SPECIAL ATTACKU!", () => 11, battleAudio.PlayPlayerSpecial));
    }

    private IEnumerator PlayerAttack(string text, Func<int> damage, Action playSound)
    {
        battleHud.SwitchHud(true, false);
        battleHud.SetText(text);
        Vector3 startPos = playerUnit.transform.position;

        Vector3 dir = (startPos - enemyUnit.transform.position).normalized;
        Vector3 attackPos = enemyUnit.transform.position + dir * stoppingDistance;

        yield return playerUnit.MoveTo(attackPos, moveSpeed);

        playerUnit.PlayAnimation("PlayerAttack");
        playSound();

        bool isDead;
        if (isSpecialAttackUsed)
        {
            isDead = enemyUnit.TakeDamagePrecise(damage());
        }
        else
        {
            isDead = enemyUnit.TakeDamage(damage());
        }

        battleHud.UpdateHealth(playerUnit, enemyUnit);
        yield return _waitForSeconds_5;

        yield return playerUnit.MoveTo(startPos, moveSpeed);

        if (isDead)
        {
            battleState = BattleState.Won;
            battleHud.SwitchHud(false, false);
            EndBattle();
        }
        else
        {
            battleState = BattleState.EnemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator PlayerDefense()
    {
        isPlayerDefending = true;
        battleHud.SwitchHud(true, false);
        battleHud.SetText("Defend the attack!");
        playerUnit.currentHealth += 1;

        playerUnit.PlayAnimation("PlayerDefense");
        battleAudio.PlayPlayerDefense();
        battleHud.UpdateHealth(playerUnit, enemyUnit);

        yield return _waitForSeconds1;

        battleState = BattleState.EnemyTurn;
        StartCoroutine(EnemyTurn());
    }

    public void EndBattle()
    {
        battleHud.SwitchHud(false, false);
        battleHud.SetPlayerHudActive(false);

        if (battleState == BattleState.Won)
        {
            flowchart.ExecuteBlock("WonBattle");
        }
        else if (battleState == BattleState.Lost)
        {
            flowchart.ExecuteBlock("LoseBattle");
        }
    }

    public void LoadWinScene() => SceneManager.LoadScene("WinScene");
    public void LoadLoseScene() => SceneManager.LoadScene("LoseScene");
}
