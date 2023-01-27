using Sandbox;

namespace PlaceLib.Placer.Abstractions
{
	public interface IAdditionalVisualisation
	{

		public void OnCorrectPosition(Player owner );

		public void OnIncorrectPosition(Player owner );

		public void DeleteAdditionalVisualisation();


		public void CreateAdditionalVisualisation(ModelEntity visEntity);

		public void HideAdditionalVisualisation();

		public void ShowAdditionVisualisation();

	}
}
