using Sandbox.Placer.Placer.PlacableChoiser;
using Sandbox.Placer.Placer.PlaceSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer
{
	public partial class BasePlacerTool : Carriable
	{
		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";


		protected PlaceVisualisator placeVisualisator;

		protected IPlacableChoiser placableChoiser;


		protected EntityCreator entityCreator;


		public BasePlacerTool()
		{
			entityCreator = new EntityCreator();

			placableChoiser = new BasePlacableChoiser();


			if ( Game.IsClient )
			{
				placeVisualisator = new PlaceVisualisatiorBuilder().WithBaseEntityRendered().SetPlacableChoiser( placableChoiser ).Build();
			}
		}








		public override void DestroyViewModel()
		{

			placeVisualisator.HideVisualisation();
			base.DestroyViewModel();

			
		}



		public override void Simulate( IClient client )
		{

			if ( Owner is not Player owner ) return;


			var eyePos = owner.EyePosition;
			var eyeDir = owner.EyeRotation.Forward;
			var eyeRot = Rotation.From( new Angles( 0.0f, owner.EyeRotation.Yaw(), 0.0f ) );
			var placing = false;


			if ( Input.Pressed( InputButton.PrimaryAttack ) )placing = true;



			if ( Game.IsClient )
			{

				using ( Prediction.Off() )
				{

					placeVisualisator.UpdateVisualisation( eyePos, eyeDir, eyeRot, owner );
					
					if (placing && placeVisualisator.IsInCorrectPosition() )
					{
						
						var posandrot = placeVisualisator.GetVisualisationModelPositionAndRotation();

						entityCreator.TryCreateEntityOnServer(owner, this.placableChoiser.GetCurrentChoise(), posandrot.Position, posandrot.Rotations );
					}
					

				}
			}
		


		}



	}
}
