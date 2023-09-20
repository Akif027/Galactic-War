using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
public class TempScript : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        // Trigger a custom event when a player joins the room
        string playerName = PhotonNetwork.LocalPlayer.NickName;
        object[] content = new object[] { playerName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(0, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
