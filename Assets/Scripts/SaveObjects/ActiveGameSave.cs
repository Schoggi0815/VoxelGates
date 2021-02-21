using System;
using System.Collections.Generic;

namespace SaveObjects
{
	[Serializable]
	public class ActiveGameSave
	{
		public List<GridObjectSave> GridObjectSaves = new List<GridObjectSave>();
		
		public List<LineSave> lineSaves = new List<LineSave>();

		public void Create()
		{
			foreach (var gridObjectSave in GridObjectSaves)
			{
				gridObjectSave.Create();
			}

			foreach (var lineSave in lineSaves)
			{
				lineSave.Create();
			}
		}

		public void Clear()
		{
			GridObjectSaves.Clear();
			lineSaves.Clear();
		}
	}
}