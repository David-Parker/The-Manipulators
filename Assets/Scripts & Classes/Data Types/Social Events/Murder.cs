using UnityEngine;

/** a Murder instance is created when a murder is discovered.
 * Whether the murder is believed to have occured, and who has done it, depends on the evidence gathered.
 * 
 * As opposed to SuspectedMurder, this instance is unique?
 */
public class Murder : SocialEvent {

	Person murderedPerson;
	bool solved;
	bool confirmedByAuthorities = false;
	Person killer;
	int eventSeverity = 10;
	Evidence body;

	public Murder(int identityNum, float currentGameTime, Person perpetrator, Person theDeceased) : base(identityNum, currentGameTime){

		base.trustImpact = Settings.MurderIntegrity; //FIXME: calibrate values
		base.globalSuspicionImpact = 10; //FIXME: Input from Settings()
		murderedPerson = theDeceased;
	}


	/** sets murderSolved to true and decreases its globalSuspicion, while increasing the 
	 */
	public void solveMurder(Person whosGuilty){
		//FIXME: add official integrity check
		confirmedByAuthorities = true;
		base.globalSuspicionImpact /= 10;
		base.trustImpact *= 2;
		bool solved;
	}



	public int Severity{get{return eventSeverity;}}
	public Person Killer{get{return killer;}}
	public bool MurderSolved{get{return confirmedByAuthorities;}}
	public Person MurderedPerson{get{return murderedPerson;}}
}
