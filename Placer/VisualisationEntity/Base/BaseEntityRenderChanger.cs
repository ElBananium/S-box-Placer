using Sandbox.Placer.VisualisationEntity.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.VisualisationEntity.Base
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
