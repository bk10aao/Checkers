# Checkers

This project is designed within Unity and developed to currently implement two AI opponents, a breadth-first search based opponent and a depth-first search opponent.

This was developed using version unity-5.3.5.

##Changing opponent algorithm

To switch between breadth-first and depth-first algorithms the following changes must be made to the ControllerV13 file inside the update method:

### Original code

``` if (playerNo == 2) {
				if(logicController.playerHasTakeableMoves(2, gameBoard)) {
					AITake();
					//comment out bline below when implementing depth first search
					getAIQueueMoves();
				} else {
					//comment out lines below when implementing depth first search
					AIQueueMove();
					getAIQueueMoves();
					//uncomment line below to implement depth first search
					//AIMove();
				}
			} ```

### Breadth-first search:

``` if (playerNo == 2) {
				if(logicController.playerHasTakeableMoves(2, gameBoard)) {
					AITake();
					getAIQueueMoves();
				} else {
					AIQueueMove();
					getAIQueueMoves();

				}
			} ```

###Depth-first search:

``` if (playerNo == 2) {
				if(logicController.playerHasTakeableMoves(2, gameBoard)) {
					AITake();
				} else {
					AIMove();
				}
			}```
