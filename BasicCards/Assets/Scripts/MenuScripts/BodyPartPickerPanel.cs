using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartPickerPanel : MonoBehaviour {


	private BodyPartSelectionCanvasScript partSelectionCanvas;
	bodyPartPickerButtonScript[] listOfAllTheText;
	public bodyPartPickerButtonScript prefabBPartButton;
	string nameOfPartPanel;
	int pickerListNumber = -1;
	int currentSelectionIDnum;
	List<BodyPartDataHolder> allBodyPartsOfThisType;

//	bool completedStartup = false;
	public BodyPartPreviewWindowScript bodyPartpreviewerPrefab;
	BodyPartPreviewWindowScript bodyPartpreviewer;
	VisualOnlyBPartGenericScript currentVisualOfPart;
//	public void Start(){
//		StartCoroutine (ManualStart ());	//don't ask me
//	}
	public void ManualStart(){
		GameObject canvasFinderTemp = GameObject.FindWithTag ("PartSelectionCanvas");
		if (canvasFinderTemp != null) {
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript> ();
		} else {
			print ("didn't find canvas");
		}
//		print ("Panel has started");
	
		bodyPartpreviewer = Instantiate (bodyPartpreviewerPrefab, gameObject.GetComponent<Transform> ().position + new Vector3(75f, 0f, 0f), gameObject.GetComponent<Transform> ().rotation);
		bodyPartpreviewer.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
		bodyPartpreviewer.GetComponent<Transform> ().localScale = new Vector3 (75f, 75f, 0f);

		nameOfPartPanel = gameObject.name;		//the name is also the type of bodypart it is
//		print(partSelectionCanvas);
		allBodyPartsOfThisType = partSelectionCanvas.getAllBodyDataForType (nameOfPartPanel);
		listOfAllTheText = new bodyPartPickerButtonScript[allBodyPartsOfThisType.Count];
//		foreach (BodyPartDataHolder bodyPartText in allBodyPartsOfThisType) {
////			print (bodyPartText.name);
////			print (bodyPartText.typeOfpart);
////			print (bodyPartText.maxHealth);
////			print (bodyPartText.BpartIDnum);
//		}
		//print ("all body parts of this type: " + allBodyPartsOfThisType.Count);
		//get the list of data for the type of body part this is from the xml file via the canvas script 

		int tempX = 0;
		foreach(BodyPartDataHolder bodyPartText in allBodyPartsOfThisType){
			bodyPartPickerButtonScript newButton1 = Instantiate(prefabBPartButton, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//			print ("body part text: " + bodyPartText.name);
			newButton1.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			newButton1.GetComponent<Transform> ().localScale = new Vector3 (1f, 1f, 1f);
//			print (nameOfPartPanel +" "+bodyPartText.BpartIDnum);
			StartCoroutine (newButton1.ManualStart (bodyPartText.name, bodyPartText.BpartIDnum, tempX));
			listOfAllTheText [tempX] = newButton1;

			tempX++;
		}

//		StartCoroutine (checkIfChildrenAreDone());		//checks if children of panel are done starting up

//		listOfAllTheText = gameObject.GetComponentsInChildren<bodyPartPickerButtonScript> ();
//		bodyPartpreviewer = gameObject.GetComponentInChildren<BodyPartPreviewWindowScript>();////////////////////////////////// after grabbing sprite object, make a small version of PlayAreaScript to display part

//		foreach(bodyPartPickerButtonScript bodyPartText in listOfAllTheText){
//			StartCoroutine (bodyPartText.ManualStart ());
//		}
//		StartCoroutine (checkIfChildrenAreDone());		//checks if children of panel are done starting up
//		completedStartup = true;
		//		print ("panel startup done");
//		while (!completedStartup) {
//			print ("not done with panel");
////			yield return new WaitForEndOfFrame();
//		} 
//		print ("done with panel");
		StartCoroutine(bodyPartpreviewer.ManualStart());

		StartCoroutine (partSelected (listOfAllTheText [0].getBodyPartIDnum(), 0));
		listOfAllTheText [0].turnOnActiveGreen ();		//turns on the first option as default
	}

	public IEnumerator partSelected(int incomingSelectionIDnum, int incomingPickerListNumber){		//sending the value to the greater UI canvas to get the info about the body parts
//		foreach (VisualOnlyBPartGenericScript child in transform) {///////////////////////////
//			Destroy (child);
//		}
//		print(incomingPickerListNumber +" "+incomingSelectionIDnum);
		currentSelectionIDnum = incomingSelectionIDnum;
		pickerListNumber = incomingPickerListNumber;		//setting current part selected number value
//		print("picker list number being selected" +pickerListNumber);
		foreach(bodyPartPickerButtonScript bodyPartText in listOfAllTheText){		//turns off all the text buttons if they are not the currently selected option
			if (bodyPartText.getPickerListNumber () != pickerListNumber) {
				bodyPartText.turnOffSelectedColor ();
			}
		}
//		if (incomingPanelSelection <0){	//command comes through as -1 if the selection coming through is deselecting everything
//			partSelectionCanvas.markSelectedBodyPartAsNull(incomingPanelSelection);
//			StartCoroutine( bodyPartpreviewer.clearSquares());
//		}
//		else{
//		print(incomingPickerListNumber);
		currentVisualOfPart = partSelectionCanvas.markSelectedBodyPart(incomingSelectionIDnum);		//name of panel is built in to each version	
		currentVisualOfPart.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
//		print (currentVisualOfPart.getName ());
		StartCoroutine( bodyPartpreviewer.refreshSquares (currentVisualOfPart));		//sends the bodypart data to the preview square to populate the visual


//		}
		yield return null;
	}
	public IEnumerator partDeselected(int incomingSelectionIDnum){
//		print ("incoming selection for unselecting" + incomingSelectionIDnum);
		partSelectionCanvas.markSelectedBodyPartAsNull(incomingSelectionIDnum);
		pickerListNumber = -1;
		StartCoroutine( bodyPartpreviewer.clearSquares());
		yield return null;
	}
	public int getPartSelectedIDnum(){
		return currentSelectionIDnum;
	}
	public int getPickerListNumber(){
		return pickerListNumber;
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
	public IEnumerator upwardsModuleSelected(int incomingModuleIDnumber, int incomingModuleSocketLabel){		//coming from
		//alreadySelectedModules.Add (incomingModuleIDnumber);
		//foreach (BodyPartPreviewWindowScript BPartWindow in allBPartWindows) {		//the loop for setting all of the already active module picker's  buttons to turn off
		StartCoroutine( partSelectionCanvas.upwardsModuleSelected (incomingModuleIDnumber, nameOfPartPanel, incomingModuleSocketLabel));
		//}
		yield return null;
	}
	public IEnumerator upwardsModuleDeselected(int incomingModuleIDnumber, int incomingModuleSocketLabel){
		//		List<int> tempAlreadySelectedModules = new List<int> ();
		//		tempAlreadySelectedModules = alreadySelectedModules;
//		int tempCount = alreadySelectedModules.Count;
//		for (int i = 0; i < tempCount; i++) {
//			if (alreadySelectedModules[i] == incomingModuleIDnumber) {
//				alreadySelectedModules.Remove(incomingModuleIDnumber);
//				i--;
//				tempCount--;
//			}
//		}

//		foreach (BodyPartPreviewWindowScript BPartWindow in allBPartWindows) {		//the loop for setting all of the already active module picker's  buttons to turn off
		StartCoroutine(partSelectionCanvas.upwardsModuleDeselected (incomingModuleIDnumber, nameOfPartPanel, incomingModuleSocketLabel));
		yield return null;
	}
	public IEnumerator downwardsModuleSelected(int incomingModuleIDnumber){
		StartCoroutine( bodyPartpreviewer.downwardsModuleSelected (incomingModuleIDnumber));
		yield return null;
	}
	public IEnumerator downwardsModuleDeselected(int incomingModuleIDnumber){
		StartCoroutine( bodyPartpreviewer.downwardsModuleDeselected (incomingModuleIDnumber));
		yield return null;
	}

}
