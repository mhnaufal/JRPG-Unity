using System.Collections;
using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }

public class BattleController : MonoBehaviour
{
    public Flowchart flowchart;
    public BattleState battleState;
    public GameObject statHUD;
    public GameObject battleTextHUD;
    public GameObject playerHUD;
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
    private static WaitForSeconds _waitForSeconds_5 = new(.5f);
    private static WaitForSeconds _waitForSeconds1 = new(1f);
    private static WaitForSeconds _waitForSeconds2 = new(2);
    private bool isPlayerDefending;

    void Start()
    {
        battleState = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        UpdateHealthUI();

        SwitchHUD(true, false);
        battleText.text = "Kill the " + enemyStats.charName + "!";

        yield return _waitForSeconds_5;
        StartCoroutine(PlayerTurn());
    }

    private IEnumerator PlayerTurn()
    {
        SwitchHUD(true, false);
        battleState = BattleState.PLAYER_TURN;
        battleText.text = "Choose an Action!";
        yield return _waitForSeconds_5;
        SwitchHUD(false, true);
    }

    private IEnumerator EnemyTurn()
    {
        SwitchHUD(true, false);
        battleState = BattleState.ENEMY_TURN;
        battleText.text = "Watch out of the attack!";

        yield return _waitForSeconds_5;

        Vector3 startPos = enemyUnit.transform.position;
        Vector3 dir = (startPos - playerUnit.transform.position).normalized;
        Vector3 attackPos = playerUnit.transform.position + dir * stoppingDistance;

        yield return enemyUnit.MoveTo(attackPos, moveSpeed);

        enemyUnit.PlayAnimation("ShamanAttack");
        audioSource.PlayOneShot(enemyAttackSound);

        int damage = Random.Range(enemyUnit.stats.minAttackDamage, enemyUnit.stats.maxAttackDamage + 1);
        if (isPlayerDefending)
        {
            damage = Mathf.Max(0, Mathf.RoundToInt(damage * 0.5f) - 1);
            isPlayerDefending = false;
        }

        bool isDead = playerUnit.TakeDamage(damage);
        UpdateHealthUI();
        yield return _waitForSeconds1;

        yield return enemyUnit.MoveTo(startPos, moveSpeed);

        if (isDead)
        {
            battleState = BattleState.LOST;
            EndBattle();
        }
        else
        {
            battleState = BattleState.PLAYER_TURN;
            StartCoroutine(PlayerTurn());
        }
    }

    public void OnAttackButton()
    {
        if (battleState != BattleState.PLAYER_TURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

    private IEnumerator PlayerAttack()
    {
        SwitchHUD(true, false);
        battleText.text = "You attack!";
        Vector3 startPos = playerUnit.transform.position;

        Vector3 dir = (startPos - enemyUnit.transform.position).normalized;
        Vector3 attackPos = enemyUnit.transform.position + dir * stoppingDistance;

        yield return playerUnit.MoveTo(attackPos, moveSpeed);

        playerUnit.PlayAnimation("PlayerAttack");
        audioSource.PlayOneShot(playerAttackSound);

        bool isDead = enemyUnit.TakeDamage(Random.Range(playerStats.minAttackDamage, playerStats.maxAttackDamage + 1));
        UpdateHealthUI();
        yield return _waitForSeconds_5;

        yield return playerUnit.MoveTo(startPos, moveSpeed);

        if (isDead)
        {
            battleState = BattleState.WON;
            SwitchHUD(false, false);
            EndBattle();
        }
        else
        {
            battleState = BattleState.ENEMY_TURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void OnDefenseButton()
    {
        if (battleState != BattleState.PLAYER_TURN)
        {
            return;
        }

        StartCoroutine(PlayerDefense());
    }

    public void EndBattle()
    {
        SwitchHUD(false, false);
        playerHUD.SetActive(false);

        if (battleState == BattleState.WON)
        {
            flowchart.ExecuteBlock("WonBattle");
        }
        else if (battleState == BattleState.LOST)
        {
            flowchart.ExecuteBlock("LoseBattle");
        }
    }

    private IEnumerator PlayerDefense()
    {
        isPlayerDefending = true;
        SwitchHUD(true, false);
        battleText.text = "Defend the attack!";
        playerUnit.currentHealth += 1;

        playerUnit.PlayAnimation("PlayerDefense");
        audioSource.PlayOneShot(playerDefenseSound);

        yield return _waitForSeconds1;

        battleState = BattleState.ENEMY_TURN;
        StartCoroutine(EnemyTurn());
    }

    private void SwitchHUD(bool isBattleText = false, bool isStatHUD = false)
    {
        battleTextHUD.SetActive(isBattleText);
        statHUD.SetActive(isStatHUD);
    }

    private void UpdateHealthUI()
    {
        playerHealth.text = $"{playerUnit.currentHealth}/{playerUnit.stats.maxHealth}";
        enemyHealth.text = $"{enemyUnit.currentHealth}/{enemyUnit.stats.maxHealth}";
    }

    public void LoadWinScene() => SceneManager.LoadScene("WinScene");
    public void LoadLoseScene() => SceneManager.LoadScene("LoseScene");
}
