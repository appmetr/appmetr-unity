using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;

public class AppMetrSample : MonoBehaviour
{
	[SerializeField]
	private string token = "";
	
	private string fieldTrackSession = "";
	private string fieldTrackLevel = "";
	private string fieldTrackEvent = "";
	private string fieldTrackPayment = "";
	
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
		float spaceSize = 90;
		
		float buttonWidth = 150;
		float buttonHeight = 50;
		
		float fieldWidth = 500;
		float fieldHeight = 50;
		
		float spacer = 100;

		float buttonOffsetX = 20;
		float fieldOffsetX = buttonOffsetX * 2 + buttonWidth;
		
		if (GUI.Button(new Rect(buttonOffsetX, spacer, buttonWidth, buttonHeight), "Track Session"))
		{
			AppmetrPlugin.TrackSession();
		}
		
		//fieldTrackSession = GUI.TextField(new Rect(fieldOffsetX, spacer, fieldWidth, fieldHeight), fieldTrackSession);
		
		spacer += spaceSize;
		if (GUI.Button(new Rect(buttonOffsetX, spacer, buttonWidth, buttonHeight), "Track Level"))
		{
			AppmetrPlugin.TrackLevel(Convert.ToInt32(fieldTrackLevel));
		}
		
		fieldTrackLevel = GUI.TextField(new Rect(fieldOffsetX, spacer, fieldWidth, fieldHeight), fieldTrackLevel);
		
		spacer += spaceSize;
		if (GUI.Button(new Rect(buttonOffsetX, spacer, buttonWidth, buttonHeight), "Track Event"))
		{
			AppmetrPlugin.TrackEvent(fieldTrackEvent);
		}
		
		fieldTrackEvent = GUI.TextField(new Rect(fieldOffsetX, spacer, fieldWidth, fieldHeight), fieldTrackEvent);
		
		spacer += spaceSize;
		if (GUI.Button(new Rect(buttonOffsetX, spacer, buttonWidth, buttonHeight), "Track Payment"))
		{
			Dictionary<string, string> value = (Dictionary<string, string>)MiniJSON.jsonDecode(fieldTrackPayment);
			if (value != null)
			{
				AppmetrPlugin.TrackPayment(value);
			}
		}
		
		fieldTrackPayment = GUI.TextField(new Rect(fieldOffsetX, spacer, fieldWidth, fieldHeight), fieldTrackPayment);
	}
	
	#endregion
}
