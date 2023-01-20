using Sandbox.Placer.Placer.PlacableChoiser;
using Sandbox.Placer.Placer.Utils;
using Sandbox.Placer.VisualisationEntity;
using Sandbox.Placer.VisualisationEntity.Abstraction;
using Sandbox.Placer.VisualisationEntity.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer.PlaceSystem
{
	public class PlaceVisualisator
	{

		protected float MaxTargetDistance = 200f;


		protected VisEntity VisualisationEntity { get; set; }

		protected IPlacableChoiser PlacableChoiser { get; set; }

		public bool IsInCorrectPosition()
		{
			return VisualisationEntity.isInCorrectPosition;
		} 

		public virtual void UpdateVisualisation( Vector3 eyePos, Vector3 eyeDir, Rotation eyeRot, Player owner )
		{



			var tr = Trace.Ray( eyePos, eyePos + eyeDir * MaxTargetDistance )
				.WorldAndEntities()
				.Ignore( owner )
				.Run();



			var currentchoise = PlacableChoiser.GetCurrentChoise();

			if ( !tr.Hit )
			{
				VisualisationEntity.Hide();
				return;
			}



			if ( (tr.Normal - Game.PhysicsWorld.Gravity.Normal).Length <1.8f )
			{
				VisualisationEntity.Hide();
				return;
			}
			

			VisualisationEntity.SetModel( currentchoise.Model );



			VisualisationEntity.Position = tr.HitPosition + tr.Normal* VisualisationEntity.CollisionBounds.Size * 0.5f; ;


			VisualisationEntity.Rotation = Rotation.LookAt(eyeRot.Forward, tr.Normal );

			VisualisationEntity.Show();








		}


		public PlaceVisualisator( VisEntity visEntity, IPlacableChoiser placableChoiser)
		{
			Game.AssertClient();

			VisualisationEntity = visEntity;

			PlacableChoiser = placableChoiser;
		}

		~PlaceVisualisator()
		{
			VisualisationEntity.Delete();
		}
	}

	public class PlaceVisualisatiorBuilder
	{
		private IEntityRenderChanger _renderChanger;

		private IPlacableChoiser _placableChoiser;



		public PlaceVisualisatiorBuilder WithBaseEntityRendered()
		{
			_renderChanger = new BaseEntityRenderChanger();

			return this;
		}

		public PlaceVisualisatiorBuilder SetEntityRenderer(IEntityRenderChanger renderChanger )
		{
			_renderChanger = renderChanger;

			return this;
		}




		public PlaceVisualisatiorBuilder SetPlacableChoiser( IPlacableChoiser choiser)
		{
			_placableChoiser = choiser;

			return this;
		}

		public PlaceVisualisatiorBuilder WithBasePlacableChoiser()
		{
			_placableChoiser = new BasePlacableChoiser();
			return this;
		}




		public PlaceVisualisator Build()
		{
			if(_renderChanger== null )
			{
				WithBaseEntityRendered();
			}
			if(_placableChoiser == null )
			{
				WithBasePlacableChoiser();
			}

			var visent = new VisEntity( _placableChoiser.GetCurrentChoise().Model, _renderChanger );

			return new PlaceVisualisator(visent, _placableChoiser);
		}
	}



}
