using UnityEngine;
using System.Collections.Generic;
using Manipulators;

//TODO : perhaps change main personIndex (all people index) database from dictionary<int,Person> to List<Person> sorted by index?

/**SocialStructure is a parent class for most social structure classes in game, including classes like: packs, households, and workplaces.
 * SocialStructure is created as an object of its own only once, as townDatabase, by the Town() class.
 * the 2D Dictionary influenceTable, is modified in several influence methods of this class, it should not be accessed by outside functions.
 */
public class SocialStructure {


	// * * * * Level designer variables * * * * *

	public float leadRatioThreshold = 1.2f;
	public float leadAbsoluteThreshold = 5f;
	public float updateInfInterval = 300f;
	public bool debugMode = false;

	// * * * * private vars only to be used in the instance townPeopleDatabase * * * * 
	Dictionary<int, Dictionary<int,PersonalScore> > globalInfluenceTable = new Dictionary<int, Dictionary<int,PersonalScore> >(); 
	bool influenceTableInitialised = false;


	// * * * * regular vars * * * * 
	protected bool staticLeadership = false; 
	protected Person[] leaders = new Person[2];
	protected string leadershipType; //valid values: monopoly, duopoly & anarchy
	/** key = person's id
	 */
	protected Dictionary<int,Person> personIndex = new Dictionary<int,Person>();


	// * * * * get/set * * * * 

	public bool StaticLeadership{get{return staticLeadership;}set{staticLeadership=value;}}
	public Person[] Leaders{get{return leaders;}} 
	public Dictionary<int,Person> PersonIndex{get{return personIndex;}set{personIndex=value;}}
	public string LeadershipType{get{return leadershipType;}}
	public Dictionary<int, Dictionary<int,PersonalScore> > GlobalInfluenceTable{get{return globalInfluenceTable;}}





	// * * * * * constructors * * * * * *
	public SocialStructure (){

	}



	// * * * * * Debug & Logging * * * * * 
	
	void printTable(float[,] aTable){
		for(int i=0;i<5;){
			Debug.Log("Row " + (i+1) + ":  "  + aTable[i,0] + "  |  " + aTable[i,1] + "  |  "  + aTable[i,2] + "  |  "  + aTable[i,3] + "  |  "  + aTable[i,4]);
			i++;
		}
	}
	
	void printArray(float[] anArray){
		Debug.Log("Result: "  + anArray[0] + "  |  " + anArray[1] + "  |  "  + anArray[2] + "  |  "  + anArray[3] + "  |  "  + anArray[4]);
	}

	// * * * * * Influence & leadership * * * * * *


	/** sets Leaders & LeadershipType, gets personIndex & influenceTable if !staticLeadership
	*/
	public void computeLeaders(){
		if (!StaticLeadership){
			if (personIndex.Count > 1){ //checks if there's more than 1 person in group
				TabledPersonValue[] scoresArray = aggregateInfScores(personIndex);
				if(personIndex.Count >2){ 
					TabledPersonValue[] leaderResults = calculateHighestScores(scoresArray);
					if(leaderResults[0].Value > leaderResults[1].Value * leadRatioThreshold && leaderResults[0].Value - leadRatioThreshold >= leaderResults[1].Value){
						setMonopoly(leaderResults[0].ThePerson);
						//since you know [0] > [1] is true from calculateHighestScores(), the other remaining options are either a duopoly or anarchy

					}else if(leaderResults[1].Value > leaderResults[2].Value * leadRatioThreshold && leaderResults[1].Value - leadRatioThreshold >= leaderResults[2].Value){
						Person[] twoLeaders = new Person[2]{leaderResults[0].ThePerson,leaderResults[1].ThePerson};
						setDuopoly(twoLeaders);
					}else{
						setAnarchy();
					}
				}else{
					if(scoresArray[0].Value > scoresArray[1].Value * leadRatioThreshold && scoresArray[0].Value - leadAbsoluteThreshold >= scoresArray[1].Value){
						setMonopoly(scoresArray[0].ThePerson);
					}else if( scoresArray[1].Value > scoresArray[0].Value * leadRatioThreshold && scoresArray[1].Value - leadAbsoluteThreshold >= scoresArray[0].Value){
						setMonopoly(scoresArray[1].ThePerson);
					}else{
						setAnarchy();
					}
				}
			}else{ // for a single group member
				setAnarchy();
			}		
		}else if(Town.debugMode){
			Debug.Log("Compute leaders hasn't returned a new leader since StaticLeadership is enabled for" + this.GetType());
		}
	}

	/** Only to be used with townPersonDatabase 
	 * An initialisation function, creates an influenceTable: Dictionary<int,Dictionary<int,PersonalScore>> 
	 * from the input Dictionary<int,Person>
	 */
	public void generateGlobalInfluenceTable(Dictionary<int,Person> grpMembers){
		if(influenceTableInitialised){
			Debug.LogError("influenceTable may not be initialised a second time");
		}else if(this.GetType() == typeof(SocialStructure)){
			Dictionary<int, Dictionary<int,PersonalScore> > result = new Dictionary<int, Dictionary<int,PersonalScore> >();
			foreach(KeyValuePair<int,Person> aValPair in grpMembers){
				Dictionary<int,PersonalScore> aDictionary = new Dictionary<int, PersonalScore>(); //create 
				foreach(KeyValuePair<int,Person> aSecondaryValPair in grpMembers){
					if(aValPair.Key != aSecondaryValPair.Key){ //avoiding recording of self referencing values
						Person otherPerson = aSecondaryValPair.Value;
						Person iPerson = aValPair.Value;
						float infScore = iPerson.calculateInfluence(otherPerson);
						PersonalScore singleScore = new PersonalScore(iPerson,otherPerson,infScore,Time.timeSinceLevelLoad);
						aDictionary.Add(aSecondaryValPair.Key,singleScore);
					}
				}
				result.Add(aValPair.Key,aDictionary);
			}
			globalInfluenceTable = result;
			influenceTableInitialised = true;
		}else{
			Debug.LogError("updateGlobalInfluenceTable should only be used with the base class SocialStructure");
		}
	}

	/** Should only be used with townPersonDatabase!
	 * This is a specific update function and uses an additional Dictionary<int,Person> as Input
	 * It checks all PersonalScores and if they've been updated
	 * ->Use true as input to update all scores.
	 * Gets the influenceTable attached to this class
	 */
	public void updateGlobalInfluenceTable(Dictionary<int,Person> updateThesePersons, bool updateAll){
		if(this.GetType() == typeof(SocialStructure)){ 
			Dictionary<int, Dictionary<int,PersonalScore> > currentInfTable = globalInfluenceTable;
			foreach(KeyValuePair<int,Person> affectingKVPair in personIndex){
				Dictionary<int,PersonalScore> aDictionary = new Dictionary<int,PersonalScore>(); //getting all affected inf Scores of Person
				if(!currentInfTable.TryGetValue(affectingKVPair.Key,out aDictionary)){
					Debug.LogError("failed to get Dictionary from affecting pair ID in updateGlobalInfluenceTable()");
				}else{
					foreach(KeyValuePair<int,Person> affectedKVPair in personIndex){ //getting a specific influence score
						if(affectedKVPair.Key != affectingKVPair.Key){ //avoiding self reference
							bool update = false;
							PersonalScore aPScore;
							if(!aDictionary.TryGetValue(affectedKVPair.Key,out aPScore)){
								Debug.LogError("Failed to get PersonalScore from affected pair ID in updaeGlobalInfluenceTable()");
							}else{
								if(aPScore.AffectingPerson.IdentityNum != affectingKVPair.Key){
									Debug.LogError("Authentication of affectingPerson in PersonalScore failed while executing updateGlobalInfluenceTable()");
								}else{
									if(Time.timeSinceLevelLoad - aPScore.LastCalculated > updateInfInterval || updateAll){ 
										update = true;
									}else if(updateThesePersons.ContainsKey(aPScore.AffectingPerson.IdentityNum) || updateThesePersons.ContainsKey(aPScore.AffectedPerson.IdentityNum)){
										update = true;
									}
									if (update){ //the actual update()
										aPScore.changeScore(affectingKVPair.Value.calculateInfluence(affectedKVPair.Value),Time.timeSinceLevelLoad);
									}
								}
							}
						}
					}
				}
			}
			globalInfluenceTable = currentInfTable;
		}else{
			Debug.LogError("updateGlobalInfluenceTable() should only be used with the base class SocialStructure");
		}
	}

	/** Should only be used with townPersonDatabase!
	 * This is the general update function.
	 * It checks all PersonalScores and if they've been updated
	 * ->Use true as input to update all scores.
	 * Gets the influenceTable attached to this class
	 */
	public void updateGlobalInfluenceTable(bool updateAll){ 
		if(this.GetType() == typeof(SocialStructure)){ 
			Dictionary<int, Dictionary<int,PersonalScore> > currentInfTable = globalInfluenceTable;
			foreach(KeyValuePair<int,Person> affectingKVPair in personIndex){
				Dictionary<int,PersonalScore> aDictionary = new Dictionary<int,PersonalScore>(); //getting all affected inf Scores of Person
				if(!currentInfTable.TryGetValue(affectingKVPair.Key,out aDictionary)){
					Debug.LogError("failed to get Dictionary from affecting pair ID in updateGlobalInfluenceTable()");
				}else{
					foreach(KeyValuePair<int,Person> affectedKVPair in personIndex){ //getting a specific influence score
						if(affectedKVPair.Key != affectingKVPair.Key){ //avoiding self reference
							PersonalScore aPScore;
							if(!aDictionary.TryGetValue(affectedKVPair.Key,out aPScore)){
								Debug.LogError("Failed to get PersonalScore from affected pair ID in updaeGlobalInfluenceTable()");
							}else{
								if(!ManipulatorsUtils.checkPersonalScoreConnected(aPScore,affectedKVPair.Value,affectedKVPair.Value)){
									Debug.LogError("Authentication of affectingPerson in PersonalScore failed while executing updateGlobalInfluenceTable()");
								}else{
									if(Time.timeSinceLevelLoad - aPScore.LastCalculated > updateInfInterval || updateAll){ 
										aPScore.changeScore(affectingKVPair.Value.calculateInfluence(affectedKVPair.Value),Time.timeSinceLevelLoad); //the actual update()
									}
								}
							}
						}
					}
				}
			}
			globalInfluenceTable = currentInfTable;
		}else{	
			Debug.Log("updateGlobalInfluenceTable() should only be used with the base class SocialStructure");
		}
	}

	/** Aggregate Infuence Scores
	 * Gets the scores of thesePersons from 2D Dictionary refInfTable. /n
	 * It then iterates the Inner Dictionary, combining all PersonalScore.getScore 
	 * and outputs them in an array of custom class TabledPersonValue /n
	 */
	TabledPersonValue[] aggregateInfScores(Dictionary<int,Person> thesePersons){
		//gets appropriate scores from refInfTable, and aggregates them
		TabledPersonValue[] result = new TabledPersonValue[thesePersons.Count];
		int i =0; //for result[i]
		foreach(KeyValuePair<int,Person> affectingKeyVal in thesePersons){ //iteration of affecting Person
			Dictionary<int,PersonalScore> aDictionary = new Dictionary<int, PersonalScore>();
			float iValue = 0;
			if(Town.TownPeopleDatabase.GlobalInfluenceTable.TryGetValue(affectingKeyVal.Key,out aDictionary)){ //sets aDictionary
				foreach(KeyValuePair<int,Person> affectedKeyVal in thesePersons){ //iterates affected Person
					if(affectingKeyVal.Key != affectedKeyVal.Key){ //ignores self-reference
						PersonalScore aScore;
						if(aDictionary.TryGetValue(affectedKeyVal.Key,out aScore)){ //gets the Personal score
							if(ManipulatorsUtils.checkPersonalScoreConnected(aScore,affectingKeyVal.Value,affectedKeyVal.Value) ){ //authentication
								iValue += aScore.getScore; //aggregating scores
							}
						}else{
							Debug.LogError("TryGetValue() of affected Person in aggregateInfScore failed");
						}
					}
				}
			}else{
				Debug.LogError("TryGetValue() of affecting Person in aggregateInfScores failed");
			}
			TabledPersonValue iResult = new TabledPersonValue(iValue,affectingKeyVal.Value);
			result[i]=iResult;
			i++; //update for result[i]
		}
		return result;
	}


	TabledPersonValue[] calculateHighestScores(TabledPersonValue[] inputArray){
		//returns an array containing the 2 Persons with the highest scores and the 3 highest score with null as its Person pointer
		if (inputArray.Length < 2){ 
			Debug.LogError("An array of length<2 has been passed to calculateLeadersFromArray, this is not allowed");
			return null;
		}else{ 
			TabledPersonValue highest = inputArray[0];
			TabledPersonValue secondHighest = inputArray[1];
			TabledPersonValue thirdHighest = inputArray[2];

			if(highest.Value < secondHighest.Value){ //checks first two values
				highest = inputArray[1];
				secondHighest = inputArray[0];
			}
			for(int i=2;i<inputArray.Length;){ //starts at 2, to check thirdLargest
				if(thirdHighest.Value <= inputArray[i].Value){ 
					if(secondHighest.Value < inputArray[i].Value){ //[i] is higher than current 2nd highest
						if(highest.Value < inputArray[i].Value){ //[i] is the highest, move both highest and 2nd Highest back a rank.
							thirdHighest = secondHighest;
							secondHighest = highest;
							highest = inputArray[i];

						}else{ // replace 2nd highest with i
							thirdHighest = secondHighest;
							secondHighest=inputArray[i];
						}
					}else{ //secondLargest > [i] > thirdLargest; Since you don't need to get the third largest person, only the value is changed for comparison
						thirdHighest = inputArray[i];
					}
				}
				i++;
			}
			TabledPersonValue[] result = new TabledPersonValue[3]{highest,secondHighest,thirdHighest};
			return result;
		} 
	}

	/**Replaces the current leader/s, setting their appropriate leadership bool to false and sets both leaders[] to null;
	 */
	void setAnarchy(){ 
		if(leaders[0] != null){
			leaders[0].PersonalStats.removeLeadership(this.GetType());
			leaders[0] = null;
		}
		if(leaders[1] != null){
			leaders[1].PersonalStats.removeLeadership(this.GetType());
			leaders[1] = null;
		}
		leadershipType = "anarchy";
	}

	/**Replaces the current leader/s, setting their appropriate leadership bool to false.
	 * Sets the input Person as leader, modifying its appropriate leadership bool to true. 
	 * sets leaders[2] to null;
	 */
	void setMonopoly(Person aPerson){

		if(leaders[0] != null){
			if(leaders[0] = aPerson){
				//do nothing
			}else{
				leaders[0].PersonalStats.removeLeadership(this.GetType());

				leaders[0] = aPerson;
			}
		}else{
			aPerson.PersonalStats.assignLeadership(this.GetType());
			leaders[0] = aPerson;
		}
		if(leaders[1] != null){
			leaders[1].PersonalStats.removeLeadership(this.GetType());
			leaders[1] = null;
		}
		leadershipType = "monopoly";
	}

	/**Replaces the current leader/s, setting their appropriate leadership bool to false.
	 * Sets the twoPeople input as leaders, modifying their appropriate leadership bool to true. 
	 */
	void setDuopoly(Person[] twoPeople){
		for(int i=0; i <2;i++){
			if(leaders[i] != null){
				if(leaders[i] = twoPeople[i]){
					//do nothing
				}else{
					leaders[i].PersonalStats.removeLeadership(this.GetType());
					twoPeople[i].PersonalStats.assignLeadership(this.GetType());
					leaders[i] = twoPeople[i];
				}
			}else{
				twoPeople[i].PersonalStats.assignLeadership(this.GetType());
				leaders[i] = twoPeople[i];
			}
		}
		leaders = twoPeople;
		leadershipType = "duopoly";
	}

	/** Gets the inluenceTable attached to this instance. 
	 * Should be used with townPersonDatabase 
	 */
	public PersonalScore getAnInfluenceScore(int affectingPersonID, int affectedPersonID){
		Dictionary<int,PersonalScore> scoresDict=new Dictionary<int, PersonalScore>();//placeholder to be populated with the Dictionary you get from influenceTable
		if(!globalInfluenceTable.TryGetValue(affectingPersonID,out scoresDict)){ 
			Debug.LogError("affecting person's ID not found in influence table");
			return null;
		}else{
			PersonalScore result = new PersonalScore(); 
			if(!scoresDict.TryGetValue(affectedPersonID,out result)){
				Debug.Log("affected person's ID not found in influence table");
				return null;
			}else{
				if(!ManipulatorsUtils.checkPersonalScoreConnected(result,affectedPersonID,affectedPersonID)){ //authentication
					Debug.LogError("authenctication failed in getAnInfluenceScore()");
					return null;
				}else{ //everything is okay
					return result;
				}
			}
		}
	}



	// * * * * * GROUP MEMBER MANAGEMENT * * * * *

	/**Creates a new Dictionary<int,Person> using persons' IDs. 
	 */
	public Dictionary<int,Person> populatePersonsIndex(List<Person> personsList){
		Dictionary<int,Person> result = new Dictionary<int, Person>();
		foreach(Person aPerson in personsList){
			int idNum = aPerson.IdentityNum;
			if(!result.ContainsKey(idNum)){
				result.Add(idNum,aPerson);
			}else{
				Debug.LogError("populatePersonsIndex() was passed a List<Person> containing Persons with identical ID numbers");
			}
		}
		return result;
	}
}