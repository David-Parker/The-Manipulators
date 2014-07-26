/*
 * Overview
 * --------------------------------------
 * There is a single instance of the Town class which acts as root.
 * The Town generates all SocialStructures, Person statistics, etc.
 * Person Statistics are in a class called PersonStats. 
 * All PersonStats belong to an instance of a PersonStatsGroup to keep things consistent.
 * Every Person has a unique identityNum (sometimes referred to as ID)
 * 
 * SocialStructures are the parent class of Workplace, Household, Clique and Pack. It has the following useful database:
 * 		All group members are saved in a Dictionary<int, Person> personIndex; where  key = identityNum
 * SocialStructure is also instantiated once as a static townPeopleDatabase, which stores global information on every Person in town, eg:
 * 		a Dictionary of all people
 * 		a 2D Dictionary of people's influence scores, which are used to calculate social action effectiveness and choose group leaders. 
 * 
 * Settings keeps all game affecting varialbes centralised in one class
 * 
 * 
 * Quick facts
 * --------------------------------------
 * The town has ~200 people
 * 
 * 
 * 
 * 
 */
