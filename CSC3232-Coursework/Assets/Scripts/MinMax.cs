using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MinMaxLibrary.algorithms;
using MinMaxLibrary.utils;
using MinMaxProjects.tests;
//using System.Diagnostics;

public class MinMax : MonoBehaviour
{
    public void Start()
    {
        // The player can perform two different actions:
        // - TurboPunch: gives the opponent a damage of 0.5
        // - MiniPunch:  gives the opponent a damage of 0.2
        HashSet<string> actionsPlayer = new HashSet<string>();
        actionsPlayer.Add("TurboPunch");
        actionsPlayer.Add("MiniPunch");

        // The NPC can only perform 0.3 damage at a time
        HashSet<string> actionsNPC = new HashSet<string>();
        actionsNPC.Add("Punch");

        // Initialization of the minmax algorithm
        var cgs = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState();
        // Setting some parameters out of the constructor, so to provide a better comment section...
        var initConf = new TreeBasedGameConfiguration<string>(); // initial state of the tree game
        cgs.gameConfiguration = initConf;                // initial board configuration
        cgs.opponentLifeBar = new TreeBasedPlayerConf(1.0, true); // normalized life bar
        cgs.playerLifeBar = new TreeBasedPlayerConf(1.0, true);   // normalized life bar
        cgs.isPlayerTurn = false;                        // starting with max
        cgs.parentAction = "";                           // the root node has NO parent action, represented as an empty string!


        {

            // In a more realistic setting, we do not care if we reached the final state or not, let the algorithm decide
            // upon the score of each single player! in here, I only set the damage for each player.
            Func<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState, string, Optional<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState>> f = (conff, act) =>
            { 
                // Creating a new configuration, where we change the turn
                var result = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState(conff, act);
                // Appending the action in the history
                result.gameConfiguration = conff.gameConfiguration.appendAction(act);
                result.parentAction = act;
                // Setting up the actions by performing some damage
                if (conff.isPlayerTurn) {
                    //System.Diangnostics.Debug.Assert(actionsPlayer.Contains(act));
                    if (act.Equals("TurboPunch")) 
                        result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - 0.5, 0.0), true);
                    else // MiniPunch
                        result.opponentLifeBar = new TreeBasedPlayerConf(Math.Max(result.opponentLifeBar.getScore() - 0.1, 0.0), true);

                } else {
                    //System.Diangnostics.Debug.Assert(actionsNPC.Contains(act));
                    result.playerLifeBar = new TreeBasedPlayerConf(Math.Max(result.playerLifeBar.getScore() - 0.3, 0.0), true);
                }
                return result;
            };

            var conf = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>(actionsNPC, actionsPlayer, f);

            // Running the whole MinMax algorithm, with no pruning
            Console.WriteLine("Tree with Min/Max approach:");
            Console.WriteLine("===========================");
            NTree<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState> tree = conf.fitModel(cgs);
            tree.Print((x) => x.ToString());
            Console.WriteLine();
            Console.WriteLine();

            // Running the whole MinMax algorithm with alpha/beta pruning
            Console.WriteLine("Tree with Min/Max approach and Alpha/Beta Pruning:");
            Console.WriteLine("==================================================");
            var truncTree = conf.fitWithAlphaBetaPruning(cgs);
            truncTree.Print((x) => x.ToString());
            Console.WriteLine();
            Console.WriteLine();
        }

        {
            MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.Player Player = new MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.Player(actionsPlayer, new TreeBasedPlayerConf(1.0, true), false);
            MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.Player NPC = new MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.Player(actionsNPC, new TreeBasedPlayerConf(1.0, true), true);
            var playersList = new List<MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.Player>();
            playersList.Add(Player);
            playersList.Add(NPC);

            Func<MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState, 
                string, Optional<MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState>> f = (conff, act) =>
            {

                // Even in this case, let us assume that we play in turns. 
                int nextPlayerTurn = (conff.playerIdTurn + 1) % 2;
                var result = new MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState(conff, act, nextPlayerTurn);
                // Recording the action in the collection of all the actions up to this point
                result.gameConfiguration = conff.gameConfiguration.appendAction(act);
                // Action performed by the parent, leading to the current child state
                result.parentAction = act;


                if (conff.playerIdTurn == 0)
                { 
                    //System.Diangnostics.Debug.Assert(actionsPlayer.Contains(act));
                    if (act.Equals("TurboPunch")) 
                        result.players[1].lifeBar = new TreeBasedPlayerConf(Math.Max(result.players[1].lifeBar.getScore() - 0.5, 0.0), true);
                    else // MiniPunch
                        result.players[1].lifeBar = new TreeBasedPlayerConf(Math.Max(result.players[1].lifeBar.getScore() - 0.1, 0.0), true);
                }
                else
                { 
                    //System.Diangnostics.Debug.Assert(actionsNPC.Contains(act));
                    result.players[0].lifeBar = new TreeBasedPlayerConf(Math.Max(result.players[0].lifeBar.getScore() - 0.3, 0.0), true);
                }
                return result;

            };

            MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf> algo = new MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>(playersList, f);


            // Setting the initial configuration of the world, in a similar way from the former game
            MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState cgs2 = new MaxNAlgorithm<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState();
            cgs2.players = playersList;
            cgs2.gameConfiguration = new MinMaxProjects.tests.TreeBasedGameConfiguration<string>();
            cgs2.playerIdTurn = 1; // NPC, the second in the list, moves first

            var vecTree = algo.fitModel(cgs2);
            vecTree.Print((x) => x.ToString());
            //Debug.Log("");
            //Debug.Log("");
        }
    }
}
