using UnityEngine;
using UnityEngine.Networking;

class Networking_OnPlayerAction : MonoBehaviour
{

    private static string nick1;

    //Check whether it is a character of the player and turn the player on its index, and gave him the coordinates of movement
    static public void OnPlMove(NetworkMessage netms)
    {
        Message_Sr.PlayerGoTo_Sr Go = netms.ReadMessage<Message_Sr.PlayerGoTo_Sr>();
        if (ListScene.CheckPlayer(Go.index, Go.login, Go.password))
        {
            try
            {
                if (Go.keySend)
                {
                    ListScene.playerMoveScr[Go.index].Newposit(Go.key, Go.down);
                }
                else
                {
                    ListScene.playerMoveScr[Go.index].axisY = Go.axisY;
                }
            }
            catch (UnityException ex)
            {
                Debug.Log(ex.Message);
                Debug.Log("Networking_OnPlayerMove: ERROR");
            }
        }
    }

    static public void OnMouseButton(NetworkMessage netmsg)
    {
        Message_Sr.MouseButton_Sr mouse = netmsg.ReadMessage<Message_Sr.MouseButton_Sr>();
        if (ListScene.CheckPlayer(mouse.index, mouse.log, mouse.pass))
        {
            if (mouse.down)
            {
                ListScene.playerMoveScr[mouse.index].StartFire();
            }
            else
            {
                ListScene.playerMoveScr[mouse.index].StopFire();
            }
        }
    }

    static public void ReaplyPosition(int index, Vector3 positon, int scene)
    {
        Message_Sr.PlayerGoTo_Sr go = new Message_Sr.PlayerGoTo_Sr();
        go.index = index;
        go.sendVec = positon;
        Networking_Server.SendMSG(Networking_msgType_Sr.PlayerMove, go);
    }

    public static void HandleRespawn(NetworkMessage netmsg)
    {
        Message_Sr.Respawn_Sr rs = netmsg.ReadMessage<Message_Sr.Respawn_Sr>();
        if (ListScene.CheckPlayer(rs.index, rs.log, rs.pass))
        {
            Player_MovePlayer pl = ListScene.GetPlayerControll(rs.index);
            if(pl != null)
            {
                pl.Respawn();
            }
        }
    }

    public static void HandleReload(NetworkMessage netMsg)
    {
        Message_Sr.PlayerAction action = netMsg.ReadMessage<Message_Sr.PlayerAction>();
        if(ListScene.CheckPlayer(action.index, action.log, action.pass))
        {
            Player_MovePlayer pl = ListScene.GetPlayerControll(action.index);
            if(pl != null)
            {
                pl.StartReload();
            }
        }
    }

    public static void HandleDropWeapon(NetworkMessage netMsg)
    {
        Message_Sr.DropWeapon_Sr drop = netMsg.ReadMessage<Message_Sr.DropWeapon_Sr>();

        if(ListScene.CheckPlayer(drop.index, drop.log, drop.pass))
        {
            Player_MovePlayer controll = ListScene.GetPlayerControll(drop.index);
            if (controll)
            {
                controll.DropWeapon();
            }
        }
    }

    public static void HandlePickUpItem(NetworkMessage netMsg)
    {
        Message_Sr.PickUpWeapon_Sr pick = netMsg.ReadMessage<Message_Sr.PickUpWeapon_Sr>();

        if (ListScene.CheckPlayer(pick.index, pick.log, pick.pass))
        {
            Player_MovePlayer controll = ListScene.GetPlayerControll(pick.index);
            if (controll)
            {
                controll.DropWeapon();
                PickUpItem(pick.indexItem, controll);
            }
        }
    }

    static void PickUpItem(int itemIndex, Player_MovePlayer controll)
    {
        Player_Item_Sr item = ListScene.GetItem(itemIndex);

        if (item)
        {
            switch (item.ItemType)
            {
                case ItemType_Sr.weapon:
                    PickUpWeapon(item, controll);
                    break;
            }
        }
    }

    static void PickUpWeapon(Player_Item_Sr item, Player_MovePlayer controll)
    {
        Player_Weapon_Sr weap = null;
        
        weap = item.gameObject.GetComponent<Player_Weapon_Sr>();

        if (weap)
        {
            weap.TakeWeapon(controll.weapon, controll.index);
        }
    }
}
