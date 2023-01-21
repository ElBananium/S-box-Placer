using Sandbox.Placer.VisualisationEntity.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.VisualisationEntity
{
	public class VisEntity : ModelEntity
	{

		protected string _modelpath;


		protected IEntityRenderChanger _renderChanger;

		public VisEntity( string modelpath, IEntityRenderChanger renderChanger)
		{
			Game.AssertClient();

			_modelpath = modelpath;
			_renderChanger = renderChanger;



		}

		public VisEntity()
		{
			Log.Error( "Visualisation entity need model and EntityRenderChanger" );
		}

		public override void Spawn()
		{
			Game.AssertClient();


			SetModel( _modelpath );
			SetupPhysicsFromModel( PhysicsMotionType.Keyframed, false );

			base.Spawn();


		}

		protected bool inCorrectPoisition = false;

		[Event( "client.tick" )]

		public void OnClientTick()
		{



			if ( ishidden ) return;



			if ( IsBodyHitWorld() )
			{
				_renderChanger.OnWrongPosition(this);
				inCorrectPoisition = false;
			}
			else
			{

				_renderChanger.OnCorrectPosition(this);
				inCorrectPoisition = true;
			}
		}

		protected bool IsBodyHitWorld()
		{
			var tr = Trace.Body( this.PhysicsBody, this.Position ).WorldAndEntities().Run();

			DebugOverlay.TraceResult( tr );

			return tr.Hit;
		}



		protected bool ishidden = true;

		public bool isInCorrectPosition => !ishidden && inCorrectPoisition;

		public void Hide()
		{
			this.RenderColor = this.RenderColor.WithAlpha( 0 );
			ishidden = true;
		}

		public void Show()
		{
			this.RenderColor = this.RenderColor.WithAlpha( 1f );
			ishidden = false;
		}
	}
}
