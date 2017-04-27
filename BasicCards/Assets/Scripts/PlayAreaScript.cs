using System.Collections;
using System.Collections.Generic;
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using UnityEngine;

public class PlayAreaScript: MonoBehaviour {

	public List<XMLBodyHitData> bodyLoaderData;

	public TargetSquareScript smallSquare; //added manually inside unity from prefabs
	Transform transformOriginal;
	public GameControllerScript gameControllerScript; //added manually inside unity
	PlayerScript playerScript;

	private string controllerParentIDtag;

	public int boxCountX;
	public int boxCountY;

	public float sizeRatioOfSmallBox = 1.0f;

	private TargetSquareScript[][] grid;
	private Vector2 gridDimensions;
	//private TargetSquareState[][] gridOfStates;		//tracks the states of the squares in the targeting box. Boolean of Occupied, HardTargeted, SoftTargeted

	Vector3 zeroCord = Vector3.zero;
	Vector3 framingBoxSize;
	public Vector3 firstBoxCord;


	public IEnumerator ManualStart () {
		
		controllerParentIDtag = gameObject.transform.parent.tag;
		//print ("My daddy is "+controllerParentIDtag);

		Transform PlayerScriptTemp = gameObject.transform.parent;
		if(PlayerScriptTemp != null){
			playerScript = PlayerScriptTemp.GetComponent<PlayerScript>();
		}
		if(PlayerScriptTemp == null){
			Debug.Log ("Cannot find 'playerScript'object");}
		
		gridDimensions = new Vector2(boxCountX, boxCountY);
//		print ("dimensions: " + gridDimensions);
		playerScript.setPlayAreaDimensions(gridDimensions);

		TargetSquareScript smallSquareInst;
		transformOriginal = gameObject.transform;
		framingBoxSize = new Vector3(1.0f/boxCountX, 1.0f/boxCountY, 0.0f);
		firstBoxCord = zeroCord + new Vector3 ((-0.5f + framingBoxSize.x / 2), (-0.5f + framingBoxSize.y / 2), 0.0f);
		//int yi = 0;
		//int xi = 0;
		//gridOfStates = new TargetSquareState[(int)gridDimensions.x][];	//grid of data for the prefab squares' states
		grid = new TargetSquareScript[(int)gridDimensions.x][];		//grid of prefab ActiveSquare
//		Vector2 offSetToCenter = new Vector2(Mathf.Round(boxCountX/2)-Mathf.Round(bodyHitBoxWidth/2),0);

		//Debug.Log (offSetToCenter);
		for (int x = 0; x < gridDimensions.x; x++){
			//gridOfStates [x] = new TargetSquareState[(int)gridDimensions.y];
			grid[x] = new TargetSquareScript[(int)gridDimensions.y];
			for (int y = 0; y < gridDimensions.y; y++)
			{
				smallSquareInst = Instantiate (smallSquare, zeroCord, transformOriginal.rotation);
				StartCoroutine( smallSquareInst.ManualStart (gameObject.GetComponent<PlayAreaScript>()));
				smallSquareInst.transform.SetParent (gameObject.transform);
				smallSquareInst.transform.localScale = framingBoxSize * sizeRatioOfSmallBox;
				smallSquareInst.transform.localPosition = firstBoxCord + new Vector3(framingBoxSize.x*x, framingBoxSize.y*y, 0.0f);
				smallSquareInst.SetGridCordX (x);
				smallSquareInst.SetGridCordY (y);
				smallSquareInst.SetPlayerAs (getControllerParentIdTag());
				grid[x][y] = smallSquareInst;
				//gridOfStates[x][y] = smallSquareInst.activeSquareState;

			}
		}
//		print ("grid: "+grid[7][7]);
		//playerScript.signalThatPlayAreaIsDone ();
		//populateEnemyPlayAreaSquares ();
		yield return null;
	}
	public void populateEnemyPlayAreaSquares(){		//triggered by enemyscript when both the play area is done being made and the enemy squares are done being set up
//		print("grid [0][0] "+grid[0][0].activeSquareState.getOccupiedState());
//		print("grid "+grid.Length);
		grid = playerScript.populateCorrectPlayAreaSquares (grid);
	}

	public TargetSquareScript getSmallSquare(int x, int y){
		//Debug.Log (grid [0] [0].GetComponent<BoxCollider2D>);
		return grid [x] [y];
	}

	public bool getTargetSquareStateSoftTarget(int xcordT, int ycordT){
		return grid [xcordT] [ycordT].activeSquareState.getSoftTargetedState ();
	}
	public bool getTargetSquareStateOccupied(int xcordT, int ycordT){
		return grid [xcordT] [ycordT].activeSquareState.getOccupiedState ();
	}

	public Vector2 getGridDimensions(){
		return gridDimensions;
	}

	public void squareHoveredOver(int xCord, int yCord){		//method used by the grid of active squares to signal that they are being hovered over
		if(gameControllerScript.currentClickedOnCardWeaponMatrix.isCardClickedOn && (gameControllerScript.actingPlayer.getWhichPlayer() != playerScript.getWhichPlayer())){	//checks the main game controller to see if a card on the table has sent the signal that it is clicked on
			//and checks to see if the current card hovering over play area is from the opponent or the ownder of the play area. Disallows owner to use one on self
			Vector2 middleOfWeaponHitArea = new Vector2(Mathf.Round((gameControllerScript.currentClickedOnCardWeaponMatrix.weaponHitData.gridOfHit[0].Length/2)),
				Mathf.Round((gameControllerScript.currentClickedOnCardWeaponMatrix.weaponHitData.gridOfHit.Length/2)));		//rounding the dimensions of the weaponhitArea to find the 'center' to base activate the grid
			Vector2 upperLeftStartingPoint = new Vector2(xCord - middleOfWeaponHitArea.x, yCord - middleOfWeaponHitArea.y);
			//Debug.Log ("test");
			for (int x =0; x < gameControllerScript.currentClickedOnCardWeaponMatrix.weaponHitData.gridOfHit[0].Length; x++){
				for (int y = 0; y < gameControllerScript.currentClickedOnCardWeaponMatrix.weaponHitData.gridOfHit.Length; y++) {
					Vector2 tempStartingPoint = new Vector2 (upperLeftStartingPoint.x, upperLeftStartingPoint.y);

					if (gameControllerScript.currentClickedOnCardWeaponMatrix.weaponHitData.gridOfHit[x][y] != 0	//checks the grid hit area to see if its turned 'on' with 1, or 'off' with 0
							&& ((tempStartingPoint.x +x)>=0) && ((tempStartingPoint.y +y)>=0)						//checks if the grid hit area is outside of the grid target up and to the left
							&& ((tempStartingPoint.x +x)<boxCountX) && ((tempStartingPoint.y +y)<boxCountY)){		//checks if the grid hit area is outside of the grid target down and to the right
						grid [(int)tempStartingPoint.x + x] [(int)tempStartingPoint.y + y].TargetSquare ();		//activates the squares inside the area
					}
				}
			}
		}
		//gameControllerScript.square

		//testVec2 = new Vector2 (xCord, yCord);
	}
	public void squareHoveredOff(){ 			//used by the TargetSquareScript to send a signal that it is no longer activated by the player
		hardResetSmallSquares ();
		//grid [xCord] [yCord].DeactivateSquare ();	
	}
	public void squareClickedOn(int xCord, int yCord){		//when a small square is clicked on
		if (gameControllerScript.currentClickedOnCardWeaponMatrix.isCardClickedOn && (gameControllerScript.actingPlayer.getWhichPlayer() != playerScript.getWhichPlayer())) {		//checks to see if there was a card in play and is in the opposite players play area
			playerScript.getGameController().transferOfCardDamage ();		//if there was, make the enemy take damage
			gameControllerScript.actingPlayer.getActiveDeck().turnOffCurrentCard();
		}
	}

	public void softResetSmallSquares(){
		foreach(TargetSquareScript[] gridY in grid){
			foreach (TargetSquareScript square in gridY) {
				square.softUntargetSquare ();
			}
		}
	}
	public void hardResetSmallSquares(){		//gamecontroller uses this once another card is clicked on, signalling that the previous kept data from another card is no longer needed
		foreach(TargetSquareScript[] gridY in grid){
			foreach (TargetSquareScript square in gridY) {
				square.hardUntargetSquare ();
			}
		}
	}
//	public void takeAHit(PlayerScript playerScript, CurrentWeaponHitBox incomingWeaponHitBox, int incomingX, int incomingY){
//		grid [incomingX] [incomingY].takeOneSquareDamage (incomingWeaponHitBox.weaponDamage);		//sends damage to the grid square occuppying the incoming location
//	}
	public string getControllerParentIdTag(){
		return controllerParentIDtag;
	}
}
public class TargetSquareState{
	bool occupied = false;
	bool hardTargeted = false;
	bool softTargeted = false;
//	bool hardBPTargeted = false;
//	bool softBPTargeted = false;
	public TargetSquareState(){
	}
	public bool getOccupiedState(){
		return occupied;}
	public void setOccupiedState(bool incomingState){
		occupied = incomingState;}

	public bool getHardTargetedState(){
		return hardTargeted;}
	public void setHardTargetedState(bool incomingState){
		hardTargeted = incomingState;
	}

	public bool getSoftTargetedState(){
		return softTargeted;}
	public void setSoftTargetedState(bool incomingState){
		softTargeted = incomingState;
	}

//	public bool getHardBPTargetedState(){
//		return hardBPTargeted;}
//	public void setHardBPTargetedState(bool incomingState){
//		hardBPTargeted = incomingState;
//	}
//
//	public bool getSoftBPTargetedState(){
//		return softBPTargeted;}
//	public void setSoftBPTargetedState(bool incomingState){
//		softBPTargeted = incomingState;
//	}


}
//public class activeSquareState