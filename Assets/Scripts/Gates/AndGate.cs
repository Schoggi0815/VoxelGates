namespace Gates
{
	public class AndGate : Gate
	{
		public override void HandleChange()
		{
			outputKnobs[0].IsActive = inputKnobs.TrueForAll(x => x.IsActive);
		}
	}
}