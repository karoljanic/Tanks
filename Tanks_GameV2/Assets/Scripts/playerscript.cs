using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerscript : MonoBehaviour
{

    [SerializeField]private SpriteRenderer[] _BodyViews;
    [SerializeField]private SpriteRenderer[] _TurretViews;
    [SerializeField]private SpriteRenderer[] _TankTrackViews;

    public TextMesh _Text;
    public int maxHealth = 100;

    public HealthBar healthBar;


    // Start is called before the first frame update
    void Start()
    {
        object tankID;
        View.owner.CustomProperties.TryGetValue("TankID", out tankID);
        int iTankID = (int)tankID;

        for(var i = 0; i < _BodyViews.Length; i++)
        {
            if(i == iTankID)
                _BodyViews[i].enabled = true;
            else
                _BodyViews[i].enabled = false;

        }

        for (var i = 0; i < _TurretViews.Length; i++)
        {
            if (i == iTankID)
                _TurretViews[i].enabled = true;
            else
                _TurretViews[i].enabled = false;
        }

        for (var i = 0; i < _TankTrackViews.Length-1; i++)
        {
            if (i == iTankID)
                _TankTrackViews[i].enabled = true;
            else
                _TankTrackViews[i].enabled = false;
        }

        if (2 == iTankID)
            _TankTrackViews[_TankTrackViews.Length-1].enabled = true;
        else
            _TankTrackViews[_TankTrackViews.Length-1].enabled = false;


        var bars = FindObjectOfType<HealthBars>();
        if (View != null && View.owner != null)
        {
            healthBar = bars.GetTankBar(View.owner);
        }
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        hitPoints = MAX_HP;
        //healthBar.SetMaxHealth(maxHealth);
    }

    private PhotonView _View;
    public PhotonView View
    {
        get
        {
            if (_View == null)
            {
                _View = GetComponent<PhotonView>();
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
            if (View.owner.CustomProperties.TryGetValue("HP", out hp))
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
                hashtable.Add("HP", value);
                View.owner.SetCustomProperties(hashtable);
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

        if (View.isMine)
        {
            if (PhotonNetwork.playerList.Length < 2)
            {
                if (!PlayerQuitPopup.Instance.IsVisible)
                {
                    PlayerQuitPopup.Instance.Show();
                }
                return;
            }
        }

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
        if (!View.isMine)
        {
            hitPoints -= damage;
        }
    }
}
