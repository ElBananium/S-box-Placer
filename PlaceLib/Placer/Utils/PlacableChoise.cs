
namespace PlaceLib.Placer.Utils
{
	public class PlacableChoise
	{

		public string Model { get; protected set; }

		public string EntityName { get; protected set; }


	public PlacableChoise(string entName, string model)
		{
			EntityName = entName;

			Model = model;
		}
	}
}
