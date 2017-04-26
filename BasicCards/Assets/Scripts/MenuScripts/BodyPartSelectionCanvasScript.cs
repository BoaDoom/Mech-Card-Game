using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BodyPartSelectionCanvasScript : MonoBehaviour {

	public EventSystem eventSystem;	
	public SceneTransferVariablesScript sceneTransferVariablesScript;
//	public int what;
	int headSelection;
	int armSelection;
	int torsoSelection;
	int shoulderSelection;
	int legSelection;
	//PickedBodyPart[] listOfPickedBodyParts;

	public Button nextButton;
//	bool thingsChecked;
	public void Start(){
		GameObject sceneTransferVariablesScriptTemp = GameObject.FindWithTag("SceneTransferVariables");
		if (sceneTransferVariablesScriptTemp != null) {
			sceneTransferVariablesScript = sceneTransferVariablesScriptTemp.GetComponent<SceneTransferVariablesScript> ();
		} else {
			print ("Couldnt find SceneTransferVariablesScript");
		}

		GameObject eventFinderTemp = GameObject.FindWithTag("EventSystem");
		if (eventFinderTemp != null) {
			eventSystem = eventFinderTemp.GetComponent<EventSystem> ();
		} else {
			print ("Couldnt find event system");
		}
		nextButton.onClick.AddListener(checkToMoveToPlayScreen);


	}

//	public void checkThing(int incomingint, int otherthing){
//		thingsChecked = true;
//	}
	public void markSelectedBodyPart(string nameOfPart, int incomingSelection){
		switch (nameOfPart) {
		case("Head"):
			headSelection =incomingSelection;
			break;
		case("Arm"):
			armSelection = incomingSelection;
			break;
		case("Torso"):
			torsoSelection = incomingSelection;
			break;
		case("Shoulder"):
			shoulderSelection = incomingSelection;
			break;
		case("Leg"):
			legSelection = incomingSelection;
			break;
		default:
			Debug.Log ("Unknown bodypart");
			break;
		}
		//Debug.Log(legSelection);
//		print (incomingSelection +" "+ nameOfPart);
//		bodyPartPickerButtonScript tempbodyPartPickerButtonScript =	eventSystem.currentSelectedGameObject.GetComponents<bodyPartPickerButtonScript> ();
//		tempbodyPartPickerButtonScript.setAsSelected ();
	}
	public bool checkIfBodyIsComplete(){
		return (headSelection >= 0 && armSelection >= 0 && torsoSelection >= 0 && shoulderSelection >= 0 && legSelection >= 0);
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
}
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