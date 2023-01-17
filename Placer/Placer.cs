using Sandbox.Placer.Utilities;
using Sandbox.Placer.Visualtisation_Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer
{
	public class PlaceSystem
	{
		private VisualisationEntityMemento VisualisationEntityMemento;

		public readonly PlacerState PlacerState;

		protected float MaxTargetDistance = 100f;


		public bool IsVisualisationShow { get => VisualisationEntityMemento.IsEntityExist(); }




		public PlaceSystem(float maxTargetDistance, VisualisationEntityMemento visualisationEntityMemento, PlacerState placerState )
		{
			Game.AssertClient();

			MaxTargetDistance = maxTargetDistance;

			VisualisationEntityMemento = visualisationEntityMemento;

			PlacerState= placerState;

			placerState.SelectEntity( "Acid" );
		}





		public virtual void TryPlaceEntity()
		{
			Game.AssertClient();
			if ( VisualisationEntityMemento.CanEntityBePlaced() )
			{
				PlaceEntity( VisualisationEntityMemento.GetEntityPosition(), VisualisationEntityMemento.GetEntityRotation(), PlacerState.GetEntitySpawnTag() );
			}
		}


		public virtual void UpdateVisualisation( Vector3 eyePos, Vector3 eyeDir, Rotation eyeRot, Player owner )
		{


			Game.AssertClient();

			var tr = Trace.Ray( eyePos, eyePos + eyeDir * MaxTargetDistance )
				.WorldOnly()
				.Ignore( owner )
				.Run();







			if ( tr.Hit && PlacerState.IsSelectedAny )
			{


				VisualisationEntityMemento.CreateOrUpdateEntity( PlacerState, tr.HitPosition, eyeRot );
			}



			else if ( IsVisualisationShow ) VisualisationEntityMemento.DeleteEntity();
		}





		public virtual void HideVisualisation()
		{
			if ( !IsVisualisationShow ) return;
			
			
			this.VisualisationEntityMemento.DeleteEntity();
		}







		[ConCmd.Server]
		public static void PlaceEntity( Vector3 position, Rotation rotation, string spawntag )
		{
			Game.AssertServer();





			var ent = TypeLibrary.Create<Entity>( spawntag );

			ent.Position = position;

			ent.Rotation = rotation;



		}






	}
}
