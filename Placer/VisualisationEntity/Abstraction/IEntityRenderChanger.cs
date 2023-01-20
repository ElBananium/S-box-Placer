using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.VisualisationEntity.Abstraction
{
	public interface IEntityRenderChanger
	{

		public void OnCorrectPosition(ModelEntity entity);

		public void OnWrongPosition(ModelEntity entity);
	}
}
