using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Linq;

public class BodyPartSelectionCanvasScript : MonoBehaviour {

	public EventSystem eventSystem;	
	public SceneTransferVariablesScript sceneTransferVariablesScript;

	BodyPartVariationPanel[] listOfAllThePanels;
	BodyPartDataHolder partData = null;
	public VisualOnlyBPartGenericScript visualOnlyBodyPartObject;

	VisualOnlyBPartGenericScript tempBodyPart;

	BPartXMLReaderScript bPartXMLReader;
	XMLModuleLoaderScript XMLModuleLoader;
//	public int what;
	BPartWithModuleInfo headSelection = new BPartWithModuleInfo();
	BPartWithModuleInfo armSelection = new BPartWithModuleInfo();
	BPartWithModuleInfo torsoSelection = new BPartWithModuleInfo();
	BPartWithModuleInfo shoulderSelection = new BPartWithModuleInfo();
	BPartWithModuleInfo legSelection = new BPartWithModuleInfo();
	//PickedBodyPart[] listOfPickedBodyParts;
	public XMLModuleData[] listOfWeaponModules;
	public XMLModuleData[] listOfUtilityModules;


	public Button nextButton;
//	bool thingsChecked;
	public void Start(){

		//grab XML data for modules and store it here for the ModulePickerScript to request and grab later
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");		//grabbing the object with the body part info taken from xml data	
		if (loaderScriptTemp == null) {
			SceneManager.LoadScene ("XMLLoaderScene");
			return;
		} else if (loaderScriptTemp != null) {
			tempBodyPart = Instantiate (visualOnlyBodyPartObject, Vector3.zero, gameObject.GetComponent<Transform>().rotation);

			bPartXMLReader = loaderScriptTemp.GetComponent<BPartXMLReaderScript> ();
			XMLModuleLoader = loaderScriptTemp.GetComponent<XMLModuleLoaderScript> ();

			int tempWeaponInt = 0;
			int tempUtilityInt = 0;
			foreach (XMLModuleData moduleData in XMLModuleLoader.data) {
				if (moduleData.moduleType == "Weapons") {
					tempWeaponInt++;
				}
				if (moduleData.moduleType == "Utility") {
					tempUtilityInt++;
				}
			}
			listOfWeaponModules = new XMLModuleData[tempWeaponInt];
			listOfUtilityModules = new XMLModuleData[tempUtilityInt];

			tempWeaponInt = 0;
			tempUtilityInt = 0;
			foreach(XMLModuleData moduleData in XMLModuleLoader.data){
				if (moduleData.moduleType == "Weapons") {
					listOfWeaponModules[tempWeaponInt] = new XMLModuleData();
					listOfWeaponModules [tempWeaponInt].CopyData (moduleData);
					tempWeaponInt++;
				}
				else if (moduleData.moduleType == "Utility") {
					listOfUtilityModules[tempUtilityInt] = new XMLModuleData();
					listOfUtilityModules [tempUtilityInt].CopyData (moduleData);
					tempUtilityInt++;
				}
			}
		
			GameObject sceneTransferVariablesScriptTemp = GameObject.FindWithTag ("SceneTransferVariables");
			if (sceneTransferVariablesScriptTemp != null) {
				sceneTransferVariablesScript = sceneTransferVariablesScriptTemp.GetComponent<SceneTransferVariablesScript> ();
			} else {
				print ("Couldnt find SceneTransferVariablesScript");
			}

			GameObject eventFinderTemp = GameObject.FindWithTag ("EventSystem");
			if (eventFinderTemp != null) {
				eventSystem = eventFinderTemp.GetComponent<EventSystem> ();
			} else {
				print ("Couldnt find event system");
			}
			nextButton.onClick.AddListener (checkToMoveToPlayScreen);
			//VisualOnlyBPartGenericScript tempBodyPart;
			listOfAllThePanels = gameObject.GetComponentsInChildren<BodyPartVariationPanel> ();
			foreach (BodyPartVariationPanel panel in listOfAllThePanels) {
				StartCoroutine (panel.ManualStart ());
			}


		}
			
			//		print ("done panel");
	}

//	public void checkThing(int incomingint, int otherthing){
//		thingsChecked = true;
//	}
	public VisualOnlyBPartGenericScript markSelectedBodyPart(string nameOfPart, int incomingSelection){		//almost the same as PlayerScript method populate body

		//need to swap over the identifying variable from a string to the new class so it can carry the module info and choices. Maybe? needs to convey more info at some point

		switch (nameOfPart) {
		case("Head"):
			headSelection.nameOfSelection = "head "+ intToStringNumber(incomingSelection); //intToStringNumber(
			return makeBodyPart(headSelection.nameOfSelection);
		case("Arm"):
			armSelection.nameOfSelection = "arm "+ intToStringNumber(incomingSelection);
			return makeBodyPart(armSelection.nameOfSelection);
		case("Torso"):
			torsoSelection.nameOfSelection = "torso " + intToStringNumber(incomingSelection);
			return makeBodyPart(torsoSelection.nameOfSelection);
		case("Shoulder"):
			shoulderSelection.nameOfSelection = "shoulder "+ intToStringNumber(incomingSelection);
			return makeBodyPart(shoulderSelection.nameOfSelection);
		case("Leg"):
			legSelection.nameOfSelection = "leg "+ intToStringNumber(incomingSelection);
			return makeBodyPart(legSelection.nameOfSelection);
		default:
			Debug.Log ("Unknown bodypart");
			return makeBodyPart (legSelection.nameOfSelection);
		}
	}
	public void markSelectedBodyPartAsNull(string nameOfPart){		//for deselecting the body part
		switch (nameOfPart) {
		case("Head"):
			headSelection.nameOfSelection = null;
			break;
		case("Arm"):
			armSelection.nameOfSelection = null;
			break;
		case("Torso"):
			torsoSelection.nameOfSelection = null;
			break;
		case("Shoulder"):
			shoulderSelection.nameOfSelection = null;
			break;
		case("Leg"):
			legSelection.nameOfSelection = null;
			break;
		default:
			Debug.Log ("Unknown bodypart");
			break;
		}
	}
	public bool checkIfBodyIsComplete(){
		return (headSelection != null && armSelection != null && torsoSelection != null && shoulderSelection != null && legSelection != null);
	}
	public void onClick(){
		print ("clicked");
	}
	public void checkToMoveToPlayScreen(){
		if (checkIfBodyIsComplete()) {
			AllPickedBodyParts allPickedBodyPartsTemp = new AllPickedBodyParts ();
			allPickedBodyPartsTemp.setAllPickedBodyParts(headSelection.nameOfSelection, armSelection.nameOfSelection, torsoSelection.nameOfSelection, shoulderSelection.nameOfSelection, legSelection.nameOfSelection);
//			print (allPickedBodyPartsTemp.pickedHead);
//			sceneTransferVariablesScript.bleh ();
			sceneTransferVariablesScript.setPartsPicked(allPickedBodyPartsTemp);
			SceneManager.LoadScene ("_Main");
		} else {
			Debug.Log ("Not all body parts have a selection");
		}
	}


	public VisualOnlyBPartGenericScript makeBodyPart(string nameOfpart){	//takes the clicked on part name, and finds it in the xmldata and creates a visual only body part to display
		partData = bPartXMLReader.getBodyData (nameOfpart);

		//VisualOnlyBPartGenericScript tempBodyPart = Instantiate (visualOnlyBodyPartObject, Vector3.zero, gameObject.GetComponent<Transform>().rotation);

		tempBodyPart.CreateNewPart (partData);		// 
		return tempBodyPart;
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
	public XMLModuleData[] getListOfModules(string incomingRequestForList){
//		XMLModuleData[] listOfAvailibleModules = new XMLModuleData()[];
		if (incomingRequestForList == "Weapons"){
			return listOfWeaponModules;
		}
		if (incomingRequestForList == "Utility"){
			return listOfUtilityModules;
		}
		if (incomingRequestForList == "Both"){
			int tempWeaponListCount = listOfWeaponModules.Length;
			int tempUtilityListCount = listOfUtilityModules.Length;
			XMLModuleData[] listOfBothTypesOfModules = new XMLModuleData[tempWeaponListCount+tempUtilityListCount];
			for (int i = 0; i < tempWeaponListCount; i++) {
				listOfBothTypesOfModules [i] = listOfWeaponModules[i];
			}
			for (int i = 0; i < tempUtilityListCount; i++) {
				listOfBothTypesOfModules [tempWeaponListCount+i] = listOfUtilityModules[i];
			}
			return listOfBothTypesOfModules;
		}
		return null;
	}
}
public class BPartWithModuleInfo{
	public string nameOfSelection{ get; set; }
	int[] listOfSelectedModules;
	public BPartWithModuleInfo(){
	}
}

//public class BodyPartNode{
//	private bool exists;
//	public BodyPartNode(){
//		exists = false;
//	}
//	public bool getState(){
//		return exists;
//	}
//
//
//	public void turnOn(){
//		exists = true;
//	}
//	public void turnOff(){
//		exists = false;
//	}
//}
//public class PickedBodyPart{
//	public string nameOfPartType{ get; set; }
//	public int numberOfPart{ get; set; }
//	public PickedBodyPart(int incomingNumberOfPart, string incomingNameOfPartType){
//		nameOfPartType = incomingNameOfPartType;
//		numberOfPart = incomingNumberOfPart;
////		Debug.Log(nameOfPartType);
////		Debug.Log(numberOfPart);
//	}
//}
//public class AllPickedBodyParts{
//	public int pickedHead{ get; private set; }
//	public int pickedArm{ get; private  set; }
//	public int pickedTorso{ get; private  set; }
//	public int pickedShoulder{ get; private  set; }
//	public int pickedLeg{ get; private  set; }
//	public void setAllPickedBodyParts(int head, int arm, int torso, int shoulder, int leg){
//		pickedHead = head;
//		pickedArm = arm;
//		pickedTorso = torso;
//		pickedShoulder = shoulder;
//		pickedLeg = leg;
//	}
//}