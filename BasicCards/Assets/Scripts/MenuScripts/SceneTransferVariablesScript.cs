using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransferVariablesScript : MonoBehaviour {
	AllPickedBodyParts allPickedBodyParts;
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
	public AllPickedBodyParts getAllPartsPicked(){
		return allPickedBodyParts;
	}
	public void bleh(){
		print ("bleh");
	}
}
public class AllPickedBodyParts{
	public string pickedHead{ get; private set; }
	public string pickedArm{ get; private  set; }
	public string pickedTorso{ get; private  set; }
	public string pickedShoulder{ get; private  set; }
	public string pickedLeg{ get; private  set; }
	public void setAllPickedBodyParts(string head, string arm, string torso, string shoulder, string leg){
		pickedHead = head;
		pickedArm = arm;
		pickedTorso = torso;
		pickedShoulder = shoulder;
		pickedLeg = leg;
	}
}

