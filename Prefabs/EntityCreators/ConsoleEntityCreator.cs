

using PlaceLib.Placer.Abstractions;
using PlaceLib.Placer.Utils;
using Sandbox;

namespace Prefabs.EntityCreators
{
	public class ConsoleEntityCreator : BaseEntityCreator
	{
		



		public override void TryCreateEntityOnServer(Player creator, PlacableChoise choise, Vector3 position, Rotation rotation )
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
