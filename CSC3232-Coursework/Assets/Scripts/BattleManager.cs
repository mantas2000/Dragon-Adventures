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
	[SerializeField] private MinMaxAlgorithm _minMax;
	public int playerAttackAmount = 3;
	public int playerHealAmount = 2;
	public int enemyAttackAmount = 2;
	public int enemyHealAmount = 5;
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
	    var bestAction = _minMax.BestAction();
	    Debug.Log(bestAction);

	    switch (bestAction)
	    {
		    case "Attack":
			    StartCoroutine(EnemyAttack());
			    break;
		    case "Heal":
			    StartCoroutine(EnemyHeal());
			    break;
	    }
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
		var isDead = enemyInfo.TakeDamage(playerAttackAmount);
		_minMax.UpdateLifeBar(0, -playerAttackAmount);

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
	    _minMax.UpdateLifeBar(-enemyAttackAmount, 0);

	    yield return new WaitForSeconds(1f);

	    var isDead = playerInfo.TakeDamage(enemyAttackAmount);

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
		playerInfo.Heal(playerHealAmount);
		_minMax.UpdateLifeBar(playerHealAmount, 0);

		_playerHUD.SetHP(playerInfo.currentHP);
		dialogueText.text = "You feel renewed strength!";

		yield return new WaitForSeconds(2f);

		state = BattleState.EnemyTurn;
		EnemyTurn();
	}
    
    private IEnumerator EnemyHeal()
    {
	    enemyInfo.Heal(enemyHealAmount);
	    _minMax.UpdateLifeBar(0, enemyHealAmount);

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
