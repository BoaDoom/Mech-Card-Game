using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartVariationPanel : MonoBehaviour {


	private BodyPartSelectionCanvasScript partSelectionCanvas;
	bodyPartPickerButtonScript[] listOfAllTheText;
	string nameOfPartPanel;
	int currentSelectedPart = 0;
	public void Start(){
		GameObject canvasFinderTemp = GameObject.FindWithTag("PartSelectionCanvas");
		if(canvasFinderTemp != null){
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript>();
		}
		nameOfPartPanel = gameObject.name;
		listOfAllTheText = gameObject.GetComponentsInChildren<bodyPartPickerButtonScript> ();
		listOfAllTheText [0].turnOnActiveGreen ();
		StartCoroutine (partSelected (1));
		print ("done panel");
	}

	public IEnumerator partSelected(int incomingSelection){		//sending the value to the greater UI canvas to deal with populating the demo of the part on screen
		partSelectionCanvas.markSelectedBodyPart(nameOfPartPanel, incomingSelection);
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
}
