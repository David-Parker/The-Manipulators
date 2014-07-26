using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

/*	TODO
 * 
 * 	1. pathfinding() 
 * 	2. recurrent tasks loop
 * 	3. random full constructor -> first+last name generator (with unique last names)
 * 	4. info() method
 * 	5. panicMode() - modifies stats. Also add a method in update(0 that checks for panicModeOn == true to lower energy
 *	if( 8 > hoursSleptToday|| lastSlept - currentTime > 20){S+E will diminish}
 *	6. Finish SocialSructures
 *	7. Organise all vars/methods by sections
 */


/** Holds all variables of a person.
 */
public class Person : Info {



	// * * * * overrides * * * * 

	public override string ItemDescription{get{return itemDescription;}set{itemName = value;}}
	public override string ItemName{get{return itemName;}set{itemName = value;}}

	// * * * *  constructors * * * * 

	public Person(){

	}

	
	// * * * * Temporary & Debug * * * * 

	public float calculateInfluence(Person otherPerson){ // !!! Curently random !!!
		//if(otherPerson.identityNum != identityNum){
		if(otherPerson != this){
			if(Random.Range(0,2) < 1){ // binary randomisation of returned value sign
				//personStats.LastChangedInf = Time.realtimeSinceStartup; FIXME apply
				return Random.Range(1,10);
			}else{
				//personStats.LastChangedInf = Time.realtimeSinceStartup; FIXME apply
				return -Random.Range(1,10);
			}
		}else{
			return 0;
		}
	}


	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//				 * * * * NATURAL / STATIC STATISTICS  * * * * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

	int identityNum;
	bool idNumSet= false;

	PersonStats personStats;
	public PersonStats PersonalStats{get{return personStats;}set{personStats = value;}}

	public int IdentityNum{
		get{return identityNum;
		}set{
			if(!idNumSet){
				idNumSet = true;
				identityNum=value;
			}else{
				Debug.LogError("Changing idNum after it was set is not allowed");
			}
		}
	}




	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//			 * * * * * GROUP REFERENCES, NON FAMILY * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

	//FIXME this section to be superceeded by other sections of specific groups ...???



	/**If current householdAssoc!=null, it will update both households' members+leaders and the global influence table. 
	 */
	public void changeHousehold(Household newHousehold, bool recalculateInfluencesAndLeaders){
		if(personStats.HouseholdAssoc!=null){ //not the first time
			if(!personStats.HouseholdAssoc.PersonIndex.Remove(this.identityNum)){
				Debug.LogError("Could not remove person from old household");
			}
			newHousehold.PersonIndex.Add(this.identityNum,this);
			if(recalculateInfluencesAndLeaders){
				personStats.HouseholdAssoc.computeLeaders();
				newHousehold.computeLeaders();
				Town.TownPeopleDatabase.updateGlobalInfluenceTable(true);
			}
		}		
		personStats.HouseholdAssoc = newHousehold;
	}


	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//		 			* * * * * * * * FAMILY * * * * * * * * * 
	// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*- 

		// The following methods are written because family Dictionaries are read-only.
	public bool addChild(Person aChild){
		if(personStats.IsAdult){
			if(!ageLowerMismatch(aChild)){
				if(personStats.addFamilyRelation(personStats.Children,aChild)){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		}else{
			Debug.LogError("Adding a child to a non-adult person is not allowed");
			return false;
		}
	}
	public bool addAdoptedChild(Person adoptedChild){
		if(personStats.IsAdult){
			if(!ageLowerMismatch(adoptedChild)){
				if(personStats.addFamilyRelation(personStats.AdoptedChildren,adoptedChild)){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		}else{
			Debug.LogError("Adding a child to a non-adult person is not allowed");
			return false;
		}
	}
	public bool addExSpouse(Person ex){ //a divorce method will be used within the game
		if(personStats.IsAdult){
			if(!sameAgeMismatch(ex)){
				if(personStats.addFamilyRelation(personStats.ExSpouses,ex)){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		}else{
			Debug.LogError("Adding an ex-spouse to a non-adult person is not allowed");
			return false;
		}
	}
	public bool addGrandChild(Person grandChild){
		if(personStats.AgeGroup == PersonStats.ageGroup.Senior){
			if(!grandChild.personStats.IsAdult){
				if(personStats.addFamilyRelation(personStats.GrandChildren,grandChild)){
					return true;
				}else{
					return false;
				}
			}else{
				Debug.LogError("Adding an adult as a grandchild is not allowed");
				return false;
			}
		}else{
			Debug.LogError("Adding a grandChild to a non-senior person is not allowed");
			return false;
		}
	}
	public bool addParent(Person parent){
		if(personStats.AgeGroup != PersonStats.ageGroup.Senior){
			if(personStats.Parents.Count<3){
				if(!ageHigherMismatch(parent)){
					if(personStats.addFamilyRelation(personStats.Parents,parent)){
						return true;
					}else{
						return false;
					}
				}else{
					return false;
				}
			}else{
				Debug.LogError("Can't add 3rd parent");
				return false;
			}
		}else{
			Debug.LogError("Adding a parent to a senior person is not allowed");
			return false;
		}
	}

	/**Doesn't marry parents
	 */
	public bool addParents(Person parent1,Person parent2){
		if(personStats.AgeGroup != PersonStats.ageGroup.Senior){
			if(personStats.Parents.Count ==0){	
				if(!ageHigherMismatch(parent1) && !ageHigherMismatch(parent2)){
					if(personStats.addFamilyRelation(personStats.Parents,parent1) && personStats.addFamilyRelation(personStats.Parents,parent2)){
						return true;
					}else{
						return false;
					}
				}else{
					return false;
				}
			}else{
				Debug.LogError("Can't have more than 2 parents");
				return false;
			}
		}else{
			Debug.LogError("Adding parents to a senior person is not allowed");
			return false;
		}
	}
	/**FIXME: might want to add another method to allow to do this in-game?
	 */
	public bool addAdoptiveParent(Person adoptiveParent){
		if(personStats.AgeGroup != PersonStats.ageGroup.Senior){
			if(personStats.AdoptingParents.Count < 2){
				if(!ageHigherMismatch(adoptiveParent)){
					if(personStats.addFamilyRelation(personStats.AdoptingParents,adoptiveParent)){
						return true;
					}else{
						return false;
					}
				}else{
					return false;
				}
			}else{
				Debug.LogError("Adding a second adoptive parent is not allowed");
				return false;
			}
		}else{
			Debug.LogError("Adding adoptive parent to a senior person is not allowed");
			return false;
		}
	}
	public bool addGrandParent(Person grandParent){
		if(!personStats.IsAdult){
			if(personStats.GrandParents.Count<5){
				if(grandParent.personStats.AgeGroup == PersonStats.ageGroup.Senior){
					if(personStats.addFamilyRelation(personStats.GrandParents,grandParent)){
						return true;
					}else{
						return false;
					}
				}else{
					Debug.LogError("adding a non senior person as grandparent is not allowed");
					return false;
				}
			}else{
				Debug.LogError("can't add 5th grandparent");
				return false;
			}
		}else{
			Debug.LogError("Adding a grandparent to an adult is no allowed");
			return false;
		}
	}
	public bool addGrandParents(Person grandParent1,Person grandParent2){
		if(!personStats.IsAdult){
			if(grandParent1.personStats.AgeGroup == PersonStats.ageGroup.Senior && grandParent2.personStats.AgeGroup == PersonStats.ageGroup.Senior){
				if(personStats.addFamilyRelation(personStats.GrandParents,grandParent1) && personStats.addFamilyRelation(personStats.GrandParents,grandParent2)){
					return true;
				}else{
					return false;
				}
			}else{
				Debug.LogError("adding a non senior person as grandparent is not allowed");
				return false;
			}
		}else{
			Debug.LogError("Adding grandparents to an adult is not allowed");
			return false;
		}
	}
	public bool addSibling(Person sibling){
		if(!sameAgeMismatch(sibling)){
			if(personStats.addFamilyRelation(personStats.Siblings,sibling)){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	public bool addHalfSibling(Person halfSibling){
		if(!sameAgeMismatch(halfSibling)){
			if(personStats.addFamilyRelation(personStats.HalfSiblings,halfSibling)){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	public bool addCousin(Person cousin){
		if(!sameAgeMismatch(cousin)){
			if(personStats.addFamilyRelation(personStats.Cousins,cousin)){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	public bool addUncleAunt(Person uncleAunt){
		if(personStats.AgeGroup != PersonStats.ageGroup.Senior){
			if(!ageHigherMismatch(uncleAunt)){
				if(personStats.addFamilyRelation(personStats.UnclesAunts,uncleAunt)){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		}else{
			Debug.LogError("Adding an aunt/uncle to a senior is not allowed");
			return false;
		}
	}
	public bool addNephew(Person nephew){
		if(!personStats.IsAdult){
			if(!ageLowerMismatch(nephew)){
				if(personStats.addFamilyRelation(personStats.Nephews,nephew)){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		}else{
			Debug.LogError("Adding a nephew to a non-adult is not allowed");
			return false;
		}
	}
	public bool addParentInLaw(Person parentInLaw){
		if(personStats.AgeGroup != PersonStats.ageGroup.Senior){
			if(!ageHigherMismatch(parentInLaw)){
				if(personStats.addFamilyRelation(personStats.ParentsInLaw,parentInLaw)){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		}else{
			Debug.LogError("Adding a parent in law to a senior person is not allowed");
			return false;
		}
	}
	public bool addSiblingInLaw(Person siblingInLaw){
		if(!sameAgeMismatch(siblingInLaw)){
			if(personStats.addFamilyRelation(personStats.SiblingsInLaw,siblingInLaw)){
				return true;
			}else{
				return false;
			}
		}else{
			return false;
		}
	}
	public bool addChildInLaw(Person childInLaw){
		if(!personStats.IsAdult){
			if(!ageLowerMismatch(childInLaw)){
				if(personStats.addFamilyRelation(personStats.ChildrenInLaw,childInLaw)){
					return true;
				}else{
					return false;
				}
			}else{
				return false;
			}
		}else{
			Debug.LogError("Adding a child in law to a non-adult is not allowed");
			return false;
		}
	}

	/**Checks if sibling/cousin realtion is possible while allowing for teen-child relations.
	 * returns true if impossible
	 */
	bool sameAgeMismatch(Person sibling){
		if(sibling.personStats.AgeGroup != personStats.AgeGroup){//could still be teen-child link
			if(sibling.personStats.IsAdult || personStats.IsAdult){ //if either is an adult it's a mismatch
				return false;
			}else{ //allowing child-teen links
				Debug.LogError("Can't create sibling connection between "+sibling.personStats.AgeGroup.ToString()+" and "+personStats.AgeGroup.ToString());
				return true;
			}
		}else{
			return false;
		}
	}
	
	/**returns true if there is a mismatch
	 */
	bool ageLowerMismatch(Person youngerPerson){
		if(personStats.AgeGroup == PersonStats.ageGroup.Senior){
			if(youngerPerson.personStats.AgeGroup != PersonStats.ageGroup.Senior){
				return false;
			}else{
				Debug.LogError("A senior person recieved input of another senior person");
				return true;
			}
		}else if(personStats.AgeGroup == PersonStats.ageGroup.Adult){
			if(!youngerPerson.personStats.IsAdult){
				return false;
			}else{
				Debug.LogError("An adult person recieved input of another adult person");
				return true;
			}
		}else{ // teens or children
			Debug.LogError("originating Person is not an adult");
			return true;
		}
	}
	
	/** Returns true if there is a mismatch
	 */
	bool ageHigherMismatch(Person olderPerson){
		if(!personStats.IsAdult){ // teen or child
			if(olderPerson.PersonalStats.IsAdult){
				return false;
			}else{
				Debug.LogError("Child/teen received a non-adult input");
				return true;
			}
		}else if(personStats.AgeGroup == PersonStats.ageGroup.Adult){ 
			if(olderPerson.personStats.AgeGroup == PersonStats.ageGroup.Senior){
				return false;
			}else{
				Debug.LogError("adult receieved a non-senior input of "+olderPerson.PersonalStats.AgeGroup.ToString());
				return true;
			}
		}else{ //senior
			Debug.LogError("Originating Person is a senior");
			return true;
		}
	}



	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	//				  * * * * STATS INITIALISATION  * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-



	/** Sets static person attributes. Uses a int input to create a person of specific difficulty.
	* Difficulty modifier is in range 1-3(including), the higher the more difficult
	*/
	public void initialisePersonStats(int difficultyModifier){
		int min=Settings.HumanMinStat;
		int max=Settings.HumanMaxStat;
		if(difficultyModifier==1){
			max /= 2;
		}else if(difficultyModifier==2){
			min = max/4;
			max = (max/4)*3;
		}else if(difficultyModifier==3){
			min= max/2;
		}else{
			Debug.LogError("Invalid input of "+ difficultyModifier+" has been passed to initialisePersonStats()");
		}

		personStats.Intelligence = Random.Range(min,max);
		personStats.Beauty = Random.Range(min,max);
		personStats.NatSuspicion = Random.Range(min,max);
		personStats.Autonomy = Random.Range(min,max);
		personStats.NatStrength = Random.Range(min,max);
		personStats.NatEnergy = Random.Range(min,max);
	}

	/** Sets static Person attributes, the random type method.
	*/
	public void initialisePersonStats(){
		personStats.Intelligence = Random.Range(Settings.HumanMinStat,Settings.HumanMaxStat);
		personStats.Beauty = Random.Range(Settings.HumanMinStat,Settings.HumanMaxStat);
		personStats.NatSuspicion = Random.Range(Settings.HumanMinStat,Settings.HumanMaxStat);
		personStats.Autonomy = Random.Range(Settings.HumanMinStat,Settings.HumanMaxStat);
		personStats.NatStrength = Random.Range(Settings.HumanMinStat,Settings.HumanMaxStat);

		if(Random.Range(0,2) < 1){
			personStats.IsMale = true;
		}else{
			personStats.IsMale = false;
		}
	}






	// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 			* * * * * * * INTEGRITY / SOCIALEVENTS  * * * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-



	/** If the SocialEventAttribution doesn't have a Person attributed (null), it adds the SEA to unnatributedSocialEvents.
	 * If it does, it will add it to the appropriate opinion list.
	 */
	public void addSocEventAttr(SocialEventAttribution thisSEA){
		if(thisSEA.AttributedTo == null){ //searching unnatributed Social Events instead of opinions
			if(!personStats.UnnatributedSocialEvents.ContainsKey(thisSEA.SocEvent.ID)){ 
				personStats.UnnatributedSocialEvents.Add(thisSEA.SocEvent.ID,thisSEA);
			}else{
				Debug.LogWarning("You are trying to add a SocialEventAttribution to unnatributedSocialEvents which already exists");
			}
		}else{//need to search the correct opinion Dictionary to confirm this key doesn't already exist
			Dictionary<int,SocialEventAttribution> anOpinionList = new Dictionary<int, SocialEventAttribution>();
			if(personStats.OpinionsOfPeople.TryGetValue(thisSEA.AttributedTo.IdentityNum, out anOpinionList)){
				if(anOpinionList.ContainsKey(thisSEA.SocEvent.ID)){
					Debug.LogWarning("you are trying to add a SocialEventAttributtion to an opinion list about a Person with id: "+thisSEA.AttributedTo.IdentityNum);
				}else{ 
					anOpinionList.Add(thisSEA.SocEvent.ID,thisSEA);
					personStats.OpinionsOfPeople[thisSEA.AttributedTo.IdentityNum] = anOpinionList; //overwrite old opinion list with a new one
				}
			}else{ //no opinion list exists for this Person's ID, create one
				anOpinionList.Add(thisSEA.SocEvent.ID,thisSEA);
				personStats.OpinionsOfPeople.Add(thisSEA.AttributedTo.identityNum,anOpinionList);
			}
		}
	}

	public void addWitnessedIntegrityEvent(SocialEvent witnessedEvent){
		//addSocEventAttr()
	}

	public void addRelayedIntegrityEvent(SocialEventAttribution relayedEvent){
		//addSocEventAttr()
	}

	public void modifyIntegrityvent(SocialEventAttribution theEvent){

	}

	/** Other than the Manager's opinion, colleagues' opinion is taken into account and factored against quota/hours worked.
	 */
	public int calculateWorkPerformance(){
		//FIXME: write me
		return 0;
	}

	public int opinionOfPerson(Person onWho){
		Dictionary<int, SocialEventAttribution> listOfSocEvents = new Dictionary<int, SocialEventAttribution>();
		if(personStats.OpinionsOfPeople.TryGetValue(onWho.identityNum,out listOfSocEvents)){
			int calculation = 0; 
			int modifier = 0;//difference between positive - negative events
			foreach(KeyValuePair<int,SocialEventAttribution> aKVP in listOfSocEvents){
				int impactOfAnEvent = aKVP.Value.Credibility * aKVP.Value.TrustImpact / Settings.MaxEvidenceCred ;//Removing 100 as credibility is in 1-100 range.
				//Assumed max possible value of ~HumanMaxOpinion (being int it is < |128| )
				calculation += impactOfAnEvent; //the main iteration
				if(impactOfAnEvent > Settings.MaxEvidenceCred/20){ //removing negligible events
					modifier++;
				}else if(impactOfAnEvent < -Settings.MaxEvidenceCred/20){ //removing negligible events
					modifier--;
				}
			}
			if(calculation > 0 && modifier > 0 || calculation < 0 && modifier < 0){ //a modifier should only reinforce the same signed function
				calculation *= ((Mathf.Abs(modifier)/(Settings.MaxEvidenceCred/10)) + 1); 
			}
			if(calculation > Settings.PersonMaxOpinion){
				return Settings.PersonMaxOpinion;
			}else if(calculation < -Settings.PersonMaxOpinion){
				return -Settings.PersonMaxOpinion;
			}
			return calculation;
		}else{ //no opinion, return neutral
			return 0;
		}
	}


	void Start(){
	}

	void Update(){
	}
}
