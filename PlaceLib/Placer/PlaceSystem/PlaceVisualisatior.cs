


using PlaceLib.Placer.Abstractions;
using PlaceLib.Placer.Utils;
using PlaceLib.VisualisationEntity;
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
			foreach ( var visualisator in AdditionalVisualisations )
			{
				visualisator.DeleteAdditionalVisualisation();
			}

			VisualisationEntity.Hide();
		}

		public void Hide()
		{
			foreach ( var visualisator in AdditionalVisualisations )
			{
				visualisator.HideAdditionalVisualisation();
			}

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
						Hide();
						return;
					}
				}

				if ( VisualisationEntity.Model == null ) VisualisationEntity.SetModel( currentchoise.Model );


				else if ( currentchoise.Model != VisualisationEntity.Model.ResourcePath ) VisualisationEntity.SetModel( currentchoise.Model );


			var newpos = tr.HitPosition;

			var newrot = Rotation.LookAt(eyeRot.Backward, tr.Normal);
			var newtransform = new Transform( newpos, newrot );

			




			if(Trace.Body(VisualisationEntity.PhysicsBody, newtransform, newtransform.Position ).WorldOnly().Run().Hit)
			{
				newpos = tr.HitPosition + tr.Normal * 15f;
				newtransform = new Transform( newpos, newrot );
			}


			
			VisualisationEntity.Transform = newtransform;

			VisualisationEntity.Show();





				foreach ( var visualisator in AdditionalVisualisations )
				{
					visualisator.ShowAdditionVisualisation();

					if ( IsInCorrectPosition )
					{

						visualisator.OnCorrectPosition( owner );
					}
					else
					{
						visualisator.OnIncorrectPosition( owner );
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


			foreach(var advis in adVisual )
			{
				advis.CreateAdditionalVisualisation(VisualisationEntity);
			}
		}

		~PlaceVisualisaton()
		{
			Delete();
		}
	}

	



}
