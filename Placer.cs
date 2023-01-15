using Sandbox.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.tools.MyTools
{
	public partial class Placer : Carriable
	{
		public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";


		private IVisualisationEntityMemento visualisationEntityMemento;

		private PlacerState placerState;

		public Placer()
		{
			if ( Game.IsClient )
			{
				this.visualisationEntityMemento = new VisualisationEntityMemento();

				
			}

			placerState = new PlacerState();
		}



		

		[Net,Predicted]
		public string SpawnType { get; set; }


		protected float MaxTargetDistance = 100f;

		public override void Spawn()
		{
			base.Spawn();

			Tags.Add( "weapon" );
			SetModel( "weapons/rust_pistol/rust_pistol.vmdl" );
		}


		public override void DestroyViewModel()
		{
			base.DestroyViewModel();
			if ( visualisationEntityMemento.IsEntityExist() && Game.IsClient ) visualisationEntityMemento.DeleteEntity();
		}

		public override void Simulate( IClient client )
		{
			if ( Owner is not Player owner ) return;


			var eyePos = owner.EyePosition;
			var eyeDir = owner.EyeRotation.Forward;
			var eyeRot = Rotation.From( new Angles( 0.0f, owner.EyeRotation.Yaw(), 0.0f ) );
			var placing = false;


			if ( Input.Pressed( InputButton.PrimaryAttack ) )
			{
				(Owner as AnimatedEntity)?.SetAnimParameter( "b_attack", true );
				placing = true;


			}


			if ( !Game.IsClient ) return;

			using ( Prediction.Off() )
			{

				if ( Input.Pressed( InputButton.Use ) )
				{
					placerState.SelectEntity( "Acid" );


				}

				if ( Input.Pressed( InputButton.Reload ) )
				{
					placerState.UnSelectEntity();
				}


				var tr = Trace.Ray( eyePos, eyePos + eyeDir * MaxTargetDistance )
				.WorldOnly()
				.Ignore( this )
				.Run();







				if ( tr.Hit && placerState.IsSelectedAny )
				{
					if ( placing && this.visualisationEntityMemento.CanEntityBePlaced() )
					{
						PlaceEntity( this.visualisationEntityMemento.GetEntityPosition(), this.visualisationEntityMemento.GetEntityRotation(), placerState.GetEntitySpawnTag() );
					}

					visualisationEntityMemento.CreateOrUpdateEntity( placerState, tr.HitPosition, eyeRot );
				}



				else if ( visualisationEntityMemento.IsEntityExist() ) visualisationEntityMemento.DeleteEntity();










			}


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

	public class PlacerState
	{


		public bool IsSelectedAny { get; protected set; }




		protected string SelectedEntityName { get; set; }


		protected string SelectEntityModel { get; set; }

		protected string SelectEntityTag{ get; set; }


		public PlacerState()
		{
			IsSelectedAny = false;
		}

		public string GetModelPath()
		{

			return SelectEntityModel;
		}

		public string GetEntitySpawnTag()
		{

			return SelectEntityTag;
		}

		public void SelectEntity(string name)
		{
			var attr = TypeLibrary.GetTypesWithAttribute<PlacableAttribute>();

			foreach(var a in attr )
			{
				if( a.Attribute.Name == name )
				{
					SelectedEntityName = a.Attribute.Name;
					SelectEntityModel = a.Attribute.Model;
					SelectEntityTag = a.Attribute.SpawnTag;
					IsSelectedAny= true;
				}
			}


		}

		public void UnSelectEntity()
		{
			SelectedEntityName = default;
			SelectEntityModel = default;
			SelectEntityTag = default;
			IsSelectedAny = false;

		}












	}


	public interface IVisualisationEntityMemento
	{

		public bool CanEntityBePlaced();


		public Vector3 GetEntityPosition();

		public Rotation GetEntityRotation();

		public bool IsEntityExist();

		public void CreateOrUpdateEntity( PlacerState placerState, Vector3 entPosition, Rotation entRotation );


		public void DeleteEntity();


	}


	public partial class VisualisationEntityMemento : IVisualisationEntityMemento
	{


		protected bool CanBePlaced { get; set; }



		




		protected ClientPlacerVisualtsationModelEntity visualisationModel = default;

		protected bool isEntExist;

		public bool CanEntityBePlaced()
		{
			return isEntExist && CanBePlaced;
		}

		public virtual void CreateOrUpdateEntity( PlacerState placerState, Vector3 entPosition, Rotation entRotation )
		{
			Game.AssertClient();
			var placermodelpath = placerState.GetModelPath();

			var oldmodelpath = "default";

			if (!isEntExist)
			{
				visualisationModel = new ClientPlacerVisualtsationModelEntity(placermodelpath);
				visualisationModel.Spawn();
				isEntExist = true;
			}
			else
			{
				oldmodelpath = visualisationModel.ModelPath;
			}

			if(oldmodelpath != placermodelpath )
			{
				visualisationModel.SetModel( placermodelpath );
			}





			
			visualisationModel.SetPositionAndRotation( entPosition, entRotation );

			CanBePlaced= visualisationModel.IsInCorrectPosition;


			

		}

		public virtual void DeleteEntity()
		{
			visualisationModel.Delete();

			CanBePlaced= false;

			isEntExist = false;
		}

		public virtual bool IsEntityExist()
		{
			return isEntExist;
		}

		public Vector3 GetEntityPosition()
		{
			return visualisationModel.Position;
		}

		public Rotation GetEntityRotation()
		{
			return visualisationModel.Rotation;
		}

		public VisualisationEntityMemento()
		{
			Game.AssertClient();
		}
	}






	public class ClientPlacerVisualtsationModelEntity : ModelEntity
	{
		public string ModelPath { get; set; }


		public ClientPlacerVisualtsationModelEntity( string modelpath)
		{
			ModelPath = modelpath;
		}


		public ClientPlacerVisualtsationModelEntity()
		{
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
		




		[Event("client.tick")]
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



	public class PlacableAttribute : Attribute
	{
		public string Model { get; set; }

		public string SpawnTag { get; set; }


		public string Name { get; set; }


		
		public PlacableAttribute(string model, string spawnTag, string name)
		{
			Model = model;
			SpawnTag = spawnTag;

			Name = name;
		}

	}


	
}
