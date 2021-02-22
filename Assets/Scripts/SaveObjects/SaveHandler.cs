using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace SaveObjects
{
	public class SaveHandler : MonoBehaviour
	{
		private ActiveGameSave _activeGameSave = new ActiveGameSave();

		[HideInInspector] public List<GridObject> gridObjects;
		[HideInInspector] public List<Line.Line> lines;

		private string _json;

		private static bool _shouldLoad;
		private static string loadPath;

		private static readonly string SaveDirectory = $@"{Directory.GetCurrentDirectory()}\saves";

		private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
		{
			Formatting = Formatting.Indented,
			PreserveReferencesHandling = PreserveReferencesHandling.All,
			TypeNameHandling = TypeNameHandling.All
		};
		
		private void Start()
		{
			Constants.C.saveHandler = this;
			
			if (!Directory.Exists(SaveDirectory))
			{
				Directory.CreateDirectory(SaveDirectory);
			}
			
			Constants.C.saveMenuResponseText.text = string.Empty;

			if (_shouldLoad)
			{
				_activeGameSave.Clear();

				var readAllText = File.ReadAllText(loadPath);

				_activeGameSave = JsonConvert.DeserializeObject<ActiveGameSave>(readAllText, _settings);
				
				_activeGameSave.Create();

				_shouldLoad = false;
			}
		}

		private string saveName;

		public void Save()
		{
			saveName = Constants.C.saveNameText.text;
			
			if (string.IsNullOrWhiteSpace(saveName))
			{
				ShowErrorMessage("The file name cannot be empty!");
				
				return;
			}

			if (File.Exists(@$"{SaveDirectory}\{saveName}.json"))
			{
				Constants.C.overwritePopupUi.MoveRight(saveName);
				
				return;
			}
			
			SaveForce();
		}

		public void SaveForce()
		{
			if (string.IsNullOrWhiteSpace(saveName)) return;

			_activeGameSave.Clear();
				
			foreach (var gridObject in gridObjects)
			{
				_activeGameSave.GridObjectSaves.Add(gridObject.ToSaveObject());
			}

			foreach (var line in lines)
			{
				_activeGameSave.lineSaves.Add(line.ToLineSave());
			}

			var json = JsonConvert.SerializeObject(_activeGameSave, _settings);

			using var streamWriter = File.CreateText(@$"{SaveDirectory}\{saveName}.json");
			streamWriter.Write(json);
			
			ShowSuccessMessage("Saved Successfully!");
		}

		public void OnTextChange()
		{
			var text = Constants.C.saveNameText;

			text.text = text.text.Replace(" ", string.Empty).Replace(".", string.Empty);
		}

		private void ShowErrorMessage(string message)
		{
			Constants.C.saveMenuResponseText.color = Constants.C.saveMenuErrorColor;
			Constants.C.saveMenuResponseText.text = message;
		}
		
		private void ShowSuccessMessage(string message)
		{
			Constants.C.saveMenuResponseText.color = Constants.C.saveMenuSuccessColor;
			Constants.C.saveMenuResponseText.text = message;
		}

		private void DeleteAll()
		{
			var count = lines.Count;
			
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

		public static IEnumerable<string> GetAllSaves()
		{
			return Directory.GetFiles(SaveDirectory).Select(x => x.Remove(0, SaveDirectory.Length + 1)).Where(x => x.EndsWith(".json")).Select(x => x.Remove(x.Length - 5, 5));
		}

		public static void Load(string saveName)
		{
			loadPath = @$"{SaveDirectory}\{saveName}.json";
			
			if (File.Exists(loadPath))
			{
				_shouldLoad = true;
			}
		}
	}
}