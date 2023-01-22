


using PlaceLib.Placer.Abstractions;
using PlaceLib.Placer.Utils;
using PlaceLib.VisualisationEntity;
using PlaceLib.VisualisationEntity.Abstraction;
using Prefabs.RenderChangers;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlaceLib.Placer.PlaceSystem
{

	public sealed class PlaceVisualisaton
	{

		private float MaxTargetDistance;


		private VisEntity VisualisationEntity { get; set; }

		private IEnumerable<Predicate<TraceResult>> PlaceTerms;



		private IEnumerable<IAdditionalVisualisation> AdditionalVisualisations;


		public bool IsInCorrectPosition => VisualisationEntity.isInCorrectPosition;


		public void Delete()
		{
			VisualisationEntity.Hide();
		}


		public void UpdateVisualisation( Vector3 eyePos, Vector3 eyeDir, Rotation eyeRot, Player owner, PlacableChoise currentchoise )
		{
			

				var tr = Trace.Ray( eyePos, eyePos + eyeDir * MaxTargetDistance )
					.WorldAndEntities()
					.WithTag( "solid" )
					.Ignore( owner )
					.Run();




			if ( PlaceTerms.Count() > 0 )
			{
				bool isAllOk = true;
				foreach ( var term in PlaceTerms )
				{
					isAllOk = isAllOk && !term.Invoke( tr );
				}
				if ( !isAllOk )
				{
					VisualisationEntity.Hide();
					return;
				}
			}


				if ( VisualisationEntity.Model == null ) VisualisationEntity.SetModel( currentchoise.Model );

				if ( currentchoise.Model != VisualisationEntity.Model.ResourcePath ) VisualisationEntity.SetModel( currentchoise.Model );


				var newpos = tr.HitPosition + tr.Normal * VisualisationEntity.CollisionBounds.Size * 0.55f; ;


				var newrot = Rotation.LookAt( eyeRot.Forward, tr.Normal );

				var newtransform = new Transform( newpos, newrot );

				VisualisationEntity.Transform = newtransform;

				VisualisationEntity.Show();


			foreach ( var visualisator in AdditionalVisualisations )
			{
				visualisator.DeleteAdditionalVisualisation();
			}


			foreach ( var visualisator in AdditionalVisualisations )
			{
				if ( IsInCorrectPosition )
				{
					visualisator.OnCorrectPosition( eyePos, eyeDir, eyeRot, newtransform, owner );
				}
				else
				{
					visualisator.OnIncorrectPosition( eyePos, eyeDir, eyeRot, newtransform, owner );
				}
			}












		}

		public Transform GetVisualisationTransform()
		{

			return new Transform( VisualisationEntity.Position, VisualisationEntity.Rotation );

		}


		public PlaceVisualisaton( VisEntity visEntity, IEnumerable<IAdditionalVisualisation> adVisual, IEnumerable<Predicate<TraceResult>> placeterms, float maxTargetDistance)
		{
			Game.AssertClient();

			VisualisationEntity = visEntity;

			AdditionalVisualisations = adVisual;

			PlaceTerms = placeterms;
			MaxTargetDistance= maxTargetDistance;
		}

		~PlaceVisualisaton()
		{
			Delete();
		}
	}

	



}
