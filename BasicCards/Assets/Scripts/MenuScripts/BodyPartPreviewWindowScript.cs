using System.Collections;
using System.Collections.Generic;
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using UnityEngine;

public class BodyPartPreviewWindowScript: MonoBehaviour {

	public List<XMLBodyHitData> bodyLoaderData;

	public VisualSquareScript smallSquare; //added manually inside unity from prefabs//?
	Transform transformOriginal;
//	BodyPartVariationPanel bodyPartPanel;

	Sprite emptySquare;
	Sprite occupiedSquare;

	int staticNumberOfBoxesX = 9;	//max size needed to fit all currently made parts
	int staticNumberOfBoxesY = 9;

	//public float sizeRatioOfSmallBox = 1.0f;

	private VisualSquareScript[][] grid;
	private Vector2 gridDimensions;
	//private TargetSquareState[][] gridOfStates;		//tracks the states of the squares in the targeting box. Boolean of Occupied, HardTargeted, SoftTargeted

	//Vector2 zeroCord = Vector2.zero;
	Vector2 framingBoxSize;
	public Vector3 firstBoxCord;
	bool completedStartup = false;

	public ModulePickerScript ModulePickerPanel;		//orginal prefab of panel for picking variable modules
	public ModuleSocketCount moduleSocketCount;		//stored count of number of module sockets
	public ModulePickerScript[] modulePanels; 		//the list of panels that are open for selection after the part has been picked. Can be 0-3 panels
	public int numberOfModularSocketsShown;

	BodyPartSelectionCanvasScript partSelectionCanvas;
	public void Start(){
		GameObject partSelectionCanvasTemp = GameObject.FindWithTag ("PartSelectionCanvas");
		if (partSelectionCanvasTemp != null) {
			partSelectionCanvas = partSelectionCanvasTemp.GetComponent<BodyPartSelectionCanvasScript> ();
		} else {
			print ("Couldnt find SceneTransferVariablesScript");
		}
		modulePanels = new ModulePickerScript[3];

		Transform BodyPartPanelTemp = gameObject.transform.parent;

//		if(BodyPartPanelTemp != null){
//			bodyPartPanel = BodyPartPanelTemp.GetComponent<BodyPartVariationPanel>();
//		}
		if(BodyPartPanelTemp == null){
			Debug.Log ("Cannot find 'BodyPartVariationPanel'object");}

		VisualSquareScript smallSquareInst;
		gridDimensions = new Vector2(staticNumberOfBoxesX, staticNumberOfBoxesY);
		transformOriginal = gameObject.transform;
		framingBoxSize = new Vector2(1.0f/staticNumberOfBoxesX, 1.0f/staticNumberOfBoxesY);	//ratio of small boxes relative to the full size of the framing box
		firstBoxCord = new Vector2 ((-0.5f + framingBoxSize.x / 2), (-0.5f + framingBoxSize.y / 2));

		grid = new VisualSquareScript[(int)gridDimensions.x][];		//grid of prefab ActiveSquare

		for (int x = 0; x < gridDimensions.x; x++){
			//gridOfStates [x] = new TargetSquareState[(int)gridDimensions.y];
			grid[x] = new VisualSquareScript[(int)gridDimensions.y];
			for (int y = 0; y < gridDimensions.y; y++)
			{
				smallSquareInst = Instantiate (smallSquare, Vector3.zero, transformOriginal.rotation);
				StartCoroutine( smallSquareInst.ManualStart (gameObject.GetComponent<PlayAreaScript>()));
				smallSquareInst.transform.SetParent (gameObject.transform);
				smallSquareInst.transform.localScale = framingBoxSize;
				Vector2 tempVector2;
				tempVector2 = new Vector2((framingBoxSize.x*x), (framingBoxSize.y*y));	//used to turn the vector2s into vector3s
				tempVector2 = new Vector2(firstBoxCord.x +tempVector2.x, firstBoxCord.y + tempVector2.y);

				smallSquareInst.transform.localPosition = new Vector3(tempVector2.x, tempVector2.y, 0.0f );
				smallSquareInst.SetGridCordX (x);
				smallSquareInst.SetGridCordY (y);
				smallSquareInst.GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>());
				grid[x][y] = smallSquareInst;
				//gridOfStates[x][y] = smallSquareInst.activeSquareState;

			}
		}
		completedStartup = true;
	}
	public bool getcompletedStartup(){
		return completedStartup;
	}

	public IEnumerator refreshSquares (VisualOnlyBPartGenericScript incomingVisualOfBpart) {

		StartCoroutine (clearSquares ());
		modulePanels = new ModulePickerScript[ incomingVisualOfBpart.getModuleSocketCount ().getTotalCount()];
		numberOfModularSocketsShown = incomingVisualOfBpart.getModuleSocketCount ().getTotalCount();		//grabbing the count of sockets
		Vector2 incomingGridDimensions = incomingVisualOfBpart.getDimensionsOfPart ();

		Vector2 offSetPoint = new Vector2 (Mathf.Ceil((staticNumberOfBoxesX/2)-(incomingGridDimensions.x)/2), Mathf.Ceil((staticNumberOfBoxesY/2)-(incomingGridDimensions.y)/2));
//		print (offSetPoint);
		float floatOffset = 1.25f;
		int totalCount = 0;
		for (int i = 0; i <incomingVisualOfBpart.getModuleSocketCount().getWeaponCount(); i++){
			modulePanels [totalCount] = Instantiate (ModulePickerPanel, Vector3.zero + new Vector3((1.25f + floatOffset*totalCount), 0.0f, 0.0f), transformOriginal.rotation);
			modulePanels [totalCount].takePartSelectionCanvas (partSelectionCanvas, "weapon");
			modulePanels[totalCount].GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>(), false);
			totalCount += 1;
//			print ("weapon made" + i);
//			print ("totalCount: "+ totalCount);
		}
		for (int i = 0; i <incomingVisualOfBpart.getModuleSocketCount().getUtilityCount(); i++){
			modulePanels [totalCount] = Instantiate (ModulePickerPanel, Vector3.zero + new Vector3((1.25f + floatOffset*totalCount), 0.0f, 0.0f), transformOriginal.rotation);
			modulePanels [totalCount].takePartSelectionCanvas (partSelectionCanvas, "utility");
			modulePanels[totalCount].GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>(), false);
			totalCount += 1;
//			print ("utility made" + i);
//			print ("totalCount: "+ totalCount);
		}
		for (int i = 0; i <incomingVisualOfBpart.getModuleSocketCount().getBothCount(); i++){
			modulePanels [totalCount] = Instantiate (ModulePickerPanel, Vector3.zero + new Vector3((1.25f + floatOffset*totalCount), 0.0f, 0.0f), transformOriginal.rotation);
			modulePanels [totalCount].takePartSelectionCanvas (partSelectionCanvas, "both");
			modulePanels[totalCount].GetComponent<Transform>().SetParent(gameObject.GetComponent<Transform>(), false);
			totalCount += 1;
//			print ("both made" + i);
//			print ("totalCount: "+ totalCount);
		}





//			put in the module choices here to send to module picker to list as choices


		for(int x = 0; x < incomingGridDimensions.x; x++){
			for(int y = 0; y <incomingGridDimensions.y; y++){
				//grid [x] [y].DeactivateSquare ();
				if (incomingVisualOfBpart.getGridPoint(new Vector2(x,y))){	//checks the dimensions of the incoming body part and sees if its occupied
					grid [x+ (int)offSetPoint.x] [y +(int)offSetPoint.y].OccupiedSquare ();		//sets the preview windows square as occupied if the above is true
				}
			}
		}
		yield return null;
	}
	public IEnumerator clearSquares(){
		while (!completedStartup) {
			//yield return null;
			print("Loop that doesn't do anythingg");
		}
		for(int x = 0; x < staticNumberOfBoxesX; x++){
			for(int y = 0; y <staticNumberOfBoxesX; y++){
				grid [x] [y].DeactivateSquare ();		//sets the preview windows square as occupied if the above is true
			}
		}
		if (modulePanels[0] != null) {
//			print("modulePanels.Length: "+modulePanels.Length);
			for (int i = 0; i < modulePanels.Length; i++) {
//				print("modulePanels deleted: "+i);
				//ModulePickerScript tempToDestroy = modulePanels [0].gameObject;
				if (modulePanels [i] != null) {
					DestroyObject (modulePanels [i].gameObject);
				}
				//print ("Destroy!");
			}
		}
		yield return null;
	}
}