using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Utilities
{
	public class PlacableAttribute : Attribute
	{
		public string Model { get; set; }

		public string SpawnTag { get; set; }


		public string Name { get; set; }



		public PlacableAttribute( string model, string spawnTag, string name )
		{
			Model = model;
			SpawnTag = spawnTag;

			Name = name;
			
		}

	}
}
