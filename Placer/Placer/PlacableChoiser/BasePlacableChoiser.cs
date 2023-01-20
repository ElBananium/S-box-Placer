﻿using Sandbox.Placer.Placer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer.PlacableChoiser
{
	public partial class BasePlacableChoiser : IPlacableChoiser
	{
		public PlacableChoise GetCurrentChoise()
		{
			return SettedChoise;
		}


		public static PlacableChoise SettedChoise => _settedChoise ?? new PlacableChoise(typeof(BouncyBallEntity), "models/ball/ball.vmdl");

		private static PlacableChoise _settedChoise;

		[ConCmd.Client("set_choise")]
		public static void SetChoise(string model, string TypeName) 
		{
			_settedChoise = new PlacableChoise( TypeLibrary.GetType( TypeName ).GetType(), model );
		
		}
	}
}