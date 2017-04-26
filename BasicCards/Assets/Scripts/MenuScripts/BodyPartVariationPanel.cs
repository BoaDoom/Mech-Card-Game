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
	SpriteRenderer bodyPartMenuViewer;
	VisualOnlyBPartGenericScript currentVisualOfPart;
	public void Start(){
		GameObject canvasFinderTemp = GameObject.FindWithTag ("PartSelectionCanvas");
		if (canvasFinderTemp != null) {
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript> ();
		}
		nameOfPartPanel = gameObject.name;
		listOfAllTheText = gameObject.GetComponentsInChildren<bodyPartPickerButtonScript> ();
		bodyPartMenuViewer = gameObject.GetComponentInChildren<SpriteRenderer>();////////////////////////////////// after grabbing sprite object, make a small version of PlayAreaScript to display part


		StartCoroutine (checkIfChildrenAreDone());		//checks if children of panel are done starting up
		completedStartup = true;
//		print ("panel startup done");
	}
	public IEnumerator ManualStart(){
		while (!completedStartup) {
//			print ("not done with panel");
			yield return new WaitForEndOfFrame();
		} 

	listOfAllTheText [0].turnOnActiveGreen ();
	StartCoroutine (partSelected (1));
//	print ("done with panel");
	yield return null;
	}

	public IEnumerator partSelected(int incomingSelection){		//sending the value to the greater UI canvas to deal with populating the demo of the part on screen
		currentVisualOfPart = partSelectionCanvas.markSelectedBodyPart(nameOfPartPanel, incomingSelection);			
		currentSelectedPart = incomingSelection;		//setting current part selected
		foreach(bodyPartPickerButtonScript bodyPartText in listOfAllTheText){
			if (bodyPartText.getBodyPartNumber () != currentSelectedPart) {
				bodyPartText.turnOffSelectedColor ();
			}
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
