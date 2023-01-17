using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Visualtisation_Entity
{
	public class VisualisationEntity : ModelEntity
	{
		public string ModelPath { get; set; }


		public VisualisationEntity( string modelpath )
		{
			ModelPath = modelpath;
		}


		public VisualisationEntity()
		{
			Log.Error( "VisualisationEntity need model" );

			ModelPath = "models/ball/ball.vmdl";

		}

		public override void Spawn()
		{
			Game.AssertClient();


			SetModel( ModelPath );







			SetupPhysicsFromModel( PhysicsMotionType.Keyframed, false );



			base.Spawn();


		}


		public virtual void SetPositionAndRotation( Vector3 position, Rotation rotation )
		{
			if ( GetAttachment( "bottom" ).GetValueOrDefault() == default )
			{
				Log.Warning( "Need to bottom attachment to model" );
			}


			this.Rotation = rotation;



			var attachmentpos = GetAttachment( "bottom", true ).GetValueOrDefault().Position;

			var delta = attachmentpos - this.Position;



			this.Position = position - delta;










		}


		[Event("client.tick")]

		public void OnClientTick()
		{
			var tr = Trace.Body( this.PhysicsBody, this.Position ).WorldOnly().Run();

			if ( tr.Hit )
			{
				OnEntityInNotCorrectPosition();
			}
			else
			{
				
				OnEntityInCorrectPosition();
			}
		}

		public bool IsInCorrectPosition { get; protected set; }






		public virtual void OnEntityInCorrectPosition()
		{
			IsInCorrectPosition = true;
			RenderColor = Color.Green;
		}


		public virtual void OnEntityInNotCorrectPosition()
		{
			RenderColor = Color.Red;
			IsInCorrectPosition = false;
		}
	}
}
