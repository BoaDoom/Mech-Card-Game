using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument
using System.Linq;

public class BPartXMLReaderScript : MonoBehaviour {
	bool finishedLoading = false;

	public BPartGenericScript bPartGenericPrefab;


	XDocument xmlDoc; //create Xdocument. Will be used later to read XML file 
	IEnumerable<XElement> typeOfParts; //Create an Ienumerable list. Will be used to store XML Items. 
	IEnumerable<XElement> listOfParts;
	IEnumerable<XElement> storedAnchorPoints;
	IEnumerable<XElement> storedComplexAnchorPoints;
	public List <BodyPartDataHolder> BPartData = new List <BodyPartDataHolder>(); //Initialize List of XMLWeaponData objects.
	public List <ComplexAnchorPoints> listOfComplexAnchorPoints;
//	private BPartGenericScript LeftViewerArm;
//	private BPartGenericScript RightViewerArm;
//	private BPartGenericScript Arms;
//	private BPartGenericScript Arms;
//	private BPartGenericScript Arms;

	private string BpartName = "none";
	private string BpartType = "none";
	IEnumerable<XElement> gridHitBox;
	IEnumerable<XElement> AnchorPoints;
	private int MaxHealth = 0;
	private int[][] gridOfBodyPart;
	private Vector2 anchorVector2;
	private bool complexAnchorPointsBoolCheck;

	void Start ()
	{
		//DontDestroyOnLoad (gameObject); //Allows Loader to carry over into new scene 
		LoadXML (); //Loads XML File. Code below. 
		StartCoroutine(AssignData()); //Starts assigning XML data to data List. Code below
		//		Debug.Log("inside bodydata count "+BPartData.Count);
	}

	void LoadXML()

	{
		//Assigning Xdocument xmlDoc. Loads the xml file from the file path listed. 
		xmlDoc = XDocument.Load("assets/XMLdata/Bparts.xml");

		//This basically breaks down the XML Document into XML Elements. Used later. 
		typeOfParts = xmlDoc.Descendants("BPartsList").Elements ();
	}

	//this is our coroutine that will actually read and assign the XML data to our List 
	IEnumerator AssignData()
	{
//		int t = 0;
		/*foreach allows us to look at every Element of our XML file and do something with each one. Basically, this line is saying “for each element in the xml document, do something.*/ 
		foreach (var partType in typeOfParts)
		{
			listOfParts = partType.Elements ();
			foreach (var part in listOfParts) {
				if (BpartName != part.Attribute ("name").Value.Trim ()) {		//if the next element has a new name, select that parrent and assign all it's children to these values
					BpartName = part.Attribute ("name").Value.Trim ();	
					BpartType = part.Parent.Name.ToString();
					MaxHealth = int.Parse (part.Element ("Health").Value.Trim ());

					int numberXCord = part.Element ("gridHitBox").Element ("line").Value.Trim ().Length;	//the length of the design shape line of 1's and 0's
					gridHitBox = part.Element ("gridHitBox").Descendants ();
					int numberYCord = part.Element ("gridHitBox").Descendants ().Count ();		//counts how many lines there are in the targeting grid, giving Y cords size
					int interationY = numberYCord - 1;
					string complexBool = part.Element ("ComplexAnchorPoints").Value.Trim ();
					//Debug.Log (complexBool);

					if (complexBool == "true") {
						complexAnchorPointsBoolCheck = true;
					} else {
						complexAnchorPointsBoolCheck = false;
					}

					gridOfBodyPart = new int[(int)numberXCord][];
					for (int i = 0; i < numberXCord; i++) {
						gridOfBodyPart [i] = new int[(int)numberYCord];		//instantiating the grid beforehand
					}
					foreach (XElement line in gridHitBox) {
						int interationX = 0;
						string lineOfNumbers = line.Value;
						foreach (char num in lineOfNumbers) {
							int newNum = (int)char.GetNumericValue (num);
							gridOfBodyPart [interationX] [interationY] = newNum;
							interationX++;
						}
						interationY--;
					}
					
					if (complexAnchorPointsBoolCheck) {
						listOfComplexAnchorPoints = new List <ComplexAnchorPoints>();
						ComplexAnchorPoints uniqueAnchorPoints;
						bool sexOfSocket = false;
						storedComplexAnchorPoints = part.Element ("AnchorPoints").Elements ();
						foreach (XElement point in storedComplexAnchorPoints) {
							//Debug.Log ("complex point: "+point.Name);
							storedAnchorPoints = point.Elements();
							foreach (XElement cord in storedAnchorPoints) {
								//Debug.Log ("cord: "+cord.Name);
								if (cord.Name == "xCord") {
									anchorVector2.x = int.Parse (cord.Value);
								}
								if (cord.Name == "yCord") {
									anchorVector2.y = int.Parse (cord.Value);
								}
							}
							if (point.Attribute ("typeOfSocket").Value.Trim () == "male") {
								sexOfSocket = true;
							} else {
								sexOfSocket = false;
							}
							uniqueAnchorPoints = new ComplexAnchorPoints (point.Name.ToString(), anchorVector2 , sexOfSocket);

							listOfComplexAnchorPoints.Add (uniqueAnchorPoints);
						}
						BPartData.Add (new BodyPartDataHolder (BpartName, BpartType, MaxHealth, gridOfBodyPart, listOfComplexAnchorPoints));
						//anchorVector2 = new Vector2 (0.0f, 0.0f);		//placeholder
					} else {
						//Debug.Log ((string)partType.Name.ToString());
						storedAnchorPoints = part.Element ("AnchorPoint").Descendants ();
						foreach (XElement cord in storedAnchorPoints) {
							if (cord.Name == "xCord") {
								anchorVector2.x = int.Parse (cord.Value);
							}
							if (cord.Name == "yCord") {
								anchorVector2.y = int.Parse (cord.Value);
							}
						}
						BPartData.Add (new BodyPartDataHolder (BpartName, BpartType, MaxHealth, gridOfBodyPart, anchorVector2));
					}
				}
			}
		}
		finishedLoading = true;
		yield return null;
	}
	public bool checkIfFinishedLoading(){
		return finishedLoading;
	}

	public BodyPartDataHolder getBodyData(string requestedNameOfPart){			//future efficiency, have each part be catagorized acording to their part type for better searching
		//used by BodyPartMakerScript when asked by enemyscript to makebodypart()
		//Debug.Log("name: "+ requestedNameOfPart); 
		//Debug.Log("requested found "+BPartData.Find (BodyPartDataHolder => BodyPartDataHolder.name == requestedNameOfPart).name);
		return BPartData.Find (BodyPartDataHolder => BodyPartDataHolder.name == requestedNameOfPart);
	}
}

public class BodyPartDataHolder{
	public string name;
	public string typeOfpart;
	public int maxHealth;
	public int[][] bodyPartGrid;
	public Vector2 anchor;
	public List<ComplexAnchorPoints> listOfComplexAnchorPoints;
	public bool simpleAnchorPoints;
	public BodyPartDataHolder(string BpartName, string incBpartName, int incMaxHealth, int[][] incomingBodyPartGrid, Vector2 AnchorPoint){
		simpleAnchorPoints = true;
		name = BpartName;
		typeOfpart = incBpartName;
		maxHealth = incMaxHealth;
		anchor = AnchorPoint;
		bodyPartGrid = new int[incomingBodyPartGrid.Length][];
		for(int i=0; i < incomingBodyPartGrid.Length; i++){	//transfering the int[][] grid
			bodyPartGrid [i] = new int[incomingBodyPartGrid[0].Length];
			for(int j=0; j < incomingBodyPartGrid[0].Length; j++){
				bodyPartGrid [i][j] = incomingBodyPartGrid[i][j];
			}
		}
	}
	public BodyPartDataHolder(string BpartName, string incBpartName, int incMaxHealth, int[][] incomingBodyPartGrid, List<ComplexAnchorPoints> incomingListOfComplexAnchorPoints){
		simpleAnchorPoints = false;
		name = BpartName;
		typeOfpart = incBpartName;
		maxHealth = incMaxHealth;
		listOfComplexAnchorPoints = incomingListOfComplexAnchorPoints;
		bodyPartGrid = new int[incomingBodyPartGrid.Length][];
		for(int i=0; i < incomingBodyPartGrid.Length; i++){	//transfering the int[][] grid
			bodyPartGrid [i] = new int[incomingBodyPartGrid[0].Length];
			for(int j=0; j < incomingBodyPartGrid[0].Length; j++){
				bodyPartGrid [i][j] = incomingBodyPartGrid[i][j];
			}
		}
		//Debug.Log ("Data holder point: "+BpartName);
	}
}
public class ComplexAnchorPoints{
	public string nameOfPoint;
	public Vector2 anchorPoint;
	//public Vector2 globalAnchorPointLocation;
	public bool male;
	public ComplexAnchorPoints(string incomingName, Vector2 incomingAnchorPoint, bool incomingType){
		nameOfPoint = incomingName;
		anchorPoint = incomingAnchorPoint;
		male = incomingType;
	}
//	public void setGlobalAnchorPointLocation(Vector2 incomingGlobalLocation){
//		globalAnchorPointLocation = incomingGlobalLocation;
//	}
}

