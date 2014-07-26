using System.Collections.Generic;
using UnityEngine;

/**
 * A class with unique instances which automatically contains all information that is possible to gather. The amount of information that is actually gathered is found in evidenceCredibility 
 * Evidence which may be continously measured and re-witnessed (though it may be hidden).
 * It is also the type a person can witness, while the Subjective class is only relayed?
 * The subtypes are divided into the categories: tool/weapon, prints/locationBased, identity confirmation, alien artifacts.
 * 
 * Analysis method depends on sub-type, may require different CS kits, and knowledge of their use. Person() will have appropriate methods for each subtype of ObjectiveEvidence.
 */
public class ObjectiveEvidence : Evidence {

	protected Info attachedToThis;
	protected bool detectionRequiresCSkit; //requires CS kit to detect, with the corresponding child class containing an appropriate method.
	protected bool analysisRequiresCSkit;
	protected int detectionChance; //likelyhood of being detected, if 
		
	/* * * * * LEADS * * * *
	 * An unidentified lead(unproven connection) may already contain all the information an identified (proven) lead has. 
	 * It usually does, however, since leadIdentified == false, the person can not access the Info pointer, but only compare suspected items until they match the stored info.
	 * After which the lead becomes identified and an identified lead affects event credibilty (and then opinions on Person()s.
	 */
	protected Info leadsToItem; 
	protected Evidence leadsToEvidence; //FIXME: is this needed for establishing the links?
	protected System.Type leadsToType;




	// * * * * CONSTRUCTORS * * * *

	
	/** When the leads can never be found
	 */
	public ObjectiveEvidence(int id,  Info attachedToThis_): base (id){ 
		attachedToThis = attachedToThis_;
		
	}

	/** When only the type of lead may be found
	 */
	public ObjectiveEvidence(int id,  Info attachedToThis_,System.Type leads_ToType): base (id){ 
		attachedToThis = attachedToThis_;
		leadsToType = leads_ToType;
	}
	/** When the entire lead information may be gathered
	 */
	public ObjectiveEvidence(int id, Info attachedToThis_, Info leadsTo): base (id){
		attachedToThis = attachedToThis_;
		leadsToItem = leadsTo;
		leadsToType = leadsTo.GetType();
	}

	// * * * *  Properties * * * * *

	public bool DetectionRequiresCSkit{get{return detectionRequiresCSkit;}}
	public bool AnalysisRequiresCSkit{get{return analysisRequiresCSkit;}}
	public Info LeadsToItem{get{return leadsToItem;}}
	public System.Type LeadsToType{get{return leadsToType;}}

}