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
        var actionsPlayer = new HashSet<string> {"Attack", "Heal"};

        // The NPC can also perform 3 damage different actions
        var actionsNPC = new HashSet<string> {"Attack", "Heal"};

        // Initialization of the minmax algorithm
        cgs = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState();
        // Setting some parameters out of the constructor, so to provide a better comment section...
        var initConf = new TreeBasedGameConfiguration<string>(); // initial state of the tree game
        cgs.gameConfiguration = initConf;                // initial board configuration
        cgs.opponentLifeBar = new TreeBasedPlayerConf(playerInfo.currentHP, true); // normalized life bar
        cgs.playerLifeBar = new TreeBasedPlayerConf(opponentInfo.currentHP, true);   // normalized life bar
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
                    if (conff.isPlayerTurn) {
                        if (act.Equals("Attack"))
                            result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - bm.playerAttackAmount, 0.0), true);
                        else if (act.Equals("Heal"))
                            result.playerLifeBar = new TreeBasedPlayerConf(Math.Min(result.playerLifeBar.getScore() + bm.playerHealAmount, 0.0), true);
                    } 
                    else
                    {
                        if (act.Equals("Attack"))
                            result.playerLifeBar = new TreeBasedPlayerConf(Math.Max(result.playerLifeBar.getScore() - bm.enemyAttackAmount, 0.0), true);
                        else if (act.Equals("Heal"))
                            result.opponentLifeBar = new TreeBasedPlayerConf(Math.Min(result.opponentLifeBar.getScore() + bm.enemyHealAmount, 0.0), true);
                    }
                    return result;
                };

                conf = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>(actionsNPC, actionsPlayer, f);
                
                // Running the whole MinMax algorithm with alpha/beta pruning
                //Debug.Log("Tree with Min/Max approach and Alpha/Beta Pruning:");
                //var truncTree = conf.fitWithAlphaBetaPruning(cgs);
                //Log(truncTree, (x) => x.ToString());
                //Debug.Log(truncTree.data.action.getBestAction());
                tree = conf.fitWithAlphaBetaPruning(cgs);
        }
    }

    public string BestAction()
    {
        return tree.data.action.getBestAction();
    }
    
    public void UpdateLifeBar(int playerHp, int opponentHp)
    {
        // Increase/Decrease health for player
        if (playerHp > 0)
        {
            cgs.playerLifeBar = new TreeBasedPlayerConf(Math.Min(cgs.playerLifeBar.getScore() + playerHp, playerInfo.maxHP), true);
        }
        else if (playerHp < 0)
        {
            cgs.playerLifeBar = new TreeBasedPlayerConf(Math.Max(cgs.playerLifeBar.getScore() + playerHp, 0), true);
        }
        
        // Increase/Decrease health for opponent
        if (opponentHp > 0)
        {
            cgs.opponentLifeBar = new TreeBasedPlayerConf(Math.Min(cgs.opponentLifeBar.getScore() + opponentHp, opponentInfo.maxHP), true);
        }
        else if (opponentHp < 0)
        {
            cgs.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(cgs.opponentLifeBar.getScore() + opponentHp, 0), true);
        }
        
        Debug.Log("PLAYER HEALTH: " + cgs.playerLifeBar.getScore());
        Debug.Log("ENEMY HEALTH: " + cgs.opponentLifeBar.getScore());
        Debug.Log("-----------------------------------");
    }
}
