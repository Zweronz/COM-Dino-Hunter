using System;
using System.Collections;
using Boomlagoon.JSON;
using LitJson;
using UnityEngine;

public class iServerTime : MonoBehaviour
{
	public delegate void OnServerTimeSuccess(double ticks);

	public delegate void OnServerTimeFailed();

	protected static iServerTime m_Instance;

	protected OnServerTimeSuccess m_OnServerTimeSuccess;

	protected OnServerTimeFailed m_OnServerTimeFailed;

	protected string m_sServerName = string.Empty;

	protected string m_sServerUrl = string.Empty;

	protected string m_sKey = string.Empty;

	protected float m_fTimeOut = -1f;

	public static iServerTime GetInstance()
	{
		if (m_Instance == null)
		{
			GameObject gameObject = new GameObject("_iServerTime");
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.rotation = Quaternion.identity;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			m_Instance = gameObject.AddComponent<iServerTime>();
		}
		return m_Instance;
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Initialize(string sServerName, string sServerUrl, string sKey, float fTimeOut)
	{
		m_sServerName = sServerName;
		m_sServerUrl = sServerUrl;
		m_sKey = sKey;
		m_fTimeOut = fTimeOut;
		HttpClient.Instance().AddServer(m_sServerName, m_sServerUrl, m_fTimeOut, (m_sKey.Length >= 1) ? m_sKey : null);
	}

	public void GetServerTime(OnServerTimeSuccess onsuccess, OnServerTimeFailed onfailed)
	{
		m_OnServerTimeSuccess = onsuccess;
		m_OnServerTimeFailed = onfailed;
		SendGetServerTimeRequest();
	}

	protected void OnSuccess(double ticks)
	{
		if (m_OnServerTimeSuccess != null)
		{
			m_OnServerTimeSuccess(ticks);
		}
	}

	protected void OnFailed()
	{
		if (m_OnServerTimeFailed != null)
		{
			m_OnServerTimeFailed();
		}
	}

	protected void SendGetServerTimeRequest()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["cmd"] = "GetServerTime";
		HttpClient.Instance().SendRequest(m_sServerName, "groovy", JsonMapper.ToJson(hashtable), "_iServerTime", "iServerTime", "OnGetServerTimeRequest", null);
	}

	protected void OnGetServerTimeRequest(int taskId, int result, string server, string action, string response, string param)
	{
		UnityEngine.Debug.Log("OnGetServerTimeRequest result = " + result);
		if (result != 0)
		{
			return;
		}
		string empty = string.Empty;
		try
		{
			JSONObject jSONObject = JSONObject.Parse(response);
			if (jSONObject["code"].Type == JSONValueType.String)
			{
				empty = jSONObject.GetString("code");
			}
			else
			{
				if (jSONObject["code"].Type != JSONValueType.Number)
				{
					throw new Exception();
				}
				empty = jSONObject.GetNumber("code").ToString();
			}
			UnityEngine.Debug.Log(empty);
			if (empty == "0")
			{
				double ticks = jSONObject["time"].Number;
				OnSuccess(ticks);
			}
			else
			{
				OnFailed();
			}
		}
		catch
		{
			OnFailed();
		}
	}
}
