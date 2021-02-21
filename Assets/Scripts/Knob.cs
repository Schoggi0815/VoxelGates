using System.Linq;
using Line;
using SaveObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Knob : LineDrawer
{
	private SpriteRenderer _spriteRenderer;
	[SerializeField]
	private Text numberText;

	public int ID { get; set; }

	protected override void StartAddon()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();

		if (lineDrawerMode == LineDrawerMode.Output)
		{
			Constants.C.outputKnobs.Add(this);
			ID = Constants.C.outputKnobs.Count;
		}
		else
		{
			Constants.C.inputKnobs.Add(this);
			ID = Constants.C.inputKnobs.Count;
		}
		
		OnUpdateID();
	}

	public override GridObjectSave ToSaveObject()
	{
		lineDrawerSave = new KnobSave(GridPosition, lineDrawerMode, IsActive, ID);
		return lineDrawerSave;
	}

	protected override void OnMouseOverAddon()
	{
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && lineDrawerMode != LineDrawerMode.Input && !Constants.C.lineDrawers.Any(x => x.IsSelected))
		{
			IsActive = !IsActive;
		}
	}

	protected override void SpriteColorUpdate()
	{
		_spriteRenderer.sprite = IsSelected ? Constants.C.knobSpriteSelected : IsActive ? Constants.C.knobSpriteOn : Constants.C.knobSpriteOff;
	}

	public void OnDelete()
	{
		if (lineDrawerMode == LineDrawerMode.Output)
		{
			Constants.C.outputKnobs.Remove(this);
		}
		else
		{
			Constants.C.inputKnobs.Remove(this);
		}
		
		Constants.C.UpdateKnobIDs();
	}

	public void OnUpdateID()
	{
		numberText.text = ID.ToString();
	}
}