using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePickerScript : MonoBehaviour {

	public modulePickerButtonScript buttonPrefab;
	public BodyPartSelectionCanvasScript partSelectionCanvas;
	private ModuleSocketCount moduleSocketCount;
	XMLModuleData[] weaponModules;
	XMLModuleData[] utilityModules;
	XMLModuleData[] genericModules;

	public void takePartSelectionCanvas(BodyPartSelectionCanvasScript incomingPartSelectionCanvas, VisualOnlyBPartGenericScript incomingBPartGeneric){
		partSelectionCanvas = incomingPartSelectionCanvas;
		moduleSocketCount = incomingBPartGeneric.getModuleSocketCount();

		if (moduleSocketCount.weaponModuleSocketCount > 0) {
			weaponModules = partSelectionCanvas.getListOfModules ("Weapons");
			foreach (XMLModuleData modularData in weaponModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
//				print("modulardata " + modularData.moduleType);
			}
		}
		if (moduleSocketCount.utilityModuleSocketCount > 0) {
			utilityModules = partSelectionCanvas.getListOfModules ("Utility");
			foreach (XMLModuleData modularData in utilityModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				//				print("modulardata " + modularData.moduleType);
			}
		}
		if (moduleSocketCount.genericModuleSocketCount > 0) {
			genericModules = partSelectionCanvas.getListOfModules ("Both");
			foreach (XMLModuleData modularData in genericModules) {
				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> (), false);
				//				print("modulardata " + modularData.moduleType);
			}
		}
			

//		modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		modulePickerButtonScript newButton2 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//		newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
//		newButton2.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
		
	}
	// Update is called once per frame
	void Update () {
		
	}
}
