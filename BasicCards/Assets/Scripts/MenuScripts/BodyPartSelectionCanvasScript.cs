using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BodyPartSelectionCanvasScript : MonoBehaviour {

	public EventSystem eventSystem;	
//	public int what;
	int headSelection = -1;
	int armSelection = -1;
	int torsoSelection = -1;
	int shoulderSelection = -1;
	int legSelection = -1;
//	bool thingsChecked;
	public void Start(){
		GameObject eventFinderTemp = GameObject.FindWithTag("EventSystem");
		if (eventFinderTemp != null) {
			eventSystem = eventFinderTemp.GetComponent<EventSystem> ();
		} else {
			print ("Couldnt find event system");
		}
	}

//	public void checkThing(int incomingint, int otherthing){
//		thingsChecked = true;
//	}
	public void markSelectedBodyPart(string nameOfPart, int incomingSelection){
		switch (nameOfPart) {
		case("Head"):
			headSelection = incomingSelection;
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
		print (incomingSelection +" "+ nameOfPart);
//		bodyPartPickerButtonScript tempbodyPartPickerButtonScript =	eventSystem.currentSelectedGameObject.GetComponents<bodyPartPickerButtonScript> ();
//		tempbodyPartPickerButtonScript.setAsSelected ();
	}
	public bool checkIfBodyIsComplete(){
		return (headSelection >= 0 && armSelection >= 0 && torsoSelection >= 0 && shoulderSelection >= 0 && legSelection >= 0);
	}
	public void onClick(){
		print ("clicked");
	}
}
