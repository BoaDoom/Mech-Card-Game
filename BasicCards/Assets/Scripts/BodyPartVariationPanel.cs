using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartVariationPanel : MonoBehaviour {


	private BodyPartSelectionCanvasScript partSelectionCanvas;
	string nameOfPartPanel;
	public void Start(){
		GameObject canvasFinderTemp = GameObject.FindWithTag("PartSelectionCanvas");
		if(canvasFinderTemp != null){
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript>();
		}
		nameOfPartPanel = gameObject.name;
	}

	public void partSelected(int incomingSelection){
		partSelectionCanvas.markSelectedBodyPart(nameOfPartPanel, incomingSelection);
	}
}
