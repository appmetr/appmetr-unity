using UnityEngine;
using System.Collections.Generic;
using System;
using Appmetr.Unity.Json;

namespace Appmetr.Unity.Sample
{
	public class AppMetrSample : MonoBehaviour
	{	
		private string _fieldTrackEvent = "testEvent";

		private const string LabelProperties = "Properties";
		private const string LabelEvent = "Event";
		private const string LabelPayment = "Payment";

		private int _selectedTrack;
		private readonly string[] _trackLabels = {"Track Session", "Track Event", "Track Payment"};

		private const int IdSession = 0;
		private const int IdEvent = 1;
		private const int IdPayment = 2;

		private float _leftFieldCenter;
		private float _rightFieldCenter;

		private float _fieldWidth;
		private float fieldHeight = 25;

		private float _propFieldWidth;
		private float propFieldIndent = 15;
	
		private float labelWidth = 60;
	
		private float propButtonWidth = 50;
		private float propButtonHeight = 40;
		private float propButtonOffset = 30;
	
		private int _maxPropFields;

		private readonly Dictionary<string, object> _sessionProperties = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _eventProperties = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _paymentProperties = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _paymentList = new Dictionary<string, object>();
		private readonly Dictionary<string, object> _tempList = new Dictionary<string, object>();
	
		private string _sessionLastKey = "";
		private string _eventLastKey = "";
		private string _paymentLastKey = "";
		private string _paymentPropLastKey = "";
	
		private bool _isShowAlert;
		private string _alertMessage = "";

		#region GUI for sample app
	
		void OnGUI()
		{
			float centerX = (float)Screen.width / 2;
			_leftFieldCenter = Screen.width * 0.25f;
			_rightFieldCenter = Screen.width * 0.75f;
		
			_fieldWidth = Screen.width * 0.35f;
			_propFieldWidth = Screen.width * 0.2f;
		
			_maxPropFields = (int)((Screen.height * 0.55f) / (fieldHeight + propFieldIndent));
		
			float spacer = 40;
			_selectedTrack = GUI.SelectionGrid (new Rect(centerX - 300, spacer, 600, 40), _selectedTrack, _trackLabels, 4);
		
			spacer += 50;
		
			if (_selectedTrack == IdSession)
			{
				DoSessionGui(spacer);
			}
			else if (_selectedTrack == IdEvent)
			{
				DoEventGui(spacer);
			}
			else if (_selectedTrack == IdPayment)
			{
				DoPaymentGui(spacer);
			}
		
			if (GUI.Button(new Rect(centerX - 100, Screen.height - 60, 200, 40), "Run"))
			{
				DoTrack();
			}

			if (_isShowAlert)
			{
				float width = Screen.width * 0.9f;
				GUI.Label(new Rect ((float)Screen.width / 2 - width / 2, Screen.height - 130, width, 72), _alertMessage);
			}
		}
	
		private void DoSessionGui(float spacer)
		{
			GUI.Label(new Rect (_leftFieldCenter - labelWidth / 2, spacer, labelWidth, 24), LabelProperties);
		
			spacer += 30;
		
			if (_sessionProperties.Count < _maxPropFields && GUI.Button(new Rect(_leftFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
			{
				if (!_sessionProperties.ContainsKey(""))
				{
					_sessionProperties.Add("", "");
					_sessionLastKey = "";
				}
			}
			if (_sessionProperties.Count > 0 && GUI.Button(new Rect(_leftFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
			{
				_sessionProperties.Remove(_sessionLastKey);
			}
		
			float fieldSpacer = spacer + propButtonHeight + propFieldIndent;
			_tempList.Clear();
			foreach (KeyValuePair<string, object> pair in _sessionProperties)
			{
				string key = GUI.TextField(new Rect(_leftFieldCenter - _propFieldWidth - 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Key);
				string value = GUI.TextField(new Rect(_leftFieldCenter + 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Value.ToString());
				if ((key == "" && !checkDictionary(_sessionProperties)) || _sessionProperties.ContainsKey(key))
				{
					key = pair.Key;
				}
				_tempList.Add(key, value);
				fieldSpacer += fieldHeight + propFieldIndent;
			}
			_sessionProperties.Clear();
			foreach (KeyValuePair<string, object> pair in _tempList)
			{
				_sessionProperties.Add(pair.Key, pair.Value);
				_sessionLastKey = pair.Key;
			}
		}
	
		private void DoEventGui(float spacer)
		{
			GUI.Label(new Rect (_leftFieldCenter - labelWidth / 2, spacer, labelWidth, 24), LabelEvent);
			GUI.Label(new Rect (_rightFieldCenter - labelWidth / 2, spacer, labelWidth, 24), LabelProperties);
		
			spacer += 30;
		
			_fieldTrackEvent = GUI.TextField(new Rect(_leftFieldCenter - _fieldWidth / 2, spacer, _fieldWidth, fieldHeight), _fieldTrackEvent);
		
			if (_eventProperties.Count < _maxPropFields && GUI.Button(new Rect(_rightFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
			{
				if (!_eventProperties.ContainsKey(""))
				{
					_eventProperties.Add("", "");
					_eventLastKey = "";
				}
			}
			if (_eventProperties.Count > 0 && GUI.Button(new Rect(_rightFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
			{
				_eventProperties.Remove(_eventLastKey);
			}
		
			float fieldSpacer = spacer + propButtonHeight + propFieldIndent;
			_tempList.Clear();
			foreach (KeyValuePair<string, object> pair in _eventProperties)
			{
				string key = GUI.TextField(new Rect(_rightFieldCenter - _propFieldWidth - 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Key);
				string value = GUI.TextField(new Rect(_rightFieldCenter + 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Value.ToString());
				if ((key == "" && !checkDictionary(_eventProperties)) || _eventProperties.ContainsKey(key))
				{
					key = pair.Key;
				}
				_tempList.Add(key, value);
				fieldSpacer += fieldHeight + propFieldIndent;
			}
			_eventProperties.Clear();
			foreach (KeyValuePair<string, object> pair in _tempList)
			{
				_eventProperties.Add(pair.Key, pair.Value);
				_eventLastKey = pair.Key;
			}
		}	
	
		private void DoPaymentGui(float spacer)
		{
			GUI.Label(new Rect (_leftFieldCenter - labelWidth / 2, spacer, labelWidth, 24), LabelPayment);
			GUI.Label(new Rect (_rightFieldCenter - labelWidth / 2, spacer, labelWidth, 24), LabelProperties);
		
			spacer += 30;
			if (_paymentList.Count < _maxPropFields && GUI.Button(new Rect(_leftFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
			{
				if (!_paymentList.ContainsKey(""))
				{
					_paymentList.Add("", "");
					_paymentLastKey = "";
				}
			}
			if (_paymentList.Count > 0 && GUI.Button(new Rect(_leftFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
			{
				_paymentList.Remove(_paymentLastKey);
			}
	
			float fieldSpacer = spacer + propButtonHeight + propFieldIndent;
			_tempList.Clear();
			foreach (KeyValuePair<string, object> pair in _paymentList)
			{
				string key = GUI.TextField(new Rect(_leftFieldCenter - _propFieldWidth - 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Key);
				string value = GUI.TextField(new Rect(_leftFieldCenter + 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Value.ToString());
				if ((key == "" && !checkDictionary(_paymentList)) || _paymentList.ContainsKey(key))
				{
					key = pair.Key;
				}
				_tempList.Add(key, value);
				fieldSpacer += fieldHeight + propFieldIndent;
			}
			_paymentList.Clear();
			foreach (KeyValuePair<string, object> pair in _tempList)
			{
				_paymentList.Add(pair.Key, pair.Value);
				_paymentLastKey = pair.Key;
			}
	
			if (_paymentProperties.Count < _maxPropFields && GUI.Button(new Rect(_rightFieldCenter - propButtonOffset, spacer, propButtonWidth, propButtonHeight), "+"))
			{
				if (!_paymentProperties.ContainsKey(""))
				{
					_paymentProperties.Add("", "");
					_paymentPropLastKey = "";
				}
			}
			if (_paymentProperties.Count > 0 && GUI.Button(new Rect(_rightFieldCenter + propButtonOffset, spacer, propButtonWidth, propButtonHeight), "-"))
			{
				_paymentProperties.Remove(_paymentPropLastKey);
			}

			fieldSpacer = spacer + propButtonHeight + propFieldIndent;
			_tempList.Clear();
			foreach (KeyValuePair<string, object> pair in _paymentProperties)
			{
				string key = GUI.TextField(new Rect(_rightFieldCenter - _propFieldWidth - 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Key);
				string value = GUI.TextField(new Rect(_rightFieldCenter + 5, fieldSpacer, _propFieldWidth, fieldHeight), pair.Value.ToString());
				if ((key == "" && !checkDictionary(_paymentProperties)) || _paymentProperties.ContainsKey(key))
				{
					key = pair.Key;
				}
				_tempList.Add(key, value);
				fieldSpacer += fieldHeight + propFieldIndent;
			}
			_paymentProperties.Clear();
			foreach (KeyValuePair<string, object> pair in _tempList)
			{
				_paymentProperties.Add(pair.Key, pair.Value);
				_paymentPropLastKey = pair.Key;
			}
		}	
	
		private void DoTrack()
		{
			_isShowAlert = false;
		
			if (_selectedTrack == IdSession)
			{
				if (checkDictionary(_sessionProperties))
				{
					AppMetr.TrackSession(_sessionProperties);
				}
				else
				{
					AppMetr.TrackSession();
				}
			}
			else if (_selectedTrack == IdEvent)
			{
				if (_fieldTrackEvent == "")
				{
					// error
					ShowAlert("Please fill in all fields");
				}
				else
				{
					if (checkDictionary(_eventProperties))
					{
						AppMetr.TrackEvent(_fieldTrackEvent, _eventProperties);
					}
					else
					{
						AppMetr.TrackEvent(_fieldTrackEvent);
					}
				}
			}
			else if (_selectedTrack == IdPayment)
			{
				if (checkDictionary(_paymentList))
				{
					if (checkDictionary(_paymentProperties))
					{
						AppMetr.TrackPayment(_paymentList, _paymentProperties);
					}
					else
					{
						AppMetr.TrackPayment(_paymentList);
					}
				}
				else
				{
					// error
					ShowAlert("Please fill in all fields");
				}
			}

			AppMetr.Flush();
		}
	
		private bool checkDictionary(IDictionary<string, object> dict)
		{
			if (dict.Count == 0)
				return false;
			
			foreach (KeyValuePair<string, object> pair in dict)
			{
				if (pair.Key == "")
					return false;
			}
			return true;
		}
	
		private void ShowAlert(string message)
		{
			_alertMessage = message;
			_isShowAlert = true;
		}
	
		#endregion
	}
}
