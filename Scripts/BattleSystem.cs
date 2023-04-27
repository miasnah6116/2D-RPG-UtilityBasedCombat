using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, PLAYERACTION }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerBattlePrefab;
    public GameObject enemyPrefab;
    
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    public bool ultimate = false;

    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerBattlePrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches...";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator EnemyTurn()
    {      
        // Create empty array to hold action choices
        List<string> enemyActionChoice = new List<string>();

        int physAttackUtility;
        int magAttackUtility;
        int healUtility;
        int ultimateUtility;
        int defendUtility;
        int runUtility;

        int hpDifference = (enemyUnit.maxHP - enemyUnit.currentHP);
        int percentageHP = (enemyUnit.currentHP / enemyUnit.maxHP * 100);
        int defenseDiff = (enemyUnit.physDamage - playerUnit.defense);
        if(defenseDiff < 0)
        {
            defenseDiff = 0;
        }

        bool isDead = false;

        // Utility based decision making for enemy combat
        physAttackUtility = ((1 + (playerUnit.maxHP - playerUnit.currentHP)) * defenseDiff);
        magAttackUtility = ((1 + (playerUnit.maxHP - playerUnit.currentHP)) * enemyUnit.magDamage);
        healUtility = (hpDifference / 4);
        if(percentageHP > 50)
            ultimateUtility = (1 * defenseDiff);
        else
            ultimateUtility = ((hpDifference / 4) * defenseDiff);
        defendUtility = 5;
        runUtility = (hpDifference / 25);

        // Fill enemyActionChoice array with choices based on utility
        for(int i = 0; i < physAttackUtility; i++)
        {
            enemyActionChoice.Add("physAttack");
        }
        for(int i = 0; i < magAttackUtility; i++)
        {
            enemyActionChoice.Add("magAttack");
        }
        for(int i = 0; i < healUtility; i++)
        {
            enemyActionChoice.Add("heal");
        }
        for(int i = 0; i < ultimateUtility; i++)
        {
            enemyActionChoice.Add("ultimateAttack");
        }
        for(int i = 0; i < defendUtility; i++)
        {
            enemyActionChoice.Add("defend");
        }
        for(int i = 0; i < runUtility; i++)
        {
            enemyActionChoice.Add("run");
        }

        // Random choice made with weighted choice based on utility
        var random = new System.Random();
        int listRange = enemyActionChoice.Count;
        int rInt = random.Next(0, listRange);
        string enemyChoice = enemyActionChoice[rInt];

        yield return new WaitForSeconds(1f);

        // Choose between PhysAttack, MagAttack, Heal, Ultimate, Run
        if(ultimate == true)
        {
            dialogueText.text = enemyUnit.unitName + " jumps up and body slams you!!";

            yield return new WaitForSeconds(1f);

            isDead = playerUnit.TakeDamage((enemyUnit.physDamage - playerUnit.defense) * 4);
            playerHUD.SetHP(playerUnit.currentHP);

            ultimate = false;
        }
        else if(enemyChoice == "physAttack")
        {
            dialogueText.text = enemyUnit.unitName + " attacks with its club!";

            yield return new WaitForSeconds(1f);

            isDead = playerUnit.TakeDamage(enemyUnit.physDamage - playerUnit.defense);
            playerHUD.SetHP(playerUnit.currentHP);
        }
        else if(enemyChoice == "magAttack")
        {
            dialogueText.text = enemyUnit.unitName + " throws some mud!";

            yield return new WaitForSeconds(1f);

            isDead = playerUnit.TakeDamage(enemyUnit.magDamage);
            playerHUD.SetHP(playerUnit.currentHP);
        }
        else if(enemyChoice == "heal")
        {
            dialogueText.text = enemyUnit.unitName + " heals its wounds!";

            yield return new WaitForSeconds(1f);

            enemyUnit.Heal(enemyUnit.magDamage);
            enemyHUD.SetHP(enemyUnit.currentHP);

            state = BattleState.PLAYERTURN;
        }
        else if(enemyChoice == "ultimateAttack")
        {
            ultimate = true;
            dialogueText.text = enemyUnit.unitName + " is charging up for a big attack!";

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
        }
        else if(enemyChoice == "defend")
        {
            enemyUnit.Defend(1);
            dialogueText.text = enemyUnit.unitName + " increases its defense!";

            yield return new WaitForSeconds(1f);

            state = BattleState.PLAYERTURN;
        }
        else if(enemyChoice == "run")
        {
            dialogueText.text = enemyUnit.unitName + " runs away!!";

            yield return new WaitForSeconds(1f);

            state = BattleState.WON;
        }

        enemyActionChoice.Clear();

        if(isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        } 
        else if(state == BattleState.WON)
        {
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            dialogueText.text = "You won the battle!";
        } 
        else if(state == BattleState.LOST)
        {
            dialogueText.text = "You were defeated...";
        }

        yield return new WaitForSeconds(1f);

        if(state == BattleState.WON)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
        }
        else if(state == BattleState.LOST)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }
    }

    void PlayerTurn()
    {
        dialogueText.text = "Choose an action:";
    }

    IEnumerator PlayerPhysAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.physDamage - enemyUnit.defense);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "You slash the " + enemyUnit.unitName + "!";

        if(isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        } 
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        yield return new WaitForSeconds(2f);
    }

    public void OnPhysAttackButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;

        state = BattleState.PLAYERACTION;
        StartCoroutine(PlayerPhysAttack());
    }

    IEnumerator PlayerMagAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.magDamage);

        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = "You throw a fireball!";

        if(isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        } 
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        yield return new WaitForSeconds(2f);
    }

    public void OnMagAttackButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;

        state = BattleState.PLAYERACTION;
        StartCoroutine(PlayerMagAttack());
    }

    IEnumerator PlayerHealAction()
    {
        playerUnit.Heal(playerUnit.magDamage);

        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

        yield return new WaitForSeconds(2f);
    }

    public void OnHealActionButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;

        state = BattleState.PLAYERACTION;
        StartCoroutine(PlayerHealAction());
    }

    IEnumerator PlayerDefendAction()
    {
        playerUnit.Defend(1);

        dialogueText.text = "You raise your shield!";

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

        yield return new WaitForSeconds(2f);
    }

    public void OnDefendActionButton()
    {
        if(state != BattleState.PLAYERTURN)
            return;

        state = BattleState.PLAYERACTION;
        StartCoroutine(PlayerDefendAction());
    }
}
