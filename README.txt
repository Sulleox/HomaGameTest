Hello !

Thanks for taking the time to review my code test

Here are some informations about how I did it. Sorry as it may not look beautifull at all but I didn't got so much the time to polish the graphics

GameManager is the place where the life cycle of the game is being managed, basically from lauching, initializing, and then selection and dynamic mode. 
It is meant to be the container of that logic and only it

BoardManager is the script that work all the board logic and interaction, it knows all cell and tell them what to do depending which game state it is.

Boardcell are only managing themself and have the knowledge of their surrounding cell. Also I precompute all the possible move to allow it to be quicker afterward.

Pawn are just data container to know which pawn is what.

And the UI is shitty ...

Have a nice day,

Sullivan