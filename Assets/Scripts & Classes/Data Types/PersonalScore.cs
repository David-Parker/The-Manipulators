/** Used to store a calculation of a person's effect over another 
 * It's different than TabledPersonValue by:
 * 	a. that it saves both of the persons in the exchange 
 * 	b. the time when the calculation was made
 */
using UnityEngine;

public class PersonalScore {

	Person affectingPerson;
	Person affectedPerson;
	float score;
	float lastCalculated;
	
	public Person AffectingPerson{get{return affectingPerson;}}
	public Person AffectedPerson{get{return affectedPerson;}}
	public float LastCalculated{get{return lastCalculated;}}
	
	public float getScore{get{return score;}}
	public float getInverseScore(){return -score;}
	
	// * * * * constructors  * * * * * 

	public PersonalScore(Person affecting,Person affected,float aScore, float currentGameTime){
		if(affecting == affected){
			Debug.LogError("A creation of a self referencing PersonalScore has been denied");
		}else{
			affectingPerson = affecting;
			affectedPerson =affected;
			score = aScore;
			lastCalculated = currentGameTime;
		}
	}

	public PersonalScore(){

	}


	// * * * * * * * functions * * * * * * * * 

	
	public void changeScore(float newScore, float currentGameTime){
		score = newScore;
		lastCalculated = currentGameTime;
	}

}
