using System.Collections.Generic;
using UnityEngine;
using System;
using MinMaxLibrary.algorithms;
using MinMaxLibrary.utils;
using MinMaxProjects.tests;

public class MinMaxAlgorithm : MonoBehaviour
{
    [SerializeField] private BattleManager bm;
    [SerializeField] private FighterInfo playerInfo;
    [SerializeField] private FighterInfo opponentInfo;
    private MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf> conf;
    private NTree<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState> tree;
    private MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState cgs;
    
    public void Start()
    {
        // The player can perform three different actions:
        var actionsPlayer = new HashSet<string> {"Bite", "Fire Attack"};

        // The NPC can also perform 3 damage different actions
        var actionsNPC = new HashSet<string> {"Kick", "Magic Spell"};

        // Initialization of the minmax algorithm
        cgs = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState();
        // Setting some parameters out of the constructor, so to provide a better comment section...
        var initConf = new TreeBasedGameConfiguration<string>(); // initial state of the tree game
        cgs.gameConfiguration = initConf;                // initial board configuration
        cgs.opponentLifeBar = new TreeBasedPlayerConf(opponentInfo.currentHP, true); // normalized life bar
        cgs.playerLifeBar = new TreeBasedPlayerConf(playerInfo.currentHP, true);   // normalized life bar
        cgs.isPlayerTurn = false;                        // starting with max
        cgs.parentAction = "";                           // the root node has NO parent action, represented as an empty string!

        { // In a more realistic setting, we do not care if we reached the final state or not, let the algorithm decide upon the score of each single player! in here, I only set the damage for each player.
          Func<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState, string, Optional<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState>> f = (conff, act) =>
                {
                    // Creating a new configuration, where we change the turn
                    var result = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState(conff, act);
                    // Appending the action in the history
                    result.gameConfiguration = conff.gameConfiguration.appendAction(act);
                    result.parentAction = act;
                    // Setting up the actions by performing some damage
                    if (conff.isPlayerTurn)
                    {
                        switch (act)
                        {
                            case "Bite":
                                result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - bm.playerBiteDamage[1], 0.0), true);
                                break;
                            case "Fire Attack":
                                result.playerLifeBar = new TreeBasedPlayerConf(Math.Min(result.playerLifeBar.getScore() + bm.playerFireAttackDamage[0], 0.0), true);
                                result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - bm.playerFireAttackDamage[1], 0.0), true);
                                break;
                        }
                    } 
                    else
                    {
                        switch (act)
                        {
                            case "Kick":
                                result.playerLifeBar = new TreeBasedPlayerConf(Math.Max(result.playerLifeBar.getScore() - bm.enemyKickDamage[0], 0.0), true);
                                break;
                            case "Magic Spell":
                                result.playerLifeBar = new TreeBasedPlayerConf(Math.Max(result.playerLifeBar.getScore() - bm.enemyMagicSpellDamage[0], 0.0), true);
                                result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - bm.enemyMagicSpellDamage[1], 0.0), true);
                                break;
                        }
                    }
                    return result;
                };

                conf = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>(actionsNPC, actionsPlayer, f);
                
                // Running the whole MinMax algorithm with alpha/beta pruning
                tree = conf.fitWithAlphaBetaPruning(cgs);
        }
    }

    public string BestAction()
    {
        // Running the whole MinMax algorithm with alpha/beta pruning
        tree = conf.fitWithAlphaBetaPruning(cgs);
        
        // Return best possible action
        return tree.data.action.getBestAction();
    }
    
    public void UpdateLifeBar(int playerHp, int opponentHp)
    {
        // Decrease health for player
        cgs.playerLifeBar = new TreeBasedPlayerConf(Math.Max(cgs.playerLifeBar.getScore() - playerHp, 0), true);

        // Decrease health for opponent
        cgs.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(cgs.opponentLifeBar.getScore() - opponentHp, 0), true);

        Debug.Log("PLAYER HEALTH: " + cgs.playerLifeBar.getScore());
        Debug.Log("ENEMY HEALTH: " + cgs.opponentLifeBar.getScore());
        Debug.Log("-----------------------------------");
    }

    public int GetPlayerLifeBar()
    {
        // Get player's current HP
        return (int) cgs.playerLifeBar.getScore();
    }
    
    public int GetOpponentLifeBar()
    {
        // Get opponent's current HP
        return (int) cgs.opponentLifeBar.getScore();
    }
}
