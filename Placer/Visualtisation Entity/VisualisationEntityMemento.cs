using Sandbox.Placer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Visualtisation_Entity
{




	public class VisualisationEntityMemento
	{


		protected bool CanBePlaced { get; set; }








		protected VisualisationEntity visualisationModel = default;

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

			if ( !isEntExist )
			{
				visualisationModel = new VisualisationEntity( placermodelpath );
				visualisationModel.Spawn();
				isEntExist = true;
			}
			else
			{
				oldmodelpath = visualisationModel.ModelPath;
			}

			if ( oldmodelpath != placermodelpath )
			{
				visualisationModel.SetModel( placermodelpath );
			}






			visualisationModel.SetPositionAndRotation( entPosition, entRotation );

			CanBePlaced = visualisationModel.IsInCorrectPosition;




		}

		public virtual void DeleteEntity()
		{
			visualisationModel.Delete();

			CanBePlaced = false;

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
}
