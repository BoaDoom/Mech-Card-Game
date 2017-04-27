using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartVariationPanel : MonoBehaviour {


	private BodyPartSelectionCanvasScript partSelectionCanvas;
	bodyPartPickerButtonScript[] listOfAllTheText;
	string nameOfPartPanel;
	int currentSelectedPart = 0;
	bool completedStartup = false;
	BodyPartPreviewWindowScript bodyPartpreviewer;
	VisualOnlyBPartGenericScript currentVisualOfPart;
	public void Start(){
		GameObject canvasFinderTemp = GameObject.FindWithTag ("PartSelectionCanvas");
		if (canvasFinderTemp != null) {
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript> ();
		}
		nameOfPartPanel = gameObject.name;
		listOfAllTheText = gameObject.GetComponentsInChildren<bodyPartPickerButtonScript> ();
		bodyPartpreviewer = gameObject.GetComponentInChildren<BodyPartPreviewWindowScript>();////////////////////////////////// after grabbing sprite object, make a small version of PlayAreaScript to display part

		foreach(bodyPartPickerButtonScript bodyPartText in listOfAllTheText){
			StartCoroutine (bodyPartText.ManualStart ());
		}
		StartCoroutine (checkIfChildrenAreDone());		//checks if children of panel are done starting up
		completedStartup = true;
//		print ("panel startup done");
	}
	public IEnumerator ManualStart(){
		while (!completedStartup) {
//			print ("not done with panel");
			yield return new WaitForEndOfFrame();
		} 

	listOfAllTheText [0].turnOnActiveGreen ();		//turns on the first option as default
	StartCoroutine (partSelected (1));
//	print ("done with panel");
	yield return null;
	}

	public IEnumerator partSelected(int incomingSelection){		//sending the value to the greater UI canvas to get the info about the body parts
//		foreach (VisualOnlyBPartGenericScript child in transform) {///////////////////////////
//			Destroy (child);
//		}
		currentSelectedPart = incomingSelection;		//setting current part selected number value
		foreach(bodyPartPickerButtonScript bodyPartText in listOfAllTheText){		//turns off all the text buttons if they are not the currently selected option
			if (bodyPartText.getBodyPartNumber () != currentSelectedPart) {
				bodyPartText.turnOffSelectedColor ();
			}
		}
		if (incomingSelection <0){	//command comes through as -1 if the selection coming through is deselecting everything
			partSelectionCanvas.markSelectedBodyPartAsNull(nameOfPartPanel);
			StartCoroutine( bodyPartpreviewer.clearSquares());
		}
		else{
			currentVisualOfPart = partSelectionCanvas.markSelectedBodyPart(nameOfPartPanel, incomingSelection);		//name of panel is built in to each version	
			currentVisualOfPart.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			StartCoroutine( bodyPartpreviewer.refreshSquares (currentVisualOfPart));		//sends the bodypart data to the preview square to populate the visual
		}
		yield return null;
	}
	public int getPartSelected(){
		return currentSelectedPart;
	}
	public IEnumerator checkIfChildrenAreDone(){	//checks to see if all of the text button scripts have started before continueing
		foreach(bodyPartPickerButtonScript bodyPartText in listOfAllTheText){
			while (!bodyPartText.completedStartQuery()){
				yield return new WaitForEndOfFrame();
			}

		}
//		print ("text button done");
		yield return null;
	}
}
