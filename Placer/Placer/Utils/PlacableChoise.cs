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

		public string EntityName { get; protected set; }


	public PlacableChoise(string entName, string model)
		{
			EntityName = entName;

			Model = model;
		}
	}
}
