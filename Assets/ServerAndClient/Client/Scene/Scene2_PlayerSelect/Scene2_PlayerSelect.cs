using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//We show all available players on your account and show their data

public class Scene2_PlayerSelect : MonoBehaviour
{
	[Header ("Characters")]
	public Text Nick1;
	public Text Nick2;
	public Text Nick3;
	public Button play;
	[Header ("CreateChar")]
	public InputField nick;

	public Text StatString;
	
	public Text text;

	public static List<Data_PlayerFile> temp = new List<Data_PlayerFile> ();

	// Use this for initialization
	void Start ()
	{
		Nick1.text = "";
		Nick2.text = "";
		Nick3.text = "";
		Networking_client.net.RegisterHandler (Networking_msgType.CreateChar, HandlerCreateChar);
	}

	// Update is called once per frame
	void Update ()
	{
		if (temp.Count == 3) {
			Nick1.text = temp [0].nick;
			Nick2.text = temp [1].nick;
			Nick3.text = temp [2].nick;
		}
		if (temp.Count == 2) {
			Nick1.text = temp [0].nick;
			Nick2.text = temp [1].nick;
		}
		if (temp.Count == 1) {
			Nick1.text = temp [0].nick;
		} 
		if (temp.Count == 0) {
			text.text = "Charecter no created. Please create new charecter.";
			play.enabled = true;
		}
		if (temp.Count >= 1) {
			if (Data_MyData.PlayerSelect == temp [0].nick) {
				StatString.text = string.Format("Name: {0} \r\n\r\nMaxHP: {1}\r\nPlayerScore: {2}", temp[0].nick, temp[0].HPMax, temp[0].PlayerScores);
			}
		}

		if (temp.Count >= 2) {
			if (Data_MyData.PlayerSelect == temp [1].nick) {
				StatString.text = string.Format("Name: {0} \r\n\r\nMaxHP: {1}\r\nPlayerScore: {2}", temp[1].nick, temp[1].HPMax, temp[1].PlayerScores);
            }
		}

		if (temp.Count == 3) {
			if (Data_MyData.PlayerSelect == temp [2].nick) {
				StatString.text = string.Format("Name: {0} \r\n\r\nMaxHP: {1}\r\nPlayerScore: {2}", temp[2].nick, temp[2].HPMax, temp[2].PlayerScores);
            }
		}
	}

	public void PlayerSelected ()
	{
		if (Data_MyData.PlayerSelect != "") {
			Message.PlayerSelected Sel = new Message.PlayerSelected ();
			Sel.playerNick = Data_MyData.PlayerSelect;
			Sel.Login = Data_MyData.Login;
			Sel.Password = Data_MyData.Password;
			Networking_client.net.Send (Networking_msgType.PlayerSelect, Sel);
		}
	}

	public void OnCreateChar ()
	{
		Message.CreateCh create = new Message.CreateCh ();
		create.nick = nick.text;
		Networking_client.net.Send (Networking_msgType.CreateChar, create);
	}

	void HandlerCreateChar (NetworkMessage netmsg)
	{
		Message.CreateCh ch = netmsg.ReadMessage<Message.CreateCh> ();
		text.text = ch.msg;
	}

	public void Quit ()
	{
		Application.Quit ();
	}

	public static void NextScen ()
	{
		SceneManager.LoadScene (1);
	}
}

 


