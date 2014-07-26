public class FingerPrints : ObjectiveEvidence {

	/** When there can't be any information on leads
	 */
	public FingerPrints(int id, Info attachedToThis): base (id, attachedToThis){
		base.detectionRequiresCSkit = true;
		base.analysisRequiresCSkit = true;
	}

	/** When only the type of lead can be known
	 */
	public FingerPrints(int id, Info attachedToThis, System.Type leadsToType): base (id, attachedToThis, leadsToType){
		base.detectionRequiresCSkit = true;
		base.analysisRequiresCSkit = true;
	}

	/** When the most information about the lead is available
	 */
	public FingerPrints(int id, Info attachedToThis, Info leadsTo): base (id, attachedToThis, leadsTo){
		base.detectionRequiresCSkit = true;
		base.analysisRequiresCSkit = true;
	}
}