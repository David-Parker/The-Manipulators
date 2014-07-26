using UnityEngine;
using System.Collections.Generic;


public class TestClass 
{
	static Dictionary<int, SocialEvent> aDictionary = new Dictionary<int, SocialEvent>();
	static int counter = 0;

	static SocialEvent addNewSocialEventToRegister(){
		SocialEvent anEvent = new SocialEvent(counter);
		aDictionary.Add(counter,anEvent);
		counter++;
		return anEvent;
	}

	static SocialEvent addNewSocialEventToRegister(float currentGameTime){
		SocialEvent anEvent = new SocialEvent(counter,currentGameTime);
		aDictionary.Add(counter,anEvent);
		counter++;
		return anEvent;
	}

	static Murder createMurder(Person perpetrator, Person deceased){
		SocialEvent anEvent = addNewSocialEventToRegister();
		Murder aMurder = new Murder(anEvent.ID,anEvent.TimeOccured,perpetrator,deceased);
		aDictionary[anEvent.ID] = aMurder;
		return aMurder;
	}

	public static void testThis(){
		Person a;
		Person b;
		if(!Town.TownPeopleDatabase.PersonIndex.TryGetValue(1,out a)){
			Debug.LogError("failed to get person a");
		}
		if(!Town.TownPeopleDatabase.PersonIndex.TryGetValue(2, out b)){
			Debug.LogError("failed to get person b");
		}
		addNewSocialEventToRegister();
		Murder aMurder = createMurder(a,b);

		SocialEvent c;
		if(!aDictionary.TryGetValue(1,out c)){
			Debug.Log("failed to get value of key 1");
		}else{
			Debug.Log("type of c is:"+c.GetType());
		}

		if(aDictionary.Count != 2){
			Debug.LogError("Invalid dictionary size");
		}

		foreach(KeyValuePair<int,SocialEvent> aKVP in aDictionary){
			Debug.Log("first SE type is: "+ aKVP.Value.GetType());
			if(aKVP.Value.GetType() == typeof(Murder)){
				Debug.Log("success");
			}else{
				Debug.Log("fail");
			}
		}

		
	}
}


