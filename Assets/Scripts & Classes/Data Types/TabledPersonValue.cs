/*
 * This class is for storage of a Person and a float value, best for aggregated values or calculations.
 * Use PersonalScore to save effect of one specific person on another specific Person
 * 
 */


public class TabledPersonValue {

	float theValue;
	Person thePerson;

	public float Value{get{return theValue;}set{theValue=value;}}
	public Person ThePerson{get{return thePerson;}set{thePerson=value;}}


	public TabledPersonValue(float inputValue, Person inputPerson){
		theValue = inputValue;
		thePerson = inputPerson;
	}

}
