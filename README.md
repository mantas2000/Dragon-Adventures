# CSC3232-Coursework

<h1>Ownership</h1>
● This project is soly developed by Mantas Burcikas (Newcastle University).

<h1>How to run a project?</h1>
● Just simply open project's folder in Unity Development Platform, open 'Scenes' folder and select 'Menu' scene to play.

<h1>Things done:</h1>

<h3>Coursework 1</h3>
● For Newtonian Physics part, multiple features were implemented, including 'Teleport' feature, 'Dash', 'Duck' & 'Super Jump' moves, wall jumping and many more.

● Specific destructible objects (bricks), bridges, obstacles and etc. covers Collision Detection part. For advanced use, doors have been created to be active or locked depending on the current player's gameplay status.

● Collision Response and Feedback is covered by player's interaction with other game objects such as enemies or obstacles. Different approaches results in different feedbacks. For example, player can stun enemy, kill or be killed depending on the way it interacts with the enemy. Also, score system was implemented.

● Enemy AI uses hierarchical state machine to control enemy's behaviour. This state machine consists of 2 Meta states (Passive & Aggressive) along with 2 sub-states inside each Meta state and 1 other state. Other things, like fully functional Menu system, were implemented.

● For probability and game design task part, Markov Decision Processes were implemented. This class analyses and measures player's skills. Using Markov Chains and Reinforced Learning, next enemy's steps and approach to the player are chosen depending on the analysis' output. By doing this, game fairness are guaranteed. Also, if player struggles, the game adjusts difficulty by itself to give player a chance to complete the game. Stochastic behaviour is also used to randomise moving spikes (both its' next stage and the time when the next change occurs).

<h3>Coursework 2</h3>
● Created a new type of enemy that uses advanced pathfinding features to chase down a player in a map with dynamic changes (Level 4). For this task, I used A* search algorithm. To improve game performance, I created a script that, instead of scanning the whole scene during the runtime, only scans and updates moving object's locations.

● Implementation of 'Flocking' (Level 5) covers advanced real-time AI techniques task. Five different flocking behaviours were created (Alignment, Avoidance, Steered Cohesion, Obstacle Avoidance, Seek) to control flock agent's behaviour and mimic a flock. To make flocking more adjustable, all behaviours are made as scriptable objects, meaning it is easy to add/remove behaviours or adjust their weights.

● For Special effects and sound task, I added a bunch of particle systems (dust, trail effects, etc.) to the player's character. This makes game physics feel more live and robust. In addition to that, I also implemented camera shake effect which is triggered by performing an 'Air Attack'. Every different move of the player or the enemy is animated by using Animator. Sound effects and background music are also used. 'Audio Manager' manages all sound sources and provides smooth and seamless sound transition between the scenes.

● In this project, I followed appropriate design patterns and game structure rules. That includes using prefabs in the scenes, clean & commented code structure, well structured game design and quality of features implementation.

● Implementation of advanced gameplay progression techniques can be seen in Level 6. In this level, I decided to make a mini turn-based game. For this reason, I am using Min/Max algorithm to find the best possible move for the enemy in order to beat the player. Battle between the player and the final boss are controlled by the managing states. This helps to manage the flow and rules of the battle and have a clear outcome of the fight.