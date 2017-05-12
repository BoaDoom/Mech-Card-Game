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
	public void Start(){
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
	}

	public IEnumerator refreshSquares (VisualOnlyBPartGenericScript incomingVisualOfBpart) {
		StartCoroutine (clearSquares ());
		Vector2 incomingGridDimensions = incomingVisualOfBpart.getDimensionsOfPart ();
		Vector2 offSetPoint = new Vector2 (Mathf.Ceil((staticNumberOfBoxesX/2)-(incomingGridDimensions.x)/2), Mathf.Ceil((staticNumberOfBoxesY/2)-(incomingGridDimensions.y)/2));
//		print (offSetPoint);
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
		for(int x = 0; x < staticNumberOfBoxesX; x++){
			for(int y = 0; y <staticNumberOfBoxesX; y++){
				grid [x] [y].DeactivateSquare ();		//sets the preview windows square as occupied if the above is true
			}
		}
		yield return null;
	}
}