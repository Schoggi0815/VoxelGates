using System.Collections.Generic;
using System.Linq;
using Line;
using UnityEngine;
using UnityEngine.EventSystems;

public class Knob : LineDrawer
{
	private SpriteRenderer _spriteRenderer;

	protected override void StartAddon()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void OnMouseOverAddon()
	{
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && lineDrawerMode != LineDrawerMode.Input)
		{
			IsActive = !IsActive;
		}
	}

	protected override void SpriteColorUpdate()
	{
		_spriteRenderer.sprite = IsSelected ? Constants.C.knobSpriteSelected : IsActive ? Constants.C.knobSpriteOn : Constants.C.knobSpriteOff;
	}
}