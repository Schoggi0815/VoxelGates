﻿using System;

[Serializable]
public struct GridPosition
{
	public GridPosition(int x, int y)
	{
		X = x;
		Y = y;
	}
	
	public readonly int X;
	public readonly int Y;
	public override string ToString()
	{
		return $"{X}, {Y}";
	}
}