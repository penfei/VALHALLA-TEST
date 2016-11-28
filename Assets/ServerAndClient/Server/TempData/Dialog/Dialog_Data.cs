using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Dialog_Data : MonoBehaviour
{
	public static List<Dialog> Dialogs = new List<Dialog> ();
}

[System.Serializable]
public class Dialog
{
	public int dialogID;
	public List<Dialog_Window> window = new List<Dialog_Window> ();
}

[System.Serializable]
public class Dialog_Window
{
	public int windowID;
	public string windowText;
	public List<Dialog_Answer> answer = new List<Dialog_Answer> ();
}

[System.Serializable]
public class Dialog_Answer
{
	public int ID;
	public int dataID;
	public Dialog_Type type;
	public string answerText;
	public Dictionary<int, int> needQuestItems = new Dictionary<int, int> ();
	public Dictionary<int, int> revardQuestItems = new Dictionary<int, int> ();
}

public enum Dialog_Type
{
	defaul,
	gatekeeper,
	shop,
	quest
}
