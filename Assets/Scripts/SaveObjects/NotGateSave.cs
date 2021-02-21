using System;
using System.Linq;
using Gates;
using Object = UnityEngine.Object;

namespace SaveObjects
{
	[Serializable]
	public class NotGateSave : GridObjectSave
	{
		public LineDrawerSave inputKnob;
		public LineDrawerSave outputKnob;

		public NotGateSave(GridPosition gridPosition, LineDrawerSave inputKnob, LineDrawerSave outputKnob) : base(gridPosition)
		{
			this.inputKnob = inputKnob;
			this.outputKnob = outputKnob;
		}

		public override object Create()
		{
			var instantiate = Object.Instantiate(Constants.C.notGatePrefab, Constants.C.gateParent, true);

			var notGate = instantiate.GetComponentInChildren<NotGate>();
			notGate.GridPosition = gridPosition;

			var componentsInChildren = instantiate.GetComponentsInChildren<KnobSmall>();

			var inputKnobSmall = componentsInChildren.First(x => x.id == 0);
			var outputKnobSmall = componentsInChildren.First(x => x.id == 1);

			inputKnob.CreateWithObject(inputKnobSmall);
			outputKnob.CreateWithObject(outputKnobSmall);

			return notGate;
		}
	}
}