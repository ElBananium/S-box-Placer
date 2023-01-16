using Sandbox.Placer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if ( user is not Player player ) return false;



			if ( !player.Inventory.Contains(new BasePlacerTool()) )
				{
				player.Inventory.Add( new BasePlacerTool(), true );
					
				}
				else
				{
				player.Inventory.DeleteContents();





			}
			


			return true;


		}




		
	}
}
