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
		if (PlayerPrefs.GetString(value.key) == "")
		{
			return 0;
		}

		return double.Parse(PlayerPrefs.GetString(value.key));
	}

	public static implicit operator bool(JSONSpoofValue value)
	{
		return PlayerPrefs.GetString(value.key) == "true";
	}

	private static JSONSpoofArray ParseArray(JSONSpoofValue spoofValue, string key)
	{
		JSONSpoofArray value = new JSONSpoofArray(key + "_" + spoofValue.key);
		for (int i = 0;; i++)
		{
			string stringValue = PlayerPrefs.GetString(spoofValue.key + "_" + i);
			if (stringValue == "")
			{
				break;
			}
			value.Add(new JSONSpoofValue(stringValue, new JSONSpoof("object_" + key + "_" + spoofValue.key)));
		}
		return value;
	}
}

public class JSONSpoofArray
{
	public List<JSONSpoofValue> values;

	private string extraKey;

	public JSONSpoofArray(string extraKey)
	{
		values = new List<JSONSpoofValue>();
		this.extraKey = extraKey;
	}

	public JSONSpoofValue this[int index]
	{
		get
		{
			return values[index];
		}
		set
		{
			values[index] = value;
		}
	}

	public void Add(JSONSpoofValue value)
	{
		values.Add(value);
	}
}
