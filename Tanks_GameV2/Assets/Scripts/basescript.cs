using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basescript : MonoBehaviour
{
    [SerializeField]private bool _Master;
    public int maxHealth = 100;

    public HealthBar healthBar;


    // Start is called before the first frame update
    void Start()
    {
        var bars = FindObjectOfType<HealthBars>();
        if (View != null && View.owner != null)
        {
            healthBar = bars.GetPlayerBar(View.owner);
        }
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        //healthBar.SetMaxHealth(maxHealth);
        hitPoints = MAX_HP;
    }

    private PhotonView _View;
    public PhotonView View
    {
        get
        {
            if (_View == null)
            {
                _View = GetComponent<PhotonView>();

                PhotonPlayer player1 = PhotonNetwork.playerList[0];
                PhotonPlayer player2 = PhotonNetwork.playerList.Length < 2 ? null : PhotonNetwork.playerList[1];

                PhotonPlayer resultPlayer = null;

                if (_Master)
                {
                    if (player1.IsMasterClient)
                    {
                        resultPlayer = player1;
                    }
                    else
                    {
                        resultPlayer = player2;
                    }
                }
                else
                {

                    if (player1.IsMasterClient)
                    {
                        resultPlayer = player2;
                    }
                    else
                    {
                        resultPlayer = player1;
                    }
                }
                if (resultPlayer != null)
                {
                    _View.TransferOwnership(resultPlayer);
                }
            }
            return _View;
        }
    }

    public bool IsDead { get; private set; }

    const int MAX_HP = 100;
    int hitPoints
    {
        get
        {
            object hp;
            if (View == null || View.owner == null)
            {
                return MAX_HP;
            }
            if (View.owner.CustomProperties.TryGetValue("BaseHP", out hp))
                return (int)hp;
            else
                return MAX_HP;
        }
        set
        {
            if (!IsDead)
            {
                value = Mathf.Clamp(value, 0, int.MaxValue);
                var hashtable = new ExitGames.Client.Photon.Hashtable();
                hashtable.Add("BaseHP", value);
                if (View != null && View.owner != null)
                {
                    View.owner.SetCustomProperties(hashtable);
                }
            }
        }
    }

    private void DeadHandler()
    {
        if (!VictoryPopup.Instance.IsVisible && !DefeatPopup.Instance.IsVisible)
        {
            if (View.isMine)
            {
                DefeatPopup.Instance.Show();
            }
            else
            {
                VictoryPopup.Instance.Show();
            }
        }
    }

    void Update()
    {
        int hp = hitPoints;
        if (healthBar != null)
        {
            healthBar.SetHealth(hp);
            if (!IsDead)
            {
                IsDead = hp == 0;
                if (IsDead)
                {
                    DeadHandler();
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (View != null)
        {
            if (!View.isMine)
            {
                hitPoints -= damage;
            }
        }
    }
}
