����� AppmetrPlugin

��� �������� �����, ��������������� ������ � �������� ����������.

������:

- Setup
�������������� ����������

���������
token - ����� ����������


- TrackSession
������������ ������ ����������

���������
properties - �������������� ��������� (�������� � ������������, ��������, ���� �����������, ���, �������)


- TrackLevel
������������ ���������� ������ ������ �������

���������
level - ����������� �������
properties - �������������� ���������


- TrackEvent
������������ ������� �������

���������
_event - �������� �������
properties - �������������� ���������


- TrackPayment
������������ ���������� �������

���������
payment - �������� � ����������� �������
properties - �������������� ���������


����� AppMetrCommandListener

���� ����� ������������� ������ � ������� ��������� ������� �� �������.

������ �������������:

AppMetrCommandListener.OnCommand += HandleOnCommand; 

public void HandleOnCommand(string command)
{
	Debug.Log("HandleOnCommand:\n" + command);
}

������ ������������� ������� ����� ���������� � Plugins/Appmetr/Sample/AppMetrSample.cs

�����: ���������� �������� ������� ������ �� �����, ������ ��� "AppMetrCommandListener", 
 � ���������� � ������� ������ AppMetrCommandListener.cs.
