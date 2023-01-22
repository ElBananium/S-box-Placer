using PlaceLib.Placer.Abstractions;
using PlaceLib.VisualisationEntity.Abstraction;
using PlaceLib.VisualisationEntity;
using Prefabs.RenderChangers;
using System;
using System.Collections.Generic;

using Sandbox;

namespace PlaceLib.Placer.PlaceSystem
{
	public class PlaceVisualisationBuilder
	{
		private IEntityRenderChanger _renderChanger;

		private List<Predicate<TraceResult>> _placeterms;

		private List<IAdditionalVisualisation> AdditionalVisualisations;

		private float maxTargetDistance = 100f;


		public PlaceVisualisationBuilder WithBaseEntityRendered()
		{
			_renderChanger = new BaseEntityRenderChanger();

			return this;
		}

		public PlaceVisualisationBuilder SetMaxTargetDistance( float distance )
		{
			maxTargetDistance = distance;

			return this;
		}

		public PlaceVisualisationBuilder SetEntityRenderer( IEntityRenderChanger renderChanger )
		{
			_renderChanger = renderChanger;

			return this;
		}


		public PlaceVisualisationBuilder AddAdditionalVisualisator( IAdditionalVisualisation additionalVisualisation )
		{
			AdditionalVisualisations.Add( additionalVisualisation );

			return this;
		}

		public PlaceVisualisationBuilder AddPlaceTerm( Predicate<TraceResult> placeterm )
		{
			_placeterms.Add( placeterm );
			return this;
		}



		public PlaceVisualisationBuilder()
		{
			AdditionalVisualisations = new List<IAdditionalVisualisation>();

			_placeterms = new List<Predicate<TraceResult>>();
		}








		public PlaceVisualisaton Build()
		{
			if ( _renderChanger == null )
			{
				WithBaseEntityRendered();
			}

			var visent = new VisEntity( _renderChanger );

			return new PlaceVisualisaton( visent, AdditionalVisualisations, _placeterms, maxTargetDistance );
		}
	}
}
