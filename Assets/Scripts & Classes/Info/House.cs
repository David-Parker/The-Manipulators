using UnityEngine;
using System.Collections.Generic;

public class House : Building{

	public bool abandonedHouse = false; //A game designer may designate this house as abandoned so it doesn't generate errors for household creation
	Dictionary<int,Person> personsInHouse = new Dictionary<int,Person>(); //updated value that gets 
	Rect houseArea = new Rect();
	Household assocHousehold;


	// * * * * * * get/sets * * * * * * * *

	public Dictionary<int,Person> PersonsInHouse{get{return personsInHouse;}set{personsInHouse=value;}}
	public Rect HouseArea{get{return houseArea;}set{houseArea = value;}}
	public Household AssociatedHousehold{get{return assocHousehold;}set{assocHousehold = value;}}


	Dictionary<int,Person> whichPeopleAreInTheHouse(){
		//FIXME: write this w/ an trigger box
		return null;
	}


} 
