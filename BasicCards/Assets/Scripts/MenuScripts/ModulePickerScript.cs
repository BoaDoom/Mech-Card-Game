using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePickerScript : MonoBehaviour {

	public modulePickerButtonScript buttonPrefab;
	public BodyPartSelectionCanvasScript partSelectionCanvas;
	private string socketType;
	int currentModuleSelected = -1;
	XMLModuleData[] weaponModules;
	XMLModuleData[] utilityModules;
	XMLModuleData[] genericModules;

	modulePickerButtonScript[] listOfAllTheText;

	public void takePartSelectionCanvas(BodyPartSelectionCanvasScript incomingPartSelectionCanvas, string incomingSocketType){

		partSelectionCanvas = incomingPartSelectionCanvas;
		socketType = incomingSocketType;

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
//		StartCoroutine (setPartSelected (1));
//		modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		modulePickerButtonScript newButton2 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//		newButton2.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
		
	}
	public int getPartSelected(){
		return currentModuleSelected;
	}

	public IEnumerator setPartSelected(int incomingSelection){		//sending the value to the greater UI canvas to get the info about the body parts
		//		foreach (VisualOnlyBPartGenericScript child in transform) {///////////////////////////
		//			Destroy (child);
		//		}
		currentModuleSelected = incomingSelection;		//setting current part selected number value
		foreach(modulePickerButtonScript moduleText in listOfAllTheText){		//turns off all the text buttons if they are not the currently selected option
			if (moduleText.getModuleNumber () != currentModuleSelected) {
				moduleText.turnOffSelectedColor ();
			}
		}
		if (incomingSelection <0){	//command comes through as -1 if the selection coming through is deselecting everything
//			partSelectionCanvas.markSelectedBodyPartAsNull(nameOfPartPanel);		**//this stores the number of the module selected somewhere, need to figure out best place to send module info to next screen so the correct--
																					//--cards can be picked as well as have the body part that it's atatched to for destruction purposes
			//StartCoroutine( bodyPartpreviewer.clearSquares());		**//needs to be replaced with the text that displays the module stats
		}
		else{
//			currentVisualOfPart = partSelectionCanvas.markSelectedBodyPart(nameOfPartPanel, incomingSelection);		//name of panel is built in to each version	
//			currentVisualOfPart.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			//StartCoroutine( bodyPartpreviewer.refreshSquares (currentVisualOfPart));		//sends the bodypart data to the preview square to populate the visual 	**//needs to be replaced with the text that displays the module stats


		}
		yield return null;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
