using System.Collections.Generic;

/**Allows for a single, unique SocialEvent to be attributed to different Person()s.
 */
public class SocialEventAttribution{

	SocialEvent thisEvent;
	Person attributedTo;
	int combinedCredibility;
	float timeOccured;
	bool disputed = false; //when an event is disputed it will always remain on the town's suspicion record
	Dictionary<int, EvidenceCredibility> evidenceList = new Dictionary<int, EvidenceCredibility>(); //int = the Evidence's ID

	SocialEventAttribution(SocialEvent whichEvent, Person attributeToWho){
		thisEvent = whichEvent;
		attributedTo = attributeToWho;
	}

	/** Aggregates each evidence's credibilityImpact. These values have to be tweaked in the Evidence class for their correct application
	 */
	void calculateCombinedCredibility(){

	}

	public SocialEvent SocEvent {get{return thisEvent;}}
	public Person AttributedTo {get{return attributedTo;}set{attributedTo = value;}}
	public int Credibility {get{return combinedCredibility;}}
	public int TrustImpact {get{return thisEvent.TrustImpact;}}

}
