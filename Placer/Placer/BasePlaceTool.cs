using Sandbox.Placer.Placer.PlaceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer
{
	public partial class BasePlacerTool : Carriable
	{
		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";


		protected PlaceVisualisator placeVisualisator = new PlaceVisualisatiorBuilder().WithBaseEntityRendered().WithBasePlacableChoiser().Build();

		public BasePlacerTool()
		{


		}








		public override void Spawn()
		{
			base.Spawn();

			Tags.Add( "weapon" );
			SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
		}











		public override void Simulate( IClient client )
		{
			if ( Owner is not Player owner ) return;


			var eyePos = owner.EyePosition;
			var eyeDir = owner.EyeRotation.Forward;
			var eyeRot = Rotation.From( new Angles( 0.0f, owner.EyeRotation.Yaw(), 0.0f ) );
			var placing = false;


			if ( Input.Pressed( InputButton.PrimaryAttack ) )
			{
				(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
				placing = true;


			}


			if ( !Game.IsClient ) return;

			using ( Prediction.Off() )
			{

				placeVisualisator.UpdateVisualisation( eyePos, eyeDir, eyeRot, owner );



			}


		}
	}
}
