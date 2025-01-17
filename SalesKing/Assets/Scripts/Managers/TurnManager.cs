using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public int curTurn;

    public void ConvoStarted()
    {
        StopAndRestartTime(true);
        Managers.Cam.SwitchToDialogueCam();
        Managers.UI.ShowTalkOrNotPanel();
    }

    public void ConvoFinished()
    {
        StopAndRestartTime(false);
        Managers.Player.MyPlayer.GetComponent<Player>().PlayerExitConvo();
        Managers.NPC.curTalkingNPC.GetComponent<NPC>().currentTalkable = NPCDefine.Talkable.Not;
        Managers.NPC.curTalkingNPC.GetComponent<NPC>().myCanvas.SetActive(false);
        Managers.NPC.curTalkingNPC = null;
        Managers.Cam.SwitchToFirstPersonCam();
        Managers.Player.MyPlayer.GetComponent<Player>().PlayerBody.SetActive(true);
    }

    public void StopAndRestartTime(bool isStop)
    {
        if(isStop) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

}
