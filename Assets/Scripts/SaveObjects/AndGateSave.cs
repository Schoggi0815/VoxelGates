using System;
using System.Linq;
using Gates;
using Object = UnityEngine.Object;

namespace SaveObjects
{
	[Serializable]
	public class AndGateSave : GridObjectSave
	{
		public LineDrawerSave inputKnobTop;
		public LineDrawerSave inputKnobBottom;
		public LineDrawerSave outputKnob;

		public AndGateSave(GridPosition gridPosition, LineDrawerSave inputKnobTop, LineDrawerSave inputKnobBottom, LineDrawerSave outputKnob) : base(gridPosition)
		{
			this.inputKnobTop = inputKnobTop;
			this.inputKnobBottom = inputKnobBottom;
			this.outputKnob = outputKnob;
		}

		public override object Create()
		{
			var instantiate = Object.Instantiate(Constants.C.andGatePrefab, Constants.C.gateParent, true);

			var andGate = instantiate.GetComponentInChildren<AndGate>();
			andGate.GridPosition = gridPosition;

			var componentsInChildren = instantiate.GetComponentsInChildren<KnobSmall>();

			var inputKnobSmallTop = componentsInChildren.First(x => x.id == 0);
			var inputKnobSmallBottom = componentsInChildren.First(x => x.id == 1);
			var outputKnobSmall = componentsInChildren.First(x => x.id == 2);

			inputKnobTop.CreateWithObject(inputKnobSmallTop);
			inputKnobBottom.CreateWithObject(inputKnobSmallBottom);
			outputKnob.CreateWithObject(outputKnobSmall);

			return andGate;
		}
	}
}