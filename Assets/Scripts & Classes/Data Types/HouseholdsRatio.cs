using UnityEngine;

/** Generates a projected exact number by dividing the totals by the percentages assigned in Settings()
*/
public class HouseholdsRatio{

	public HouseholdsRatio(int numHouseholds){
		totalHouseholds = numHouseholds;
	}

	bool ratioGenerated = false;
	int totalHouseholds;
	int seniorHouseholds;
	int friendsHouseholds;
	int lonerHouseholds;
	int familiesHouseholds;

	public void generateRatio(){
		int seniorPercent = Settings.HouseholdSeniorsPercent;
		int friendPercent = Settings.HouseholdFriendsPercent;
		int lonePercent = Settings.HouseholdLonersPercent;

		if(!Settings.DisableRandomisation){
			seniorPercent *= (int)Random.Range(0.5f,1.5f);
			friendPercent *= (int)Random.Range(0.5f,1.5f);
			lonePercent *= (int)Random.Range(0.5f,1.5f);
		}
		seniorHouseholds = totalHouseholds * seniorPercent / 100;
		friendsHouseholds = totalHouseholds * friendPercent /100;
		lonerHouseholds = totalHouseholds * lonePercent / 100;
		if(!Settings.DisableMinimums){
			if(seniorHouseholds < 3){
				seniorHouseholds =2;
			}
			if(friendsHouseholds < 3){
				friendsHouseholds =2;
			}
			if(lonerHouseholds < 2){
				lonerHouseholds =1;
			}
		}
		familiesHouseholds = totalHouseholds - seniorHouseholds - friendsHouseholds - lonerHouseholds;
		ratioGenerated = true;
	}

	public int TotalHouseholds{get{return totalHouseholds;}}
	public int SeniorHouseholdsNum{
		get{
			if(ratioGenerated){
				return seniorHouseholds;
			}else{
				Debug.LogError("generateRatio() must be run before HouseholdRatio data is accessed");
				return 0;
			}
		}
	}
	public int FamilyHouseholdsNum{
		get{
			if(ratioGenerated){
				return familiesHouseholds;
			}else{
				Debug.LogError("generateRatio() must be run before HouseholdRatio data is accessed");
				return 0;
			}
		}
	}
	public int LonerHouseholdsNum{
		get{
			if(ratioGenerated){
				return lonerHouseholds;
			}else{
				Debug.LogError("generateRatio() must be run before HouseholdRatio data is accessed");
				return 0;
			}
		}
	}
	public int FriendsHouseholdsNum{
		get{
			if(ratioGenerated){
				return friendsHouseholds;
			}else{
				Debug.LogError("generateRatio() must be run before HouseholdRatio data is accessed");
				return 0;
			}
		}
	}
}
