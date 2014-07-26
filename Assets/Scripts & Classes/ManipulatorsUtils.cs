using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Manipulators{

	public class ManipulatorsUtils {


		/**Shuffles a string[] using the Fisher-Yates method.
		 */
		public static string[] shuffleStringArray(string[] anArray){
			for(int i=0;i<anArray.Length-1;i++){
				int randomInt = Random.Range(i,anArray.Length);
				string swapValue = anArray[randomInt];
				anArray[randomInt] = anArray[i];
				anArray[i] = swapValue;
			}
			return anArray;
		}


		// * * * * GET COMPONENTS * * * *


		/** looks for a component in a GameObject, if unavailable, it calls findInParents() to search in parent GameObjects
		 */
		public static Component findAComponent (GameObject whichObject, int numOfHierarchies, string objectType) { 
			
			System.Type findThisType = System.Type.GetType(objectType);  // get objectType from string
			if (findThisType != null){ // check to see GetType() returned a type
				Component foundComponent = whichObject.GetComponent(findThisType); // gets component in current GameObject
				if ( foundComponent==null && numOfHierarchies != 0){ //component not found in current object, try parents
					foundComponent = findComponentInParents(whichObject,numOfHierarchies,objectType); //note string is passed
				}
				return foundComponent;
			}
			else{
				Debug.Log("invalid objectType passed to Player.getAComponent of string " + objectType);
				return null;
			}
			
		}

		/**
		 * Used by findAComponent()
		 */
		static Component findComponentInParents (GameObject whichChildObject, int numOfIterations, string componentType){
			GameObject parentObject;
			System.Type aType = System.Type.GetType(componentType);
			
			if(aType==null){ //checks if aType can be found
				return null;
			}else{
				for (int i=0; i < numOfIterations;){ //iterates by rising through hierarchy
					i++;
					if (whichChildObject.transform.parent.gameObject){ // check if gameObject has parent
						parentObject = whichChildObject.transform.parent.gameObject;
						Component foundComponent = parentObject.GetComponent(aType) ; //aType is the type matching the componentType string input for this f()
						if (foundComponent != null){ //Component found
							return foundComponent;
						}
					}else{ //check for parents failed
						Debug.Log("this object has no parent objects ");
						return null;
					}
				}
				return null; //if there were no iterations
			}
		}

		/**
		 * Gets component from a GameObject or its children, throws an error if component found in children
		 */
		static Component checkGOContainsObject(GameObject aGameObject, System.Type findThisType){
			Component gotComponent = aGameObject.GetComponent(findThisType);
			if(gotComponent != null){
				return gotComponent;
			}else{
				gotComponent = aGameObject.GetComponentInChildren(findThisType);
				if(gotComponent !=null){
					Debug.LogError("The object of Type " + findThisType + "was not placed directly in the Gameobject which is at location "+aGameObject.transform.position+", but instead in one of its children, please fix this");
					return gotComponent;
				}else{
					Debug.LogError("No object of Type " + findThisType + " exists in Gameobject at location "+aGameObject.transform.position);
					return null;
				}
			}
		}


		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
		// 							* * * * GET OBJECTS BY GAMEOBJECT TAG * * * *
		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-


		/** Returns all House() Objects contained in gameObjects with the tag "Houses"
		 */
		public static List<House> getAllHouses(){
			GameObject[] housesArray = GameObject.FindGameObjectsWithTag("Houses"); 
			if(housesArray.Length==0){
				Debug.LogError("No Gameobjects with tag \"Houses\" were found by Town.cs");
				return null;
			}else{
				List<House> gotHouses = new List<House>(); 
				foreach(GameObject aGO in housesArray){
					gotHouses.Add(checkGOContainsObject( aGO,typeof(House) ) as House);
				}
				return gotHouses;
		        }
		}                                    

		/** Returns a List<Person> of all Persons contained in GameObjects with the tag "Persons"
		 */
		public static List<Person> getAllPersons (){ 
			//finds all Gameobjects with tag 'Persons' and returns their Person() Component in an array
			//use with initialiseAllPersons
			GameObject[] anArrayOfGOs = GameObject.FindGameObjectsWithTag("Persons");
			if(anArrayOfGOs.Length == 0 || anArrayOfGOs == null){
				Debug.LogError("No GameObjects with tag \"Persons\" were found by Town.cs");
				return null;
			}else{

				List<Person> gotPersons = new List<Person>();
				foreach(GameObject aGO in anArrayOfGOs){
					gotPersons.Add(checkGOContainsObject( aGO,typeof(Person) ) as Person);
				}
				return gotPersons;
			}
		}

		/** Returns all Player() objects contained in Gameobjects with tag "Player" as a List<Player>
		 */
		public static List<Player> getAllPlayers (){ 
			//finds all players through the 'player' tag
			GameObject[] arrayOfGOs = GameObject.FindGameObjectsWithTag("Player");
			if(arrayOfGOs.Length == 0 || arrayOfGOs == null){
				Debug.LogError("No GameObjects with tag \"Player\" were found by Town.cs");
				return null;
			}else{
				List<Player> gotPlayers = new List<Player>();
				foreach(GameObject aGameObject in arrayOfGOs){

					gotPlayers.Add(checkGOContainsObject( aGameObject,typeof(Player) ) as Player);
				}
				return gotPlayers;
			}
		}




		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
		// 							* * * * FIND PERSON()S IN AREA * * * *
		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-


		/** Checks input Dictionary<int,Person> for People whose transform.position (x & z only, y is meaningless) is in the Rect provided. 
		 * Returns them as a Dictionary<int,Person>
		 */
		public static Dictionary<int,Person> checkWhichPersonsInArea (Rect theArea, Dictionary<int,Person> checkThesePeople){
			Dictionary<int,Person> result = new Dictionary<int,Person>();
			foreach(KeyValuePair<int,Person> aKeyVal in checkThesePeople){
				float xPos = aKeyVal.Value.transform.position.x;
				float zPos = aKeyVal.Value.transform.position.z;
				if( theArea.xMax > xPos && xPos > theArea.xMin && theArea.yMin > zPos && zPos > theArea.yMax){ //theArea.y is used/saved as z
					result.Add(aKeyVal.Key,aKeyVal.Value);
				}
				
			}
			return result;
		}

		/** Checks input List<Person> for People whose transform.position (x & z only, y is meaningless) is in the Rect provided. 
		 * Returns them as a List<Person>
		 */
		public static List<Person> checkWhichPersonsInArea (Rect theArea, List<Person> thesePersons){
			List<Person> result = new List<Person>();
			foreach(Person aPerson in thesePersons){
				float xPos = aPerson.transform.position.x;
				float zPos = aPerson.transform.position.z;
				if( theArea.xMax > xPos && xPos > theArea.xMin && theArea.yMax > zPos && zPos > theArea.yMin){ //theArea.y is used/saved as z
					result.Add(aPerson);
				}
			}
			return result;
		}


		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
		// 							* * * * FIND PERSON()S IN AREA * * * *
		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-


		/**Authenticates PersonalScore's affecting & affected person MATCH the input (affecting & affected)<Person>
	 	*/
		public static bool checkPersonalScoreConnected(PersonalScore checkThis,Person affectingP,Person affectedP){
			if(checkThis.AffectedPerson == affectedP){
				if(checkThis.AffectingPerson == affectingP){
					return true;
				}else{
					Debug.LogError("Personal score inconsistent with input");
					return false;
				}
			}else{
				Debug.LogError("Personal score inconsistent with input");
				return false;
			}
		}
		
		/**Authenticates PersonalScore's affecting & affected person MATCH the input (affecting & affected ID)<int>
		 */
		public static bool checkPersonalScoreConnected(PersonalScore checkThis,int affectingID,int affectedID){
			if(checkThis.AffectedPerson.IdentityNum == affectedID){
				if(checkThis.AffectingPerson.IdentityNum == affectingID){
					return true;
				}else{
					Debug.LogError("Personal score inconsistent with input");
					return false;
				}
			}else{
				Debug.LogError("Personal score inconsistent with input");
				return false;
			}
		}



		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
		// 				* * * * ADD/ REMOVE MULTIPLE PERSON OBJECTS FROM DICTIONARIES * * * *
		//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
		//****redundant????

		/** Logs an error if person already in Dictionary
		 */
		public static Dictionary<int,Person> addPersonsToGroup(Person[] personsToAdd, Dictionary<int,Person> currentDictionary){
			foreach(Person aPerson in personsToAdd){
				if(currentDictionary.ContainsKey(aPerson.IdentityNum)){ //Person found in index
					Debug.Log("warning: you are trying to add a Person that is already in the group");
				}else{ //Person not found in index, added
					currentDictionary.Add(aPerson.IdentityNum,aPerson);
				}
			}
			return currentDictionary;
		}

		/** Logs an error if person already in Dictionary
		 */
		public static Dictionary<int,Person> addPersonsToGroup(List<Person> personsToAdd, Dictionary<int,Person> currentDictionary){
			foreach(Person aPerson in personsToAdd){
				if(currentDictionary.ContainsKey(aPerson.IdentityNum)){ //Person found in index
					Debug.Log("warning: you are trying to add a Person that is already in the group");
				}else{ ///Person not found in index, added
					currentDictionary.Add(aPerson.IdentityNum,aPerson);
				}
			}
			return currentDictionary;
		}

		/** Logs an error if person already in Dictionary
		 */
		public static Dictionary<int,Person> addPersonsToGroup(Dictionary<int,Person> personsToAdd, Dictionary<int,Person> currentDictionary){
			foreach(KeyValuePair<int,Person> aKeyVal in personsToAdd){
				if(currentDictionary.ContainsKey(aKeyVal.Key)){ //Person found in index
					Debug.Log("warning: you are trying to add a Person that is already in the group");
				}else{ //Person not found in index, added
					currentDictionary.Add(aKeyVal.Key,aKeyVal.Value);
				}
			}
			return currentDictionary;
		}

		/** Logs an error if person isn't currently in Dictionary
		 */
		public static Dictionary<int,Person> removePersonsFromGroup(Person[] personsToRemove, Dictionary<int,Person> currentDictionary){
			foreach(Person aPerson in personsToRemove){
				if(currentDictionary.ContainsKey(aPerson.IdentityNum)){ //found person,removing from dictionary
					currentDictionary.Remove(aPerson.IdentityNum);
				}else{
					Debug.Log("warning: you are trying to remove a Person that is not in the group");
				}
			}
			return currentDictionary;
		}

		/** Logs an error if person isn't currently in Dictionary
		 */
		public static Dictionary<int,Person> removePersonsFromGroup(List<Person> personsToRemove, Dictionary<int,Person> currentDictionary){
			foreach(Person aPerson in personsToRemove){
				if(currentDictionary.ContainsKey(aPerson.IdentityNum)){ //found person,removing from dictionary
					currentDictionary.Remove(aPerson.IdentityNum);
				}else{
					Debug.Log("warning: you are trying to remove a Person that is not in the group");
				}
			}
			return currentDictionary;
		}

		/** Logs an error if person isn't currently in Dictionary
		 */
		public static Dictionary<int,Person> removePersonsFromGroup(Dictionary<int,Person> personsToRemove, Dictionary<int,Person> currentDictionary){
			foreach(KeyValuePair<int,Person> aKeyVal in personsToRemove){
				if(currentDictionary.ContainsKey(aKeyVal.Key)){ //found person,removing from dictionary
					currentDictionary.Remove(aKeyVal.Key);
				}else{
					Debug.Log("warning: you are trying to remove a Person that is not in the group");
				}
			}
			return currentDictionary;
		}
	}
}