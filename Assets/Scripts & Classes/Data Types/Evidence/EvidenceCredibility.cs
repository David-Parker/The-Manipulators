using UnityEngine;

/** Used as a List of variables in SocialEvent to determine if the Person believes the event has occured, and what the Person knows/believes about the event.
 */
public class EvidenceCredibility {

	Evidence whichEvidence;
	int impactOnEventCred; //Main value
		
	int believability;// The degree to which the person believes the evidence exists, not its accuracy. Major influence on impactOnEventCred. 

	bool isObjective;
	bool witnessedDirectly; //a WitnessedSocEvent may have this as false, since the person witnessing will relay that Evidence to someone else.
	bool refuted = false; //when a convincing argument against this evidence has been made
	bool fabricated = false; //FIXME implementation
	Person relayedBy; // when !witnessedDirectly

	//* * * * LEADS * * * * FIXME: write lead comparison methods
	Person personResponsible; 
	Info knownObjectLead;
	System.Type knownTypeLead;
	bool leadIdentified = false;

	/** get Default credibility Impact from evidence * Evidence believability.
	 * believability maxed if witnessed.
	 * believability is at minimum if refuted==true.
	 */
	public int calculateImpactOnEventCredibility(){ //FIXME : write me
		return 0;
	}


	/** The witnessed directly constructor for objective evidence
	 */
	public EvidenceCredibility(ObjectiveEvidence evidenceInstance,int impactOnEventCred_, Person evidenceOrigin_){
		whichEvidence = evidenceInstance;
		impactOnEventCred = impactOnEventCred_;
		believability = Settings.MaxBelievability;
		// adding all available data
		knownObjectLead = evidenceInstance.LeadsToItem;
		knownTypeLead = evidenceInstance.LeadsToType;
		if(knownObjectLead.GetType() == typeof(Person)){
			personResponsible = knownObjectLead as Person;
		}
		leadIdentified = true;
		isObjective = true;
		witnessedDirectly = true;
	}
	
	/** The witnessed directly constructor for subjective evidence
	 */
	public EvidenceCredibility(SubjectiveEvidence evidenceInstance,int impactOnEventCred_, Person evidenceOrigin_){
		whichEvidence = evidenceInstance;
		impactOnEventCred = impactOnEventCred_;
		believability = Settings.MaxBelievability;

		isObjective = false;
		witnessedDirectly = true;
	}

	/** The relayed evidence constructor for objective evidence
	 */
	public EvidenceCredibility(ObjectiveEvidence evidenceInstance,int impactOnEventCred_ ,int believability_, Person evidenceOrigin_, Person relayedBy_){
		whichEvidence = evidenceInstance;
		impactOnEventCred = impactOnEventCred_;
		believability = believability_;
		relayedBy = relayedBy_;

		isObjective = true;
		witnessedDirectly = false;
	}
	
	/** The relayed evidence constructor for subjective evidence
	 */
	public EvidenceCredibility(SubjectiveEvidence evidenceInstance,int impactOnEventCred_ ,int believability_, Person evidenceOrigin_, Person relayedBy_){
		whichEvidence = evidenceInstance;
		impactOnEventCred = impactOnEventCred_;
		believability = believability_;
		relayedBy = relayedBy_;

		isObjective = false;
		witnessedDirectly = false;
	}

	/** INCOMPLETE: the adjustment of believability if evidence is subjective has to be written
	 */
	public void changeToWitnessedDirectly(){
		witnessedDirectly = true;
		relayedBy = null;
		refuted = false;
		believability = Settings.MaxBelievability;
	}

	void calculateBelievabilty(Person whosCalculating){
		if(witnessedDirectly){
			believability = Settings.MaxBelievability;
		}else{

		}
	}

	/** Interacts with many members of the class and outputs to impactOnEventCred.
	 * Calculates believability before running.
	 */
	public void calculateImpactCred(Person whosCalculating){
		calculateBelievabilty(whosCalculating);
		if(isObjective){
			
		}else{

		}
	}

	// * * * * * Properties * * * * * 
	public Evidence TheEvidence {get{return whichEvidence;}}
	public int Impact {get{return impactOnEventCred;}}
	

}
