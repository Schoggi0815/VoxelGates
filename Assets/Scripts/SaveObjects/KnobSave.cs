using System;
using Line;

namespace SaveObjects
{
	[Serializable]
	public class KnobSave : LineDrawerSave
	{
		public int id;

		public KnobSave(GridPosition gridPosition, LineDrawerMode lineDrawerMode, bool isActive, int id) : base(gridPosition, lineDrawerMode, isActive)
		{
			this.id = id;
		}
	}
}
