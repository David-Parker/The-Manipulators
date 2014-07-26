/**Subclasses to write: WitnessedSocialEvent, voicedOpinion, subjectiveEvidenceFabricated, savedSomeone, helped someone.
 * WitnessedSocialEvent as a good general type? output all SocialEvent() contents into it. Problematic since there are so many variables, even if you specialise it with a System.Type var.
 */
public class SubjectiveEvidence : Evidence {

	protected Person _witness;
	protected Person _suspect;

	public SubjectiveEvidence(int id, Person witness): base (id){
		_witness = witness;
	}
	public SubjectiveEvidence(int id,Person witness, Person suspect): base (id){
		_witness = witness;
		_suspect = suspect;
	}
}
