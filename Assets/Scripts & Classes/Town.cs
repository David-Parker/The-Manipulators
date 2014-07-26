using UnityEngine;
using System.Collections.Generic;
using Manipulators;


/** The root class of the game. Only 1 may exist.
 * Generates The town's layout
 * 
 * FIXME
 * PersonalStats generation
 * 
 * TODO
 * Regroup all family methods into a separate family class
 * Create bool returns in lower methods to abort link creations and attempts at salvaging.
 *
 */
public class Town : MonoBehaviour {

	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 						* * * * DEBUG MODE & TEMPORARY VARIABLES * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	bool runOnce =true;
	static public bool debugMode = false;


	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 							* * * * NAME DATABASES * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

	/**Has to be shuffled before use
	 */
	static public string[] lastNameDB = new string[]{ 
		"Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor", "Anderson", "Jackson", "Harris", "Thompson",
		"Robinson", "Clark", "Lewis", "Lee", "Walker", "Hall", "Allen", "Wright", "Hill", "Scott", "Green", "Adams", "Baker", "Nelson", "Carter",
		"Mitchell", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins", "Stewart", "Morris", "Rogers", "Reed", "Cook",
		"Morgan", "Bell", "Murphy", "Bailey", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Peterson", "Gray", "Watson", "Brooks","Kelly", "Price",
		"Wennet", "Wood", "Barnes", "Henderson", "Coleman", "Jenkins", "Perry", "Powell", "Long", "Patterson", "Hughes", "Washington", "Butler", "Simmons",
		"Foster", "Bryant", "Russell", "Hayes", "Myers", "Ford", "Hamilton", "Graham", "Sullivan", "Wallace", "Woods", "Cole", "West", "Owens", "Reynolds"
	};

	static public string[] firstNameFemaleDB = new string[]{
		"Mary","Patricia","Barbara","Elizabeth","Jennifer","Maria","Susan","Margaret","Dorothy","Lisa","Nancy","Karen","Betty","Helen","Sandra","Carol",
		"Ruth","Sharon","Michelle","Laura","Sarah","Kimberly","Deborah","Jessica","Shirley","Cynthia","Melissa","Amy","Anna","Rebecca","Kathleen","Martha",
		"Amanda","Stephanie","Carolyn","Christine","Marie","Janet","Catherine","Ann","Alice","Julie","Heather","Gloria","Evelyn","Cheryl","Ashley","Rose",
		"Nicole","Judy","Christina","Kathy","Beverly","Irene","Jane","Tammy","Lori","Louise","Anne","Julia","Lois","Tina","Emma","Emily","Connie","Grace",
		"Victoria","Josephine","Carrie","Charlotte","Monica","Anita","Amber","Eva","April","Clara","Leslie","Joanne","Eleanor","Danielle","Megan","Alicia",
		"Suzanne","Veronica","Jill","Britney","Bridget","Erin","Lauren","Cathy","Sally","Regina","Erica","Audrey","Vivian","Holly","Katie","Beth","Jeanne",
		"Naomi","Nina","Jennie","Becky","Violet","Courteney","Ramona","Daisy","Lindsey","Natasha","Rosie","Iris","Amelia","Alison","Olivia","Candice","Roxanne",
		"Robyn","Rachel","Angelina","Estelle","Hope","Rochelle","Sophie","May","Faith","Elisa","Francine","Tracie","Elisabeth","Caitlin","Fay","Haley","Hillary"
	};

	static public string[] firstNameMaleDB = new string[]{
		"James","John","Robert","Michael","William","David","Richard","Charles","Joseph","Thomas","Christopher","Daniel","Paul","Mark","Donald","George","Kenneth",
		"Steven","Edward","Brian","Anthony","Kevin","Jason","Matthew","Gary","Timothy","Jeffrey","Frank","Eric","Stephen","Andrew","Joshua","Walter","Patrick",
		"Peter","Harold","Douglas","Henry","Carl","Arthur","Ryan","Roger","Joe","Jack","Albert","Jonathan","Justin","Terry","Gerald","Keith","Samuel","Ralph",
		"Lawrence","Nicholas","Roy","Benjamin","Bruce","Brandon","Adam","Harry","Fred","Billy","Steve","Louis","Jeremy","Aaron","Howard","Jesse","Craig","Alan",
		"Shawn","Sean","Chris","Johnny","Jimmy","Bryan","Tony","Mike","Stanley","Dale","Nathan","Curtis","Marvin","Vincent","Jeffery","Jacob","Kyle","Francis",
		"Bradley","Frederick","Ray","Eddie","Randall","Jay","Jim","Tom","Calvin","Alex","Jon","Bill","Lloyd","Leon","Derek","Leo","Dean","Greg","Sam","Rick",
		"Charlie","Tyler","Raul","Ben","Franklin","Brad","Ron","Harvey","Jared","Adrian","Karl","Erik","Jamie","Neil","Ted","Mathew","Cody","Kurt","Hugh","Max",
		"Ian","Ken","Bob","Dave","Julian","Andy","Kirk","Seth","Sebastian","Eduardo","Freddie","Austin","Stuart","Frederick","Joey","Nick","Evan","Trevor","Oliver"
	};

	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 							* * * * CHECKING FOR ERRORS * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	static int numberOfTownInstances = 0;
	static bool householdsCreated = false;
	static bool familiesCreated = false;

	public static bool FamiliesCreated{get{return familiesCreated;}} 



	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 							* * * * MAIN VARIABLES * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

	static SocialStructure townPeopleDatabase = new SocialStructure();
	static PersonStatsGroup activePersonStatsCollection;

	public static SocialStructure TownPeopleDatabase{get{return townPeopleDatabase;}}
	public static PersonStatsGroup ActivePersonStatsGroup{get{return activePersonStatsCollection;}}



	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 							* * * * INDEXES * * * * 
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-


	static List<House> allHouses = new List<House>();
	static List<Player> allPlayers = new List<Player>(); 
	static List<Household> allHouseholds = new List<Household>();
	static List<Workplace> allWorkplaces = new List<Workplace>();
	static List<Clique> allCliques = new List<Clique>();
	static List<SocialEvent> allSocialEvents = new List<SocialEvent>();
	/** key = murders num
	 */
	static Dictionary<int, Murder> allMurders = new Dictionary<int, Murder>();
	/** key = evidence num
	 */
	static Dictionary<int, Evidence> allEvidence = new Dictionary<int, Evidence>();
	/** key = series num
	 */
	static Dictionary<int, PersonStatsGroup> allPersonStatsCollections = new Dictionary<int, PersonStatsGroup>();

	static int socialEventCounter = 0;
	static int evidenceCounter = 0;
	static int lastNameCounter = 0;
	static int personStatsVersionCounter = 0;

	static float maleGenerationChance = 0.5f; // == (1 - femaleGenerationChance)
	static int divorcedCounter = 0;
	static int nonDivorcedCounter = 0;


	// 	* * * * indexes' get  * * * * *
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

	public static List<Player> AllPlayers{get{return allPlayers;}}
	public static List<Household> AllHouseholds{get{return allHouseholds;}}
	public static List<House> AllHouses{get{return allHouses;}}
	public static List<Workplace> AllWorkplaces {get{return allWorkplaces;}}
	public static List<Clique> AllCliques{get{return allCliques;}}



	// * * * * * Time Vars * * * * *
	//For reference: 3600s / h ; 86400s / d = realtime
	//Current game time speed: 45s / h ; 1080s / d ; at a ration of 80x to realtime
	//Time approach #1 for single player: dilation for tactics & regular 'timelapse' time for strategic action.
	//Time approach #2 for single player: Warfare RTS (SC, RA, etc), the player is always engaged (except maybe a int time at the start).
	static float dayLength = Settings.DayLength;


	/** Returns a the time passed since the day started, adjusted for current timeScale
	 */
	public static float RealSecsToday{
		get{
			return Mathf.Round(Time.timeSinceLevelLoad % dayLength * Time.timeScale);
		}
	}

	/** Returns the current game length adjusted for current timeScale
	 */
	public static float DayLength{
		get{
			return dayLength * Time.timeScale;
		}
	}

	/**Returns total num of hours passed today
	 */
	public static float HoursPassedToday{
		get{
			return Town.RealSecsToday / (Town.DayLength /24);
		}
	}

	/**Returns total num of minutes passed today
	 */
	public static float MinutesPassedToday{
		get{
			return Town.RealSecsToday / (Town.DayLength/24/60);
		}
	}

	/** Returns the current Hours
	 */
	public static float ClockHours{
		get{
			return HoursPassedToday - HoursPassedToday %1; //Rounds down
		}
	}

	/** Returns the current Minutes 
	 */
	public static float ClockMinutes{
		get{
			//rounds down & substracts the total minutes passed,leaving only the minutes passed this hour
			return MinutesPassedToday - ClockHours*60 - MinutesPassedToday %1;
		}
	}

	/** Takes in a float and returns it as a string.
	 * if input < 10, adds a 0 digit to the string returned
	 */
	static string addZeroDigitIfNeeded(float timeF){
		if(timeF > 59){
			Debug.LogError("Invalid input of "+timeF+" has been passed");
		}
		if(timeF<10){
			return "0"+timeF.ToString();
		}else{
			return timeF.ToString();
		}
	}

	/** Returns a string of in-game time in format HH:MM 
	 */
	public static string ClockTime24h{
		get{
			return addZeroDigitIfNeeded(ClockHours)+":"+addZeroDigitIfNeeded(ClockMinutes);
		}
	}

	/** Returns a string of in-game time in format HH:MM, should be used with AM/PM
	 */
	public static string ClockTime12h{
		get{
			if(ClockHours<12){
				return addZeroDigitIfNeeded(ClockHours)+":"+addZeroDigitIfNeeded(ClockMinutes);
			}else{
				return addZeroDigitIfNeeded(ClockHours-12)+":"+addZeroDigitIfNeeded(ClockMinutes);
			}
		}
	}

	public static bool Time_is_AM{
		get{
			if(ClockHours<12){
				return true;
			}else{
				return false;
			}
		}
	}

	
	/** Returns a List<Person> with all ID scores initialised.
	 */ 
	static List<Person> initialisePersonsID (List<Person> thesePersons){
		int counter = 0;
		foreach(Person iPerson in thesePersons){
			iPerson.IdentityNum = (int)(counter+1);
			counter++;
		}
		return thesePersons;
	}

	/** Randomises gender while keeping the overall town ratio of Male:Female ≈ 1
	*/
	static Dictionary<int,Person> setRandomGendersAndFNames (Dictionary<int,Person> thesePeople){
		foreach(KeyValuePair<int,Person> aKVP in thesePeople){
			if(Random.value > maleGenerationChance){ 
				aKVP.Value.PersonalStats.IsMale = true;
				aKVP.Value.PersonalStats.FirstName = firstNameMaleDB[Random.Range(0,firstNameMaleDB.Length)];
				maleGenerationChance += 0.1f;
			}else{
				aKVP.Value.PersonalStats.IsMale = false;
				aKVP.Value.PersonalStats.FirstName = firstNameFemaleDB[Random.Range(0,firstNameFemaleDB.Length)];
				maleGenerationChance -= 0.1f;
			}
		}
		return thesePeople;
	}

	static void setFNamesByGender (Dictionary<int,Person> thesePeople){
		foreach(KeyValuePair<int,Person> aKVP in thesePeople){
			if(aKVP.Value.PersonalStats.IsMale){
				aKVP.Value.PersonalStats.FirstName = firstNameMaleDB[Random.Range(0,firstNameMaleDB.Length)];
			}else if(!aKVP.Value.PersonalStats.IsMale){
				aKVP.Value.PersonalStats.FirstName = firstNameFemaleDB[Random.Range(0,firstNameFemaleDB.Length)];
			}else{
				Debug.LogError("Gender unnasigned");
			}
		}
	}

	/** Generates a random gender, with result weighted to conserve an equal male/female ratio.
	*/
	static void generateGenderAndName(Person thePerson){
		if(Random.value > maleGenerationChance){ 
			thePerson.PersonalStats.IsMale = true;
			thePerson.PersonalStats.FirstName = firstNameMaleDB[Random.Range(0,firstNameMaleDB.Length)];
			maleGenerationChance += 0.1f;
		}else{
			thePerson.PersonalStats.IsMale = false;
			thePerson.PersonalStats.FirstName = firstNameFemaleDB[Random.Range(0,firstNameFemaleDB.Length)];
			maleGenerationChance -= 0.1f;
		}
	}

	/** The non-randomised gender assignment option
	 */
	static void generateGenderAndName(Person thePerson, bool male){
		if(male){
			thePerson.PersonalStats.IsMale=true;
			thePerson.PersonalStats.FirstName = firstNameMaleDB[Random.Range(0,firstNameMaleDB.Length)];
			maleGenerationChance += 0.1f;
		}else{
			thePerson.PersonalStats.IsMale=false;
			thePerson.PersonalStats.FirstName = firstNameFemaleDB[Random.Range(0,firstNameFemaleDB.Length)];
			maleGenerationChance -= 0.1f;
		}
	}

	// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 				* * * * * * * SOCIAL EVENTS * * * * * * * *
	// *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-


	/** adds new SocialEvent to allSocialEvents dictionary for another function to overwrite with a subtype.
	*/
	static SocialEvent addNewSocialEventToRegister(){
		SocialEvent anEvent = new SocialEvent(socialEventCounter);
		allSocialEvents.Add(anEvent);
		socialEventCounter++;
		return anEvent;
	}

	/** adds new SocialEvent to allSocialEvents dictionary for another function to overwrite with a subtype.
	*/
	static SocialEvent addNewSocialEventToRegister(float currentGameTime){
		SocialEvent anEvent = new SocialEvent(socialEventCounter,currentGameTime);
		allSocialEvents.Add(anEvent);
		socialEventCounter++;
		return anEvent;
	}

	/** Adds the new Murder event to the allSocialEvents and allMurders Dicitionaries
	*/
	public static Murder createMurderEvent(Person perpetrator, Person deceased){
		SocialEvent anEvent = addNewSocialEventToRegister();
		Murder aMurder = new Murder(anEvent.ID,anEvent.TimeOccured,perpetrator,deceased);
		allSocialEvents[anEvent.ID] = aMurder;
		allMurders.Add(anEvent.ID,aMurder);
		return aMurder;
	}

	/** Adds the new ObjeciveEvidence to the Dictionary
	*/
	static ObjectiveEvidence newObjEvidence(Info attachedTo){
		ObjectiveEvidence newEvidence = new ObjectiveEvidence(evidenceCounter,attachedTo);
		allEvidence.Add(evidenceCounter,newEvidence);
		evidenceCounter++;
		return newEvidence;
	}

	/** Adds the new ObjeciveEvidence to the Dictionary
	*/
	static ObjectiveEvidence newObjEvidence(Info attachedTo, System.Type leadsToType){
		ObjectiveEvidence newEvidence = new ObjectiveEvidence(evidenceCounter,attachedTo,leadsToType);
		allEvidence.Add(evidenceCounter,newEvidence);
		evidenceCounter++;
		return newEvidence;
	}

	/** Adds the new ObjeciveEvidence to the Dictionary
	*/
	static ObjectiveEvidence newObjEvidence(Info attachedTo, Info leadsTo){
		ObjectiveEvidence newEvidence = new ObjectiveEvidence(evidenceCounter,attachedTo, leadsTo);
		allEvidence.Add(evidenceCounter,newEvidence);
		evidenceCounter++;
		return newEvidence;
	}

	/** Adds the new SubjectiveEvidence to the Dictionary
	*/
	static SubjectiveEvidence newSubjEvidence(Person witness){
		SubjectiveEvidence newEvidence = new SubjectiveEvidence(evidenceCounter,witness);
		allEvidence.Add(evidenceCounter,newEvidence);
		evidenceCounter++;
		return newEvidence;
	}

	/** Adds the new SubjectiveEvidence to the Dictionary
	*/
	static SubjectiveEvidence newSubjEvidence(Person witness, Person suspect){
		SubjectiveEvidence newEvidence = new SubjectiveEvidence(evidenceCounter,witness, suspect);
		allEvidence.Add(evidenceCounter,newEvidence);
		evidenceCounter++;
		return newEvidence;
	}

	// * * * * * * FAMILIES, HOUSEHOLDS  * * * * * * *
	// * * * * * * * * * * * * * * * * * * * * * * * * *



	void createFamilies(){ //FIXME: currently assigns household/family type by using Person() objects in the scene
		// write a f() that runs before  createhouseholds(), creates a householdratio & places persons in houses accordingly.
		//friends household < 4
		//senior household < 4
		if(householdsCreated){
			if(!familiesCreated){
				List<Household> lonerHouseholds = new List<Household>();
				List<Household> familyHouseholds = new List<Household>();
				List<Household> seniorHouseholds = new List<Household>();
				List<Household> friendsHouseholds = new List<Household>();
				List<Household> undecidedHouseholds = new List<Household>(); //undecided between friends/seniors/families NOT loners
				foreach(Household aHousehold in allHouseholds){
					if(aHousehold.PersonIndex.Count==1){
						lonerHouseholds.Add(aHousehold);
					}else if(aHousehold.PersonIndex.Count>3){
						familyHouseholds.Add(aHousehold);
					}else{
						undecidedHouseholds.Add(aHousehold);
					}
				}
				HouseholdsRatio townHHRatio = new HouseholdsRatio((int)AllHouseholds.Count);
				townHHRatio.generateRatio();
				foreach(Household aHousehold in undecidedHouseholds){ //filling quotas for loner, friends & senior households. The remaining houses are set to families.
					bool allowSenior = true; //limits chances of a senior household with a child, blocks senior if ==false.
					if(aHousehold.PersonIndex.Count==3){
						if(Settings.SeniorsLivingWithOffspring < Random.value){ //default: likely
							allowSenior = false;
						}
					}
					if(seniorHouseholds.Count < townHHRatio.SeniorHouseholdsNum && allowSenior){
						seniorHouseholds.Add(aHousehold);
					}else if(lonerHouseholds.Count < townHHRatio.LonerHouseholdsNum){
						lonerHouseholds.Add(aHousehold);
					}else if(friendsHouseholds.Count < townHHRatio.FriendsHouseholdsNum){
						friendsHouseholds.Add(aHousehold);
					}else{
						familyHouseholds.Add(aHousehold);
					} 
				}// >> at this point all the households are in their corresponding list

				// >> we will now set household members' names, genders and family relations within the household
				// along with setting their familyIDs which we will later merge/change to actual last names
				int familyNumCounter = 0;
				foreach(Household aHousehold in seniorHouseholds){
					createSeniorHousehold(aHousehold,familyNumCounter);
					familyNumCounter++;
				}
				foreach(Household aHousehold in friendsHouseholds){  // FIXME: rewrite into method
					int addToCounter = createFriendsHousehold(aHousehold, familyNumCounter);
					familyNumCounter++; //FIXME variable
				}
				foreach(Household aHousehold in familyHouseholds){
					createFamilyHousehold(aHousehold,familyNumCounter);
					familyNumCounter++;
				}
				//loner households must be run last, as the households are reassigned to different lists
				foreach(Household aHousehold in lonerHouseholds){ 
					if(seniorHouseholds.Count  > townHHRatio.SeniorHouseholdsNum){ 
						//create friends(adult) household
						createLonerHousehold(aHousehold,familyNumCounter,false);
						lonerHouseholds.Remove(aHousehold);
						friendsHouseholds.Add(aHousehold);
						familyNumCounter++;
					}else{ //create a senior household
						createLonerHousehold(aHousehold,familyNumCounter,true);
						lonerHouseholds.Remove(aHousehold);
						seniorHouseholds.Add(aHousehold);
						familyNumCounter++;
					}
				}
				if(Settings.AverageFamilyConnectionsPerHousehold!=0){ 
					createTownFamilies();
				}
				Debug.Log("senior projected "+ townHHRatio.SeniorHouseholdsNum);				
				Debug.Log("# of senior households "+seniorHouseholds.Count);
				Debug.Log("loners projected "+ townHHRatio.LonerHouseholdsNum);				
				Debug.Log("# of loner households "+lonerHouseholds.Count);
				Debug.Log("friends projected "+townHHRatio.FriendsHouseholdsNum);				
				Debug.Log("# of friends households "+friendsHouseholds.Count);
				Debug.Log("families projected "+townHHRatio.FamilyHouseholdsNum);
				Debug.Log("# of family househods "+familyHouseholds.Count);


				//..scan all humans for familyIDs and assign them real last names through a Dictionary<int,string> (int=familyID) 

				//..crawl back to validate families and check all ties are connected in both persons.

				familiesCreated = true;
			}else{ //create a case with a method that may create  estranged parents link
				Debug.LogError("createfamilies() is only allowed to run once");
			}
		}else{
			Debug.LogError("createFamilies() requires createHouseholds to be run first");
		}
	}


	/* *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	 * 				Familial connections creation
	 * *-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	* In this Section: 
	* link - intermediate level method. Creation of all family relations as to facilitate specified family connection (by calling Merge() methods)
	* Merge - low level method. The actual creation of family relations between selected method and group only
	*/

	/*TODO
	 * formulating a top-down algorithm for families instead of generic, specific assignments.
	 * /////////
	 * checking current senior families and creating sibling/cousin links
	 * checking senior
	 * 
	 * 
	 * ///ratios needed:
	 * int findMaxConnections(allhouseholds){}
	 *
	 * 
	 * ////calculated stuff:
	 * max number of children per household: 7, while forcing 2 parents
	 * max number of siblings due to this: 6
	 * max number of siblings in law: 6
	 * max number of cousins : 6 * 7 = 42
	 * max number of grandchildren: 7 * 7 = 495
	 * 
	 * 
	 * future relations calculator(type of connection):
	 * 	cases for individual connections
	 * 		count all new connections
	 * 		return count
	 * 	case 2:
	 * 		pass
	 * 	case 3:
	 * 		pass
	 * 	etc
	 * 
	 * compare the following:
	 * 	specific household average relations
	 * 	target average household relations (global)
	 * 	actual average household relations
	 * 
	 * how to create variations in the number of connections?
	 * 	through a 0-1 value
	 * also write a function that calculates all possible connections
	 * 
	 * 
	 * //////////
	 * This method may require creation of People objects.
	 * -> First, create a mathematical model and then follow it by creating it.
	 * 
	 * 
	 */



	static bool createTownFamilies(){
		float actualAvgNumConnections = 0f;
		/* Calculate a slightly randomised total
		 * while(actual<total||break=1)
		 * 	if(difference < a bit)
		 * 		break=1
		 * 	else
		 * 		loop households until you find one with the least average connections
		 * 		loop households until you find the second lowest
		 * 		loop through possible links to calculate how may new connections the link will create, choose the link which is the closest average connection
		 * 		
		 * 		
		 *
		*/	

		return false;//FIXME placeholder
	}

	static void createEmptyActivePersonStatsGroup(){
		PersonStatsGroup newPsg = createEmptyPersonStatsGroup();
		foreach(Person aPerson in townPeopleDatabase.PersonIndex.Values){
			PersonStats newPs = new PersonStats(aPerson,newPsg.VersionNum,newPsg.TimeCreated,newPsg);
			aPerson.PersonalStats = newPs;
		}
	}

	static PersonStatsGroup createEmptyPersonStatsGroup(){
		System.DateTime timeNow = System.DateTime.UtcNow;
		PersonStatsGroup aPSG = new PersonStatsGroup(personStatsVersionCounter,timeNow);
		allPersonStatsCollections.Add(personStatsVersionCounter, aPSG);
		personStatsVersionCounter++;
		return aPSG;
	}

	/** Returns the new PersonStatsGroup.
	 */
	static PersonStatsGroup backupCurrentPersonStatsGroup(){
		PersonStatsGroup newPsg = ActivePersonStatsGroup.makeCopy(personStatsVersionCounter);
		personStatsVersionCounter++;
		if(!setPersonStatsGroupAsActive(newPsg)){
			//TODO functionality on fail?
		}
		allPersonStatsCollections.Add(newPsg.VersionNum,newPsg);
		return newPsg;
	}

	/** Should not be used to set the first Town.eActivePersonStatsGroup
	 * returns false if a sever inconsistency is founnd and then makes supplied PersonalStatsGroup invalid
	 */
	public static bool setPersonStatsGroupAsActive(PersonStatsGroup aPSG){
		if(Town.ActivePersonStatsGroup != aPSG){
			PersonStatsGroup formerPSG = Town.ActivePersonStatsGroup;
			activePersonStatsCollection = aPSG;
			bool reverse = false; //in case of critical error, revert to previous aPSG
			foreach(PersonStats aPS in aPSG.PersonStatsCollection.Values){
				if(aPS.VersionNum == aPS.ParentCollection.VersionNum){
					//no critical errors, so all paths here still perform reassignments while logging errors
					if(aPS.OfPerson.PersonalStats.ParentCollection == formerPSG){ 
						aPS.OfPerson.PersonalStats = aPS; //all good
					}else if(aPS.OfPerson.PersonalStats.ParentCollection == null){ 
						Debug.LogError("PersonalStats.ParentCollection of Person with id of "+aPS.OfPerson.IdentityNum+" was previously null");
						aPS.OfPerson.PersonalStats = aPS;
					}else if(aPS.OfPerson.PersonalStats.ParentCollection == aPSG){
						Debug.LogError("The PersonalStats of person with id of "+ aPS.OfPerson.IdentityNum+
						               " is already set to this PersonalStatsGroup, while Town.ActivePersonStatsGroup did not represent this");
					}else{
					Debug.LogError("PersonalStats.ParentCollection of Person with id of "+aPS.OfPerson.IdentityNum+
						" was set not set to previous value OR null, and was instead set to PSG with version number of: "
						+aPS.OfPerson.PersonalStats.ParentCollection.VersionNum);
					aPS.OfPerson.PersonalStats = aPS;
					}
				}else{
					Debug.LogError("Version numbers of new active PersonalStats and its Parent Collection do not match./n PersonalStats Version number is "+
					               aPS.VersionNum+" while the parent collection's is "+aPS.ParentCollection.VersionNum);
					reverse = true;
					break;
				}
			}		
			if(reverse){
				activePersonStatsCollection = formerPSG; //revert active indicator back
				foreach(PersonStats aPS in formerPSG.PersonStatsCollection.Values){ //revert individual PersonStats
					aPS.OfPerson.PersonalStats = aPS; 
				}
				aPSG.makeInvalid();
				return false;
			}
		}else{
			Debug.LogError("PersonStatsGroup of series number "+aPSG.VersionNum+" is already set as ActivePersonStatsGroup");
		}
		return true;
	}



	/** tries to link families across households, returns false if unsuccessful (or incompatible).
	 */
	static bool linkFamilies(Person person1, Person person2, PersonStats.relativeType relativeType){ //FIXME: no accounts for exes' or half-relations
		if(person1.PersonalStats.IsAdult && person2.PersonalStats.IsAdult){ //non adult family links are not currently allowed
			//FIXME create checks for family relations within the targets families to avoid a later failure of overwriting
			if(relativeType == PersonStats.relativeType.Sibling){
				if(checkParentsMerge(person1, person2)){ //adds non-separated parents if possible
					if(person1.PersonalStats.Children.Count != 0){//if adults have children, their new siblings will be their children's aunts/Uncles
						makeSiblingConnections(person2,person1.PersonalStats.Children[0]);
					}
					if(person2.PersonalStats.Children.Count != 0){
						makeSiblingConnections(person2,person1.PersonalStats.Children[0]);

					}
					//if adults were nephews, their new siblings will also become nephews
					//get age of siblings

					List<Person> allSiblings = new List<Person>();
					allSiblings.AddRange(person1.PersonalStats.Siblings.Values); //TODO: convert search for siblings to method?
					foreach(Person otherSibling in person2.PersonalStats.Siblings.Values){
						if(!allSiblings.Contains(otherSibling)){
							allSiblings.Add(otherSibling);
						}
					}
					if(!linkSiblings(person1,person2)){
						return false;
					}
					if(!linkAnySiblingsInLaw(allSiblings)){
						return false;
					}

					//TODO
					//change family names/id
					//link secondary connections: 
						//grandparents (parents of siblings)&grandchildren (children of siblings) 
						//cousins (children of siblings)
					return true;
				}else{ //can't merge parents, sibling link impossible
					return false; //connection can't be made, parent clash
				}
			}else if(relativeType == PersonStats.relativeType.Parent){ //for senior-adult link

				//!check for clash & add parent
				//add siblings (parent's children)
				//secondary: 
					//grandparents for children of children.
					//new siblings may add nephew/aunt/cousin links
					//new siblings' spouses are siblings in law

			}else if(relativeType == PersonStats.relativeType.Cousin){
				//justCousin
			}else{ //FIXME: +exSpouse relativeType case?
				Debug.LogError("Couldn't ascertain type of person, or the type is not allowed for family linking");
				return false;
			}
		}else{
			Debug.LogError("Linking families based on non-adult connections is not allowed");//FIXME: could this be changed?
			return false;
		}
		return true;//FIXME temp
	}

	/** 
	 */
	static bool checkAndMergeSepParents(Person child1,Person child2){
		//TODO
		//check w/ divorced ratio
		return false;//FIXME false
	}

	/** Creates all relations between sibling's relatives
	 * Also assumes not more than one of the siblings already has parents.
	 * TODO method before this one to check to avoid merging siblings in law into siblings, in the case that this will result in married siblings.
	 */
	static bool makeSiblingConnections(Person Sib1,Person Sib2){


		/*In this section:
		 * New - refering to the sibling without parents (who's joining the family)
		 * Old - referring to the sibling with parents (who's already in the family)
		 * sibs - siblings
		 * abort - attempt at illegal connection stopped
		 */

		PersonStatsGroup newPersonStatsGroup = backupCurrentPersonStatsGroup();

		bool abort=false;
		Person sibWithParents;
		Person sibWithoutParents;
		if(Sib1.PersonalStats.Parents.Count !=0){
			sibWithParents=Sib1;
			sibWithoutParents = Sib2;
		}else{
			sibWithParents=Sib2;
			sibWithoutParents=Sib1;
		}

		List<Person> oldSiblings = new List<Person>();
		List<Person> newSiblings = new List<Person>();
		List<Person> oldSibsInLaw = new List<Person>();
		List<Person> newSibsInLaw = new List<Person>();
		List<Person> cousins = new List<Person>(); //cousins of old sibling 
		List<Person> parents = new List<Person>();
		List<Person> parentSibs = new List<Person>();
		List<Person> parentSibsInLaw = new List<Person>();
		List<Person> oldChildren = new List<Person>(); //also current children's cousins
		List<Person> newChildren = new List<Person>(); //also current children's cousins

		oldSiblings.AddRange(sibWithParents.PersonalStats.Siblings.Values);
		oldSiblings.Add(sibWithParents);
		newSiblings.AddRange(sibWithoutParents.PersonalStats.Siblings.Values); 
		newSiblings.Add(sibWithoutParents);
		foreach(Person oldSibling in oldSiblings){
			oldSibsInLaw.Add(oldSibling.PersonalStats.Spouse);
			oldChildren.AddRange(oldSibling.PersonalStats.Children.Values);
		}
		foreach(Person newSibling in newSiblings){
			newSibsInLaw.Add(newSibling.PersonalStats.Spouse);
			newChildren.AddRange(newSibling.PersonalStats.Children.Values);
		}
		cousins.AddRange(sibWithParents.PersonalStats.Cousins.Values);
		parents.AddRange(sibWithParents.PersonalStats.Parents.Values);
		foreach(Person parent in parents){
			parentSibs.AddRange(parent.PersonalStats.Siblings.Values);
		}
		foreach(Person parentSib in parentSibs){
			parentSibsInLaw.Add(parentSib.PersonalStats.Spouse);
		}
		//begin asssignment of family relations
		foreach(Person newChild in newChildren){
			foreach(Person oldChild in oldChildren){
				if(!oldChild.addCousin(newChild) || !newChild.addCousin(oldChild)){
					abort = true;
					break;
				}
				
			}
			if(abort){break;}
			foreach(Person parent in parents){
				if(!parent.addGrandChild(newChild) || !newChild.addGrandParent(parent)){
					abort = true;
					break;
				}
			}
			if(abort){break;}
			foreach(Person sibInLaw in oldSibsInLaw){
				if(!sibInLaw.addNephew(newChild) || !newChild.addUncleAunt(sibInLaw)){
					abort = true;
					break;
				}
			}
			if(abort){break;}

		}

		//TODO also sibs in law




		if(abort){
			Debug.LogError("makeSiblingConnections() aborted");
			PersonStatsGroup revertTo = newPersonStatsGroup.PreviousPSG;
			if(!setPersonStatsGroupAsActive(revertTo)){
				//TODO some kind of rescue function to recurse back to a stable PersonalStatsGroup
			}
			return false;
		}else{
			return true; //all good
		}
	}


	/** Checks if there's a possibily to add biological non-estranged parents to other child. If not possible, returns false.
	 * Does not merge 2 parents 
	 */
	static bool checkParentsMerge(Person child1,Person child2){
		int parentsNum1 = child1.PersonalStats.Parents.Count;
		int parentsNum2 = child2.PersonalStats.Parents.Count;
		if(parentsNum1 !=0){
			if(parentsNum2 != 0){ //both have parents
				return false;
			}else{ //only child1 has parents
				return true;
			}
		}else{ 
			if(parentsNum2 != 0){ //only child2 has parents
				return true;
			}else{ //both don't have parents
				return true;
			}
		}
	}


	/** Creates sibling links between all new siblings (on both sides)
	 * Doesn't check for duplicates.
	 */
	static bool linkSiblings(Person sibling1,Person sibling2){ 
		List<Person> linkFirstSiblings = new List<Person>(); //here 1st & 2nd mean groups
		List<Person> linkSecondSiblings = new List<Person>();
		linkFirstSiblings.Add(sibling1);
		linkSecondSiblings.Add(sibling2);
		linkFirstSiblings.AddRange(sibling1.PersonalStats.Siblings.Values);
		linkSecondSiblings.AddRange(sibling2.PersonalStats.Siblings.Values);
		foreach(Person aFirstSibling in linkFirstSiblings){
			foreach(Person aSecondSibling in linkSecondSiblings){
				if(!aFirstSibling.addSibling(aSecondSibling) || !aSecondSibling.addSibling(aFirstSibling)){
					return false;
				}
			}
		}
		return true;
	}

	/** TODO: check approach below
	 * Creates half-sibling links between half-siblings' sibling groups and half-siblings themselves
	 * possible new approach - check whether the people in question already have a sibling connection, and only allow a half sibling connection if there is a verfiable parent.
	*/
	static bool linkHalfSiblings(Person halfSib1,Person halfSib2){ 
		if(!halfSib1.addHalfSibling(halfSib2) || !halfSib2.addHalfSibling(halfSib1)){
			return false;
		}
		List<Person> link1stHalfSibs = new List<Person>(); //1st & 2nd mean groups
		List<Person> link2ndHalfSibs = new List<Person>();
		link1stHalfSibs.Add(halfSib1);
		link2ndHalfSibs.Add(halfSib2);
		link1stHalfSibs.AddRange(halfSib1.PersonalStats.Siblings.Values);
		link2ndHalfSibs.AddRange(halfSib2.PersonalStats.Siblings.Values);
		foreach(Person a1stHalfSib in link1stHalfSibs){
			foreach(Person a2ndHalfSib in link2ndHalfSibs){
				if(!a1stHalfSib.addHalfSibling(a2ndHalfSib) || !a2ndHalfSib.addHalfSibling(a1stHalfSib)){
					return false;
				}
			}
		}
		return true;
	}

	/** Checks siblings for spouses to make siblings in law connections
	 * FIXME check if fits new structure
	*/
	static bool linkAnySiblingsInLaw(List<Person> allNewSiblings){
		foreach(Person sibling in allNewSiblings){ //iterating through siblings
			if(sibling.PersonalStats.AgeGroup != PersonStats.ageGroup.Child && sibling.PersonalStats.AgeGroup != PersonStats.ageGroup.Teen){
				if(sibling.PersonalStats.Spouse!=null){ //Spouse found
					Person spouseToCheck = sibling.PersonalStats.Spouse;
					foreach(Person chkOtherSibling in allNewSiblings){ //iterating through siblings again to compare
						if(sibling != chkOtherSibling){ //avoiding self reference
							if(!chkOtherSibling.addSiblingInLaw(sibling.PersonalStats.Spouse)){
								return false;
							}
						}
					}
				}
			}else{
				Debug.LogError("Linked siblings in law which are of the AgeGroup: "+sibling.PersonalStats.AgeGroup.ToString());
				return false;
			}
		}
		return true;
	}

	/** Scans the household for people who fit the age group and returns a random Person
	 * (checks for null before returning)
	 * FIXME not needed anymore?
	 */
	static Person returnRandomPersonOfAgeGroup(PersonStats.ageGroup anAgeGroup, Household fromHousehold){
		List<Person> eligible = new List<Person>();
		foreach(Person aPerson in fromHousehold.PersonIndex.Values){
			if(aPerson.PersonalStats.AgeGroup==anAgeGroup){
				eligible.Add(aPerson);
			}
		}
		if(eligible.Count==0){
			Debug.LogError("No people of ageGroup "+ anAgeGroup.ToString() +" found in the household");
		}
		Person result = eligible[Random.Range(0,eligible.Count+1)];
		if (result==null){
			Debug.LogError("A null value was returned");
			return result;
		}else{
			return result;
		}
	}

	/** calculates from divorced and non-divorced parents' counters
	*/
	static int divorcedPercentage{
		get{
			int all = nonDivorcedCounter +divorcedCounter;
			return divorcedCounter / all *100;
		}
	}

	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-
	// 							HOUSEHOLDS CREATION
	//*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-

	/** Sets inner-house familial coonections, gender, first names and ageGroup
	 */
	static void createSeniorHousehold(Household thisHousehold, int familyNum){
		if(thisHousehold.PersonIndex.Count==2){
			List<Person> householdMembers = new List<Person>();
			householdMembers.AddRange(thisHousehold.PersonIndex.Values); //conversion to list to allow index iteration
			foreach(Person aPerson in householdMembers){ //setting family ID
				aPerson.PersonalStats.FamilyID=familyNum;
			}
			if(Settings.SeniorsLivingWithOffspring > Random.value){ //senior + adult/teen
				if(Random.value > 0.5f){ //50% chance of adult living with parent
					generateGenderAndName(householdMembers[0]);
					generateGenderAndName(householdMembers[1]);
					householdMembers[0].addParent(householdMembers[1]);
					householdMembers[1].addChild(householdMembers[0]);
					householdMembers[0].PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
					householdMembers[1].PersonalStats.AgeGroup=PersonStats.ageGroup.Adult;
				}else{ //50% chance of teenager living with grandparent
					generateGenderAndName(householdMembers[0]);
					generateGenderAndName(householdMembers[1]);
					householdMembers[0].addGrandParent(householdMembers[1]);
					householdMembers[1].addGrandChild(householdMembers[0]);
					householdMembers[0].PersonalStats.AgeGroup=PersonStats.ageGroup.Teen;
					householdMembers[1].PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
				}
			}else{ //standard senior couple
				generateGenderAndName(householdMembers[0],true);
				generateGenderAndName(householdMembers[1],false);
				householdMembers[0].PersonalStats.Spouse=householdMembers[1];
				householdMembers[1].PersonalStats.Spouse=householdMembers[0];
				householdMembers[0].PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
				householdMembers[1].PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
			}
		}else if(thisHousehold.PersonIndex.Count==3){ //senior couple living with an offspring
			List<Person> householdMembers = new List<Person>();
			householdMembers.AddRange(thisHousehold.PersonIndex.Values);
			foreach(Person aPerson in householdMembers){
				aPerson.PersonalStats.FamilyID=familyNum;
			}
			generateGenderAndName(householdMembers[0],true);
			generateGenderAndName(householdMembers[1],false);
			householdMembers[0].PersonalStats.Spouse=householdMembers[1];
			householdMembers[1].PersonalStats.Spouse=householdMembers[0];
			householdMembers[0].PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
			householdMembers[1].PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
			if(Random.value > 0.5f){ //randomising offspring living with seniors
				//adult living with parents
				generateGenderAndName(householdMembers[2]);
				householdMembers[0].addChild(householdMembers[2]);
				householdMembers[1].addChild(householdMembers[2]);
				householdMembers[2].addParents(householdMembers[0],householdMembers[1]);
				householdMembers[2].PersonalStats.AgeGroup=PersonStats.ageGroup.Adult;
			}else{ // teenager living with grandparents
				generateGenderAndName(householdMembers[2]);
				householdMembers[0].addGrandChild(householdMembers[2]);
				householdMembers[1].addGrandChild(householdMembers[2]);
				householdMembers[2].addGrandParents(householdMembers[0],householdMembers[2]);
				householdMembers[2].PersonalStats.AgeGroup=PersonStats.ageGroup.Teen;
			}
			Debug.Log("senior house of size 3 created");
		}else{
			Debug.LogError("Invalid size of household for a senior household");
		}
	}

	/** Sets gender, and first name.
	 * ageGroup is either Adult or Senior Depending on the input bool
	 */
	static void createLonerHousehold(Household aHousehold,int familyNum,bool senior){
		if(aHousehold.PersonIndex.Count==1){
			foreach(KeyValuePair<int,Person> aKVP in aHousehold.PersonIndex){
				aKVP.Value.PersonalStats.FamilyID = familyNum;
				if(senior){
					generateGenderAndName(aKVP.Value);
					aKVP.Value.PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
				}else{
					generateGenderAndName(aKVP.Value);
					aKVP.Value.PersonalStats.AgeGroup=PersonStats.ageGroup.Senior;
				}
			}
		}else{
			Debug.LogError("Invalid size of household for a loner household: "+aHousehold.PersonIndex.Count);
		}
	}

	/** Sets inner-house familial coonections, gender, first names and ageGroup
	 */
	static void createFamilyHousehold(Household aHousheold, int familyNum){
		if(aHousheold.PersonIndex.Count != 1){ //checking validity
			List<Person> houseMembers = new List<Person>(); 
			houseMembers.AddRange(aHousheold.PersonIndex.Values); //conversion to list to allow index iteration
			foreach(Person aPerson in houseMembers){ //setting family ID
				aPerson.PersonalStats.FamilyID = familyNum;
			}
			if(aHousheold.PersonIndex.Count > 4){ //large family, force 2 parents
				generateGenderAndName(houseMembers[0]);
				houseMembers[0].PersonalStats.AgeGroup=PersonStats.ageGroup.Adult;
				generateGenderAndName(houseMembers[1]);
				houseMembers[1].PersonalStats.AgeGroup=PersonStats.ageGroup.Adult;
				houseMembers[0].PersonalStats.Spouse = houseMembers[1];
				houseMembers[1].PersonalStats.Spouse = houseMembers[0];
				int teenChildWeight=0; 
				for(int i=2;i<=houseMembers.Count;i++){ //children iteration
					if(teenChildWeight > 1){ //too many teens
						houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Child;
						teenChildWeight--;
					}else if(teenChildWeight < -1){ //too many children
						houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Teen;
						teenChildWeight++;
					}else if(Settings.TeenChildRatio < Random.value){ //current household ratio is balanced, randomise result
						houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Child;
						teenChildWeight--;
					}else{
						houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Teen;
						teenChildWeight++;
					}
					generateGenderAndName(houseMembers[i]);
					houseMembers[i].addParents(houseMembers[0],houseMembers[1]); //create parent connections
					houseMembers[0].addChild(houseMembers[i]); 
					houseMembers[1].addChild(houseMembers[i]);//create child connections
					for(int j=i+1;j<=houseMembers.Count;j++){ //create sibling connections
						houseMembers[i].addSibling(houseMembers[j]);
						houseMembers[j].addSibling(houseMembers[i]);
					}
				}
			}else{ // family size < 4
				if(Settings.SingleParentsNonSeniors > Random.value){ //single parent with <4 kids
					generateGenderAndName(houseMembers[0]);
					houseMembers[0].PersonalStats.AgeGroup=PersonStats.ageGroup.Adult;
					for(int i=1;i<=houseMembers.Count;i++){ //children iteration
						if(Settings.TeenChildRatio < Random.value){
							houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Child;
						}else{
							houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Teen;
						}
						generateGenderAndName(houseMembers[i]);
						houseMembers[i].addParent(houseMembers[0]); //create parent connection
						houseMembers[0].addChild(houseMembers[i]); //create child connection
						for(int j=i+1;j<=houseMembers.Count;j++){ //create sibling connections
							houseMembers[i].addSibling(houseMembers[j]);
							houseMembers[j].addSibling(houseMembers[i]);
						}
					}
				}else{ //2 parents with <4 kids
					generateGenderAndName(houseMembers[0]);
					houseMembers[0].PersonalStats.AgeGroup=PersonStats.ageGroup.Adult;
					generateGenderAndName(houseMembers[1]);
					houseMembers[1].PersonalStats.AgeGroup=PersonStats.ageGroup.Adult;
					houseMembers[0].PersonalStats.Spouse = houseMembers[1];
					houseMembers[1].PersonalStats.Spouse = houseMembers[0];
					for(int i=2;i<=houseMembers.Count;i++){ //children iteration
						if(Settings.TeenChildRatio < Random.value){
							houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Child;
						}else{
							houseMembers[i].PersonalStats.AgeGroup=PersonStats.ageGroup.Teen;
						}
						generateGenderAndName(houseMembers[i]);
						houseMembers[i].addParents(houseMembers[0],houseMembers[1]); //create parent connections
						houseMembers[0].addChild(houseMembers[i]); 
						houseMembers[1].addChild(houseMembers[i]);//create child connections
						for(int j=i+1;j<=houseMembers.Count;j++){ //create sibling connections
							houseMembers[i].addSibling(houseMembers[j]);
							houseMembers[j].addSibling(houseMembers[i]);
						}
					}
				}
			}
		}else{
			Debug.LogError("Invalid size of household for family creation: "+aHousheold.PersonIndex.Count);
		}
	}

	/**  Sets inner-house familial coonections, gender, first name and ageGroup
	 * returns an int of how many distinct families were created (and how much you need to add to familyNumCounter)
	 */
	static int createFriendsHousehold(Household theHousehold, int familyNum){
		int numFamiliesCreated = 0;
		if(theHousehold.PersonIndex.Count != 1 && theHousehold.PersonIndex.Count < 4){
			//setting names, genders and AgeGroup for all members:
			foreach(KeyValuePair<int,Person> aKvp in theHousehold.PersonIndex){ 
				generateGenderAndName(aKvp.Value);
				aKvp.Value.PersonalStats.AgeGroup = PersonStats.ageGroup.Adult;
			}
			// set household connections and FamilyIDs:
			if(Settings.AdultSiblingsSharingHouse > Random.value){ //Household contains 2 siblings
				List<Person> houseMembers = new List<Person>();
				houseMembers.AddRange(theHousehold.PersonIndex.Values); // convert to List for index iteration
				houseMembers[0].addSibling(houseMembers[1]);
				houseMembers[1].addSibling(houseMembers[0]); //connected siblings
				houseMembers[0].PersonalStats.FamilyID = familyNum;
				for(int i=1;i<=houseMembers.Count;i++){ //start from 1, since [0] & [1] have the same familyNum&familyID
					houseMembers[i].PersonalStats.FamilyID = familyNum;
					familyNum++;
				}
				numFamiliesCreated=theHousehold.PersonIndex.Count-1; //2 people share a last name, the result is 1 less unique last name
			}else{ //all adults unrelated
				foreach(KeyValuePair<int,Person> aKvp in theHousehold.PersonIndex){
					aKvp.Value.PersonalStats.FamilyID = familyNum;
					familyNum++;
				}
				numFamiliesCreated=theHousehold.PersonIndex.Count;
			}
		}else{
			Debug.LogError("Invalid input of Household size "+theHousehold.PersonIndex.Count);
		}
		return numFamiliesCreated;
	}
	
	
	/**assigns most house & household variables
	*/
	static void createHouseHolds(){ 
		/*FIXME: create a method to populate the houses with created Person()s instead of searching abailable ones
		 * Rename to createHouseMembers() when done.
		 */
		if(!householdsCreated){
			allHouses = ManipulatorsUtils.getAllHouses(); //get all houses
			if(allHouses.Count != 0){
				Debug.Log("success");
			}
			//Instantiate every house
			Dictionary<int,Person> searchPersons = new Dictionary<int, Person>(TownPeopleDatabase.PersonIndex); //creating an instance instead of a pointer
			foreach(House aHouse in allHouses){
				Dictionary<int,Person> foundPersons = new Dictionary<int, Person>();
				//transform.position is at the centre of the rectangle, the topleft point's position relative to the centre is: ( x: -width/2, z: +height/2 )
				float topLeftX = aHouse.transform.position.x - (aHouse.renderer.bounds.size.x / 2); 
				float topLeftZ = aHouse.transform.position.z + (aHouse.renderer.bounds.size.z / 2);

				Rect saveArea = new Rect(topLeftX,topLeftZ,aHouse.renderer.bounds.size.x,-aHouse.renderer.bounds.size.z);
				aHouse.HouseArea = saveArea; //OUTPUT to aHouse

				foundPersons = ManipulatorsUtils.checkWhichPersonsInArea(saveArea, searchPersons); //searching for people in household
				if (foundPersons.Count == 0){
					if(!aHouse.abandonedHouse){
						Debug.LogWarning("House in position "+aHouse.transform.position+" has no people inside, so no household was created for it. Please set this house to Abandoned if this was intentional.");
					}
				}else{
					aHouse.PersonsInHouse = foundPersons; // OUTPUT to aHouse
					Household aHousehold = new Household(aHouse,foundPersons);
					searchPersons = ManipulatorsUtils.removePersonsFromGroup(foundPersons,searchPersons);
					aHousehold.computeLeaders();
					allHouseholds.Add(aHousehold); //saving data

					//set householdAssoc for each Person
					foreach(KeyValuePair<int,Person> aKVP in aHouse.PersonsInHouse){
						aKVP.Value.PersonalStats.HouseholdAssoc = aHousehold;
					}
				}
			}
			foreach(KeyValuePair<int,Person> aKVP in searchPersons){
				Debug.LogWarning("Person in position "+aKVP.Value.transform.position+" was not placed in a house and thus was not assigned to a household");
			}
			householdsCreated = true;
		}else{
			Debug.LogError("createHouseholds is only allowed to run once");
		}
	}

	/** For runtime creation of Households
	 */
	static void createHousehold(Dictionary<int,Person> fromThesePersons, House houseAssoc){

	}

	/** For runtime deletion of households
	 */
	static void deleteHousehold(Household thisHousehold){
		thisHousehold.AttachedToHouse.AssociatedHousehold = null;

	}


	void Start() {
		numberOfTownInstances++; //checking against multiple town instances
		if(numberOfTownInstances!=1){
			Debug.LogError("Only ONE Town.cs object is allowed per scene");
		}

		TownPeopleDatabase.PersonIndex = TownPeopleDatabase.populatePersonsIndex( initialisePersonsID( ManipulatorsUtils.getAllPersons() )  );
		TownPeopleDatabase.generateGlobalInfluenceTable(TownPeopleDatabase.PersonIndex); 		
		createHouseHolds();
		createFamilies();

		
		lastNameDB = ManipulatorsUtils.shuffleStringArray(lastNameDB);
		//assign families+last names
		foreach(KeyValuePair<int,Person> aKVP in TownPeopleDatabase.PersonIndex){
			aKVP.Value.initialisePersonStats();//FIXME: customise stat generation to age group and gender
		}

		//assign jobs
		//assign cliques

		allPlayers = ManipulatorsUtils.getAllPlayers();
		Debug.Log("Num of houses: " + allHouses.Count);
		Debug.Log("Num of households: " + allHouseholds.Count );
		Debug.Log("Number of people in DB:" + TownPeopleDatabase.PersonIndex.Count);

		//randomly generate past events in families (especially if a young person is missing a parent)

		//check all persons have the necesseary statistics

		//create a finalising bool to block natural variables modification/methods
	}

	void Update () {
		//create packs

		if(!runOnce){
			foreach(Household ahousH in allHouseholds){
				Debug.Log("Num of household members " + ahousH.PersonIndex.Count);
				Debug.Log("Household leadership type is: "+ahousH.LeadershipType);
				if(ahousH.Leaders[0] != null){
					if(ahousH.Leaders.Length > 0){
						if(ahousH.Leaders[1] != null){
							Debug.Log("The leaders are: " + ahousH.Leaders[0].PersonalStats.FirstName + " and " + ahousH.Leaders[1].PersonalStats.FirstName);
						}else{
							Debug.Log("The leader is: " + ahousH.Leaders[0].PersonalStats.FirstName);
						}
					}
				}
			}
			runOnce = false;
		}
	}
}