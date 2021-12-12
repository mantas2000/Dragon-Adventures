using System.Collections;
using TMPro;
using UnityEngine;

public enum BattleState {Start, PlayerTurn, EnemyTurn, Won, Lost}

public class BattleManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private FighterInfo playerInfo;
	[SerializeField] private FighterInfo enemyInfo;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

    // Start is called before the first frame update
    private void Start()
    {
		state = BattleState.Start;
		StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
	{
		dialogueText.text = "A wild " + enemyInfo.fighterName + " approaches...";

		playerHUD.SetHUD(playerInfo);
		enemyHUD.SetHUD(enemyInfo);

		yield return new WaitForSeconds(2f);

		state = BattleState.PlayerTurn;
		PlayerTurn();
	}

    private IEnumerator PlayerAttack()
	{
		var isDead = enemyInfo.TakeDamage(playerInfo.damage);

		enemyHUD.SetHP(enemyInfo.currentHP);
		dialogueText.text = "The attack is successful!";

		yield return new WaitForSeconds(2f);

		if(isDead)
		{
			state = BattleState.Won;
			EndBattle();
		} else
		{
			state = BattleState.EnemyTurn;
			StartCoroutine(EnemyTurn());
		}
	}

    private IEnumerator EnemyTurn()
	{
		dialogueText.text = enemyInfo.fighterName + " attacks!";

		yield return new WaitForSeconds(1f);

		var isDead = playerInfo.TakeDamage(enemyInfo.damage);

		playerHUD.SetHP(playerInfo.currentHP);

		yield return new WaitForSeconds(1f);

		if(isDead)
		{
			state = BattleState.Lost;
			EndBattle();
		} else
		{
			state = BattleState.PlayerTurn;
			PlayerTurn();
		}

	}

    private void EndBattle()
    {
	    dialogueText.text = state switch
	    {
		    BattleState.Won => "You won the battle!",
		    BattleState.Lost => "You were defeated.",
		    _ => dialogueText.text
	    };
    }

    private void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}

    private IEnumerator PlayerHeal()
	{
		playerInfo.Heal(5);

		playerHUD.SetHP(playerInfo.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.EnemyTurn;
		StartCoroutine(EnemyTurn());
	}

	public void OnAttackButton()
	{
		if (state != BattleState.PlayerTurn)
			return;

		StartCoroutine(PlayerAttack());
	}

	public void OnHealButton()
	{
		if (state != BattleState.PlayerTurn)
			return;

		StartCoroutine(PlayerHeal());
	}

}
