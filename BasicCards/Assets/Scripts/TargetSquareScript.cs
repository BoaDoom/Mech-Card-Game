using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSquareScript : MonoBehaviour {

	int gridCordX;
	int gridCordY;

	public Sprite occupiedUntargetedSprite;
	public Sprite occupiedTargetedSprite;
	public Sprite targetMissedSprite;
	Sprite defaultSprite;
	private string playerControllerIDTag;

	Sprite trueUntarget;
	Sprite trueTarget;
//	private int startingIntValue;

	public TargetSquareState activeSquareState;

	SpriteRenderer spriteRenderer;
	PlayAreaScript playArea;
	BPartGenericScript bodyPartReference;
	//PlayAreaScript playAreaScript;

	public IEnumerator ManualStart(PlayAreaScript parentPlayAreaScript){
		playArea = parentPlayAreaScript;
		activeSquareState = new TargetSquareState();
//		activeSquareState.setOccupiedState (true);
//		print (activeSquareState.getOccupiedState());
		SpriteRenderer spriteRendererTemp = gameObject.GetComponent<SpriteRenderer>();
		if(spriteRendererTemp != null){
			spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
			//occupiedSprite =
			defaultSprite = spriteRenderer.sprite;
//			trueUntarget = spriteRenderer.sprite;
		}
		trueUntarget = defaultSprite;
		trueTarget = targetMissedSprite;
		if(spriteRendererTemp == null){
			Debug.Log ("Cannot find 'spriteRendererTemp'object");
		}

//		GameObject playAreaTemp = GameObject.FindWithTag ("PlayAreaController");
//		if(playAreaTemp != null){
//			playArea = playAreaTemp.GetComponent<PlayAreaScript>();
//		}
//		if(playAreaTemp == null){
//			Debug.Log ("Cannot find 'playAreaImport'object");
//		}
		hardUntargetSquare ();
		yield return null;
	}
	public void SetGridCordX(int cordx){
		gridCordX = cordx;
	}
	public void SetGridCordY(int cordy){
		gridCordY = cordy;
	}
	public int GetGridCordX(){
		return gridCordX;
	}
	public int GetGridCordY(){
		return gridCordY;
	}
//	void OnTriggerStay2D(Collider2D other){
//		if (other.CompareTag("weaponHitBox")){
//			spriteRenderer.sprite = occupiedSprite;
//		}
//	}
//	void OnTriggerExit2D(Collider2D other){
//		if (other.CompareTag("weaponHitBox")){
//			spriteRenderer.sprite = defaultSprite;
//		}
//	}
		
	void OnMouseEnter(){
		playArea.squareHoveredOver (gridCordX, gridCordY);

	}
	void OnMouseExit(){
		playArea.squareHoveredOff ();
	}
	void OnMouseDown(){
		playArea.squareClickedOn(gridCordX, gridCordY);
	}
		
	public void TargetSquare(){		//used by playarea to turn on and off targetting
		spriteRenderer.sprite = trueTarget;
		activeSquareState.setHardTargetedState(true);
		activeSquareState.setSoftTargetedState(true);
//		Debug.Log ("target triggered");

	}
	public void softUntargetSquare(){	//used by playarea to turn on and off targetting. The point of this is to turn off squares that arn't hovered over anymore, but to keep track of
										//where the last place the weapon shape was hovered over in case the user releases and 'fires' the weapon within the bounderies, the correct portions are hit
		spriteRenderer.sprite = trueUntarget;
		activeSquareState.setHardTargetedState(false);
//		Debug.Log ("soft untarget triggered");
	}
	public void hardUntargetSquare(){	//used by playarea to turn on and off targetting. Hard reset happens when Another
		spriteRenderer.sprite = trueUntarget;
		activeSquareState.setSoftTargetedState(false);
		activeSquareState.setHardTargetedState(false);		//redundent but needed
//		Debug.Log ("hard untarget triggered");
	}



	public void takeOneSquareDamage(float incomingWeaponDamage){
		bodyPartReference.takeDamage (incomingWeaponDamage);
	}

	public void OccupiedSquare(BPartGenericScript incomingBodyPartReference){	//used by playarea to turn on and off if the enemy occupies the space
		bodyPartReference = incomingBodyPartReference;
		trueTarget = occupiedTargetedSprite;
		trueUntarget = occupiedUntargetedSprite;
		activeSquareState.setOccupiedState(true);
		spriteRenderer.sprite = occupiedUntargetedSprite;
	}
	public void DeactivateSquare(){		//used by playarea to turn on and off if the enemy occupies the space
		trueUntarget = defaultSprite;
		trueTarget = targetMissedSprite;
		activeSquareState.setOccupiedState(false);
	}

	public void SetPlayerAs(string incomingPlayerControllerIDTag){
		playerControllerIDTag = incomingPlayerControllerIDTag;
	}
	public string getPlayerID(){
		return playerControllerIDTag;
	}

//	public TargetSquareState getStateOfSquare(){
//		return activeSquareState;
//	}
//	void OnMouseExit(){
//		spriteRenderer.sprite = defaultSprite;
//	}
}

