﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePickerScript : MonoBehaviour {

	public modulePickerButtonScript buttonPrefab;
	public BodyPartSelectionCanvasScript partSelectionCanvas;
	BodyPartPreviewWindowScript parentBodyPartWindow;

	private string socketType;
	int currentSelectedModuleIDnumber = -1;
	int currentAssignedModulePickerIDnumber;
	XMLModuleData[] weaponModules;
	XMLModuleData[] utilityModules;
	XMLModuleData[] genericModules;

	modulePickerButtonScript[] listOfAllTheText;

	public void takePartSelectionCanvas(BodyPartSelectionCanvasScript incomingPartSelectionCanvas, string incomingSocketType, BodyPartPreviewWindowScript incomingParentWindow, int incomingModulePickerIDnumber){
		parentBodyPartWindow = incomingParentWindow;
		currentAssignedModulePickerIDnumber = incomingModulePickerIDnumber;
		partSelectionCanvas = incomingPartSelectionCanvas;
		socketType = incomingSocketType;
		List<int> listOfModulesInUse = partSelectionCanvas.getModulesAlreadyInUse ();

		if (incomingSocketType == "weapon") {
			weaponModules = partSelectionCanvas.getListOfModules ("Weapons");		//grabs all availible modules of the weapon type
			foreach (XMLModuleData modularData in weaponModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				StartCoroutine( newButton1.ManualStart(modularData.nameOfModule, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
//				print("modulardata " + modularData.moduleType);
			}
		}
		if (incomingSocketType == "utility") {
			utilityModules = partSelectionCanvas.getListOfModules ("Utility");		//grabs all availible modules of the utility type
			foreach (XMLModuleData modularData in utilityModules) {
				modulePickerButtonScript newButton1 = Instantiate (buttonPrefab, gameObject.GetComponent<Transform> ().position, gameObject.GetComponent<Transform> ().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				StartCoroutine( newButton1.ManualStart(modularData.nameOfModule, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
				//				print("modulardata " + modularData.moduleType);
			}
		}
		if (incomingSocketType == "both") {
			genericModules = partSelectionCanvas.getListOfModules ("Both");			//grabs all availible modules of both types
			foreach (XMLModuleData modularData in genericModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				StartCoroutine( newButton1.ManualStart(modularData.nameOfModule, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
				//				print("modulardata " + modularData.moduleType);
			}
		}
		listOfAllTheText = gameObject.GetComponentsInChildren<modulePickerButtonScript> ();	
		foreach(modulePickerButtonScript buttonText in listOfAllTheText){
			foreach(int moduleIDinUse in listOfModulesInUse){
				if (moduleIDinUse == buttonText.getModuleIDNumber ()) {
					buttonText.disableButton ();
				}
			}
		}

//		StartCoroutine (setPartSelected (1));
//		modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		modulePickerButtonScript newButton2 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//		newButton2.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
		
	}
	public int getCurrentModuleSelectedIDnumber(){
		return currentSelectedModuleIDnumber;
	}

	public IEnumerator upwardsModuleSelected(int incomingModuleIDnumber){		//sending the value to the greater UI canvas to get the info about the modules
		//		foreach (VisualOnlyBPartGenericScript child in transform) {///////////////////////////
		//			Destroy (child);
		//		}
		currentSelectedModuleIDnumber = incomingModuleIDnumber;		//setting current part selected number value

		foreach(modulePickerButtonScript moduleText in listOfAllTheText){		//turns off all the text buttons if they are not the currently selected option
			if ((moduleText.getModuleIDNumber () != currentSelectedModuleIDnumber) && moduleText.selected) {
				moduleText.turnOffSelectedColor ();
				parentBodyPartWindow.upwardsOLDModuleDeselected (moduleText.getModuleIDNumber());	//turns off the selection of the module that was selected before this one
			}

		}
		if (incomingModuleIDnumber <0){	//command comes through as -1 if the selection coming through is deselecting everything
//			parentBodyPartWindow.markSelectedModuleAsNull(currentAssignedModulePickerIDnumber);
//			partSelectionCanvas.markSelectedBodyPartAsNull(nameOfPartPanel);		**//this stores the number of the module selected somewhere, need to figure out best place to send module info to next screen so the correct--
																					//--cards can be picked as well as have the body part that it's atatched to for destruction purposes
			//StartCoroutine( bodyPartpreviewer.clearSquares());		**//needs to be replaced with the text that displays the module stats
		}
		parentBodyPartWindow.upwardsModuleSelected (currentAssignedModulePickerIDnumber);		//sending the signal up the chain that a button was selected
//			currentVisualOfPart = partSelectionCanvas.markSelectedBodyPart(nameOfPartPanel, incomingModuleIDnumber);		//name of panel is built in to each version	
//			currentVisualOfPart.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			//StartCoroutine( bodyPartpreviewer.refreshSquares (currentVisualOfPart));		//sends the bodypart data to the preview square to populate the visual 	**//needs to be replaced with the text that displays the module stats


		yield return null;
	}
	public IEnumerator upwardsModuleDeselected(){
		parentBodyPartWindow.upwardsModuleDeselected (currentAssignedModulePickerIDnumber);
		yield return null;
	}

	public void downwardsModuleSelected(int incomingModuleIDnumber){		//signal coming down the chain that a button was selected
		foreach (modulePickerButtonScript moduleText in listOfAllTheText) {
			if (moduleText.getModuleIDNumber () == incomingModuleIDnumber){
				if (!moduleText.selected) {
					moduleText.disableButton();
				}
			}
		}
	}
	public void downwardsModuleDeselected(int incomingModuleIDnumber){		//signal coming down the chain that a button was deselected
		foreach (modulePickerButtonScript moduleText in listOfAllTheText) {
			if (moduleText.getModuleIDNumber () == incomingModuleIDnumber){
				moduleText.enableButton();
			}
//			print ("turned back on module picker");
		}
//		print ("turned back on outside module picker");
		//currentSelectedModuleIDnumber = -1;
	}
	public string getSocketType(){
		return socketType;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
