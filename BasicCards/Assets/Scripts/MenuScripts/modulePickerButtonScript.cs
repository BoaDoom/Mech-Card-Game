using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class modulePickerButtonScript : Selectable{
//	SelectionBaseAttribute selectorScript;
	ModulePickerScript modulePickerScript;
	public int bodyPartNumber;
	int assignedOptionNumber;
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
//		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
//		startNormalColor = cb.normalColor;
////		button.colors = cb;
////		print("Done text");
//		completedStart = true;
//	}
	public IEnumerator ManualStart(string nameOfModule, int incomingAssignedOptionNumber){
//		print ("manually started");
		ModulePickerScript modulePickerScriptTemp = gameObject.GetComponentInParent<ModulePickerScript>();
		if (modulePickerScriptTemp != null) {
			modulePickerScript = modulePickerScriptTemp;
		} else {
			print ("Couldnt find modulePickerScriptTemp");
		}

		modulePickerScript = gameObject.GetComponentInParent<ModulePickerScript>();
//		print ("name: "+ modulePickerScript.name);
		//gameObject.GetComponent<Text>().color =  startColor;
		//print (startColor);
		//Changes the button's Disabled color to the new color.
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
		startNormalColor = cb.normalColor;
		//		button.colors = cb;
		//		print("Done text");
		assignedOptionNumber = incomingAssignedOptionNumber;
//		print ("nameOfModule: "+ nameOfModule);
//		print ("pre set text: "+ gameObject.GetComponent<Text>().text);
		gameObject.GetComponent<Text>().text = "Module " +nameOfModule;
//		print ("after set text: "+ gameObject.GetComponent<Text>().text);
		completedStart = true;
//		StartCoroutine( modulePickerScript.setPartSelected (1));
		yield return null;
	}

	public void setAsSelected(){
		//selectorScript.
	}
	public override void OnPointerDown(PointerEventData eventData){
//		print ("pointer down");
		if (modulePickerScript.getPartSelected () == assignedOptionNumber) { //toggles off the the body part color on deselection if the parent panel is storing the same number
			StartCoroutine (modulePickerScript.setPartSelected(-1));
			turnOffSelectedColor ();
		} else {
//			print ("test onSelect");
			StartCoroutine (modulePickerScript.setPartSelected (assignedOptionNumber));
			turnOnActiveGreen ();
		}
	}

	public override void OnDeselect(BaseEventData eventData){
		if (modulePickerScript.getPartSelected () != bodyPartNumber) {	//only turns off the the body part color on deselection if the parent panel is storing a different number
			turnOffSelectedColor ();
		}
	}
	public int getBodyPartNumber(){
		return bodyPartNumber;
	}
	public int getModuleNumber(){
		return assignedOptionNumber;
	}
	public void turnOffSelectedColor(){
//		print ("Deseltected");
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript>().colors;
		cb.normalColor = startNormalColor;
		gameObject.GetComponent<modulePickerButtonScript>().colors = cb;
	}
	public void turnOnActiveGreen(){
		ColorBlock cb = gameObject.GetComponent<modulePickerButtonScript> ().colors;
		cb.normalColor = Color.green;
		gameObject.GetComponent<modulePickerButtonScript> ().colors = cb;
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
