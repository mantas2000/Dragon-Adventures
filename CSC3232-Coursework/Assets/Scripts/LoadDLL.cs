using System.Collections.Generic;
using UnityEngine;
using MinMaxLibrary.algorithms;
using MinMaxLibrary.utils;
using System;
using MinMaxProjects.tests;

public class LoadDLL : MonoBehaviour
{
    MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf> conf;
    NTree<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState> tree;
    MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState initialState;

    // Start is called before the first frame update
    private void Start()
    {
        /// Actions associated to both players
        HashSet<string> globalActions = new HashSet<string>();
        globalActions.Add("A");
        globalActions.Add("B");
        globalActions.Add("C");

        /// Initialization of the minmax algorithm
        initialState = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState();
        /// Setting some parameters out of the constructor, so to provide a better comment section...
        var initConf = new TreeBasedGameConfiguration<string>(); // initial state of the tree game
        initialState.gameConfiguration = initConf;                // initial board configuration
        initialState.opponentLifeBar = new TreeBasedPlayerConf(); // bogus configuration
        initialState.playerLifeBar = new TreeBasedPlayerConf();   // bogus configuration
        initialState.isPlayerTurn = false;                        // starting with max
        initialState.parentAction = "";                           // the root node has NO parent action, represented as an empty string!

        /// Please observe that, in usual games, an action just updates the lifebar values of CurrentGameState.
        /// Many several different considerations might descend from each different single game
        /// In this case, I'm just mimicking the tree data structure from the well known AI book
        Func<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState, string, Optional<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState>> f = (conff, act) =>
        {
            // If I am at the MAX root, then creates a MIN state
            if (conff.gameConfiguration.actionsPerformed.Count == 0)
            {
                if (act.Equals("A") || act.Equals("B") || act.Equals("C"))
                {
                    // Next state, where action x is performed
                    var result = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState(conff, act);
                    // Recording the action in the collection of all the actions up to this point
                    result.gameConfiguration = conff.gameConfiguration.appendAction(act);
                    // Action performed by the parent, leading to the current child state
                    result.parentAction = act;
                    // Resulting child configuration
                    return result;
                }
            }
            else if (conff.gameConfiguration.actionsPerformed.Count == 1)
            { // Otherwise, I reach the last non-leaf state
                if (act.Equals("A") || act.Equals("B") || act.Equals("C"))
                {
                    // if I am a MIN child of the root, then create some leaf states
                    var result = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState(conff, act);
                    result.gameConfiguration = conff.gameConfiguration.appendAction(act);
                    // reaching the leaves: setting up the opponent values.
                    if (result.gameConfiguration.actionsPerformed[0].Equals("A") && result.gameConfiguration.actionsPerformed[1].Equals("A"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(3.0); // Each leaf state will be associated to a score. To be effective, I could only change the opponent' score, and keep the player's unchanged
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("A") && result.gameConfiguration.actionsPerformed[1].Equals("B"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(12.0);
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("A") && result.gameConfiguration.actionsPerformed[1].Equals("C"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(8.0);
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("B") && result.gameConfiguration.actionsPerformed[1].Equals("A"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(2.0);
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("B") && result.gameConfiguration.actionsPerformed[1].Equals("B"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(4.0);
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("B") && result.gameConfiguration.actionsPerformed[1].Equals("C"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(6.0);
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("C") && result.gameConfiguration.actionsPerformed[1].Equals("A"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(14.0);
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("C") && result.gameConfiguration.actionsPerformed[1].Equals("B"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(5.0);
                    else if (result.gameConfiguration.actionsPerformed[0].Equals("C") && result.gameConfiguration.actionsPerformed[1].Equals("C"))
                        result.opponentLifeBar = new TreeBasedPlayerConf(2.0);
                    result.parentAction = act;
                    return result;
                }
            }

            // In all the other cases, I stop the iteration
            return new Optional<MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>.CurrentGameState>();
        };

        conf = new MinMax<string, TreeBasedGameConfiguration<string>, TreeBasedPlayerConf>(globalActions, globalActions, f);

        /// Running the whole MinMax algorithm, with no pruning
        Debug.Log("Tree with Min/Max approach:");
        Debug.Log("===========================");
        tree = conf.fitModel(initialState);
        // Obtaining a static strategy represented as a tree, that can be navigated!
        Log(tree, (x) => x.ToString());
    }


    public void Log<T>(NTree<T> tree, Func<T, string> printer, int depth = 0)
    {
        Debug.Log(new String('.', depth) + printer(tree.data));
        for (int i = 0, n = tree.getChildrenSize(); i < n; i++) Log(tree.GetChild(i), printer, depth + 1);
    }

    // Update is called once per frame
    void Update()
    {
        /// Running the whole MinMax algorithm with alpha/beta pruning
        Debug.Log("Tree with Min/Max approach and Alpha/Beta Pruning:");
        var truncTree = conf.fitWithAlphaBetaPruning(initialState);
        Log(truncTree, (x) => x.ToString());
    }
}
