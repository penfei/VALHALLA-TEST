using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Networking_Chat_Sr : MonoBehaviour
{
	
	enum ChatMessage : int
	{
		chat = 1,
		privat,
		system
	}

	public static void ChatHandler (NetworkMessage netmsg)
	{
		Message_Sr.Chat_Sr chatR = netmsg.ReadMessage<Message_Sr.Chat_Sr> ();

		if (ListScene.CheckPlayer (chatR.index, chatR.log, chatR.pass)) {
			switch (chatR.msgType) {
			case (int)ChatMessage.chat:
				Message_Sr.Chat_Sr chatW = new Message_Sr.Chat_Sr ();
				chatW.msgTypeW = (int)ChatMessage.chat;
				chatW.nickW = ListScene.playerList [chatR.index].nick;
				chatW.id = chatR.index;
				chatW.msgW = chatR.msg;
				Networking_Server.SendMSG (Networking_msgType_Sr.Chat, chatW);
				break;
			case (int)ChatMessage.privat:
				if (chatR.indexPriv == -1) {
					for (int i = 0; i < ListScene.playerList.Count; i++) {
						if (ListScene.playerList [i].nick == chatR.nick) {
							chatR.indexPriv = i;
							break;
						}
					}
				}
				if (chatR.indexPriv == -1) {
					NetworkWriter wr1 = new NetworkWriter ();
					wr1.StartMessage (Networking_msgType_Sr.Chat);
					wr1.Write ((int)ChatMessage.system);
					wr1.Write ("System");
					wr1.Write (-1);
					wr1.Write ("This player is offline.");
					wr1.FinishMessage ();
					netmsg.conn.SendWriter (wr1, 0);
				} else {
					NetworkWriter wr2 = new NetworkWriter ();
					wr2.StartMessage (Networking_msgType_Sr.Chat);
					wr2.Write ((int)ChatMessage.privat);
					wr2.Write (ListScene.playerList [chatR.indexPriv].nick);
					wr2.Write (chatR.index);
					wr2.Write (chatR.msg);
					wr2.FinishMessage ();
					ListScene.listConnection [chatR.index].SendWriter (wr2, 0);
					NetworkWriter wr3 = new NetworkWriter ();
					wr3.StartMessage (Networking_msgType_Sr.Chat);
					wr3.Write ((int)ChatMessage.privat);
					wr3.Write (ListScene.playerList [chatR.index].nick);
					wr3.Write (chatR.index);
					wr3.Write (chatR.msg);
					wr3.FinishMessage ();
					ListScene.listConnection [chatR.indexPriv].SendWriter (wr3, 0);
				}
				break;
			}
		}
	}
}


