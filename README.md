# Checkers

This project is designed within Unity and developed to currently implement two AI opponents, breadth-first search based and depth-first search opponent.

This was developed using version unity-5.3.5.


![alt tag](https://github.com/bk10aao/Checkers/blob/master/Screen%20Shot%202016-07-05%20at%2016.48.12.png)

##Changing opponent algorithm

To switch between breadth-first, depth-first algorithms or a random opponent comment/uncomment the relevant code within the ```Update ()``` method the following changes must be made to the ControllerV13 file inside the update method.

```
if (playerNo == 2) {
	System.Threading.Thread.Sleep(500);
	//comment/uncomment sections based on opponent type
	breadthFirstSearch ();	 
	//depthFirstSearch ();
	//randomOpponent ();
}
```

### Enjoy!!!
