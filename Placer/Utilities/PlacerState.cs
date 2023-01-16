using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Utilities
{
	public class PlacerState
	{


		public bool IsSelectedAny { get; protected set; }




		protected string SelectedEntityName { get; set; }


		protected string SelectEntityModel { get; set; }

		protected string SelectEntityTag { get; set; }


		public PlacerState()
		{
			Game.AssertClient();
			IsSelectedAny = false;
		}

		public virtual string GetModelPath()
		{

			return SelectEntityModel;
		}

		public virtual string GetEntitySpawnTag()
		{

			return SelectEntityTag;
		}

		public virtual void SelectEntity( string name )
		{
			var attr = TypeLibrary.GetTypesWithAttribute<PlacableAttribute>();

			foreach ( var a in attr )
			{
				if ( a.Attribute.Name == name )
				{
					SelectedEntityName = a.Attribute.Name;
					SelectEntityModel = a.Attribute.Model;
					SelectEntityTag = a.Attribute.SpawnTag;
					IsSelectedAny = true;
				}
			}


		}

		public virtual void UnSelectEntity()
		{
			SelectedEntityName = default;
			SelectEntityModel = default;
			SelectEntityTag = default;
			IsSelectedAny = false;

		}
	}
}
