

using Sandbox;
using PlaceLib.Placer.Utils;
using PlaceLib.Placer.Abstractions;

namespace Prefabs.PlacableChoisers
{
	public class ConsolePlacableChoiser : BasePlacableChoiser
	{
		public PlacableChoise GetCurrentChoise()
		{
			return SettedChoise;
		}

		public override PlacableChoise CurrentChoise { get => SettedChoise; protected set => throw new System.NotImplementedException(); }


		public static PlacableChoise SettedChoise => _settedChoise ?? new PlacableChoise( typeof( BouncyBallEntity ).Name, "models/ball/ball.vmdl" );

		

		private static PlacableChoise _settedChoise;

		[ConCmd.Client( "set_choise" )]
		public static void SetChoise( string model, string EntName )
		{
			_settedChoise = new PlacableChoise( EntName, model );

		}
	}
}

