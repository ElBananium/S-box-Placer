using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer.Utils
{
	public class PlacableAttribute : Attribute
	{
		public string Model { get; set; }


		public PlacableAttribute( string model )
		{
			Model = model;




		}
	}
}
