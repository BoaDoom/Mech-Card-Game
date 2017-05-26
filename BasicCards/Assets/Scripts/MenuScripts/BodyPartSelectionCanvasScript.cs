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

	BodyPartPickerPanel[] listOfPickerPanels;
	BodyPartDataHolder partData = null;
	public VisualOnlyBPartGenericScript visualOnlyBodyPartObject;

	VisualOnlyBPartGenericScript tempBodyPart;

	BPartXMLReaderScript bPartXMLReader;
	XMLModuleLoaderScript XMLModuleLoader;
//	public int what;
	TransferBodyPartInfo headSelection = new TransferBodyPartInfo();
	TransferBodyPartInfo armSelection = new TransferBodyPartInfo();
	TransferBodyPartInfo torsoSelection = new TransferBodyPartInfo();
	TransferBodyPartInfo shoulderSelection = new TransferBodyPartInfo();
	TransferBodyPartInfo legSelection = new TransferBodyPartInfo();
//	TransferBodyPartInfo[] listOfPickedBodyParts = new TransferBodyPartInfo[5];

	public XMLModuleData[] listOfWeaponModules;
	public XMLModuleData[] listOfUtilityModules;

//	BodyPartPreviewWindowScript headWindow;
//	BodyPartPreviewWindowScript armWindow;
//	BodyPartPreviewWindowScript torsoWindow;
//	BodyPartPreviewWindowScript shoulderWindow;
//	BodyPartPreviewWindowScript legWindow;
//	BodyPartPreviewWindowScript[] allBPartWindows = new BodyPartPreviewWindowScript[5];		//5 is the number of type of parts, head, arm, torso, legs, shoulders;

	public List<int> alreadySelectedModules = new List<int>();


	public Button nextButton;
//	bool thingsChecked;
	public void Start(){

		//grab XML data for modules and store it here for the ModulePickerScript to request and grab later
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");		//grabbing the object with the body part info taken from xml data	
		if (loaderScriptTemp == null) {
			SceneManager.LoadScene ("XMLLoaderScene");
			return;
		} else if (loaderScriptTemp != null) {
			print ("Actual start");
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

			listOfPickerPanels = gameObject.GetComponentsInChildren<BodyPartPickerPanel> ();
//			print ("list of all the panels" +listOfPickerPanels.Length);
			foreach (BodyPartPickerPanel panel in listOfPickerPanels) {
				panel.ManualStart ();
			}

		}
			
			//		print ("done panel");
	}

//	public void checkThing(int incomingint, int otherthing){
//		thingsChecked = true;
//	}
	public VisualOnlyBPartGenericScript markSelectedBodyPart(int incomingIDofPart){		//almost the same as PlayerScript method populate body
//		print(incomingIDofPart);
		partData = bPartXMLReader.getBodyDataByID (incomingIDofPart);
//		print(partData.name);
		string tempType = partData.typeOfpart;
		//need to swap over the identifying variable from a string to the new class so it can carry the module info and choices. Maybe? needs to convey more info at some point
//		print("incoming selection"+ incomingSelection);
		switch (tempType) {
		case("Head"):
			headSelection.nameOfPart = partData.name; 
			break;
		case("Arm"):
			armSelection.nameOfPart = partData.name;
			break;
		case("Torso"):
			torsoSelection.nameOfPart = partData.name;
			break;
		case("Shoulder"):
			shoulderSelection.nameOfPart = partData.name;
			break;
		case("Leg"):
			legSelection.nameOfPart = partData.name;
			break;
		default:
			Debug.Log ("Unknown bodypart");
			break;
		}
		tempBodyPart.CreateNewPart (partData);
		return tempBodyPart;	
	}
	public void markSelectedBodyPartAsNull(int incomingIDofPart){		//for deselecting the body part
		partData = bPartXMLReader.getBodyDataByID (incomingIDofPart);
		string tempType = partData.typeOfpart;
		switch (tempType) {
		case("Head"):
			headSelection = new TransferBodyPartInfo();
			break;
		case("Arm"):
			armSelection =  new TransferBodyPartInfo();
			break;
		case("Torso"):
			torsoSelection =  new TransferBodyPartInfo();
			break;
		case("Shoulder"):
			shoulderSelection =  new TransferBodyPartInfo();
			break;
		case("Leg"):
			legSelection =  new TransferBodyPartInfo();
			break;
		default:
			Debug.Log ("Unknown bodypart");
			break;
		}
	}
	public bool checkIfBodyIsComplete(){
		return (headSelection.nameOfPart != null && armSelection.nameOfPart != null && torsoSelection.nameOfPart != null && shoulderSelection.nameOfPart != null && legSelection.nameOfPart != null
		&& true);
	}
	public void onClick(){
		print ("clicked");
	}
	public void checkToMoveToPlayScreen(){
		if (checkIfBodyIsComplete() && (alreadySelectedModules.Count > 0)) {
			AllPickedBodyParts allPickedBodyPartsTemp = new AllPickedBodyParts ();
			allPickedBodyPartsTemp.setAllPickedBodyParts(headSelection, armSelection, torsoSelection, shoulderSelection, legSelection);
//			print (allPickedBodyPartsTemp.pickedHead);
//			sceneTransferVariablesScript.bleh ();
//			sceneTransferVariablesScript.setModulesPicked(alreadySelectedModules);
			sceneTransferVariablesScript.setPartsPicked(allPickedBodyPartsTemp);
			SceneManager.LoadScene ("_Main");
		} else {
			Debug.Log ("You are missing some body parts or no modules are selected");
		}
	}


//	public VisualOnlyBPartGenericScript markSelectedBodyPart(int incomingIDofPart){	//takes the clicked on part name, and finds it in the xmldata and creates a visual only body part to display
//		partData = bPartXMLReader.getBodyDataByID (incomingIDofPart);
//
//		//VisualOnlyBPartGenericScript tempBodyPart = Instantiate (visualOnlyBodyPartObject, Vector3.zero, gameObject.GetComponent<Transform>().rotation);
////		print("make body part partData: "+ nameOfpart);
//		tempBodyPart.CreateNewPart (partData);		// 
//		return tempBodyPart;
//	}
//	public string intToStringNumber(int incomingNumber){
//		switch (incomingNumber) {
//		case(1):
//			return "one";
//		case(2):
//			return "two";
//		case(3):
//			return "three";
//		case(4):
//			return "four";
//		}
//		return null;
//	}
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
		print ("something went wrong with gettign the list of modules");
		return null;
	}
	/////////////////////////////////////////////////////////////// 05-25-17
//	public void previewWindowTransfer(BodyPartPreviewWindowScript incomingBPartWindow){		//on startup each bpartpreviewwindow sends a reference to itself to the canvas
//		string tempNameString = incomingBPartWindow.getTypeOfBPartOnDisplay();
//		switch (tempNameString) {
//		case("Head"):
//			headWindow = incomingBPartWindow;
//			allBPartWindows [0] = headWindow;
//			break;
//		case("Arm"):
//			armWindow = incomingBPartWindow;
//			allBPartWindows [1] = armWindow;
//			break;
//		case("Torso"):
//			torsoWindow = incomingBPartWindow;
//			allBPartWindows [2] = torsoWindow;
//			break;
//		case("Shoulder"):
//			shoulderWindow = incomingBPartWindow;
//			allBPartWindows [3] = shoulderWindow;
//			break;
//		case("Leg"):
//			legWindow = incomingBPartWindow;
//			allBPartWindows [4] = legWindow;
//			break;
//		default:
//			Debug.Log ("preview window did not transfer correctly");
//			break;
//		}
//	}
	public IEnumerator upwardsModuleSelected(int incomingModuleIDnumber, string incomingModuleBPartName, int incomingModuleSocketLabel){		//coming from
		alreadySelectedModules.Add (incomingModuleIDnumber);
		switch (incomingModuleBPartName) {
		case("Head"):
			headSelection.moduleIDnum[incomingModuleSocketLabel] = incomingModuleIDnumber;
			break;
		case("Arm"):
			armSelection.moduleIDnum[incomingModuleSocketLabel] = incomingModuleIDnumber;
			break;
		case("Torso"):
			torsoSelection.moduleIDnum[incomingModuleSocketLabel] = incomingModuleIDnumber;
			break;
		case("Shoulder"):
			shoulderSelection.moduleIDnum[incomingModuleSocketLabel] = incomingModuleIDnumber;
			break;
		case("Leg"):
			legSelection.moduleIDnum[incomingModuleSocketLabel] = incomingModuleIDnumber;
			break;
		default:
			Debug.Log ("Unknown bodypart");
			break;
		}
		foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
			StartCoroutine( BPartPicker.downwardsModuleSelected (incomingModuleIDnumber));
		}
		yield return null;
	}
	public IEnumerator upwardsModuleDeselected(int incomingModuleIDnumber, string incomingModuleBPartName, int incomingModuleSocketLabel){
		switch (incomingModuleBPartName) {
		case("Head"):
			headSelection.moduleIDnum[incomingModuleSocketLabel] = -1;
			break;
		case("Arm"):
			armSelection.moduleIDnum[incomingModuleSocketLabel] = -1;
			break;
		case("Torso"):
			torsoSelection.moduleIDnum[incomingModuleSocketLabel] = -1;
			break;
		case("Shoulder"):
			shoulderSelection.moduleIDnum[incomingModuleSocketLabel] = -1;
			break;
		case("Leg"):
			legSelection.moduleIDnum[incomingModuleSocketLabel] = -1;
			break;
		default:
			Debug.Log ("Unknown bodypart");
			break;
		}

		int tempCount = alreadySelectedModules.Count;
		for (int i = 0; i < tempCount; i++) {
			if (alreadySelectedModules[i] == incomingModuleIDnumber) {
				alreadySelectedModules.Remove(incomingModuleIDnumber);
				i--;
				tempCount--;
			}
		}

		foreach (BodyPartPickerPanel BPartPicker in listOfPickerPanels) {		//the loop for setting all of the already active module picker's  buttons to turn off
			StartCoroutine(BPartPicker.downwardsModuleDeselected (incomingModuleIDnumber));
//			print("trying to deselect");
		}
		yield return null;
//		print ("outside?");
	}
	public List<int> getModulesAlreadyInUse(){		//used for any new module pickers buttons to check to see if their module is turned off
		return alreadySelectedModules;
	}
	public List<BodyPartDataHolder> getAllBodyDataForType(string BpartType){
		return bPartXMLReader.getAllBodyDataForType (BpartType);
	}
}
//public class TransferBodyPartInfo{
//	public string nameOfPart{ get; set; }
//	int[] listOfSelectedModules;
//	public TransferBodyPartInfo(){
//	}
//}

