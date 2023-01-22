
using Prefabs.PlacableChoisers;
using PlaceLib.Placer.PlaceSystem;
using Prefabs;
using Sandbox;
using PlaceLib.Placer.Abstractions;
using Prefabs.EntityCreators;

namespace PlaceLib.Placer
{
	public abstract partial class BasePlacerTool : Carriable
	{
		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";


		protected PlaceVisualisaton placeVisualisator;


		[Net]
		protected BasePlacableChoiser placableChoiser { get; set; }

		[Net]
		protected BaseEntityCreator entityCreator { get; set; }



		/// <summary>
		/// Calls on server side, to get EntityCreator, which will be responsible for the conditions for placing entities
		/// </summary>
		protected abstract BaseEntityCreator BuildEntityCreator();

		/// <summary>
		/// Calls on server side, to get PlacableChoiser, which will be responsible for the conditions for choose entity
		/// </summary>
		protected abstract BasePlacableChoiser BuildPlacableChooser();

		/// <summary>
		///  Calls on client side, to get PlacableChoiser, which will be responsible for the conditions for choose entity
		/// </summary>
		/// <returns></returns>
		protected abstract PlaceVisualisaton BuildPlaceVisualisation();




		public override void Spawn()
		{
			entityCreator = BuildEntityCreator();

			placableChoiser = BuildPlacableChooser();

			base.Spawn();
		}

		
		public override void ClientSpawn()
		{



			placeVisualisator = BuildPlaceVisualisation();




			base.ClientSpawn();
			
			
		}




		public override void DestroyViewModel()
		{

			placeVisualisator.Delete();
			base.DestroyViewModel();

			
		}



		public override void Simulate( IClient cl )
		{
			
				base.Simulate( cl );


				if ( Owner is not Player owner ) return;



				var eyePos = owner.EyePosition;
				var eyeDir = owner.EyeRotation.Forward;
				var eyeRot = Rotation.From( new Angles( 0.0f, owner.EyeRotation.Yaw(), 0.0f ) );
				var placing = false;


				if ( Input.Pressed( InputButton.PrimaryAttack ) ) placing = true;


				if ( !Game.IsClient ) return;



				placeVisualisator.UpdateVisualisation( eyePos, eyeDir, eyeRot, owner, this.placableChoiser.CurrentChoise );

				if ( placing && placeVisualisator.IsInCorrectPosition )
				{

					var posandrot = placeVisualisator.GetVisualisationTransform();

					entityCreator.TryCreateEntityOnServer( owner, this.placableChoiser.CurrentChoise, posandrot.Position, posandrot.Rotation );
				}
			
		}




	}



	}

