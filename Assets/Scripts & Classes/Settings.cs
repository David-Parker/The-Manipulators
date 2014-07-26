/** Centralises gameplay affecting vars in a single class for easy customising.
 * Except for UseCamera.cs
 * 
 */
public static class Settings{



	static class Demographics_{
		public static int gayPercentage=3;
		public static int minoritiesPercentage=12;
		public static int divorcedPercentage = 5;
		public static float teenChildRatio = 0.75f; //Teens are more complex and add more interest to the game than children.
		public static float adultsTeensLivingWithSeniors = 0.1f;
		public static float singleParentsNonSeniors = 0.05f;
		public static float adultSiblingsSharingHouse = 0.1f;
		public static int avgFamConnections= 3; //per household
	}	
	public static int GayPercentage{get{return Demographics_.gayPercentage;}}
	public static int MinoritiesPercentage{get{return Demographics_.minoritiesPercentage;}}
	public static int DivorcedPercentage{get{return Demographics_.divorcedPercentage;}}
	/** Range: 0 < x < 1
	 *  x = probability of teens, more teens makes a better game.
	 * Default: high
	 */
	public static float TeenChildRatio{get{return Demographics_.teenChildRatio;}}
	/** Range: 0 < x < 1
	 * Default: low
	 */
	public static float SeniorsLivingWithOffspring{get{return Demographics_.adultsTeensLivingWithSeniors;}}
	/** Range: 0 < x < 1
	 * Default: low
	 */
	public static float SingleParentsNonSeniors{get{return Demographics_.singleParentsNonSeniors;}}
	/** Range: 0 < x < 1
	 * Default: low
	 */
	public static float AdultSiblingsSharingHouse{get{return Demographics_.adultSiblingsSharingHouse;}}
	public static int AverageFamilyConnectionsPerHousehold{get{return Demographics_.avgFamConnections;}}




	static class HouseholdRatio_{
		public static int avgFamRelsHouseH=4;
		public static float famRelDistribution=0.5f;
		public static bool disableRandomisation=false;
		public static bool disableMinimums=false;
		public static int seniorPercentage=15;
		public static int friendsPercentage=10;
		public static int lonersPercentage=5; //loners may also be seniors (randomisation function conserves senior/total ratio);
		//families percentage is the difference between the total and removing the rest of the groups
	}
	public static int AverageFamilyRelPerHousehold{get{return HouseholdRatio_.avgFamRelsHouseH;}}
	/** 0 < x < 1
	 * x=1 : complete monopolisation of family relations by a single household (usually impossible if average connection number is not low)
	 * x=0 : perfect equality in connection randomisation
	 */
	public static float FamilyRelationsDistribution{get{return HouseholdRatio_.famRelDistribution;}}
	public static bool DisableMinimums{get{return HouseholdRatio_.disableMinimums;}}
	public static bool DisableRandomisation{get{return HouseholdRatio_.disableRandomisation;}}
	public static int HouseholdSeniorsPercent{get{return HouseholdRatio_.seniorPercentage;}}
	public static int HouseholdFriendsPercent{get{return HouseholdRatio_.friendsPercentage;}}
	public static int HouseholdLonersPercent{get{return HouseholdRatio_.lonersPercentage;}}
	public static int HouseholdFamiliesPercent{ //FIXME: may be used in settings customisation, but is useless for now.
		get{
			return 100-HouseholdRatio_.seniorPercentage-HouseholdRatio_.friendsPercentage-HouseholdRatio_.lonersPercentage;
		}
	}




	static class Time_{
		public static float dayLength = 1080f;
	}
	public static float DayLength{get{return Time_.dayLength;}}





	static class SocialEvent_{
		public static int maxCred = 100;
	}
	public static int MaxSocEventCred{get{return SocialEvent_.maxCred;}}





	static class Evidence_{ //FIXME expand
		//Witness always gets max value.
		//Some evidence is only noted and not defined here, as it uses the max or min values automatically.

		// DEFAULT CREDIBILITY IMPACT FOR EVIDENCE 
		public static int maxCred = 10;
		public static int minCred = 1;
		public static int footPrintsCred = 5;
		public static int fingerPrintsCred = 9;
		//dead body does not affect murder cred, the method of killing does, embedded in a seconday evidence instance
		public static int maxBelievability = 10;
	}
	public static int MaxEvidenceCred{get{return Evidence_.maxCred;}}
	public static int MinEvidenceCred{get{return Evidence_.minCred;}}
	public static int FingerPrintsCred{get{return Evidence_.fingerPrintsCred;}}
	public static int FootPrintsCred{get{return Evidence_.footPrintsCred;}}
	public static int MaxBelievability{get{return Evidence_.maxBelievability;}}





	static class Integrity{ //FIXME: expand

		// DEFAULT SOCIAL EVENT INTEGRITY IMPACT 
		//Note that a lot of offences necessitate other  breaches

		public static int max = 100;
		public static int min = -100; 

		public static int murder = -90;
		public static int theft = -30;
		public static int tresspass= -15;
		public static int breakIn = -10; //top up to tresspass
		public static int vandalism = -15;
		public static int assault = -30;
		public static int aggrAssault = -40;
	}
	public static int MurderIntegrity{get{return Integrity.murder;}}





	static class Suspicion{ //FIXME expand

		// DEFAULT SOCIAL EVENT SUSPICION IMPACT
		public static int max =100;
		public static int min =0;

		public static int murder=10;
		public static int unsolvedMurder = 15;
	}




	static class Person_{
		public static int maxStat = 20;
		public static int minStat = 1;
		public static int maxOpinionRange = 100; //absolute value used as highest and and its *(-1) used as lowest
	}
	public static int HumanMaxStat{get{return Person_.maxStat;}}
	public static int HumanMinStat{get{return Person_.minStat;}}
	/** Absolute number used for both highest and lowest limit with changed sign
	 */
	public static int PersonMaxOpinion{get{return Person_.maxOpinionRange;}}
}