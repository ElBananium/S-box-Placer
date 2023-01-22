using System;


namespace PlaceLib.Placer.Utils
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
