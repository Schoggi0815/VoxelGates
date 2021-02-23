using SaveObjects;

namespace Gates
{
	public class NotGate : Gate
	{
		public override GridObjectSave ToSaveObject()
		{
			var inputKnob = inputKnobs[0];
			inputKnob.lineDrawerSave = new LineDrawerSave(inputKnob.GridPosition, inputKnob.lineDrawerMode, inputKnob.IsActive);

			var outputKnob = outputKnobs[0];
			outputKnob.lineDrawerSave = new LineDrawerSave(outputKnob.GridPosition, outputKnob.lineDrawerMode, outputKnob.IsActive);

			return new NotGateSave(GridPosition, inputKnob.lineDrawerSave, outputKnob.lineDrawerSave);
		}

		protected override void HandleChange()
		{
			outputKnobs[0].IsActive = !inputKnobs[0].IsActive;
		}
	}
}