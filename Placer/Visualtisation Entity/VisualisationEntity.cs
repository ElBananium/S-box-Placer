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


		public void SetPositionAndRotation( Vector3 position, Rotation rotation )
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


		public bool IsInCorrectPosition { get; protected set; }





		[Event( "client.tick" )]
		public void OnClientTick()
		{
			Game.AssertClient();


			var tr = Trace.Body( PhysicsBody, this.Position ).WorldAndEntities().Run();


			if ( tr.Hit )
			{
				RenderColor = Color.Red;
				IsInCorrectPosition = false;

			}

			else
			{
				IsInCorrectPosition = true;
				RenderColor = Color.Green;
			}
		}
	}
}
