using Sandbox.Placer.Placer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer.PlaceSystem
{
	public partial class EntityCreator : BaseNetworkable
	{




		public virtual void TryCreateEntityOnServer(Player creator, PlacableChoise choise, Vector3 position, Rotation rotation )
		{

			CreateEntity(choise.EntityName, position, rotation);
		}


		[ConCmd.Server]
		protected static void CreateEntity(string EntityName, Vector3 position, Rotation rotation )
		{
			
			var ent = TypeLibrary.Create(EntityName, typeof(Entity)) as Entity;

			ent.Position = position;
			ent.Rotation = rotation;


		}
	}
}
