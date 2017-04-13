using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour {

	int cardSpriteNum;
	string nameOfCard; 
	int rankOfCard; 
	float attackDamageOfCard; 
	string typeOfAttack;

	private DeckScript deckScript;
	//private GridMaker gridMaker;
	//private weaponHitContainerBehaviour weaponHitSquares;
	//private ActiveSquareBehaviour tempSquares;
//	private Vector3 offSetDistance;
//	float heightOfHitSquares;
//	float widthOfHitSquares;

	//private bool clicked;
	//private bool cardInPlayArea;
	private bool active;
	private bool clickedOn;
	private Sprite storedSprite;
	private SpriteRenderer spriteRenderer;
	private int hitSquareOverflow;

	private string controllerParentIDtag;

	private XMLWeaponHitData hitBoxDataForCard;

	//private GameControllerScript gameController;

	public void ManualStart(DeckScript incomingDeckReference) {
		deckScript = incomingDeckReference;
		//ActiveSquareBehaviour[] hitSquares;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		active = true;
		//cardInPlayArea = false;
		clickedOn = false;
		hitSquareOverflow = 0;

		//GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");	
//		GameObject deckControllerObjectTemp = GameObject.FindWithTag ("DeckController");
//		if (deckControllerObjectTemp != null) {
//			deckScript = deckControllerObjectTemp.GetComponent<DeckScript> ();
//		}
//		if (deckControllerObjectTemp == null) {
//			Debug.Log ("Cannot find 'deckController'object");
//		}

//		GameObject gameControllerScriptTemp = GameObject.FindWithTag ("GameController");
//		if (gameControllerScriptTemp != null) {
//			gameController = gameControllerScriptTemp.GetComponent<GameControllerScript> ();
//		}
//		if (gameControllerScriptTemp == null) {
//			Debug.Log ("Cannot find 'GameController'object");
//		}
	}
	public void setFace(Sprite cardFaceGraphic){
		gameObject.GetComponent<SpriteRenderer>().sprite = cardFaceGraphic;
	}
	public void setWeaponHitBox(XMLWeaponHitData hitBoxDataForCardImport){
		hitBoxDataForCard = hitBoxDataForCardImport;
	}


	public int CardNumber{
		get{return cardSpriteNum;}
	}
	public float AttackValue{
		get{return attackDamageOfCard;}
	}
	public string TypeOfAttack{
		get{ return typeOfAttack; }
	}

	public XMLData CardAttributes{
		set{
			cardSpriteNum = value.cardSpriteNum;
			//nameOfCard = value.nameOfCard; /////////////////////////////////////Keep
			//rankOfCard = value.rankOfCard; /////////////////////////////////////Keep
			attackDamageOfCard = value.attackDamageOfCard; 
			typeOfAttack = value.typeOfAttack;
		}
	}



	void Update(){
////		//Debug.Log(hitSquares[0].transform.localPosition);
////		//Debug.Log(gameObject.transform.localPosition);
////		offSetDistance = hitSquares[0].transform.localPosition - gameObject.transform.localPosition;
////		//Debug.Log(offSetDistance);
////		offSetDistance = new Vector3 (offSetDistance.x -(widthOfHitSquares/2), offSetDistance.y -(heightOfHitSquares/2), 0.0f);
////		int incriment = 0;
//		foreach (ActiveSquareBehaviour hitSquare in hitSquares) {
//			hitSquare.transform.localPosition = hitSquare.transform.localPosition + offSetDistance;
//		}
	}




	public void moveCard(Vector3 newPosition){
		gameObject.transform.position = newPosition;
	}

	private void OnMouseDown(){
		if (!clickedOn) {
			deckScript.getGameController().cardClickedOn (deckScript.getPlayerScript(), hitBoxDataForCard, attackDamageOfCard);		//sends the info about attack attached to the card to the gamecontroller
			clickedOn = true;
			//Debug.Log (gameObject.GetComponent<CardScript> ().AttackValue);
			deckScript.setCurrentCard (gameObject.GetComponent<CardScript>());
		} else {
			clickedOn = false;
			deckScript.emptyCurrentCard ();
		}
	}
	private void OnMouseUp(){
		if (!clickedOn) {
			deckScript.getGameController().cardClickedOff ();
			//clicked = true;
//			if (cardInPlayArea) {
//				deactivate ();
//				deckScript.updateCards ();		//lets the deck know that a card was played and to update the active cards
//			}
		}
	}
	void OnTriggerEnter2D(Collider2D other){

		if (other.CompareTag("TargetSquare")){		//does not trigger anything if its colliding with anything else
			if (active && (hitSquareOverflow<=0) && (other.GetComponent<TargetSquareScript>().getPlayerID() != getPlayerID())){
				hideCard ();
//				cardInPlayArea = true;
			}
			hitSquareOverflow++;			//the sum of all the small squares the card has entered. If number is 0, its left play area and can becom active again
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag("TargetSquare")){
			hitSquareOverflow--;
			if (!active && (hitSquareOverflow<=0)){
				showCard ();
//				cardInPlayArea = false;
			}
		}
	}

	public void hideCard(){
		active = false;
		storedSprite = spriteRenderer.sprite;
		spriteRenderer.sprite = null;
	}
	public void showCard(){
		active = true;
		spriteRenderer.sprite = storedSprite;
		//storedSprite = null;
	}

	public void deactivate(){
		gameObject.SetActive (false);
	}
	public void SetPlayerAs(string incomingPlayerControllerIDTag){
		controllerParentIDtag = incomingPlayerControllerIDTag;
	}
	public string getPlayerID(){
		return controllerParentIDtag;
	}



}
