using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	private BodyPartMakerScript BpartMaker;

	//public BPartGenericScript bodyPartObject;
	//	public PlayAreaScript playAreaScript;

	float healthMax = 0;
	float remainingHealth;
	public Text enemyHealthDisplayNumber;

	public Transform healthBarGraphic;
	private Vector3 healthBarStartingScale;
	private Vector3 healthBarStartingPosition;
	private bool isEnemyPlayer;

	private WholeBodyOfParts wholeBodyOfParts = new WholeBodyOfParts();
	CurrentWeaponHitBox incomingWeaponhitBox;

	private Vector2 playAreaDimensions;
	private int flagForBrokenParts;
	private PlayAreaScript playAreaScript;
	private DeckScript activeDeck;
	private GameControllerScript gameController;



	//	bool bodypartIsDone = false;
	//	bool playAreaIsDone = false;


	public IEnumerator ManualStart () {
		gameController = gameObject.GetComponentInParent<GameControllerScript> ();
		playAreaScript = gameObject.GetComponentInChildren<PlayAreaScript> ();
		activeDeck = gameObject.GetComponentInChildren<DeckScript> ();
		StartCoroutine( playAreaScript.ManualStart ());

		BpartMaker = gameObject.GetComponent<BodyPartMakerScript> ();

		//		remainingHealth = healthMax;
		healthBarStartingScale = healthBarGraphic.localScale;
		healthBarStartingPosition = healthBarGraphic.localPosition;
		//		updateHealthDisplay ();

		StartCoroutine(BpartMaker.ManualStart());
		populateBody ();
		playAreaScript.populateEnemyPlayAreaSquares ();

		if (gameObject.tag == "PlayerController") {
			isEnemyPlayer = false;
		} else {
			isEnemyPlayer = true;
		}
		yield return null;
	}
	public void setPlayAreaDimensions(Vector2 incomingDimensions){
		//print ("inc dim "+incomingDimensions);
		playAreaDimensions = incomingDimensions;
		remainingHealth = healthMax;
	}

	public void ResetHealthBar(){
		healthBarGraphic.localScale = healthBarStartingScale;

	}
	public void updateHealthDisplay(){
		int tempEnemyIntCheck;
		if (isEnemyPlayer) {
			tempEnemyIntCheck = -1;
		} else {
			tempEnemyIntCheck = 1;
		}
		Vector3 tempHealth = healthBarStartingScale;
		Vector3 tempPositionForHealth = healthBarStartingPosition;
		float newHealth = 0;
		//Debug.Log ("Number of body parts: " + wholeBodyOfParts.listOfAllParts.Count);
		for (int i=0; i<wholeBodyOfParts.listOfAllParts.Count; i++){
			float currentHealth = wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ();
			newHealth += wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ();
			//Debug.Log (i+ " health: "+wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ());
			if (currentHealth <= 0 && wholeBodyOfParts.listOfAllParts [i].getActive()) {
				wholeBodyOfParts.listOfAllParts [i].setAsInactive ();
				Debug.Log ("deactivated triggered");
			}
		}
		remainingHealth = newHealth;
		tempHealth.x = healthBarStartingScale.x * (remainingHealth / healthMax);
		tempPositionForHealth.x = (healthBarStartingPosition.x - ((healthBarStartingScale.x - tempHealth.x)/2)*tempEnemyIntCheck);
		healthBarGraphic.localScale = tempHealth;
		healthBarGraphic.localPosition = tempPositionForHealth;
		enemyHealthDisplayNumber.text = remainingHealth.ToString() + "/" + healthMax.ToString();
	}
	public void populateBody(){				//currently invoked by game controller script on button press
		healthMax = 0;
		//StartCoroutine (waitForBpartMakerScript ());
		wholeBodyOfParts.resetBodyToZero ();

		int rand = Random.Range(1,5);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("torso "+intToStringNumber(rand), "none"));

		rand = Random.Range(1,5);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg "+intToStringNumber(rand), "left"));
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg "+intToStringNumber(rand), "right"));

		rand = Random.Range(1,5);				//random body part between one and four
		//print(rand);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm "+intToStringNumber(rand), "left"));
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm "+intToStringNumber(rand), "right"));
		rand = Random.Range(1,5);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("head "+intToStringNumber(rand), "left"));

		rand = Random.Range(1,5);
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder "+intToStringNumber(rand), "left"));
		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder "+intToStringNumber(rand), "right"));



//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm one", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm one", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("head one", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg one", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg one", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder one", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder one", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("torso one", "none"));

//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm two", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm two", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("head two", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg two", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg two", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder two", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder two", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("torso two", "none"));

//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm three", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm three", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("head three", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg three", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg three", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder three", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder three", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("torso three", "none"));

//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm four", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm four", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("head four", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg four", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg four", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder four", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder four", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("torso four", "none"));

//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("arm", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("head", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("leg", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder", "left"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("shoulder", "right"));
//		wholeBodyOfParts.setBodyPart( BpartMaker.makeBodyPart ("torso", "none"));
		//		print ("play area dim " + playAreaDimensions);
		wholeBodyOfParts = BpartMaker.createWholeBody (wholeBodyOfParts, playAreaDimensions);		//setting internal location positions of each of the body parts in relation to eachother
		for (int i=0; i<wholeBodyOfParts.listOfAllParts.Count; i++){
			healthMax += wholeBodyOfParts.listOfAllParts [i].getCurrentHealth ();		//makes health pool
		}
		foreach (BPartGenericScript bPart in wholeBodyOfParts.listOfAllParts) {
			bPart.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
		}
		remainingHealth = healthMax;
		updateHealthDisplay ();
	}
	public void outgoingBrokenPartNodes(Vector2[][] incomingSet){		//new replacement for turning off the targeting nodes when a bodypart is destroyed
		foreach(Vector2[] partCordsRow in incomingSet){
			foreach (Vector2 cord in partCordsRow) {
				playAreaScript.getSmallSquare ((int)cord.x, (int)cord.y).DeactivateSquare ();
			}
		}

	}
	public PlayAreaScript getPlayAreaOfPlayer(){
		return playAreaScript;
	}

	public WholeBodyOfParts getWholeBodyOfParts(){
		return wholeBodyOfParts;
	}


	public TargetSquareScript[][] populateCorrectPlayAreaSquares(TargetSquareScript[][] incomingSquareGrid){
		//Debug.Log (wholeBodyOfParts.listOfAllParts.Count);
		//print("grid x length: " +incomingSquareGrid[0].Length + " grid y length: "+incomingSquareGrid.Length);
		for (int i=0; i<wholeBodyOfParts.listOfAllParts.Count; i++){		//for every body part in the list
			for (int x=0; x<wholeBodyOfParts.listOfAllParts [i].getDimensionsOfPart ().x; x++){				//get the x dimensions and run through the grid of Y
				for (int y=0; y<wholeBodyOfParts.listOfAllParts [i].getDimensionsOfPart ().y; y++){			//get the y dimensions and run through every colloum of parts
					if (wholeBodyOfParts.listOfAllParts [i].getGridPoint(new Vector2(x, y))&& wholeBodyOfParts.listOfAllParts [i].getActive()){				//gets the body part point and asks the grid of bodypartnodes if they are on or off at the internal dimension of the part
//						print("getGlobalOriginPoint(): "+ wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint());
						int outGoingXCord = ((int)wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint().x)+x;
						int outGoingYCord = ((int)wholeBodyOfParts.listOfAllParts [i].getGlobalOriginPoint().y)+y;
//						print ("outgoing x cord: " + outGoingXCord + " outgoing y cord: " + outGoingYCord);
//						print("Type: "+wholeBodyOfParts.listOfAllParts[i].getName ());
						incomingSquareGrid[outGoingXCord][outGoingYCord].OccupiedSquare(wholeBodyOfParts.listOfAllParts [i]);	//sends a reference to the body part to the square marked as 'occupied'

						//if grid point is on, it finds the relative relation of the body part node and turns it on as an Occupiedsquare in the play area. it finds the relative location on the grid because each
						//body part knows its own global origin point, the 0,0 location is the lower left field off the square of the body part. No redundency yet for overlapping body parts.
					}
				}
			}
		}
		return incomingSquareGrid;
	}
	public DeckScript getActiveDeck(){
		return activeDeck;
	}
	public GameControllerScript getGameController(){
		return gameController;
	}
	public string getWhichPlayer(){
		return gameObject.tag;
	}
	public string intToStringNumber(int incomingNumber){
		switch (incomingNumber) {
		case(1):
			return "one";
		case(2):
			return "two";
		case(3):
			return "three";
		case(4):
			return "four";
		}
		return null;
	}

}

