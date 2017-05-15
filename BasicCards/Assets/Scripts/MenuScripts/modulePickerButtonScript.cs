using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class modulePickerButtonScript : Selectable{
//	SelectionBaseAttribute selectorScript;
	BodyPartVariationPanel bodyPartVariationPanel;
	public int bodyPartNumber;
	public Color startColor = Color.grey;
	public Color startNormalColor;

	public bool completedStart = false;
//	public Color highlightedColor;
//	public Color selectedColor;
	//public string bodyPartName;
//	protected override void Start(){
//		bodyPartVariationPanel = gameObject.GetComponentInParent<BodyPartVariationPanel>();
//		//gameObject.GetComponent<Text>().color =  startColor;
//		//print (startColor);
//		//Changes the button's Disabled color to the new color.
//		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript>().colors;
//		startNormalColor = cb.normalColor;
////		button.colors = cb;
////		print("Done text");
//		completedStart = true;
//	}
	public IEnumerator ManualStart(){
		bodyPartVariationPanel = gameObject.GetComponentInParent<BodyPartVariationPanel>();
		//gameObject.GetComponent<Text>().color =  startColor;
		//print (startColor);
		//Changes the button's Disabled color to the new color.
		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript>().colors;
		startNormalColor = cb.normalColor;
		//		button.colors = cb;
		//		print("Done text");
		completedStart = true;

		yield return null;
	}

	public void setAsSelected(){
		//selectorScript.
	}
	public override void OnPointerDown(PointerEventData eventData){
//		print ("pointer down");
		if (bodyPartVariationPanel.getPartSelected () == bodyPartNumber) { //toggles off the the body part color on deselection if the parent panel is storing the same number
			StartCoroutine (bodyPartVariationPanel.partSelected(-1));
			turnOffSelectedColor ();
		} else {
//			print ("test onSelect");
			StartCoroutine (bodyPartVariationPanel.partSelected (bodyPartNumber));
			turnOnActiveGreen ();
		}
	}
//	public override void OnSelect(BaseEventData eventData){
//		if (bodyPartVariationPanel.getPartSelected () == bodyPartNumber) { //toggles off the the body part color on deselection if the parent panel is storing the same number
//			bodyPartVariationPanel.partSelected(0);
//			turnOffSelectedColor ();
//		} else {
//			print ("test onSelect");
//			StartCoroutine (bodyPartVariationPanel.partSelected (bodyPartNumber));
//			ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript> ().colors;
//			cb.normalColor = Color.green;
//			gameObject.GetComponent<bodyPartPickerButtonScript> ().colors = cb;
//		}
//	}
	public override void OnDeselect(BaseEventData eventData){
		if (bodyPartVariationPanel.getPartSelected () != bodyPartNumber) {	//only turns off the the body part color on deselection if the parent panel is storing a different number
			turnOffSelectedColor ();
		}
//		}
	}
	public int getBodyPartNumber(){
		return bodyPartNumber;
	}
	public void turnOffSelectedColor(){
//		print ("Deseltected");
		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript>().colors;
		cb.normalColor = startNormalColor;
		gameObject.GetComponent<bodyPartPickerButtonScript>().colors = cb;
	}
	public void turnOnActiveGreen(){
		ColorBlock cb = gameObject.GetComponent<bodyPartPickerButtonScript> ().colors;
		cb.normalColor = Color.green;
		gameObject.GetComponent<bodyPartPickerButtonScript> ().colors = cb;
	}

	public bool completedStartQuery(){
		return completedStart;
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
