using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boomlagoon.JSON;

public class JSONSpoof : JSONObject
{
	public new string GetString(string key)
	{
		return PlayerPrefs.GetString(key);
	}

	public new JSONSpoofValue this[string key]
	{
		get
		{
			return new JSONSpoofValue(extraKey + "_" + key, this);
		}
	}

	public JSONSpoof(string extraKey)
	{
		this.extraKey = extraKey;
	}

	public string extraKey;
}

public class JSONSpoofValue
{
	private string key;

	public JSONObject Obj;

	public JSONSpoofValue(string key, JSONObject obj)
	{
		this.key = key;
		Obj = obj;
	}

	public static implicit operator JSONSpoof(JSONSpoofValue value)
	{
		return new JSONSpoof(value.key);
	}

	public static implicit operator string(JSONSpoofValue value)
	{
		return PlayerPrefs.GetString(value.key);
	}

	public static implicit operator double(JSONSpoofValue value)
	{
		return double.Parse(PlayerPrefs.GetString(value.key));
	}

	public static implicit operator bool(JSONSpoofValue value)
	{
		return PlayerPrefs.GetString(value.key) == "true";
	}
}
