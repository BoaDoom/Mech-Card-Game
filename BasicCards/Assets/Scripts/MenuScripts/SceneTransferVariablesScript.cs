using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransferVariablesScript : MonoBehaviour {
	AllPickedBodyParts allPickedBodyParts;
	void Start(){
		allPickedBodyParts = new AllPickedBodyParts ();
		DontDestroyOnLoad (gameObject);
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
	public int pickedHead{ get; private set; }
	public int pickedArm{ get; private  set; }
	public int pickedTorso{ get; private  set; }
	public int pickedShoulder{ get; private  set; }
	public int pickedLeg{ get; private  set; }
	public void setAllPickedBodyParts(int head, int arm, int torso, int shoulder, int leg){
		pickedHead = head;
		pickedArm = arm;
		pickedTorso = torso;
		pickedShoulder = shoulder;
		pickedLeg = leg;
	}
}

