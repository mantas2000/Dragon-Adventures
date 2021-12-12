using System.Collections;
using TMPro;
using UnityEngine;

public enum BattleState {Start, PlayerTurn, EnemyTurn, Won, Lost}

public class BattleManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private FighterInfo playerInfo;
	[SerializeField] private FighterInfo enemyInfo;
	[SerializeField] private BattleHUD _playerHUD;
	[SerializeField] private BattleHUD _enemyHUD;
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

		_playerHUD.SetHUD(playerInfo);
		_enemyHUD.SetHUD(enemyInfo);

		yield return new WaitForSeconds(2f);

		state = BattleState.PlayerTurn;
		PlayerTurn();
	}
    
    private void EnemyTurn()
    {
	    var enemyMove = (Random.value < 0.5);

	    StartCoroutine(enemyMove ? EnemyAttack() : EnemyHeal());
    }
    
    private void PlayerTurn()
    {
	    dialogueText.text = "Choose an action:";
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

    private IEnumerator PlayerAttack()
	{
		var isDead = enemyInfo.TakeDamage(playerInfo.damage);

		_enemyHUD.SetHP(enemyInfo.currentHP);
		dialogueText.text = "The attack is successful!";

		yield return new WaitForSeconds(2f);

		if(isDead)
		{
			state = BattleState.Won;
			EndBattle();
		} 
		else
		{
			state = BattleState.EnemyTurn;
			EnemyTurn();
		}
	}
    
    private IEnumerator EnemyAttack()
    {
	    dialogueText.text = enemyInfo.fighterName + " attacks!";

	    yield return new WaitForSeconds(1f);

	    var isDead = playerInfo.TakeDamage(enemyInfo.damage);

	    _playerHUD.SetHP(playerInfo.currentHP);

	    yield return new WaitForSeconds(1f);

	    if(isDead)
	    {
		    state = BattleState.Lost;
		    EndBattle();
	    } 
	    else
	    {
		    state = BattleState.PlayerTurn;
		    PlayerTurn();
	    }
    }

    private IEnumerator PlayerHeal()
	{
		playerInfo.Heal(5);

		_playerHUD.SetHP(playerInfo.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.EnemyTurn;
		EnemyTurn();
	}
    
    private IEnumerator EnemyHeal()
    {
	    enemyInfo.Heal(5);

	    _enemyHUD.SetHP(enemyInfo.currentHP);
	    dialogueText.text = enemyInfo.fighterName + " heals.";

	    yield return new WaitForSeconds(1f);
	    
	    state = BattleState.PlayerTurn;
	    PlayerTurn();
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
