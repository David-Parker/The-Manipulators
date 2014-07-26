using UnityEngine;
using System.Collections.Generic;


/* This class (and its subclasses) contains all information there is to discover about the event, and variables on how normally it would affected a Person().
 * 
 * FIXME
 * Types to create:
 * Murder , Tresspassing , assault , Fabricated rumours , theft, warrant, suspect/allegation, jailed, arson, sabotage, eavesdropping, alien, absence, conviction,
 * work performance,
 * SavedSomeone (bonus mainly to saved person), isOfficial, isLeader, marriedTo, dating, family, child, hasJob,
 * Types are used to make sure duplicate events are not created (TrustEvents are verified differently according to their type)
 */

/** Stores a unique trust event's details
 */
public class SocialEvent {

	protected int id; //makes sure all events are unique and are not overwritten OR duplicated
	protected int trustImpact; //assinged in child constructors
	protected int globalSuspicionImpact;
	protected float timeOccured;

	public SocialEvent (int id_, float currentGameTime){
		id = id_;
		timeOccured = currentGameTime;
	}

	public SocialEvent (int id_){
		id = id_;
	}

	public int ID {get{return id;}}
	public int TrustImpact {get{return trustImpact;}}
	public float TimeOccured {get{return timeOccured;}}
}


