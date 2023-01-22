using PlaceLib.VisualisationEntity.Abstraction;
using Sandbox;

namespace Prefabs.RenderChangers
{
	public class BaseEntityRenderChanger : IEntityRenderChanger
	{
		public void OnCorrectPosition( ModelEntity entity )
		{

			entity.RenderColor = Color.Green.WithAlpha( 0.9f );
		}

		public void OnWrongPosition( ModelEntity entity )
		{
			entity.RenderColor = Color.Red.WithAlpha( 0.9f );
		}
	}
}
