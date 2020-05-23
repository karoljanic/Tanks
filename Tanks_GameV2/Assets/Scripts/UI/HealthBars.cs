using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    [SerializeField]private HealthBar _BarMainPlayer;
    [SerializeField]private HealthBar _BarMainTank;
    [SerializeField]private HealthBar _BarEnemyPlayer;
    [SerializeField]private HealthBar _BarEnemyTank;

    [SerializeField]private Sprite[] _PlayersIcons;
    [SerializeField]private Sprite[] _TanksIcons;

    private bool _Initialized;

    void Start()
    {

        for(var i = 0; i < PhotonNetwork.playerList.Length; i++)
        {

            object avatarID;
            PhotonNetwork.playerList[i].CustomProperties.TryGetValue("AvatarID", out avatarID);

            object tankID;
            PhotonNetwork.playerList[i].CustomProperties.TryGetValue("TankID", out tankID);

            if (PhotonNetwork.playerList[i].IsMasterClient)
            {
                _BarMainPlayer.SetIcon(_PlayersIcons[(int)avatarID]);
                _BarMainTank.SetIcon(_TanksIcons[(int)tankID]);
            }
            else
            {
                _BarEnemyPlayer.SetIcon(_PlayersIcons[(int)avatarID]);
                _BarEnemyTank.SetIcon(_TanksIcons[(int)tankID]);
            }
        }
    }

    public HealthBar GetPlayerBar(PhotonPlayer player)
    {
        if (player == null)
        {
            return null;
        }
        return player.IsMasterClient ? _BarMainPlayer : _BarEnemyPlayer;
    }

    public HealthBar GetTankBar(PhotonPlayer player)
    {
        if (player == null)
        {
            return null;
        }
        return player.IsMasterClient ? _BarMainTank : _BarEnemyTank;
    }
}
