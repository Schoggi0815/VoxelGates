namespace Gates
{
	public class NotGate : Gate
	{
		public override void HandleChange()
		{
			outputKnobs[0].IsActive = !inputKnobs[0].IsActive;
		}
	}
}