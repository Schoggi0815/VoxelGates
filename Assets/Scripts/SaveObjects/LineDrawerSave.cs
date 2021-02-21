using System;
using Line;
using Newtonsoft.Json;
using Object = UnityEngine.Object;

namespace SaveObjects
{
	[Serializable]
	public class LineDrawerSave : GridObjectSave
	{
		public LineDrawerMode lineDrawerMode;

		public bool isActive;

		[JsonIgnore] public LineDrawer createdLineDrawer;

		public LineDrawerSave(GridPosition gridPosition, LineDrawerMode lineDrawerMode, bool isActive) : base(gridPosition)
		{
			this.lineDrawerMode = lineDrawerMode;
			this.isActive = isActive;
		}

		public override object Create()
		{
			var prefab = lineDrawerMode switch
			{
				LineDrawerMode.Input => Constants.C.inputKnobPrefab,
				LineDrawerMode.Output => Constants.C.outputKnobPrefab,
				LineDrawerMode.Adaptive => Constants.C.lineCornerPrefab,
				_ => null
			};

			var instantiate = Object.Instantiate(prefab, Constants.C.lineCornerParent, true);
			instantiate.GetComponent<GridObject>().GridPosition = gridPosition;
			var lineDrawer = instantiate.GetComponent<LineDrawer>();

			lineDrawer.IsActive = isActive;

			createdLineDrawer = lineDrawer;

			if (GetType() == typeof(KnobSave) && lineDrawer.GetType() == typeof(Knob))
			{
				var knobSave = (KnobSave) this;
				var knob = (Knob) lineDrawer;
				knob.ID = knobSave.id;
			}

			return lineDrawer;
		}

		public void CreateWithObject(LineDrawer lineDrawer)
		{
			lineDrawer.IsActive = isActive;
			createdLineDrawer = lineDrawer;
		}
	}
}