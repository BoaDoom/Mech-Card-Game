﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class bodyPartPickerButtonScript : MonoBehaviour, ISelectHandler {
//	SelectionBaseAttribute selectorScript;
//	public void Start(){
//		selectorScript = gameObject.GetComponent<SelectionBaseAttribute>();
//	}

	public void setAsSelected(){
		//selectorScript.
	}
	public void OnSelect(BaseEventData eventData){
		print ("test onSelect");
	}
//	private SelectionBaseAttribute selectorScript;

//	public void chooseButton(int choice){
//
//		//gameObject.GetComponent<BodyPartSelectionCanvasScript>().markSelectedBodyPart()
//	}
//	public void onClick(){
//		print ("clicked");
//	}
}
