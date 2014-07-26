/** leads to a specific object or a generic type of item. May also lead to a specific piece of evidence.
 */
public class FootPrints : ObjectiveEvidence {

	//FIXME: create a path var to store direction

	/** When there can't be any information on leads
	 */
	public FootPrints(int id, Info attachedToThis): base (id, attachedToThis){
		base.detectionRequiresCSkit = true;
		base.analysisRequiresCSkit = true;
	}

	/** When only the type of lead can be known
	 */
	public FootPrints(int id, Info attachedToThis, System.Type leadsToType): base (id, attachedToThis, leadsToType){
		base.detectionRequiresCSkit = true;
		base.analysisRequiresCSkit = true;
	}

	/** When the most information about the lead is available
	 */
	public FootPrints(int id, Info attachedToThis, Info leadsTo): base (id, attachedToThis, leadsTo){
		base.detectionRequiresCSkit = true;
		base.analysisRequiresCSkit = true;
	}

}
