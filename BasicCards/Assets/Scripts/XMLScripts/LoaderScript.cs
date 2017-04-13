using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic; //Needed for Lists
using System.Xml; //Needed for XML functionality
using System.Xml.Serialization; //Needed for XML Functionality
using System.IO;
using System.Xml.Linq; //Needed for XDocument

public class LoaderScript : MonoBehaviour {

	//bool finishedLoading = false;
	public XMLCardLoaderScript XMLCardLoader;
	public XMLWeaponHitLoaderScript XMLWeaponHitLoader;
	public BPartXMLReaderScript XMLBPartLoader;


	void Start ()
	{
//		print ("loading script started");
		DontDestroyOnLoad (gameObject); //Allows Loader to carry over into new scene 
		//LoadXML (); //Loads XML File. Code below. 
		//StartCoroutine(AssignData()); //Starts assigning XML data to data List. Code below
		XMLCardLoader = gameObject.GetComponent<XMLCardLoaderScript> ();
		if (XMLCardLoader == null) {
			Debug.Log ("Cannot find 'XMLCardLoaderTemp'object");
		}
		XMLWeaponHitLoader = gameObject.GetComponent<XMLWeaponHitLoaderScript> ();
		if (XMLWeaponHitLoader == null) {
			Debug.Log ("Cannot find 'XMLWeaponHitLoader'object");
		}
		XMLBPartLoader = gameObject.GetComponent<BPartXMLReaderScript> ();
		if (XMLBPartLoader == null) {
			Debug.Log ("Cannot find 'XMLBPartLoader'object");
		}
		StartCoroutine(Waiter());

	}
	IEnumerator Waiter(){
		while (!XMLCardLoader.checkIfFinishedLoading() || !XMLWeaponHitLoader.checkIfFinishedLoading() || !XMLBPartLoader.checkIfFinishedLoading())
		{
			//print ("First " +!XMLCardLoader.checkIfFinishedLoading() +" "+ !XMLWeaponHitLoader.checkIfFinishedLoading() +" "+ !XMLBPartLoader.checkIfFinishedLoading());
			//print ("inside while loop");
			//finishedLoading = false;
			yield return new WaitForEndOfFrame();
		}
		//print (XMLCardLoader.checkIfFinishedLoading() +" "+ XMLWeaponHitLoader.checkIfFinishedLoading() +" "+ XMLBPartLoader.checkIfFinishedLoading());
		//print ("outside while loop");
		SceneManager.LoadScene("_Main"); //Only happens if coroutine is finished 
		//yield return WaitForEndOfFrame();
		//yield return new WaitForSeconds(1);
//		print(Time.time);

	}
	void Update ()
	{
		//print ("checking");

	}
}