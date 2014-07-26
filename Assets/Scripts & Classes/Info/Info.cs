using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** The general class for all in-game items
 */
public class Info : MonoBehaviour {


	public string itemName; //FIXME remove these and create special functions for children classes
	public string itemDescription;

	protected List<Evidence> embeddedEvidence = new List<Evidence>();


	// * * * * * get/set * * * * * *
	public List<Evidence> AllEmbeddedEvidence {get{return embeddedEvidence;}}

	public virtual string ItemName {
		get{ return itemName;}
		set{ itemName = value;}
	}
	
	public virtual string ItemDescription {
		get{ return itemDescription;}
		set{ itemDescription = value;}
	}

}
