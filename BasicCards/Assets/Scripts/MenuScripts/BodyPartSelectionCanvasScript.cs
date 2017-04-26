using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BodyPartSelectionCanvasScript : MonoBehaviour {

	public EventSystem eventSystem;	
	public SceneTransferVariablesScript sceneTransferVariablesScript;

	BodyPartVariationPanel[] listOfAllThePanels;
	BodyPartDataHolder partData = null;
	public VisualOnlyBPartGenericScript visualOnlyBodyPartObject;

	BPartXMLReaderScript bPartXMLReader;
//	public int what;
	string headSelection;
	string armSelection;
	string torsoSelection;
	string shoulderSelection;
	string legSelection;
	//PickedBodyPart[] listOfPickedBodyParts;


	public Button nextButton;
//	bool thingsChecked;
	public void Start(){
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");		//grabbing the object with the body part info taken from xml data	
		if (loaderScriptTemp == null) {
			SceneManager.LoadScene ("XMLLoaderScene");
			return;
		} else if (loaderScriptTemp != null) {
			bPartXMLReader = loaderScriptTemp.GetComponent<BPartXMLReaderScript> ();
		

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
		switch (nameOfPart) {
		case("Head"):
			headSelection = "head "+ intToStringNumber(incomingSelection); //intToStringNumber(
			return makeBodyPart(headSelection);
		case("Arm"):
			armSelection = "arm "+ intToStringNumber(incomingSelection);
			return makeBodyPart(armSelection);
		case("Torso"):
			torsoSelection = "torso " + intToStringNumber(incomingSelection);
			return makeBodyPart(torsoSelection);
		case("Shoulder"):
			shoulderSelection = "shoulder "+ intToStringNumber(incomingSelection);
			return makeBodyPart(shoulderSelection);
		case("Leg"):
			legSelection = "leg "+ intToStringNumber(incomingSelection);
			return makeBodyPart(legSelection);
		default:
			Debug.Log ("Unknown bodypart");
			return makeBodyPart (legSelection);
		}
		//Debug.Log(legSelection);
//		print (incomingSelection +" "+ nameOfPart);
//		bodyPartPickerButtonScript tempbodyPartPickerButtonScript =	eventSystem.currentSelectedGameObject.GetComponents<bodyPartPickerButtonScript> ();
//		tempbodyPartPickerButtonScript.setAsSelected ();
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
			allPickedBodyPartsTemp.setAllPickedBodyParts(headSelection, armSelection, torsoSelection, shoulderSelection, legSelection);
//			print (allPickedBodyPartsTemp.pickedHead);
//			sceneTransferVariablesScript.bleh ();
			sceneTransferVariablesScript.setPartsPicked(allPickedBodyPartsTemp);
			SceneManager.LoadScene ("_Main");
		} else {
			Debug.Log ("Not all body parts have a selection");
		}
	}


	public VisualOnlyBPartGenericScript makeBodyPart(string nameOfpart){
		partData = bPartXMLReader.getBodyData (nameOfpart);
		VisualOnlyBPartGenericScript instaBodypart = Instantiate (visualOnlyBodyPartObject, Vector3.zero, gameObject.GetComponent<Transform>().rotation);
		instaBodypart.CreateNewPart (partData);		// 
		return instaBodypart;
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