using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Placer.Placer.Utils
{
	public class PositionAndRotation
	{

		public Vector3 Position { get; protected set; }

		public Rotation Rotations { get; protected set; }

		public PositionAndRotation(Vector3 position, Rotation rotation)
		{
			Position= position;
			Rotations= rotation;

		}
	}
}
