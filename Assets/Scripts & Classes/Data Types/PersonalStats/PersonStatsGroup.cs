using System.Collections.Generic;
using UnityEngine;

/** FIXME connect with individual people (create a var to know which personStatsGroup is currently in use, etc
 */
public class PersonStatsGroup
{	
	/** used to create a blank PersonStatsGroup
	 */
	public PersonStatsGroup (int seriesNum,System.DateTime timeCreated_){
		timeCreated = timeCreated_;
		versionNum = seriesNum;
	}

	/** key = id of person PersonStats refers to
	 */
	Dictionary<int, PersonStats> personStatsCollection = new Dictionary<int, PersonStats>();
	int versionNum;
	System.DateTimeOffset timeCreated;
	bool isInvalid = false;
	PersonStatsGroup previousPSG;

	public int VersionNum{get{return versionNum;}}
	public System.DateTimeOffset TimeCreated{get{return timeCreated;}}
	public Dictionary<int,PersonStats> PersonStatsCollection {get{return personStatsCollection;}}
	public bool IsInvalid {get{return isInvalid;}}
	/**returns the series number of the previously used PersonStatsGroup, which usually serves as a backup
	 */
	public PersonStatsGroup PreviousPSG {get{return previousPSG;}}


	/** ! Does not change Town.ActivePersonStatsGroup ! & doesn't reassign Person.PersonalStats
	 */
	public void makeInvalid(){
		isInvalid = true;
		personStatsCollection.Clear(); //removes no longer necessary data
	}

	/** Creates a blank group of Person Stats
	 */
	public void createPersonalScores(){
		int numPeople =  Town.TownPeopleDatabase.PersonIndex.Count;
		for(int i =0;i<numPeople;i++){
			Person getPerson;
			if(!Town.TownPeopleDatabase.PersonIndex.TryGetValue(i,out getPerson)){
				Debug.LogError("Couldn't get person with id of "+i+" from townPersonsDatabase.PersonIndex");
			}
			PersonStats aPStats = new PersonStats(getPerson,versionNum,timeCreated,this);
			personStatsCollection.Add(i,aPStats);
		}
	}

	/** this method doesn't increment versionStatsCounter nor adds the PersonStatsGroup to PersonStatsCollection
	 */
	public PersonStatsGroup makeCopy(int newVersionNum_){
		PersonStatsGroup copyPsg = new PersonStatsGroup(newVersionNum_,System.DateTime.UtcNow);
		copyPsg.previousPSG = this;
		foreach(PersonStats aPs in this.PersonStatsCollection.Values){
			PersonStats copyPs = aPs.makeCopy(newVersionNum_,copyPsg,copyPsg.timeCreated);
			copyPsg.personStatsCollection.Add(copyPs.OfPerson.IdentityNum,copyPs);
		}
		return copyPsg;
	}

}

