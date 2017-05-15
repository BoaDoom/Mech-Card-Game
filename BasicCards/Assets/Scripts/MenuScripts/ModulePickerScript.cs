using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModulePickerScript : MonoBehaviour {

	public modulePickerButtonScript buttonPrefab;
	public BodyPartSelectionCanvasScript partSelectionCanvas;
	private ModuleCount BPartData;
	XMLModuleData[] weaponModules;
	XMLModuleData[] utilityModules;
	XMLModuleData[] genericModules;

	public void takePartSelectionCanvas(BodyPartSelectionCanvasScript incomingPartSelectionCanvas, VisualOnlyBPartGenericScript incomingBPartGeneric){
		partSelectionCanvas = incomingPartSelectionCanvas;
		BPartData = incomingBPartGeneric.moduleSockets;

		if (BPartData.weaponModuleCount > 0) {
			weaponModules = partSelectionCanvas.getListOfModules ("Weapons");
			foreach (XMLModuleData modularData in weaponModules) {
//				modulePickerButtonScript newButton1 = Instantiate(buttonPrefab, gameObject.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().rotation);
//				newButton1.GetComponent<Transform> ().SetParent (gameObject.GetComponent<Transform> ());
				print("test");
			}
		}
		if (BPartData.utilityModuleCount > 0) {
			utilityModules = partSelectionCanvas.getListOfModules ("Utility");
		}
		if (BPartData.genericModuleCount > 0) {
			genericModules = partSelectionCanvas.getListOfModules ("Generic");
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
