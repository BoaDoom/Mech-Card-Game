using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransferVariablesScript : MonoBehaviour {
	AllPickedBodyParts allPickedBodyParts;
	List<int> selectedModules;
	void Start(){
		allPickedBodyParts = new AllPickedBodyParts ();
		GameObject loaderScriptTemp = GameObject.FindWithTag("MainLoader");		//grabbing the object with the body part info taken from xml data	
		if (loaderScriptTemp != null) {
			DontDestroyOnLoad (gameObject);
		}
	}
	public void setPartsPicked(AllPickedBodyParts incomingParts){
//		print ("set parts in scene transfer");
		allPickedBodyParts = incomingParts;
	}
	public void setModulesPicked(List<int> incomingSelectedModules){
		selectedModules = incomingSelectedModules;
	}
	public AllPickedBodyParts getAllPartsPicked(){
		return allPickedBodyParts;
	}
	public List<int> getAllModules(){
		return selectedModules;
	}
	public void bleh(){
		print ("bleh");
	}
}
public class AllPickedBodyParts{
	public TransferBodyPartInfo pickedHead;
	public TransferBodyPartInfo pickedArm;
	public TransferBodyPartInfo pickedTorso;
	public TransferBodyPartInfo pickedShoulder;
	public TransferBodyPartInfo pickedLeg;
	public void setAllPickedBodyParts(TransferBodyPartInfo head, TransferBodyPartInfo arm, TransferBodyPartInfo torso, TransferBodyPartInfo shoulder, TransferBodyPartInfo leg){
		pickedHead = head;
		pickedArm = arm;
		pickedTorso = torso;
		pickedShoulder = shoulder;
		pickedLeg = leg;
	}
}
public class TransferBodyPartInfo{
	public string typeOfPart;
	public string nameOfPart;
	public int partIDnum;
	public int[] moduleIDnum = new int[3];
	public void setAllAtributesOfBPart(string incomingNameOfPart, int incomingPartIDnum, int[] incomingModuleIDnum){
		int tempInt = 0;
		foreach (int partID in moduleIDnum) {
			moduleIDnum [tempInt] = incomingModuleIDnum [tempInt];
			tempInt++;
		}
		partIDnum = incomingPartIDnum;
		nameOfPart = incomingNameOfPart;
	}
}

