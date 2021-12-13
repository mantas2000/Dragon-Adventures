using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState {Start, PlayerTurn, EnemyTurn, Won, Lost, Tie}

public class BattleManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private FighterInfo playerInfo;
	[SerializeField] private FighterInfo enemyInfo;
	[SerializeField] private BattleHUD _playerHUD;
	[SerializeField] private BattleHUD _enemyHUD;
	[SerializeField] private MinMaxAlgorithm _minMax;
	public List<int> playerBiteDamage = new List<int>() {0, 2};
	public List<int> playerFireAttackDamage = new List<int>() {1, 4};
	public List<int> enemyKickDamage = new List<int>() {3, 0};
	public List<int> enemyMagicSpellDamage = new List<int>() {5, 2};
	public BattleState state;

    // Start is called before the first frame update
    private void Start()
    {
		state = BattleState.Start;
		StartCoroutine(SetupBattle());
    }

    private void Update()
    {
	    // Restart level
	    if (Input.GetKeyDown(KeyCode.R)){
		    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	    }
        
	    // Exit level
	    if (Input.GetKeyDown(KeyCode.Escape))
	    {
		    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex != 1 ? "Overworld" : "Menu");
	    }
    }

    private IEnumerator SetupBattle()
	{
		dialogueText.text = "A wild " + enemyInfo.fighterName + " approaches...";

		// Set up fighter HUD
		_playerHUD.SetHUD(playerInfo);
		_enemyHUD.SetHUD(enemyInfo);

		yield return new WaitForSeconds(2f);

		state = BattleState.PlayerTurn;
		PlayerTurn();
	}
    
    private void EnemyTurn()
    {
	    // Get best possible action using MinMax Algorithm
	    var bestAction = _minMax.BestAction();

	    // Start attack
	    switch (bestAction)
	    {
		    case "Kick":
			    StartCoroutine(Move(enemyKickDamage, enemyInfo, false, " kicks."));
			    break;
		    case "Magic Spell":
			    StartCoroutine(Move(enemyMagicSpellDamage, enemyInfo, false, " deploys magic spell!"));
			    break;
	    }
    }
    
    private void PlayerTurn()
    {
	    dialogueText.text = "Choose an action:";
    }

    private IEnumerator EndBattle()
    {
	    // Display outcome of the battle
	    switch (state)
	    {
		    case BattleState.Lost:
			    dialogueText.text = "You were defeated.";
			    FindObjectOfType<AudioManager>().Play("PlayerDeath");
			    break;
		    case BattleState.Tie:
			    dialogueText.text = "Both fighters have died.";
			    FindObjectOfType<AudioManager>().Play("PlayerDeath");
			    break;
		    case BattleState.Won:
			    dialogueText.text = "You won the battle!";
			    FindObjectOfType<AudioManager>().Play("LevelCompleted");
			    break;
	    }

	    yield return new WaitForSeconds(2f);

	    switch (state)
	    {
		    case BattleState.Lost:
			    // Restart level
			    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			    break;
		    case BattleState.Tie:
			    // Restart level
			    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			    break;
		    case BattleState.Won:
			    // Exit level
			    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex != 1 ? "Overworld" : "Menu");
			    break;
	    }
    }

    private IEnumerator Move(IReadOnlyList<int> damage, FighterInfo fighterInfo, bool isPlayer, string message)
    {
	    // Display attack info
	    dialogueText.text = fighterInfo.fighterName +  message;
	    
	    // Play hit sound
	    FindObjectOfType<AudioManager>().Play("Hit");

	    // Pass damage amount to MinMax algorithm
	    _minMax.UpdateLifeBar(damage[0], damage[1]);

	    // Update HP Sliders
	    _playerHUD.SetHP(_minMax.GetPlayerLifeBar());
	    _enemyHUD.SetHP(_minMax.GetOpponentLifeBar());
	    
	    yield return new WaitForSeconds(2f);
	    
	    // Both fighters have died (TIE)
	    if (_minMax.GetPlayerLifeBar() == 0 && _minMax.GetOpponentLifeBar() == 0)
	    {
		    state = BattleState.Tie;
		    StartCoroutine(EndBattle());
	    }
	    
	    // Enemy have died (WON)
	    else if (_minMax.GetPlayerLifeBar() > 0 && _minMax.GetOpponentLifeBar() == 0)
	    {
		    state = BattleState.Won;
		    StartCoroutine(EndBattle());
	    }
	    
	    // Player have died (LOST)
	    else if (_minMax.GetPlayerLifeBar() == 0 && _minMax.GetOpponentLifeBar() > 0)
	    {
		    state = BattleState.Lost;
		    StartCoroutine(EndBattle());
	    }
	    
	    // Fight continues
	    else if (_minMax.GetPlayerLifeBar() > 0 && _minMax.GetOpponentLifeBar() > 0)
	    {
		    // Pass turn to opponent
		    if (isPlayer)
		    {
			    state = BattleState.EnemyTurn;
			    EnemyTurn();
		    }
		    
		    // Pass turn to player
		    else
		    {
			    state = BattleState.PlayerTurn;
			    PlayerTurn();
		    }
	    }
    }

	public void OnBiteButton()
	{
		// Make sure it's player's turn
		if (state != BattleState.PlayerTurn)
			return;
		
		// Start attack
		StartCoroutine(Move(playerBiteDamage, playerInfo, true, " bites."));
	}

	public void OnFireAttackButton()
	{
		// Make sure it's player's turn
		if (state != BattleState.PlayerTurn)
			return;
		
		// Start attack
		StartCoroutine(Move(playerFireAttackDamage, playerInfo, true, " fire attacks."));
	}
}
