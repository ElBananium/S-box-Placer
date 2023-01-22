using Sandbox;


namespace PlaceLib.VisualisationEntity.Abstraction
{
	public interface IEntityRenderChanger
	{

		public void OnCorrectPosition(ModelEntity entity);

		public void OnWrongPosition(ModelEntity entity);
	}
}
