using Sandbox;

namespace PlaceLib.Placer.Abstractions
{
	public interface IAdditionalVisualisation
	{

		public void OnCorrectPosition( Vector3 eyePos, Vector3 eyeDir, Rotation eyeRot, Transform entTransform, Player owner );

		public void OnIncorrectPosition( Vector3 eyePos, Vector3 eyeDir, Rotation eyeRot, Transform entTransform, Player owner );

		public void DeleteAdditionalVisualisation();

	}
}
