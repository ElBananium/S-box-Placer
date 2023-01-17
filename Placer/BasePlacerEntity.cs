using Sandbox.Placer.Utilities;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sandbox.Event;

namespace Sandbox.Placer
{

	[Spawnable]
	[Library( "ent_placer", Title = "Placer" )]
	public partial class BasePlacerEntity : Prop, IUse
	{

		private PlaceSystem placeSystem;



		public override void Spawn()
		{

			base.Spawn();
			SetModel( "models/ball/ball.vmdl" );

			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );










		}

		public BasePlacerEntity()
		{

			if ( Game.IsClient )
			{
				placeSystem = new PlaceSystem( 100f, new Visualtisation_Entity.VisualisationEntityMemento(), new PlacerState() );


			}
		}
		public virtual bool IsUsable( Entity user )
		{
			return true;
		}


		

		public virtual bool OnUse( Entity user )
		{
			Game.AssertServer();

			if ( user is not Player player ) return false;


			

			if ( player.Tags.Has( $"{NetworkIdent}_placing" ) )
			{

				player.Tags.Remove( $"{NetworkIdent}_placing" );

				StartOrStopPlacing( To.Single(player.Client), false );
			}
			else
			{
				player.Tags.Add( $"{NetworkIdent}_placing" );
				//StartOrStopPlacing( To.Single( player.Client ), true );


				
					
				
			}

			


			return false;


		}



		[ClientRpc]

		public void StartOrStopPlacing(bool start)
		{
			Game.AssertClient();

			if ( !start )
			{
				Game.RootPanel.Style.Set( "display: flex;" );

				//placeSystem.UpdateVisualisation()
			}
			else
			{
				Game.RootPanel.Style.Set( "display: none;" );
			}
		}



		




		
	}

}
