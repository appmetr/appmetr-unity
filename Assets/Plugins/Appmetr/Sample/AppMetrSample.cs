﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class AppMetrSample : MonoBehaviour
{
	[SerializeField]
	private string token = "";
	
	private string fieldTrackLevel = "";
	private string fieldTrackEvent = "";
	
	private string labelProperties = "Properties";
	private string labelLevel = "Level";
	private string labelEvent = "Event";
	private string labelPayment = "Payment";
	
	private int selectedTrack = 0;
	private string[] trackLabels = {"Track Session", "Track Level", "Track Event", "Track Payment"};
	
	private int ID_SESSION = 0;
	private int ID_LEVEL = 1;
	private int ID_EVENT = 2;
	private int ID_PAYMENT = 3;
	
	private float leftFieldCenter;
	private float rightFieldCenter;

	private float fieldWidth;
	private float fieldHeight = 25;

	private float propFieldWidth;
	private float propFieldIndent = 15;
	
	private float labelWidth = 60;
	
	private float propButtonWidth = 50;
	private float propButtonHeight = 40;
	private float propButtonOffset = 30;
	
	private int maxPropFields = 0;
	
	Dictionary<string, string> sessionProperties = new Dictionary<string, string>();
	Dictionary<string, string> levelProperties = new Dictionary<string, string>();
	Dictionary<string, string> eventProperties = new Dictionary<string, string>();
	Dictionary<string, string> paymentProperties = new Dictionary<string, string>();
	Dictionary<string, string> paymentList = new Dictionary<string, string>();

	Dictionary<string, string> tempList = new Dictionary<string, string>();
	
	private string sessionLastKey = "";
	private string levelLastKey = "";
	private string eventLastKey = "";
	private string paymentLastKey = "";
	private string paymentPropLastKey = "";
	
	void Start()
	{
	}

	void Awake()
	{
		AppmetrPlugin.Setup(token);
	}
	
	void OnDisable()
	{
	}

	#region GUI for sample app
	
	void OnGUI()
	{
		float centerX = Screen.width / 2;
		leftFieldCenter = Screen.width * 0.25f;
		rightFieldCenter = Screen.width * 0.75f;
		
		fieldWidth = Screen.width * 0.35f;
		propFieldWidth = Screen.width * 0.2f;
		
		maxPropFields = (int)((Screen.height / 2) / (fieldHeight + propFieldIndent));
		
		float spacer = 80;
		selectedTrack = GUI.SelectionGrid (new Rect(centerX - 300, spacer, 600, 40), selectedTrack, trackLabels, 4);
		
		spacer += 60;
		
		if (selectedTrack == ID_SESSION)
		{
			doSessionGUI(spacer);
		}
		else if (selectedTrack == ID_LEVEL)
		{
			doLevelGUI(spacer);
		}
		else if (selectedTrack == ID_EVENT)
		{
			doEventGUI(spacer);
		}
		else if (selectedTrack == ID_PAYMENT)
		{
			doPaymentGUI(spacer);
		}
		
		if (GUI.Button(new Rect(centerX - 100, Screen.height - 60, 200, 40), "Run"))
		{
			doTrack();
		}
	}
	
	private void doSessionGUI(float spacer)
	{
		GUI.Label(new Rect (leftFieldCenter - labelWidth / 2, spacer, labelWidth, 24), labelProperties);
		
		spacer += 40;
		
		if (sessionProperties.Count < maxPropFields && GUI.Button(new Rect(leftFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
		{
			if (!sessionProperties.ContainsKey(""))
			{
				sessionProperties.Add("", "");
				sessionLastKey = "";
			}
		}
		if (sessionProperties.Count > 0 && GUI.Button(new Rect(leftFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
		{
			sessionProperties.Remove(sessionLastKey);
		}
		
		float fieldSpacer = spacer + propButtonHeight + propFieldIndent;
		tempList.Clear();
		foreach (KeyValuePair<string, string> pair in sessionProperties)
		{
			string key = GUI.TextField(new Rect(leftFieldCenter - propFieldWidth - 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Key);
			string value = GUI.TextField(new Rect(leftFieldCenter + 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Value);
			if ((key == "" && !checkDictionary(sessionProperties)) || sessionProperties.ContainsKey(key))
			{
				key = pair.Key;
			}
			tempList.Add(key, value);
			fieldSpacer += fieldHeight + propFieldIndent;
		}
		sessionProperties.Clear();
		foreach (KeyValuePair<string, string> pair in tempList)
		{
			sessionProperties.Add(pair.Key, pair.Value);
			sessionLastKey = pair.Key;
		}
	}
	
	private void doLevelGUI(float spacer)
	{
		GUI.Label(new Rect (leftFieldCenter - labelWidth / 2, spacer, labelWidth, 24), labelLevel);
		GUI.Label(new Rect (rightFieldCenter - labelWidth / 2, spacer, labelWidth, 24), labelProperties);
		
		spacer += 40;
		
		fieldTrackLevel = GUI.TextField(new Rect(leftFieldCenter - fieldWidth / 2, spacer, fieldWidth, fieldHeight), fieldTrackLevel);
		
		if (levelProperties.Count < maxPropFields && GUI.Button(new Rect(rightFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
		{
			if (!levelProperties.ContainsKey(""))
			{
				levelProperties.Add("", "");
				levelLastKey = "";
			}
		}
		if (levelProperties.Count > 0 && GUI.Button(new Rect(rightFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
		{
			levelProperties.Remove(levelLastKey);
		}
		
		float fieldSpacer = spacer + propButtonHeight + propFieldIndent;
		tempList.Clear();
		foreach (KeyValuePair<string, string> pair in levelProperties)
		{
			string key = GUI.TextField(new Rect(rightFieldCenter - propFieldWidth - 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Key);
			string value = GUI.TextField(new Rect(rightFieldCenter + 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Value);
			if ((key == "" && !checkDictionary(levelProperties)) || levelProperties.ContainsKey(key))
			{
				key = pair.Key;
			}
			tempList.Add(key, value);
			fieldSpacer += fieldHeight + propFieldIndent;
		}
		levelProperties.Clear();
		foreach (KeyValuePair<string, string> pair in tempList)
		{
			levelProperties.Add(pair.Key, pair.Value);
			levelLastKey = pair.Key;
		}
	}	
	
	private void doEventGUI(float spacer)
	{
		GUI.Label(new Rect (leftFieldCenter - labelWidth / 2, spacer, labelWidth, 24), labelEvent);
		GUI.Label(new Rect (rightFieldCenter - labelWidth / 2, spacer, labelWidth, 24), labelProperties);
		
		spacer += 40;
		
		fieldTrackEvent = GUI.TextField(new Rect(leftFieldCenter - fieldWidth / 2, spacer, fieldWidth, fieldHeight), fieldTrackEvent);
		
		if (eventProperties.Count < maxPropFields && GUI.Button(new Rect(rightFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
		{
			if (!eventProperties.ContainsKey(""))
			{
				eventProperties.Add("", "");
				eventLastKey = "";
			}
		}
		if (eventProperties.Count > 0 && GUI.Button(new Rect(rightFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
		{
			eventProperties.Remove(eventLastKey);
		}
		
		float fieldSpacer = spacer + propButtonHeight + propFieldIndent;
		tempList.Clear();
		foreach (KeyValuePair<string, string> pair in eventProperties)
		{
			string key = GUI.TextField(new Rect(rightFieldCenter - propFieldWidth - 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Key);
			string value = GUI.TextField(new Rect(rightFieldCenter + 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Value);
			if ((key == "" && !checkDictionary(eventProperties)) || eventProperties.ContainsKey(key))
			{
				key = pair.Key;
			}
			tempList.Add(key, value);
			fieldSpacer += fieldHeight + propFieldIndent;
		}
		eventProperties.Clear();
		foreach (KeyValuePair<string, string> pair in tempList)
		{
			eventProperties.Add(pair.Key, pair.Value);
			eventLastKey = pair.Key;
		}
	}	
	
	private void doPaymentGUI(float spacer)
	{
		GUI.Label(new Rect (leftFieldCenter - labelWidth / 2, spacer, labelWidth, 24), labelPayment);
		GUI.Label(new Rect (rightFieldCenter - labelWidth / 2, spacer, labelWidth, 24), labelProperties);
		
		spacer += 40;
		if (paymentList.Count < maxPropFields && GUI.Button(new Rect(leftFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
		{
			if (!paymentList.ContainsKey(""))
			{
				paymentList.Add("", "");
				paymentLastKey = "";
			}
		}
		if (paymentList.Count > 0 && GUI.Button(new Rect(leftFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
		{
			paymentList.Remove(paymentLastKey);
		}
	
		float fieldSpacer = spacer + propButtonHeight + propFieldIndent;
		tempList.Clear();
		foreach (KeyValuePair<string, string> pair in paymentList)
		{
			string key = GUI.TextField(new Rect(leftFieldCenter - propFieldWidth - 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Key);
			string value = GUI.TextField(new Rect(leftFieldCenter + 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Value);
			if ((key == "" && !checkDictionary(paymentList)) || paymentList.ContainsKey(key))
			{
				key = pair.Key;
			}
			tempList.Add(key, value);
			fieldSpacer += fieldHeight + propFieldIndent;
		}
		paymentList.Clear();
		foreach (KeyValuePair<string, string> pair in tempList)
		{
			paymentList.Add(pair.Key, pair.Value);
			paymentLastKey = pair.Key;
		}
	
		if (paymentProperties.Count < maxPropFields && GUI.Button(new Rect(rightFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
		{
			if (!paymentProperties.ContainsKey(""))
			{
				paymentProperties.Add("", "");
				paymentPropLastKey = "";
			}
		}
		if (paymentProperties.Count > 0 && GUI.Button(new Rect(rightFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
		{
			paymentProperties.Remove(paymentPropLastKey);
		}

		fieldSpacer = spacer + propButtonHeight + propFieldIndent;
		tempList.Clear();
		foreach (KeyValuePair<string, string> pair in paymentProperties)
		{
			string key = GUI.TextField(new Rect(rightFieldCenter - propFieldWidth - 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Key);
			string value = GUI.TextField(new Rect(rightFieldCenter + 5, fieldSpacer, propFieldWidth, fieldHeight), pair.Value);
			if ((key == "" && !checkDictionary(paymentProperties)) || paymentProperties.ContainsKey(key))
			{
				key = pair.Key;
			}
			tempList.Add(key, value);
			fieldSpacer += fieldHeight + propFieldIndent;
		}
		paymentProperties.Clear();
		foreach (KeyValuePair<string, string> pair in tempList)
		{
			paymentProperties.Add(pair.Key, pair.Value);
			paymentPropLastKey = pair.Key;
		}
	}	
	
	private void doTrack()
	{
		if (selectedTrack == ID_SESSION)
		{
			if (checkDictionary(sessionProperties))
			{
				AppmetrPlugin.TrackSession(sessionProperties);
			}
			else
			{
				AppmetrPlugin.TrackSession();
			}
		}
		else if (selectedTrack == ID_LEVEL)
		{
			if (fieldTrackLevel == "")
			{
				// error
				showAlert("Please fill in all fields");
			}
			else
			{
				if (checkDictionary(levelProperties))
				{
					AppmetrPlugin.TrackLevel(Convert.ToInt32(fieldTrackLevel), levelProperties);
				}
				else
				{
					AppmetrPlugin.TrackLevel(Convert.ToInt32(fieldTrackLevel));
				}
			}
		}
		else if (selectedTrack == ID_EVENT)
		{
			if (fieldTrackEvent == "")
			{
				// error
				showAlert("Please fill in all fields");
			}
			else
			{
				if (checkDictionary(eventProperties))
				{
					AppmetrPlugin.TrackEvent(fieldTrackEvent, eventProperties);
				}
				else
				{
					AppmetrPlugin.TrackEvent(fieldTrackEvent);
				}
			}
		}
		else if (selectedTrack == ID_PAYMENT)
		{
			if (checkDictionary(paymentList))
			{
				if (checkDictionary(paymentProperties))
				{
					AppmetrPlugin.TrackPayment(paymentList, paymentProperties);
				}
				else
				{
					AppmetrPlugin.TrackPayment(paymentList);
				}
			}
			else
			{
				// error
				showAlert("Please fill in all fields");
			}
		}
	}
	
	private bool checkDictionary(IDictionary<string, string> dict)
	{
		if (dict.Count == 0)
			return false;
			
		foreach (KeyValuePair<string, string> pair in dict)
		{
			if (pair.Key == "")
				return false;
		}
		return true;
	}
	
	private void showAlert(string message)
	{
	}
	
	#endregion
}
