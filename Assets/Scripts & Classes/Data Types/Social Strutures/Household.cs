using UnityEngine;
using System.Collections.Generic;


public class Household : SocialStructure {

	House attachedToHouse;
	public House AttachedToHouse{get{return attachedToHouse;}set{attachedToHouse = value;}}

	public Household(House attachedToThisHouse, Dictionary<int,Person> hMembers){
		attachedToHouse = attachedToThisHouse;
		base.personIndex = hMembers;
	}
}