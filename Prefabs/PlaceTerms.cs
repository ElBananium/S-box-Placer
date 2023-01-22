
using Sandbox;

namespace Prefabs
{
	public class PlaceTerms
	{



		public static bool OnlyOnGround( TraceResult tr )
		{
			return (tr.Normal - Game.PhysicsWorld.Gravity.Normal).Length < 1.8f;
		}
	}
}
