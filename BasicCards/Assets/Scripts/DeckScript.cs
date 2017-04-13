using System.Collections;
using System.Collections.Generic;
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using UnityEngine;


public class DeckScript : MonoBehaviour {
	
	public Sprite[] cardsFaces;		//all of the sprites to use for dealing cards
	public Sprite cardBack;			//the image for the back of the cards
	public CardScript card;		//the gameobject of the actual cards

	public UndrawnDeckScript undrawnDeck;		//the object that symbolizes the Undrawn stack of cards
	//public EnemyBehaviour enemyBehaviour;

	public List<XMLWeaponHitData> weaponHitBoxData;
	public GameControllerScript gameController;
	private PlayerScript playerScript;
	//public
	private string controllerParentIDtag;

	//public XMLloaderScript XMLloader;
	public List<XMLData> cardData;

	public List<int> orderOfDrawPile;		//the current undrawn deck of cards
	public List<int> discardedCards;		//the cards that are out of play and used
	public List<CardScript> drawnCards;			//the cards that have been drawn and are in play
	public Transform cardStartPosition;			//the location marker for the first card drawn
	public Transform deckStartPosition;			//undrawnDeck start position
	public Transform offScreenDeck;				//the actual location for storage of all the cards in the deck **//need to fix to be more efficient. Maybe not instantiate the cards untill drawn?

	//public weaponHitContainerBehaviour weaponHitSquaresPrefab;

	private Vector3 tableLocation;		//the variable location for each new card that is drawn
	public float cardGapX;				//the gap between the cards, used for spacing of the spawn points
	private float cardWidthX;			//the width of the card, used for spacing of the spawn points

	public SpriteRenderer weaponHitSmallSquarePrefab;
	private TargetSquareScript smallSquareSize;		//example of the square needed for the grid targeting
	private SpriteRenderer weaponSmallSquare;
	//private Vector3 playAreaCurrentRatioSize;
	//public GridMaker PlayArea;
	private CardScript currentCard;
	private UndrawnDeckScript undrawnDeckInst;

	void Start () {
		//print (cardStartPosition.transform.position);
		controllerParentIDtag = gameObject.transform.parent.tag;
		//print (controllerParentIDtag);

		Transform PlayerScriptTemp = gameObject.transform.parent;
		if(PlayerScriptTemp != null){
			playerScript = PlayerScriptTemp.GetComponent<PlayerScript>();
		}
		if(PlayerScriptTemp == null){
			Debug.Log ("Cannot find 'playerScript'object");}
		

		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");	
		GameObject XMLCardLoaderObject = GameObject.FindWithTag("MainLoader");
		if(XMLCardLoaderObject != null){
			cardData = XMLCardLoaderObject.GetComponent<XMLCardLoaderScript>().data;
//			Debug.Log ("cardData " + cardData.Count);
		}
		if(XMLCardLoaderObject == null && loaderScriptTemp != null){
			Debug.Log ("Cannot find 'XMLCardLoaderObject'object");}

		GameObject XMLWeaponHitLoaderScriptTEMP = GameObject.FindWithTag("MainLoader");
		if(XMLWeaponHitLoaderScriptTEMP != null){
			weaponHitBoxData = XMLWeaponHitLoaderScriptTEMP.GetComponent<XMLWeaponHitLoaderScript>().data;
//			Debug.Log ("weaponHitBoxData " + weaponHitBoxData.Count);
		}
		if(XMLWeaponHitLoaderScriptTEMP == null && loaderScriptTemp != null){
			Debug.Log ("Cannot find 'weaponHitBoxLoader'object");}

		cardWidthX = card.transform.localScale.x;															//scale of card used for spacing
		undrawnDeckInst = Instantiate (undrawnDeck, deckStartPosition.position, deckStartPosition.rotation);					//making the object that symbolized the undrawn deck of cards
		undrawnDeckInst.transform.SetParent(gameObject.transform);
		undrawnDeckInst.GetComponent<UndrawnDeckScript>().ManualStart();
		undrawnDeckInst.GetComponent<SpriteRenderer>().sprite = cardBack;										//applying the back of the card graphic to it
		for (int i=0; i < cardsFaces.Length; i++){															//making as many cards as there are graphics for faces, gets the number from the Card prefab
			orderOfDrawPile.Add(i);
			//Debug.Log (i);
		}
		discardDrawThenShuffle();							//shuffles all the cards in orderOfDrawPile

	}
	public void DealCard(){
		print (controllerParentIDtag);
		for (int i=0; i < 1; i++){
			if (drawnCards.Count < 5 && orderOfDrawPile.Count > 0) {								//does not allow a dealt card if there are more than 5 cards out and active, or if the draw pile is empty
				createCard();
				relocateDrawnCards();	
			} 
			else {
				Debug.Log ("too many cards in play or too few to draw from");
			}
		}
	}
	private void createCard(){
		CardScript instCard;
		instCard = Instantiate (card, offScreenDeck.position, cardStartPosition.rotation);
		instCard.ManualStart (gameObject.GetComponent<DeckScript>());
		instCard.transform.SetParent (gameObject.GetComponent<Transform>());
		instCard.SetPlayerAs (getPlayerScript ().tag);
		drawnCards.Add(instCard);
		instCard.CardAttributes = cardData[orderOfDrawPile[0]];
		instCard.setFace(cardsFaces[(orderOfDrawPile[0])]);
		orderOfDrawPile.RemoveAt(0);
		string instAttackType = instCard.TypeOfAttack;			//matching the card's attack with the same name of attack from the database of weaponhitdata to get the matrix of what is hit
		XMLWeaponHitData hitBoxDataForCard = weaponHitBoxData.Find (XMLWeaponHitData => XMLWeaponHitData.nameOfAttack == instAttackType);
		instCard.setWeaponHitBox(hitBoxDataForCard);
	}

	private void relocateDrawnCards(){
		print (cardStartPosition.transform.position);
		int tempCount = drawnCards.Count;
		for (int i = 0; i < tempCount; i++) {
			float cardXPosition = cardStartPosition.transform.position.x + (cardWidthX + cardGapX) * i;
			tableLocation = new Vector3(cardXPosition, cardStartPosition.transform.position.y, cardStartPosition.transform.position.z);			//creates a vector3 to send to the card
			drawnCards[i].moveCard(tableLocation);																						//sends coordinates to the card on where to start on the game board
		}
	}

	public void updateCards(){								//is called when there are possible cards played and need to be resorted into the discard pile. Is called by shuffle(), discard() and from a cardbehaviour when it's played and used
		for (int i = 0; i < drawnCards.Count; i++){ //CardScript drawnCard in drawnCards) {				//runs through all drawn cards
			if (!drawnCards[i].isActiveAndEnabled) {		//checks to see which ones are still active. Ontrigger2dCollision in CardBehavior deactivates cards when put into play area @void OnTriggerStay2D(Collider2D other)
				discardedCards.Add(drawnCards[i].CardNumber);			//moves any non active cards to discarded pile
				//enemyBehaviour.takeDamage(drawnCards[i].AttackValue);
				//gameController.enemyCardDamage();
				Destroy(drawnCards[i].gameObject);
				drawnCards.RemoveAt(i);						//removes the non active card from the drawn pile
				i--;
			}
		}
	}
	public void discardDrawThenShuffle(){									//a shuffle all command. Takes every single card from the original deck and reshuffles them. Is called after the deck is created, and after shuffleEverything()
																			//and called from gamecontroller scripte by button press
		if (orderOfDrawPile.Count > 0) {										//checks the orderOfDrawPile has cards before trying to discard the remainder
			int tempCount = orderOfDrawPile.Count;							//store the current number of cards in DeckToDrawFrom
			for (int i = 0; i < tempCount; i++) {							//discards all the cards in drawpile
				discardedCards.Add (orderOfDrawPile [0]);						//adds first card in list to discard deck
				orderOfDrawPile.RemoveAt (0);									//removes first card in list of DeckToDrawFrom
			}
		} else {				
			//Debug.Log ("the stack of cards being shuffled does not have any cards in it");
			//Debug.Log (orderOfDrawPile.Count);
		}
		shuffleDiscard ();										//calls the shuffle discard function after putting all cards into the discard.

	}
	public void shuffleDiscard(){											//function for the times when there are cards in play that you don't want to grab when you want to reshuffle, is called by shuffle discard button and at the end of shuffleall()
																	//and called from gamecontroller scripte by button press
		int tempCount = discardedCards.Count;								//stores total number of discard cards
		for (int i=0; i <tempCount; i++){									//loops for every card in discard pile
			int rand = Random.Range(0,discardedCards.Count);				//picks a random number from 0 to current number of cards of discard. Subtracted one because count starts at 1, actual deck starts at 0
			orderOfDrawPile.Add(discardedCards[rand]);
			discardedCards.RemoveAt(rand);
		}
	}
	public void discardAllActiveShuffle(){									//deactivates all active cards and then updates to add them to the discard pile and discarddrawthenshuffles to add draw pile cards to shuffle as well
																		//and called from gamecontroller scripte by button press	
		foreach (CardScript drawnCard in drawnCards) {
			drawnCard.deactivate ();
		}
		updateCards ();
		discardDrawThenShuffle ();
	}
	public void setCurrentCard(CardScript incomingCard){
		currentCard = incomingCard;

	}
	public void emptyCurrentCard(){
		currentCard = null;
	}
	public void turnOffCurrentCard(){
		//Debug.Log ("trying to turn off");
		if (currentCard == null) {
			Debug.Log ("There is currently no activated card");
		} else
			currentCard.deactivate ();
			//Debug.Log ("attack value of current card: "+currentCard.AttackValue);
			updateCards ();
		}
	public string getControllerParentIdTag(){
		return controllerParentIDtag;
	}
	public GameControllerScript getGameController(){
		return playerScript.getGameController ();

	}
	public PlayerScript getPlayerScript(){
		return playerScript;
	}
}

//	public void discardEverything(){
//		foreach (CardScript drawnCard in drawnCards) {
//			drawnCard.deactivate ();
//		}
//		updateCards ();
//	}

