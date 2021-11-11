# CSC3232-Coursework

<h2>Ownership</h2>
● This project is soly developed by Mantas Burcikas (Newcastle University).

<h2>How to run a project?</h2>
● Just simply open project's folder in Unity Development Platform, open 'Scenes' folder and select 'Menu' scene to play.

<h2>Things done:</h2>
● For Newtonian Physics part, multiple features were implemented, including 'Teleport' feature, 'Dash', 'Duck' & 'Super Jump' moves, wall jumping and many more.

● Specific destructible objects (bricks), bridges, obstacles and etc. covers Collision Detection part. For advanced use, doors have been created to be active or locked depending on the current player's gameplay status.

● Collision Response and Feedback is covered by player's interaction with other game objects such as enemies or obstacles. Different approaches results in different feedbacks. For example, player can stun enemy, kill or be killed depending on the way it interacts with the enemy. Also, score system was implemented.

● Enemy AI uses hierarchical state machine to control enemy's behaviour. This state machine consists of 2 Meta states (Passive & Aggressive) along with 2 sub-states inside each Meta state and 1 other state. Other things, like fully functional Menu system, were implemented.

● For probability and game design task part, Markov Decision Processes were implemented. This class analyses and measures player's skills. Using Markov Chains and Reinforced Learning, next enemy's steps and approach to the player are chosen depending on the analysis' output. By doing this, game fairness are guaranteed. Also, if player struggles, the game adjusts difficulty by itself to give player a chance to complete the game. Stochastic behaviour is also used to randomise moving spikes (both its' next stage and the time when the next change occurs). 