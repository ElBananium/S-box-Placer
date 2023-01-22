

using PlaceLib.Placer.Utils;
using Sandbox;

namespace PlaceLib.Placer.Abstractions
{

	/// <summary>
	/// Base class which is responsible for placing objects on the server, inherit for it to determine the placement conditions
	/// </summary>
	public abstract class BaseEntityCreator : BaseNetworkable
	{



		/// <summary>
		/// Calls when client try to place entity
		/// </summary>
		/// <param name="creator">Client which try to place</param>
		/// <param name="choise">Choise of client</param>
		/// <param name="position">Position, which client choosen</param>
		/// <param name="rotation">Rotation, which client choosen</param>
		public abstract void TryCreateEntityOnServer( Player creator, PlacableChoise choise, Vector3 position, Rotation rotation );



	}
}
