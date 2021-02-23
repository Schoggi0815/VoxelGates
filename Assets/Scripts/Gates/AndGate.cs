using SaveObjects;

namespace Gates
{
	public class AndGate : Gate
	{
		public override GridObjectSave ToSaveObject()
		{
			var inputKnobTop = inputKnobs[0];
			inputKnobTop.lineDrawerSave = new LineDrawerSave(inputKnobTop.GridPosition, inputKnobTop.lineDrawerMode, inputKnobTop.IsActive);

			var inputKnobBottom = inputKnobs[1];
			inputKnobBottom.lineDrawerSave = new LineDrawerSave(inputKnobBottom.GridPosition, inputKnobBottom.lineDrawerMode, inputKnobBottom.IsActive);

			var outputKnob = outputKnobs[0];
			outputKnob.lineDrawerSave = new LineDrawerSave(outputKnob.GridPosition, outputKnob.lineDrawerMode, outputKnob.IsActive);

			return new AndGateSave(GridPosition, inputKnobTop.lineDrawerSave, inputKnobBottom.lineDrawerSave, outputKnob.lineDrawerSave);
		}

		protected override void HandleChange()
		{
			outputKnobs[0].IsActive = inputKnobs.TrueForAll(x => x.IsActive);
		}
	}
}