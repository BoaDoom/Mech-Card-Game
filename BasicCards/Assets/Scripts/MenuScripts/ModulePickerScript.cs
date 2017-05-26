using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePickerScript : MonoBehaviour {

	public modulePickerButtonScript buttonPrefab;
	public BodyPartSelectionCanvasScript partSelectionCanvas;
	BodyPartPreviewWindowScript parentBodyPartWindow;

	private string socketType;
	int currentSelectedModuleIDnumber = -1;
	int currentAssignedModulePickerIDnumber;
	int moduleSocketLabel; 		//the socket count for each limb is a max of 3, each module picker script has a label of which socket it is, 0,1 or 2
	XMLModuleData[] weaponModules;
	XMLModuleData[] utilityModules;
	XMLModuleData[] genericModules;

	modulePickerButtonScript[] listOfAllTheText;
	public IEnumerator ManualStart(){
		GameObject canvasFinderTemp = GameObject.FindWithTag ("PartSelectionCanvas");
		if (canvasFinderTemp != null) {
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript> ();
//			print ("canvas has been found");
		}
		yield return null;
	}
	public void takePreviewWindow(string incomingSocketType, BodyPartPreviewWindowScript incomingParentWindow, int incomingModulePickerIDnumber, int incomingSocketNumber){
		parentBodyPartWindow = incomingParentWindow;
		currentAssignedModulePickerIDnumber = incomingModulePickerIDnumber;
		moduleSocketLabel = incomingSocketNumber;
//		partSelectionCanvas = incomingPartSelectionCanvas;
		socketType = incomingSocketType;
//		print (partSelectionCanvas.name);
		List<int> listOfModulesInUse = partSelectionCanvas.getModulesAlreadyInUse ();

		if (incomingSocketType == "weapon") {
			weaponModules = partSelectionCanvas.getListOfModules ("Weapons");		//grabs all availible modules of the weapon type
			foreach (XMLModuleData modularData in weaponModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				StartCoroutine( newButton1.ManualStart(modularData.IDnum, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
//				print("modulardata " + modularData.moduleType);
			}
		}
		if (incomingSocketType == "utility") {
			utilityModules = partSelectionCanvas.getListOfModules ("Utility");		//grabs all availible modules of the utility type
			foreach (XMLModuleData modularData in utilityModules) {
				modulePickerButtonScript newButton1 = Instantiate (buttonPrefab, gameObject.GetComponent<Transform> ().position, gameObject.GetComponent<Transform> ().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				StartCoroutine( newButton1.ManualStart(modularData.IDnum, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
				//				print("modulardata " + modularData.moduleType);
			}
		}
		if (incomingSocketType == "both") {
			genericModules = partSelectionCanvas.getListOfModules ("Both");			//grabs all availible modules of both types
			foreach (XMLModuleData modularData in genericModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);

				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				StartCoroutine( newButton1.ManualStart(modularData.IDnum, modularData.cardNumber));		//needs to start after assigning of parent because the button grabs it's parent to be able to send info back
				//				print("modulardata " + modularData.moduleType);
			}
		}
		listOfAllTheText = gameObject.GetComponentsInChildren<modulePickerButtonScript> ();	
		foreach(modulePickerButtonScript buttonText in listOfAllTheText){
			foreach(int moduleIDinUse in listOfModulesInUse){
				if (moduleIDinUse == buttonText.getModuleIDNumber ()) {
					StartCoroutine(buttonText.disableButton ());
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
				moduleText.markAsUnselected ();
				StartCoroutine(parentBodyPartWindow.upwardsOLDModuleDeselected (moduleText.getModuleIDNumber(), moduleSocketLabel));	//turns off the selection of the module that was selected before this one
			}

		}
//		if (incomingModuleIDnumber <0){	//command comes through as -1 if the selection coming through is deselecting everything
////			parentBodyPartWindow.markSelectedModuleAsNull(currentAssignedModulePickerIDnumber);
////			partSelectionCanvas.markSelectedBodyPartAsNull(nameOfPartPanel);		**//this stores the number of the module selected somewhere, need to figure out best place to send module info to next screen so the correct--
//																					//--cards can be picked as well as have the body part that it's atatched to for destruction purposes
//			//StartCoroutine( bodyPartpreviewer.clearSquares());		**//needs to be replaced with the text that displays the module stats
//		}
		StartCoroutine(parentBodyPartWindow.upwardsModuleSelected (currentAssignedModulePickerIDnumber, moduleSocketLabel));		//sending the signal up the chain that a button was selected
//			currentVisualOfPart = partSelectionCanvas.markSelectedBodyPart(nameOfPartPanel, incomingModuleIDnumber);		//name of panel is built in to each version	
//			currentVisualOfPart.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			//StartCoroutine( bodyPartpreviewer.refreshSquares (currentVisualOfPart));		//sends the bodypart data to the preview square to populate the visual 	**//needs to be replaced with the text that displays the module stats


		yield return null;
	}
	public IEnumerator upwardsModuleDeselected(){
		StartCoroutine(parentBodyPartWindow.upwardsModuleDeselected (currentAssignedModulePickerIDnumber, moduleSocketLabel));
		yield return null;
	}

	public IEnumerator downwardsModuleSelected(int incomingModuleIDnumber){		//signal coming down the chain that a button was selected
		foreach (modulePickerButtonScript moduleText in listOfAllTheText) {
			if (moduleText.getModuleIDNumber () == incomingModuleIDnumber){
				if (!moduleText.selected) {
					StartCoroutine (moduleText.disableButton());
				}
			}
		}
		yield return null;
	}
	public IEnumerator downwardsModuleDeselected(int incomingModuleIDnumber){		//signal coming down the chain that a button was deselected
		foreach (modulePickerButtonScript moduleText in listOfAllTheText) {
			if (moduleText.getModuleIDNumber () == incomingModuleIDnumber){
				StartCoroutine(moduleText.enableButton());
			}
//			print ("turned back on module picker");
		}
		yield return null;
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
