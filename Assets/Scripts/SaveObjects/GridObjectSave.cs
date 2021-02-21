using System;

namespace SaveObjects
{
	[Serializable]
	public abstract class GridObjectSave
	{
		public GridPosition gridPosition;

		protected GridObjectSave(GridPosition gridPosition)
		{
			this.gridPosition = gridPosition;
		}

		public abstract object Create();
	}
}