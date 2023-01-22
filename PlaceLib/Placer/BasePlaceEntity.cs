
using PlaceLib.Placer.PlaceSystem;
using System.Linq;
using Sandbox;
using PlaceLib.Placer.Abstractions;


namespace PlaceLib.Placer
{

	public abstract partial  class BasePlaceEntity : AnimatedEntity
	{

		protected abstract PlayerPlacingComponent BuildAndConfigurePlacingComponent();


		/// <summary>
		/// Start placing for current player
		/// </summary>
		/// <param name="player">Player, which start placing</param>
		protected virtual void StartOrStorPlacing(Player player )
		{
			Game.AssertServer();
			var comp = player.Components.GetAll<PlayerPlacingComponent>().FirstOrDefault( x => x.PlaceEntityId == this.NetworkIdent );

			if ( comp != default )
			{
				player.Components.Remove( comp );
			}
			else
			{
				var component = BuildAndConfigurePlacingComponent();
				player.Components.Add( component );
			}
		}


		protected override void OnDestroy()
		{
			if ( !Game.IsServer ) return;

			foreach(var player in GameManager.All )
			{
				if ( player is not Player ) continue;



				var comp = player.Components.GetAll<PlayerPlacingComponent>().FirstOrDefault( x => x.PlaceEntityId == this.NetworkIdent );

				if ( comp != default )
				{
					player.Components.Remove( comp );
				}
			}


			base.OnDestroy();
		}


	}



	
	public abstract partial class PlayerPlacingComponent : EntityComponent
	{
		[Net]
		protected BasePlacableChoiser PlacableChoiser { get; set; }

		
		protected PlaceVisualisaton placeVisualisator;

		[Net]
		protected BaseEntityCreator entityCreator { get; set; }

		[Net]
		public int PlaceEntityId { get; set; }

		[Net]
		public float MaxEntityDistance { get; set; }


		public abstract PlaceVisualisaton BuildPlaceVisualisation();

		public virtual void Configure( int EntId, BaseEntityCreator entitycreator, BasePlacableChoiser placableChoiser,float maxEntityDistnace )
		{
			PlacableChoiser = placableChoiser;
			entityCreator = entitycreator;
			PlaceEntityId = EntId;
			MaxEntityDistance= maxEntityDistnace;
		}

		protected override void OnActivate()
		{
			if ( Game.IsClient )
			{
				placeVisualisator = BuildPlaceVisualisation();
			}
			base.OnActivate();
		}

		protected override void OnDeactivate()
		{
			if ( Game.IsClient )
			{
				placeVisualisator.Delete();
			}
		}


		[Event.Tick]
		public virtual void OnTick()
		{
			var isInSphere = GameManager.FindInSphere( Entity.Position, MaxEntityDistance ).Any( x => x.NetworkIdent == PlaceEntityId );



			if ( !isInSphere ) Entity.Components.Remove( this );
		}



		[Event("client.tick")]
		public virtual void OnClientTick()
		{
			
				Game.AssertClient();
				var placing = false;
				if ( Input.Pressed( InputButton.PrimaryAttack ) ) placing = true;
				var owner = (Entity as Player);

				var eyePos = owner.EyePosition;
				var eyeDir = owner.EyeRotation.Forward;
				var eyeRot = Rotation.From( new Angles( 0.0f, owner.EyeRotation.Yaw(), 0.0f ) );


				placeVisualisator.UpdateVisualisation( eyePos, eyeDir, eyeRot, owner, this.PlacableChoiser.CurrentChoise );

				if ( placing && placeVisualisator.IsInCorrectPosition )
				{
					
					var posandrot = placeVisualisator.GetVisualisationTransform();

					entityCreator.TryCreateEntityOnServer( owner, this.PlacableChoiser.CurrentChoise, posandrot.Position, posandrot.Rotation );
				}
			
						
		}


	}
}
