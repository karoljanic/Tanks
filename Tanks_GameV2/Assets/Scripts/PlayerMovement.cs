using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : Photon.MonoBehaviour
{

    [SerializeField]private playerscript _PlayerScript;

    private PhotonView PhotonView;
    public float Movement;
    public GameObject FrontWheel;
    public GameObject MiddleWheel;
    public GameObject BackWheel;

    public GameObject CoM;
    public Rigidbody2D _R;

/*
    public Vector3 TargetPosition;
    public Quaternion TargetRotation;
    public Vector3 TargetPosition1;
    public Quaternion TargetRotation1;
*/
    private Vector3 starpos;
    private Vector3 endpos;
    private Vector3 pos;
    private GameObject[] trajectorydots;
    public GameObject trajectorydot;
    public int numberofdots;
    public Transform shotpoint;
    public Transform turret;
    public Vector3 forceatplayer;
    public GameObject bulletprefab;
    public GameObject bulletprefebaleft;
    public float force;
    private Quaternion rot;
    private bool t = false;
    private bool k = false;
    private float rotZ;
    private GameObject bullet;
    public GameObject tank;
    private Rigidbody2D rb;
    public Vector3 MousePosition;
    public Vector3 TankPosition;

    private turret_control _TurretControl;

    [SerializeField]
    private bool facingRight = true;
    Vector3 localScale;

    public float Speed = 2500;

    private WheelJoint2D Wheel1, Wheel2, Wheel3;

    [SerializeField]private TankWheel[] _Wheels;

    private JointMotor2D Motor;



    private void Awake()
    {
        Vector3 pos = new Vector3(31.5f, -0.2f, 0f);
        if (transform.position == pos)
            facingRight = false;
        PhotonView = GetComponent<PhotonView>();

        localScale = transform.localScale;
        Wheel1 = FrontWheel.GetComponent<WheelJoint2D>();
        Wheel2 = MiddleWheel.GetComponent<WheelJoint2D>();
        Wheel3 = BackWheel.GetComponent<WheelJoint2D>();

        _TurretControl = GetComponentInChildren<turret_control>();

        pos = shotpoint.position;
        trajectorydots = new GameObject[numberofdots];

        if (!photonView.isMine)
        {
            Destroy(_TurretControl);
            Destroy(this);
            Destroy(Wheel1);
            Destroy(Wheel2);
            Destroy(Wheel3);
            Destroy(FrontWheel.GetComponent<Rigidbody2D>());
            Destroy(MiddleWheel.GetComponent<Rigidbody2D>());
            Destroy(BackWheel.GetComponent<Rigidbody2D>());
            Destroy(_R);
            return;
        }
    }


    void Update()
    {
        if (_PlayerScript.IsDead)
        {
            return;
        }
        var rr = transform.rotation.eulerAngles.z;
        rr = rr > 180.0f ? (rr - 360.0f) : rr;
        rr = Mathf.Clamp(rr, -50.0f, 50.0f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rr);
        var m = 1.0f / (Mathf.Abs(transform.rotation.eulerAngles.z - 180.0f)/90.0f);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.identity,
            Time.deltaTime * 2.0f//* m
            );
        var rk = Vector2.SignedAngle(Vector2.right, transform.right);// Quaternion.Angle(transform.rotation, transform.right)
        rk = Mathf.Clamp(rk, -90.0f, 90.0f);

        _MForce = (Mathf.Abs(rk) / 90.0f);

        if (!_LockMove)
        {
            _R.AddForce((facingRight ? Vector3.right : -Vector3.right) * 30.0f * _MForce);
        }

        Speed = 180.0f + (2500.0f * _MForce);

        CoM.transform.localPosition = //_R.centerOfMass;
        
          Vector2.Lerp(
            new Vector2(facingRight ? -2.0f : 2.0f, 0.0f),
            new Vector2(facingRight ? 2.0f : -2.0f, 0.0f),
            ((rk / 90.0f) + 1.0f) * 0.5f
            );

        _R.centerOfMass = CoM.transform.localPosition;

       // Debug.Log(transform.rotation.eulerAngles.z + " "  + m + " " + rk);
        if (PhotonView.isMine)
            CheckInput();
     //   else
     //       SmoothMove();
    }
    /*
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            TargetPosition = transform.position;
            TargetRotation = transform.rotation;
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            TargetPosition1 = (Vector3)stream.ReceiveNext();
            TargetRotation1 = (Quaternion)stream.ReceiveNext();
        }
    }
    
    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500*Time.deltaTime);
    }
    */

    private bool _DrawTrajectory;

    private float _MForce;

    private bool _LockMove;
    public float _LockMoveTime;

    int wheelsWithCollision = 0;
    private void CheckInput()
    {
        wheelsWithCollision = 0;
        for (var i = 0; i < _Wheels.Length; i++)
        {
            if (_Wheels[i].CollisionsCount > 0)
                wheelsWithCollision++;
        }

        if (_LockMove)
        {
            if (wheelsWithCollision >= 2)
            {
                _LockMove = false;
            }
        }
        else
        {
            if (wheelsWithCollision < 2)
            {
                _LockMoveTime += Time.deltaTime;
                if (_LockMoveTime > 1.2f)
                {
                    _LockMove = true;
                }
            }
            else
            {
                _LockMoveTime = 0.0f;
            }
        }

        //if (CrossPlatformInputManager.GetButtonDown("Move"))
        if (InputController.IsLeftDown || InputController.IsRightDown)
            SetMovement();
        if (!InputController.IsLeftDown && !InputController.IsRightDown)
            Movement = 0;

        if (_LockMove)
        {
            _R.mass = 10.0f;
            // _R.velocity = new Vector2(_R.velocity.x, Physics2D.gravity.y * 3.0f);
        }
        else
        {
            _R.mass = 1.0f;
        }

        check_where_to_face();

        Motor = new JointMotor2D { motorSpeed = Movement, maxMotorTorque = 150000 };

        Wheel1.motor = Motor;
        Wheel2.motor = Motor;
        Wheel3.motor = Motor;

        if (InputController.IsDragActive)
        {
            if (!_DrawTrajectory)
            {
                if (_TurretControl.CanShoot())
                {
                    _TurretControl.ShowTrajectory();
                    _DrawTrajectory = true;
                }
            }
        }
        else
        {
            if (_DrawTrajectory)
            {
                _TurretControl.HideTrajectory();
                _DrawTrajectory = false;
            }
        }

        Shooting();
    }

    public void TryShoot()
    {
        if (_DrawTrajectory)
        {
            if (_TurretControl.CanShoot())
            {
                _TurretControl.Shoot();
            }
            _TurretControl.HideTrajectory();
            _DrawTrajectory = false;
        }
    }
    
    void SetMovement()
    {
        if (CrossPlatformInputManager.GetAxis("Horizontal") < 0 || Input.GetKey(KeyCode.LeftArrow) || InputController.IsLeftDown)
        {
            Movement = -Speed;
        }
        else
        {
            Movement = Speed;
        }
    }

    void check_where_to_face()
    {
        if (Movement > 0)
            facingRight = true;
        else if (Movement < 0)
            facingRight = false;
        _TurretControl.facingRight = facingRight;
        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    private Vector2 calculateposition(float time)
    {
        return new Vector2(endpos.x, endpos.y) + new Vector2(-forceatplayer.x * force, -forceatplayer.y * force) * time + 0.5f * Physics2D.gravity * time * time;
    }

    private void Shooting()
    {
        return;
        MousePosition = Input.mousePosition;
        TankPosition = tank.GetComponent<Rigidbody2D>().position;
      
        if (!k & !t)
            rot = turret.rotation;

        if (Input.mousePosition.x < 1200 & Input.mousePosition.x > 900 & Input.mousePosition.y < 700 & Input.mousePosition.y > 350)
            t = true;

        if (Input.GetMouseButton(0) & t)
        {
            k = true;

            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            if (facingRight)
            {
                rotZ = Mathf.Atan2(-difference.y, -difference.x) * Mathf.Rad2Deg;

                if (rotZ < 0)
                    rotZ = 0;
                else if (rotZ > 80)
                    rotZ = 80;
            }
            else
            {
                rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                if (rotZ > 0)
                    rotZ = 0;
                else if (rotZ < -80)
                    rotZ = -80;
            }

            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

        }

        if (Input.GetMouseButtonDown(0) & t)
        {
            t = true;
            starpos = shotpoint.transform.position;

            for (int i = 0; i < numberofdots; i++)
                trajectorydots[i] = Instantiate(trajectorydot, shotpoint.transform);

        }
        if (Input.GetMouseButton(0) & t)
        {
            endpos = shotpoint.transform.position;
                //Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
            forceatplayer = endpos - starpos;
            if (facingRight)
            {
                if (forceatplayer.x > -1)
                    forceatplayer.x = (float)-1;
                if (forceatplayer.y > 2)
                    forceatplayer.y = (float)2;
            }
            else
            {
                if (forceatplayer.x < 1)
                    forceatplayer.x = (float)1;
                if (forceatplayer.y > 2)
                    forceatplayer.y = (float)2;
            }

            endpos = shotpoint.transform.position;
            //Debug.Log("END POS = " + endpos.ToString());
            for (int i = 0; i < numberofdots; i++)
                trajectorydots[i].transform.position = calculateposition(i * 0.1f);
        }
        if (Input.GetMouseButtonUp(0) & t)
        {
            t = false;
            if (facingRight)
                bullet = Instantiate(bulletprefab, shotpoint.position, Quaternion.identity);
            else
                bullet = Instantiate(bulletprefebaleft, shotpoint.position, Quaternion.identity);

            rb = bullet.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;
            rb.velocity = new Vector2(-forceatplayer.x * force, -forceatplayer.y * force);

            for (int i = 0; i < numberofdots; i++)
                Destroy(trajectorydots[i]);

            turret.rotation = rot;
        }

    }


}