using System.Collections.Generic;

//FIXME: dividing the body into parts

/**For both murders and natural deaths.
 */
public class DeadBody : ObjectiveEvidence {

	bool murdered;
	Evidence method;

	/** When there can't be any information on leads, it's automatically considered a natural death.
	 */
	public DeadBody(int id, Info attachedToThis ) :base(id, attachedToThis){
		base.detectionRequiresCSkit = false;
		base.analysisRequiresCSkit = true;
		base.defaultCredibilityImpact = Settings.MaxEvidenceCred;
		murdered = false;
	}

	/** When only the type of lead can be known
	 */
	public DeadBody(int id, Info attachedToThis, System.Type leadsToType ) :base(id ,attachedToThis, leadsToType){
		base.detectionRequiresCSkit = false;
		base.analysisRequiresCSkit = true;
		base.defaultCredibilityImpact = Settings.MaxEvidenceCred;
		murdered = true;
	}

	/** When the most information about the lead is available
	 */
	public DeadBody(int id, Info attachedToThis, Info responsible) :base(id, attachedToThis, responsible){
		base.detectionRequiresCSkit = false;	
		base.analysisRequiresCSkit = true;
		base.defaultCredibilityImpact = Settings.MaxEvidenceCred;
		murdered = true;
		//FIXME: when the method is obvious (weapon found nearby analysis will not require a cs kit. Perhaps gather the lead from the tool?
	}

	public bool Murdered {get{return murdered;}}
}
