using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer.Utils
{
	public class PlacableChoise
	{

		public string Model { get; protected set; }

		public Type EntityType { get; protected set; }


	public PlacableChoise(Type entType, string model)
		{
			EntityType = entType;

			Model = model;
		}
	}
}
