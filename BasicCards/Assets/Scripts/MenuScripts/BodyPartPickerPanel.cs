using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyPartPickerPanel : MonoBehaviour {


	private BodyPartSelectionCanvasScript partSelectionCanvas;
	bodyPartPickerButtonScript[] listOfAllTheText;
	public bodyPartPickerButtonScript prefabBPartButton;
	string nameOfPartPanel;
	string nameOfListedPartType;
	int sideDirection; //0 for none, 1 for left, 2 for right
	int pickerListNumber = -1;
	int currentSelectionIDnum;
	List<BodyPartDataHolder> allBodyPartsOfThisType;

//	bool completedStartup = false;
	public ModulePickerScript modulePickerScript;
	public ModulePickerScript[] allModulePickerScripts;
	PartPickerAreaScript pickerPlayArea;
	BPartGenericScript bPartGeneric;
	Transform moduleTransformMarker;

	int leftOrRightPanelToPanel; //the orientation of the body part picker and the module picker panels
//	public void Start(){
//		StartCoroutine (ManualStart ());	//don't ask me
//	}
	public void ManualStart(){
//		print ("Panel manually started");
		GameObject canvasFinderTemp = GameObject.FindWithTag ("PartSelectionCanvas");
		if (canvasFinderTemp != null) {
			partSelectionCanvas = canvasFinderTemp.GetComponent<BodyPartSelectionCanvasScript> ();
		} else {
			print ("didn't find canvas");
		}

		GameObject pickerPlayAreaTemp = GameObject.FindWithTag ("PartPickerPlayArea");
		if (pickerPlayAreaTemp != null) {
			pickerPlayArea = pickerPlayAreaTemp.GetComponent<PartPickerAreaScript> ();
//			print ("picker play area " + pickerPlayArea.name);
		} else {
			print ("didn't find play area");
		}

//		ModulePickerScript modulePickerScriptTemp = gameObject.GetComponentInChildren<ModulePickerScript>();
//		if (modulePickerScriptTemp != null) {
//			modulePickerScript = modulePickerScriptTemp.GetComponent<ModulePickerScript> ();
//		} else {
//			print ("module picker is missing");
//		}ModulePickerMarker

		ModuleMarkerMarkerScript moduleMarkerTemp =  gameObject.GetComponentInChildren<ModuleMarkerMarkerScript>();
		if (moduleMarkerTemp != null) {
			moduleTransformMarker = moduleMarkerTemp.GetComponent<Transform> ();
		} else {
			print ("module marker is missing");
		}
		if ((gameObject.GetComponent<Transform>().position.x -  moduleTransformMarker.position.x) > 0){
//			print("math "+Mathf.Abs( gameObject.GetComponent<Transform>().position.x));
//			print("math "+Mathf.Abs( moduleTransformMarker.position.x));
			leftOrRightPanelToPanel = -1;
//			print ("Negative");
		}
		else{
//			print("math "+Mathf.Abs( gameObject.GetComponent<Transform>().position.x));
//			print("math "+Mathf.Abs( moduleTransformMarker.position.x));
			leftOrRightPanelToPanel = 1;
//			print ("POsitive");
		}
//		print (gameObject.name+" "+gameObject.GetComponent<Transform>().position.x);
//		print ("marker "+ moduleTransformMarker.position.x);
			

		nameOfPartPanel = gameObject.name;		//the name is also the type of bodypart it is

		switch (nameOfPartPanel) {
		case("Head"):
				nameOfListedPartType = "Head";
				sideDirection = 0;
				break;
		case("Torso"):
				nameOfListedPartType = "Torso";
				sideDirection = 0;
				break;
		case("Legs"):
				nameOfListedPartType = "Leg";
				sideDirection = 0;
				break;
		case("LeftShoulder"):
				nameOfListedPartType = "Shoulder";
				sideDirection = 1;
				break;
		case("RightShoulder"):
				nameOfListedPartType = "Shoulder";
				sideDirection = 2;
				break;
		case("LeftArm"):
				nameOfListedPartType = "Arm";
				sideDirection = 1;
				break;
		case("RightArm"):
				nameOfListedPartType = "Arm";
				sideDirection = 2;
				break;
		default:
				print ("bodypart panel name is not recognized");
				break;
		}

//		print(partSelectionCanvas);
		allBodyPartsOfThisType = partSelectionCanvas.getAllBodyDataForType (nameOfListedPartType);		//uses the assigned name to grab all the data for every type listed in the xml
		listOfAllTheText = new bodyPartPickerButtonScript[allBodyPartsOfThisType.Count];		//creates the list of the text/buttons

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
			
		StartCoroutine (partSelected (listOfAllTheText [0].getBodyPartIDnum(), 0));		//auto pick the first one on the list
//		if (nameOfPartPanel == "Legs") {
//			StartCoroutine (partSelected (listOfAllTheText [0].getBodyPartIDnum (), 0));
//		}
		listOfAllTheText [0].turnOnActiveGreen ();		//turns on the first option as default
	}

	public IEnumerator partSelected(int incomingSelectionIDnum, int incomingPickerListNumber){		//sending the value to the greater UI canvas to get the info about the body parts
		currentSelectionIDnum = incomingSelectionIDnum;
		pickerListNumber = incomingPickerListNumber;		//setting current part selected number value
		foreach(bodyPartPickerButtonScript bodyPartText in listOfAllTheText){		//turns off all the text buttons if they are not the currently selected option
			if (bodyPartText.getPickerListNumber () != pickerListNumber) {
				bodyPartText.turnOffSelectedColor ();
			}
		}
//		print (partSelectionCanvas.name);
		bPartGeneric = partSelectionCanvas.markSelectedBodyPart(allBodyPartsOfThisType[incomingPickerListNumber], sideDirection);		//sends the bodyholder info to the canvas to create and designate the part

//		print (allModulePickerScripts.Length);
		foreach (ModulePickerScript modulepickScript in allModulePickerScripts) {
//			print
			modulepickScript.destroyCompletely ();
		}
		allModulePickerScripts = new ModulePickerScript[bPartGeneric.getModuleSocketCount().getTotalCount()];
//		print (moduleTransformMarker);
//		int indyTempCount = 0;
		int weaponCount = bPartGeneric.getModuleSocketCount ().getWeaponCount ();
		int utilityCount = bPartGeneric.getModuleSocketCount ().getUtilityCount ();
		int bothCount = bPartGeneric.getModuleSocketCount ().getBothCount ();


		for (int i = 0; i < bPartGeneric.getModuleSocketCount ().getTotalCount (); i++) {
			allModulePickerScripts [i] = Instantiate (modulePickerScript, moduleTransformMarker.localPosition, moduleTransformMarker.rotation);
			allModulePickerScripts [i].GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//			allModulePickerScripts [i].GetComponent<Transform> ().localScale = new Vector3 (10f, 10f, 10f);//moduleTransformMarker.localScale;
//			print(gameObject.name+" "+ leftOrRightPanelToPanel);
			allModulePickerScripts [i].GetComponent<Transform> ().localPosition = new Vector3 ((75f * i * leftOrRightPanelToPanel), 1, 1) + moduleTransformMarker.localPosition;
//			for (int x=0; x < bPartGeneric.getModuleSocketCount().weaponModuleSocketCount; x++){
//			}
			if (weaponCount > 0) {
				StartCoroutine (allModulePickerScripts [i].ManualStart (gameObject.GetComponent<BodyPartPickerPanel>()));
				StartCoroutine (allModulePickerScripts [i].takeModuleInfo ("weapon",  i));
				weaponCount -= 1;
//				break;
			} 
			else if (utilityCount > 0) {
				StartCoroutine (allModulePickerScripts [i].ManualStart (gameObject.GetComponent<BodyPartPickerPanel>()));
				StartCoroutine (allModulePickerScripts [i].takeModuleInfo ("utility",  i));
				utilityCount -= 1;
//				break;
			} 
			else if (bothCount > 0) {
				StartCoroutine (allModulePickerScripts [i].ManualStart (gameObject.GetComponent<BodyPartPickerPanel>()));
				StartCoroutine (allModulePickerScripts [i].takeModuleInfo ("both",  i));
				bothCount -= 1;
//				break;
			}

		}
		BPartGenericScript[] choppingBlock =  gameObject.GetComponentsInChildren<BPartGenericScript>();
		if (bPartGeneric.getType () != "Leg") {
			foreach (BPartGenericScript bpart in choppingBlock) {	//destroys all the child bparts if the panel is not for legs
				bpart.destroyCompletely ();
			}
			bPartGeneric.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			StartCoroutine( pickerPlayArea.refreshSquares (bPartGeneric));		//sends the bodypart data to the preview square to populate the visual
		}
//		print(bPartGeneric.getType());
		if (bPartGeneric.getType() == "Leg"){	//holds off destroying all the child objects and sending the newly created leg till now, destroying the old ones before attatching the new one and creating another
			foreach (BPartGenericScript bpart in choppingBlock) {
				bpart.destroyCompletely ();
			}
			bPartGeneric.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			StartCoroutine( pickerPlayArea.refreshSquares (bPartGeneric));		//sends the bodypart data to the preview square to populate the visual

			bPartGeneric = partSelectionCanvas.markSelectedBodyPart(allBodyPartsOfThisType[incomingPickerListNumber], 2);		//sends the bodyholder info to the canvas to create and designate the part
			bPartGeneric.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
			StartCoroutine( pickerPlayArea.refreshSquares (bPartGeneric));		//sends the bodypart data to the preview square to populate the visual
		}
//		print ("after "+allModulePickerScripts.Length);
		yield return null;
	}
	public IEnumerator partDeselected(int incomingSelectionIDnum){
//		print ("incoming selection for unselecting" + incomingSelectionIDnum);
		partSelectionCanvas.markSelectedBodyPartAsNull(incomingSelectionIDnum, sideDirection);
		pickerListNumber = -1;
		StartCoroutine( pickerPlayArea.clearPlayAreaSquares ());
		//StartCoroutine( pickerPlayArea.clearSquares());
		//********************need to replace to make a substitute body part holder
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
//	public IEnumerator upwardsModuleSelected(int incomingModuleIDnumber, int incomingModuleSocketCountInBP){		//coming from
//		//alreadySelectedModules.Add (incomingModuleIDnumber);
//		//foreach (BodyPartPreviewWindowScript BPartWindow in allBPartWindows) {		//the loop for setting all of the already active module picker's  buttons to turn off
//		StartCoroutine( partSelectionCanvas.upwardsModuleSelected (incomingModuleIDnumber, nameOfPartPanel, incomingModuleSocketCountInBP));
//		//}
//		yield return null;
//	}
//	public IEnumerator upwardsModuleDeselected(int incomingModuleIDnumber, int incomingModuleSocketCountInBP){
//		StartCoroutine(partSelectionCanvas.upwardsModuleDeselected (incomingModuleIDnumber, nameOfPartPanel, incomingModuleSocketCountInBP));
//		yield return null;
//	}
//	public IEnumerator downwardsModuleSelected(int incomingModuleIDnumber){
//		StartCoroutine( pickerPlayArea.downwardsModuleSelected (incomingModuleIDnumber));
//		yield return null;
//	}
//	public IEnumerator downwardsModuleDeselected(int incomingModuleIDnumber){
//		StartCoroutine( pickerPlayArea.downwardsModuleDeselected (incomingModuleIDnumber));
//		yield return null;
//	}
	public IEnumerator upwardsModuleSelected(int incomingSelectedModuleIDnumber, int incomingModuleSocketCountInBP){		//signal coming from the module picker that a certain module was chosen, sending it up to the canvas
		//		print("incomingSelectedModuleIDnumber"+incomingSelectedModuleIDnumber);
//		int tempChosenNumber = allModulePickerScripts [incomingModuleSocketCountInBP].getCurrentModuleSelectedIDnumber ();
		StartCoroutine(partSelectionCanvas.upwardsModuleSelected (incomingSelectedModuleIDnumber, nameOfPartPanel, incomingModuleSocketCountInBP));
		yield return null;
	}
	public IEnumerator upwardsModuleDeselected(int incomingSelectedModuleIDnumber, int incomingModuleSocketCountInBP){		//signal coming from the module picker that a certain module was chosen, sending it up to the canvas
		//		print("incomingSelectedModuleIDnumber"+incomingSelectedModuleIDnumber);
//		int tempChosenNumber = allModulePickerScripts [incomingModuleSocketCountInBP].getCurrentModuleSelectedIDnumber ();
		StartCoroutine(partSelectionCanvas.upwardsModuleDeselected (incomingSelectedModuleIDnumber, nameOfPartPanel, incomingModuleSocketCountInBP));
		yield return null;
	}
//	public IEnumerator upwardsOLDModuleDeselected(int incomingModuleIDnumber, int incomingModuleSocketCountInBP){		//signal coming from the module picker that a certain module was chosen, sending it up to the canvas
//		StartCoroutine( partSelectionCanvas.upwardsModuleDeselected (incomingModuleIDnumber, nameOfPartPanel, incomingModuleSocketCountInBP));
//		yield return null;
//	}

	public IEnumerator downwardsModuleSelected(int incomingModuleIDnumber){		//signal coming from above from the canvas script that a module was chosen, sending it down to the module picker
		foreach(ModulePickerScript modulePicker in allModulePickerScripts){
			if (modulePicker != null) {
				StartCoroutine(modulePicker.downwardsModuleSelected (incomingModuleIDnumber));
			}
		}
		yield return null;
	}
	public IEnumerator downwardsModuleDeselected(int incomingModuleIDnumber){		//signal coming from above from the canvas script that a module was now freed up to be chosen, sending it down to the module picker
		foreach (ModulePickerScript modulePicker in allModulePickerScripts) {
			if (modulePicker != null) {
//				print ("body part picker downward");
				StartCoroutine(modulePicker.downwardsModuleDeselected (incomingModuleIDnumber));
			}
		}
		yield return null;
	}

}
