# Checkers

This project is designed within Unity and developed to currently implement two AI opponents, breadth-first search based and depth-first search opponent.

This was developed using version unity-5.3.5.


![alt tag](https://github.com/bk10aao/Checkers/blob/master/Screen%20Shot%202016-07-05%20at%2016.48.12.png)

##Changing opponent algorithm

To switch between breadth-first and depth-first algorithms the following changes must be made to the ControllerV13 file inside the update method:

### Original code

``` 
	if (playerNo == 2) {
		if(logicController.playerHasTakeableMoves(2, gameBoard)) {
			AITake();
			//comment out line below when implementing depth first search
			getAIQueueMoves();
		} else {
			//comment out lines below when implementing depth first search
			AIQueueMove();
			getAIQueueMoves();
			//uncomment line below to implement depth first search
			//AIMove();
		}
	} 
```

### Breadth-first search:

``` 
if (playerNo == 2) {
	if(logicController.playerHasTakeableMoves(2, gameBoard)) {
		AITake();
	} else {
		AIQueueMove();
	}
	
	getAIQueueMoves();
} 
```

###Depth-first search:

``` 
if (playerNo == 2) {
	if(logicController.playerHasTakeableMoves(2, gameBoard)) {
		AITake();
	} else {
		AIMove();
	}
}
``` 
