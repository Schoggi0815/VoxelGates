using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveObjects
{
	public class SaveHandler : MonoBehaviour
	{
		private ActiveGameSave _activeGameSave = new ActiveGameSave();

		[HideInInspector] public List<GridObject> gridObjects;
		[HideInInspector] public List<Line.Line> lines;

		private string _json;
		
		private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			PreserveReferencesHandling = PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling.All
		};
		
		private void Start()
		{
			Constants.C.saveHandler = this;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				_activeGameSave.Clear();
				
				foreach (var gridObject in gridObjects)
				{
					_activeGameSave.GridObjectSaves.Add(gridObject.ToSaveObject());
				}

				foreach (var line in lines)
				{
					_activeGameSave.lineSaves.Add(line.ToLineSave());
				}

				_json = JsonConvert.SerializeObject(_activeGameSave, _settings);
				
				print(_json);
			}

			if (Input.GetKeyDown(KeyCode.L))
			{
				DeleteAll();
				
				_activeGameSave = JsonConvert.DeserializeObject<ActiveGameSave>(_json, _settings);

				_activeGameSave?.Create();
			}
		}

		private void DeleteAll()
		{
			int count = lines.Count;
			
			for (var i = 0; i < count; i++)
			{
				lines.First().Delete(false);
			}

			count = gridObjects.Count;

			for (var i = 0; i < count; i++)
			{
				gridObjects.First().Delete();
			}
		}
	}
}