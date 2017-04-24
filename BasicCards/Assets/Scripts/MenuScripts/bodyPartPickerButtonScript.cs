using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class bodyPartPickerButtonScript : MonoBehaviour, ISelectHandler {
//	SelectionBaseAttribute selectorScript;
//	public void Start(){
//		selectorScript = gameObject.GetComponent<SelectionBaseAttribute>();
//	}

	public void setAsSelected(){
		//selectorScript.
	}
	public void OnSelect(BaseEventData eventData){
		print ("test onSelect");
		gameObject.GetComponent<Text> ().color = new Color(0, 0, 1, 1);
	}
	public void OnUnselect(){
		print ("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		gameObject.GetComponent<Text> ().color = new Color(0, 0, 0, 0);
	}
//	private SelectionBaseAttribute selectorScript;

//	public void chooseButton(int choice){
//
//		//gameObject.GetComponent<BodyPartSelectionCanvasScript>().markSelectedBodyPart()
//	}
//	public void onClick(){
//		print ("clicked");
//	}
}
