using System;
using Line;

namespace SaveObjects
{
	[Serializable]
	public class LineSave
	{
		public LineDirection lineDirection;

		public LineDrawerSave parent;
		public LineDrawerSave child;

		public LineSave(LineDirection lineDirection, LineDrawerSave parent, LineDrawerSave child)
		{
			this.lineDirection = lineDirection;
			this.parent = parent;
			this.child = child;
		}

		public void Create()
		{
			var line = Line.Line.Create(parent.gridPosition, child.gridPosition, parent.isActive, parent.createdLineDrawer, child.createdLineDrawer, lineDirection);

			parent.createdLineDrawer.lines.Add(line);
			child.createdLineDrawer.parentLine = line;
		}
	}
}