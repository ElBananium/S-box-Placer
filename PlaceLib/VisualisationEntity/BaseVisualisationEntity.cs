
using Editor;
using PlaceLib.VisualisationEntity.Abstraction;
using Sandbox;

namespace PlaceLib.VisualisationEntity
{

	[CanBeClientsideOnly]
	public class VisEntity : ModelEntity
	{


		protected IEntityRenderChanger _renderChanger;

		public VisEntity(IEntityRenderChanger renderChanger)
		{
			Game.AssertClient();


			_renderChanger = renderChanger;



		}

		public VisEntity()
		{
			Log.Error( "Visualisation entity need EntityRenderChanger" );
		}

		public override void Spawn()
		{
			Game.AssertClient();

			SetModel( "models/ball/ball.vmdl" );
			SetupPhysicsFromModel( PhysicsMotionType.Keyframed, false );
			EnableShadowCasting = false;
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

			return tr.Hit;
		}



		protected bool ishidden = true;

		public bool isInCorrectPosition => !ishidden && inCorrectPoisition;

		public void Hide()
		{
			_renderChanger.OnHide();
			EnableDrawing = false;
			ishidden = true;
		}

		public void Show()
		{
			EnableDrawing = true;
			ishidden = false;
		}
	}
}
