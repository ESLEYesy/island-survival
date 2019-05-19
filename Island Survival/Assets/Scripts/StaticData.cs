using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{
	private static string playerName;

	public static string PlayerName
	{
		get
		{
			return playerName;
		}
		set
		{
			playerName = value;
		}
	}
}
