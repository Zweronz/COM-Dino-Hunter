using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpoofedData 
{
	public static string LoadSpoof(string name)
	{
		return Resources.Load<TextAsset>("spoofedconfigs/" + name).text;
	}
}
