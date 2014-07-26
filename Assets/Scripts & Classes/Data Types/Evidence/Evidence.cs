/** To know a person is guilty, there needs to be the following evidence (in general):
 *  Required are: 2 links + 3 objects:
 *  a. violation (broken item/body) [contained in B]
 *  b.link between violation and method (bloodied weapon at the scene, inherent*) 
 *  c. method (axe, weapon, inherent*) [contained in D]
 *  d. link between method and person/suspect (fingerprints, location of suspect, etc)
 *  e. suspect (who may escape, but otherwise not formally processed, will be considered guilty anyway) [contained in D]
 *  *idefinition of inherent methods:
 *  	" where the crimes involve the posesssions/location of the suspect (break-in/trespassing, carying unlicensed firearms/drugs)"
 * 
 * - - - - - - - - - - 
 * FIXME
 * - - - - -- - - - - 
 * 1. Person's location evidence requires all persons to remember their daily interactions and locations of other people, is this efficient?  
 *  a list of LasretSeen() for each MEMORABLE person (and fading of which) transforms into evidence when something suspicious happens.
 * 2. Evidence being used for multiple offences (both break in and murder), multiple instances? Possible solution: 
 * Use the offences' time/space to compare in observers minds suspicious persons.
 */
public abstract class Evidence{

	protected int id;
	protected bool removed = false;
	protected int defaultCredibilityImpact;

	public Evidence(int id_){
		id=id_;
	}

	public int ID{get{return id;}}
	public bool Removed{get{return removed;}}

}