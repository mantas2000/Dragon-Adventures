using System.Collections;
using TMPro;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private FighterInfo playerFighterInfo;
	[SerializeField] private FighterInfo enemyFighterInfo;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

    // Start is called before the first frame update
    private void Start()
    {
		state = BattleState.START;
		StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
	{
		dialogueText.text = "A wild " + enemyFighterInfo.fighterName + " approaches...";

		playerHUD.SetHUD(playerFighterInfo);
		enemyHUD.SetHUD(enemyFighterInfo);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

    private IEnumerator PlayerAttack()
	{
		bool isDead = enemyFighterInfo.TakeDamage(playerFighterInfo.damage);

		enemyHUD.SetHP(enemyFighterInfo.currentHP);
		dialogueText.text = "The attack is successful!";

		yield return new WaitForSeconds(2f);

		if(isDead)
		{
			state = BattleState.WON;
			EndBattle();
		} else
		{
			state = BattleState.ENEMYTURN;
			StartCoroutine(EnemyTurn());
		}
	}

    private IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyFighterInfo.fighterName + " attacks!";

		yield return new WaitForSeconds(1f);

		bool isDead = playerFighterInfo.TakeDamage(enemyFighterInfo.damage);

		playerHUD.SetHP(playerFighterInfo.currentHP);

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.LOST;
			EndBattle();
		} else
		{
			state = BattleState.PLAYERTURN;
			PlayerTurn();
		}

	}

    private void EndBattle()
	{
		if(state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		} else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

    private void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

    private IEnumerator PlayerHeal()
	{
		playerFighterInfo.Heal(5);

		playerHUD.SetHP(playerFighterInfo.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.ENEMYTURN;
		StartCoroutine(EnemyTurn());
	}

	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
		
		Debug.Log("ATTACK");

		StartCoroutine(PlayerAttack());
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

		StartCoroutine(PlayerHeal());
	}

}
