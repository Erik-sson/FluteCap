Captain Flute – The Cat

“FluteCap, the game, is an adventure game with a hint of RPG. You, the player, 
takes control of Captain Flute and embark on a journey to tame the wild beasts in the world.”
This would be a short description intended for capturing people and making them interested.

My idea was to primarily make a 2D game in third person where the player would travel and defeat creatures in a nonviolent way. The Pokémon games have had an influence in how I wanted to create the design as something seen in this video was what I tried going for before I realised that making graphics would not be time efficient. In its current stage it is very minimalistic at design which can be seen by the lack of background in the later levels. 
I created this by making a switch witch would determine which level would be running. This made it so that the monster on the second level is not updated unless the player is on that level. This is also done in the drawing stage. The 2D props were intended to be more than just background and therefore I made an "if" condition, checking if the player intersects the tree for example, that checks on which side the player came from and moves them out of the tree. My idea for a nonviolent game was easily solved by an instrument that would charm any enemy. Codewise I made it so that the player must touch the stone on the first level to aqquire a flute. This means that an "if" condition is and if the player also presses 'f' the character starts playing the flute. At the beginning of the flute play the player cannot move, which is to simulate that it must stand still to be able to play music. This is easily done with a bool which is set to true and when this bool is true the player controlls are not accessible and movement input is ignored.
The game works so that the player is always there unless dead and the scenery shifts with switches.

As of today the game punishes the player if one tries to escape outside the border of the window and thus dealing one damage to player, the monster also deals one damage to player and moves the player back to level one.

If I were to continue working on this game I would make want to make improvements to the collision checks between the player and the tree since is an area that is not well polished. Overall I would like to give proper graphics to everything, for example the "start screen" which today only consists of green background and black text. The same with defeating the monster (there is only one yet) and upon death.
The very lack of creatures make the game less exciting than I hoped for and is a mayor area of possible improvement.

And here is how you complete the game:
Enter the grey mountain at level 1 from the south. This takes you to level two, here you will have to make sure that the monster does not catch you while you have to play flute twice to win over it. If you do win over it you must sacrifice one of your lives by going outside the border. This takes you back to level 1 where you can "talk" to the rock which will take you to a green field where the boss is friendly. This is the end, so far, Congratz!
