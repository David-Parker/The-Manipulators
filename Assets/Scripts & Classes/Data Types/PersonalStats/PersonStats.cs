using System.Collections.Generic;
using UnityEngine;

/**Class to facilitate backup of Person() stats. 
 * Since Person() inherits from MonoBehabviour, it can't be duplicated and thus we need an additional class for this.
 * FIXME: immediate integration to Person on creation
 * FIXME: Grouping class
 * 
 */
public class PersonStats{


	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//				 * * * * Constructors  * * * * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	

	public PersonStats(Person ofPerson_, int seriesNum, System.DateTimeOffset timeCreated_,PersonStatsGroup parentCollection_){
		ofPerson=ofPerson_;
		versionNum = seriesNum;
		timeCreated = timeCreated_;
	}


	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//			 * * * * Linking to specific person & Identifiers  * * * * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

	PersonStatsGroup parentCollection;
	Person ofPerson;
	int versionNum;
	System.DateTimeOffset timeCreated;

	public PersonStatsGroup ParentCollection{get{return parentCollection;}}
	public Person OfPerson{get{return ofPerson;}}
	public int VersionNum{get{return versionNum;}}
	public System.DateTimeOffset TimeCreated{get{return timeCreated;}}

	/** returns a copy with new identifiers
	 */
	public PersonStats makeCopy(int newVersionNum, PersonStatsGroup newCollection,System.DateTimeOffset creationTime){
		PersonStats newPS = this;
		newPS.versionNum = newVersionNum;
		newPS.parentCollection = newCollection;
		newPS.timeCreated = creationTime;
		return newPS;
	}


	//  * * * * Checks against runtime assignment * * * * * 

	bool genderSet = false;
	bool firstNameSet = false;
	bool lastNameSet = false;
	bool ageGroupSet =false;
	bool spouseSet = false;
	bool intelligenceSet = false;
	bool beautySet = false;
	bool natSuspicionSet = false;
	bool autonomySet = false;
	bool natStrengthSet = false;
	bool natEnergySet = false;
	
	// * * * * Used at the beginning in town generation * * * *
	int familyID;
	public int FamilyID{get{return familyID;}set{familyID = value;}}
	
	
	
	
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//				 * * * * NATURAL / STATIC STATISTICS  * * * * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	
	public enum ageGroup {
		Child,
		Teen,
		Adult,
		Senior
	}
	ageGroup ageGrouping;
	
	public enum relativeType {
		Sibling,
		Spouse,
		Parent,
		Child,
		Cousin,
		GrandParent,
		Grandchild,
		UncleAunt, 
		Nephew, 
		ParentInLaw,
		SiblingInLaw,
		HalfSibling,
		None
	}
	
	//string ageGroup; //valid values: Child, Teen, Adult, Senior 
	
	string firstName;
	//last name found in family section

	float lastChangedInf = 0;
	bool isMale;	
	bool heteroSexuality = true;
	bool hasFingerprints = true;
	int intelligence;
	int beauty;
	int natSuspicion;
	int autonomyLevel;
	int natStrength;
	int natEnergy;
	
	public bool IsAdult{
		get{
			if(this.AgeGroup == ageGroup.Adult || this.AgeGroup == ageGroup.Senior){
				return true;
			}else{
				return false;
			}
		}
	}
	
	public string FirstName{
		get{
			return firstName;
		}set{
			if(!firstNameSet){
				firstNameSet = true;
				firstName = value;
			}else{
				Debug.LogError("Changing firstName after it was set is not allowed");
			}
		}
	}
	
	public bool IsMale{
		get{
			return isMale;
		}set{
			if(!genderSet){
				genderSet = true;
				isMale=value;
			}else{
				Debug.LogError("Changing gender after it was set is not allowed");
			}
		}
	}
	public ageGroup AgeGroup{
		get{
			return ageGrouping;
		}set{
			if(!ageGroupSet){
				ageGrouping= value;
				
				/* FIXME: possibly remove
				if(value=="Child" || value=="Teen" || value=="Adult" ||value=="Senior"){
					ageGroupSet = true;
					ageGroup=value;
				}else{
					Debug.LogError("Invalid value \" " +value+ " \" can't be assigned as AgeGroup");
				}
				*/
			}else{
				Debug.LogError("Changing ageGroup after it was set is not allowed");
			}
		}
	}

	public bool HasFingerPrints{get{return hasFingerprints;}set{hasFingerprints = value;}}
	public float LastChangedInf{get{return lastChangedInf;}}
	public int Intelligence{
		get{return intelligence;}
		set{
			if(!intelligenceSet){
				intelligenceSet = true;
				intelligence = value;
			}else{
				Debug.LogError("overwriting intelligence value directly after it was set is not allowed");
			}
		}
	}
	public int Beauty{
		get{return beauty;}
		set{
			if(!beautySet){
				beautySet = true;
				beauty = value;
			}else{
				Debug.LogError("overwriting beauty value directly after it was set is not allowed");
			}
		}
	}
	public int NatSuspicion{
		get{return natSuspicion;}
		set{
			if(!natSuspicionSet){
				natSuspicionSet = true;
				natSuspicion = value;
			}else{
				Debug.LogError("overwriting natural suspicion value directly after it was set is not allowed");
			}
		}
	}
	public int Autonomy{
		get{return autonomyLevel;}
		set{
			if(!autonomySet){
				autonomySet = true;
				autonomyLevel = value;
			}else{
				Debug.LogError("overwriting autonomy value directly after it was set is not allowed");
			}
		}
	}
	public int NatStrength{
		get{return natStrength;}
		set{
			if(!natStrengthSet){
				natStrengthSet =true;
				natStrength = value;
			}else{
				Debug.LogError("overwriting natural strength value directly after it was set is not allowed");
			}
		}
	}
	public int NatEnergy{
		get{return natEnergy;}
		set{
			if(!natEnergySet){
				natStrengthSet = true;
				natStrength = value;
			}else{
				Debug.LogError("overwriting natural energy value directly after it was set is not allowed");
			}
		}
	}


	
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//			 * * * * * * * *  DYNAMIC STATISTICS * * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	
	// * * * * * * modes
	bool panicMode= false;
	bool sleeping=false;
	bool inComa=false;
	bool isOfficial=false;
	
	
	//  * * * * Energy/performance.
	int hoursSleptToday =0; 
	int hoursWorkedToday = 0; //modfies energy, but also workPerformance.\
	int workPerformace;
	float lastSlept=0; //in game time
	float currentEnergy;
	float currentStrength;
	
	
	// * * * * Damage * * * * * 
	bool isDead = false;
	bool mutilatedFace = false; //when true, limits recognition
	bool killedBySet = false;
	bool dead = false;
	Murder killedByThis;
	
	/*
	float intoxLevel = 0;
	float arcOfView;
	float perceptiveness;
	float alertness;
	float totalSuspicion;
	float personalSuspicion;
	float enviroSuspicion;
	*/
	
	
	public float Energy{get{return currentEnergy;}}
	public float Strength{get{return currentStrength;}}
	public bool Dead{get{return dead;}set{dead = value;}}
	public Murder KilledByThis{
		get{
			if(isDead){
				return killedByThis;
			}else{
				Debug.LogError("Person is not dead");
				return null;
			}
		}set{
			if(!killedBySet){
				killedByThis = value;
				killedBySet = true;
			}else{
				Debug.LogError("KilledByThis is only allowed to be set once");
			}
		}
	}
	
	
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//			 * * * * * GROUP REFERENCES, NON FAMILY * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	
	//FIXME: this section to be superceeded by specific  ...???
	
	Person boss;
	
	/*
	Clique cliqueAssoc;
	*/
	
	/*
	List<Person> gFriends= new List<Person>;
	List<Person> friends= new List<Person>;
	List<Person> acquaintances= new List<Person>;
	List<Person> enemies= new List<Person>;
	List<Person> suspiciousPersons= new List<Person>;
	*/
	
	/*
	public Person Boss{get{return boss;}set{boss=value;}}
	
	public Family FamilyAssoc{get{return familyAssoc;}}
	*/
	/*
	public Clique CliqueAssoc{get{return cliqueAssoc;}set{cliqueAssoc=value;}}
	public Workplace JobAssoc{get{return jobAssoc;}set{jobAssoc=value;}}
	*/
	
	Workplace workplaceAssoc;
	Household householdAssoc;
	
	public Household HouseholdAssoc{get{return householdAssoc;}set{householdAssoc=value;}}
	public Workplace WorksAt{get{return workplaceAssoc;}} //TODO methods of being fired, finding new job, etc

	

	
	
	
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 			* * * * * * * * FAMILY * * * * * * * * * 
	// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*- 
	
	string lastName;
	//FIXME: use socialevent/evidence to store these pointers as opinion modifiers
	Dictionary<int,Person> familyMembers = new Dictionary<int, Person>();	
	Person spouse;
	Dictionary<int,Person> exSpouses = new Dictionary<int, Person>();
	Dictionary<int,Person> children = new Dictionary<int, Person>();
	Dictionary<int,Person> adoptedChildren = new Dictionary<int, Person>();
	Dictionary<int,Person> grChildren = new Dictionary<int, Person>();
	Dictionary<int,Person> parents = new Dictionary<int, Person>();
	Dictionary<int,Person> adoptiveParents = new Dictionary<int, Person>();
	Dictionary<int,Person> grParents = new Dictionary<int, Person>();
	Dictionary<int,Person> siblings = new Dictionary<int, Person>();
	Dictionary<int,Person> halfSiblings = new Dictionary<int, Person>();
	Dictionary<int,Person> cousins = new Dictionary<int, Person>();
	Dictionary<int,Person> unclesAunts  = new Dictionary<int, Person>(); //also contains uncles & aunts in law
	Dictionary<int,Person> nephews = new Dictionary<int, Person>(); //also contains nephews in law
	Dictionary<int,Person> siblingsInLaw = new Dictionary<int, Person>();
	Dictionary<int, Person> parentsInLaw = new Dictionary<int, Person>();
	Dictionary<int, Person> childrenInLaw = new Dictionary<int, Person>();
	public string LastName{
		get{
			return lastName;
		}set{
			if(!lastNameSet){
				lastNameSet = true;
				lastName = value;
			}else{
				Debug.LogError("Changing lastName afeter it was set is not allowed");
			}
		}
	}
	public Dictionary<int,Person> FamilyMembers {get{return familyMembers;}}
	public Person Spouse{ 
		get{return spouse;}
		set{
			if(spouseSet){
				Debug.LogError("spouse may only be set once");
			}else{
				if(!familyMembers.ContainsKey(value.IdentityNum)){
					familyMembers.Add(value.IdentityNum,value);
					spouse = value;
				}else{
					relativeType findWhere = findFamilyRelation(value);
					if(findWhere == relativeType.SiblingInLaw){
						siblingsInLaw.Remove(value.IdentityNum);
						spouse = value;
					}else{
						Debug.LogError("Can't add spouse, as it would overwrite exisiting connection of " + findWhere.ToString());
					}
					
				}
			}
		}
	}
	public Dictionary<int, Person> ExSpouses{get{return exSpouses;}}
	public Dictionary<int,Person> Children{get{return children;}}
	public Dictionary<int, Person> AdoptedChildren{get{return adoptedChildren;}}
	public Dictionary<int,Person> GrandChildren{get{return grChildren;}}
	public Dictionary<int,Person> Parents{get{return parents;}}
	public Dictionary<int,Person> AdoptingParents{get{return adoptiveParents;}}
	public Dictionary<int,Person> GrandParents{get{return grParents;}}
	public Dictionary<int,Person> Siblings{get{return siblings;}}
	public Dictionary<int,Person> HalfSiblings{get{return halfSiblings;}}
	public Dictionary<int,Person> Cousins{get{return cousins;}}
	public Dictionary<int,Person> UnclesAunts{get{return unclesAunts;}}
	public Dictionary<int,Person> Nephews{get{return nephews;}}
	public Dictionary<int,Person> SiblingsInLaw{get{return siblingsInLaw;}}
	public Dictionary<int,Person> ParentsInLaw{get{return parentsInLaw;}}
	public Dictionary<int, Person> ChildrenInLaw{get{return childrenInLaw;}}


	/**
	 * Used to add children,parents,etc. 
	* Checks for duplicates before adding.
	*/
	public bool addFamilyRelation(Dictionary<int, Person> toThisDatabase, Person thePerson){ //FIXME possibly doesn't overwrite specified database, consider using relationType
		if(!Town.FamiliesCreated){
			if(thePerson.IdentityNum != ofPerson.IdentityNum){
				if(!toThisDatabase.ContainsKey(thePerson.IdentityNum)){
					if(!familyMembers.ContainsKey(thePerson.IdentityNum)){ 
						//FIXME: you might want to overwrite some roles, like cousin>sibling ?? would that create a lot of confusion? 
						//sibling in law>sibling(x's spouses' sibling's spouse was before a sibling in law and now is a sibling;2 siblings marrying 2 other siblings)
						toThisDatabase.Add(thePerson.IdentityNum,thePerson);
						familyMembers.Add(thePerson.IdentityNum,thePerson);
						return true;
					}else{ //error: persons can not serve in multiple family roles to the same person. (only a connection type of spouse may overwrite sibling in law)
						relativeType findWhere = findFamilyRelation(thePerson);
						if(findWhere == relativeType.None){
							Debug.LogError("Person added to "+toThisDatabase+" is already a family member. His family relation could not be found");
							return false;
						}else{
							Debug.LogError("Person added to "+toThisDatabase+" is already is a family member of type: "+findWhere.ToString());
							return false;
						}
					}
				}else{
					Debug.LogError("Person already exists in the database "+toThisDatabase.ToString());
					return false;
				}
			}else{
				Debug.LogError("Person can't be its own family connection");
				return false;
			}
		}else{
			Debug.LogError("Can't add additional family members after all families have been created");
			return false;
		}
	}

	/** Used for debugging information in addFamilyRelation()
	 */
	relativeType findFamilyRelation(Person ofThisPerson){ 
		if(familyMembers.ContainsKey(ofThisPerson.IdentityNum)){
			if(grParents.ContainsKey(ofThisPerson.IdentityNum)){
				return relativeType.GrandParent;
			}else if(parents.ContainsKey(ofThisPerson.IdentityNum)){
				return relativeType.Parent;
			}else if(siblings.ContainsKey(ofThisPerson.IdentityNum)){
				return relativeType.Sibling;
			}else if(cousins.ContainsKey(ofThisPerson.IdentityNum)){
				return relativeType.Cousin;
			}else if(children.ContainsKey(ofThisPerson.IdentityNum)){
				return relativeType.Child;
			}else if(grChildren.ContainsKey(ofThisPerson.IdentityNum)){
				return relativeType.Grandchild;
			}else if(unclesAunts.ContainsKey(ofThisPerson.IdentityNum)){
				return relativeType.UncleAunt;
			}else if(spouse == ofThisPerson){
				return relativeType.Spouse;
			}else{
				Debug.LogError("relation is in familyMembers index but not in any of the subgroups");
				return relativeType.None;
			}
		}else{ //no relation found
			return relativeType.None;
		}
	}
	
	// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 			* * * * * * * INTEGRITY / SOCIALEVENTS  * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	
	Dictionary<int, SocialEventAttribution> unnatributedSocialEvents = new Dictionary<int, SocialEventAttribution>();
	Dictionary<int, Dictionary<int,SocialEventAttribution> > opinionsOfPeople = new Dictionary<int, Dictionary<int, SocialEventAttribution>>(); 
	//^where <PersonID, Dictionary<SocialEventID,SocialEventAttribution> >


	public Dictionary<int, SocialEventAttribution> UnnatributedSocialEvents{get{return unnatributedSocialEvents;}set{unnatributedSocialEvents = value;}}
	public Dictionary<int, Dictionary<int,SocialEventAttribution>> OpinionsOfPeople {get{return opinionsOfPeople;} set{opinionsOfPeople = value;}}

	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//				 * * * * * * GROUP LEADERSHIP * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	
	bool cliqueLead =false; 
	bool householdLead=false;
	bool workplaceLead=false;
	bool packLead=false;

	/** Sets the appropriate leadership bool to true
	 */
	public void assignLeadership(System.Type socStructureType){
		if(socStructureType == typeof(Household)){
			householdLead = true;
		}else if(socStructureType == typeof(Clique)){
			cliqueLead = true;
		}else if(socStructureType == typeof(Pack)){
			packLead  = true;
		}else if(socStructureType == typeof(Workplace)){
			workplaceLead = true;
		}else{
			Debug.LogError("assignLeadership() recieved invalid input of type "+ socStructureType);
		}
	}
	
	/** Sets the appropriate leadership bool to false 
	 */
	public void removeLeadership(System.Type socStructureType){
		if(socStructureType == typeof(Household)){
			householdLead = false;
		}else if(socStructureType == typeof(Clique)){
			cliqueLead = false;
		}else if(socStructureType == typeof(Pack)){
			packLead = false;
		}else if(socStructureType == typeof(Workplace)){
			workplaceLead = false;
		}else{
			Debug.LogError("assignLeadership() recieved invalid input of type "+ socStructureType);
		}
	}
	
	/** Checks the appropriate leadership bool
	 */
	public bool checkIfLeader(System.Type socStructureType){
		if(socStructureType == typeof(Household)){
			return householdLead;
		}else if(socStructureType == typeof(Clique)){
			return cliqueLead;
		}else if(socStructureType == typeof(Pack)){
			return packLead;
		}else if(socStructureType == typeof(Workplace)){
			return workplaceLead;
		}else{
			Debug.LogError("assignLeadership() recieved invalid input of type "+ socStructureType);
			return false;
		}
	}

}
