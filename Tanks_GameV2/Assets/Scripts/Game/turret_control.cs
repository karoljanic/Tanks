using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class turret_control : MonoBehaviour
{
    [SerializeField]private playerscript _PlayerScript;
    [SerializeField]private camera_control _Cam;

    public GameObject bulletprefab;
    public GameObject bulletprefebaleft;
    public GameObject trajectorydot;
    public Rigidbody2D tank;
    public Transform shotpoint;
    public Transform turret;
    public Transform tank_trans;
    public float force;
    public int numberofdots;

    private GameObject bullet;
    private GameObject[] trajectorydots;
    private Rigidbody2D myRigidbody;
    private Vector3 starpos;
    private Vector3 endpos;
    private Vector3 pos;
    public Vector3 forceatplayer;
    private Vector3 difference;
    private Quaternion rot;
    private bool t = false;
    private bool k = false;
    public bool facingRight = true;
    private Vector3 localScale;
    private float speed = 100;
    private float movement;
    private float rotZ;

    public Transform _SpawnedBullet;
    private bool _FollowBullet;

    private static Vector3 _CamTankOffset = new Vector3(0.0f, -1.2f, -20.0f);
    private static Vector3 _CamAimOffset = new Vector3(0.0f, -1.2f, -25.0f);
    private static Vector3 _CamBulletOffset = new Vector3(0.0f, 0.0f, -32.0f);

    private void Start()
    {
        _Cam = FindObjectOfType<camera_control>();
        _Cam.target = tank.transform;
        _Cam.Offset = _CamTankOffset;

        pos = shotpoint.position;
        localScale = tank.transform.localScale;
    }

    public bool CanShoot()
    {
        return !_FollowBullet;
    }

    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }


    private bool _TrajectoryVisible;

    public void DrawTrajectory()
    {
        for (int i = 0; i < trajectorydots.Length; i++)
            trajectorydots[i].transform.position = shotpoint.position + calculateposition2(i * 0.1f);
    }

    public void ShowTrajectory()
    {
        if (trajectorydots == null)
        {
            trajectorydots = new GameObject[40];
            for (int i = 0; i < trajectorydots.Length; i++)
            {
                trajectorydots[i] = Instantiate(trajectorydot, shotpoint.transform);
            }
        }
        for (var i = 0; i < trajectorydots.Length; i++)
        {
            trajectorydots[i].SetActive(true);
        }
        _TrajectoryVisible = true;
        DrawTrajectory();

        _Cam.target = tank.transform;
        _Cam.Offset = _CamAimOffset;
    }

    public void HideTrajectory()
    {
        if (trajectorydots != null)
        {
            for (var i = 0; i < trajectorydots.Length; i++)
            {
                trajectorydots[i].SetActive(false);
            }
        }
        _TrajectoryVisible = false;

        if (_SpawnedBullet == null)
        {
            _Cam.target = tank.transform;
            _Cam.Offset = _CamTankOffset;
        }
        else
        {
            _FollowBullet = true;
            _Cam.target = _SpawnedBullet;
            _Cam.Offset = _CamBulletOffset;
        }
    }

    public void Shoot()
    {
        //_SpawnedBullet = Instantiate(bulletprefab, shotpoint.position, Quaternion.identity).transform;
        _SpawnedBullet = PhotonNetwork.Instantiate("bulletprefab", shotpoint.position, shotpoint.transform.rotation, 0).transform;
        //_SpawnedBullet.transform.rotation = shotpoint.transform.rotation;
        myRigidbody = _SpawnedBullet.GetComponent<Rigidbody2D>();
        myRigidbody.gravityScale = 1;
        var multiplier = force * (transform.lossyScale.x > 0.0f ? 1.0f : -1.0f);
        myRigidbody.velocity = myRigidbody.transform.right * multiplier;
    }


    private void Update()
    {
        if (_PlayerScript.IsDead)
        {
            if (_TrajectoryVisible)
            {
                HideTrajectory();
            }
            return;
        }

        if (_FollowBullet)
        {
            if (_SpawnedBullet == null)
            {
                _FollowBullet = false;
                _Cam.target = tank.transform;
                _Cam.Offset = _CamTankOffset;
            }
        }

        if (_TrajectoryVisible)
        {
            DrawTrajectory();
        }

        if (InputController.IsDragActive)
        {

            Vector3 mousePosWorld = GetWorldPositionOnPlane(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f), 0.0f);

            float distanceFromPlayer = Vector3.Distance(transform.position, mousePosWorld);
            force = distanceFromPlayer * 1.0f;

            Vector3 dir = transform.position - mousePosWorld;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (facingRight)
            {
               // angle = Mathf.Clamp(angle, 10.0f, 50.0f);
            } else
            {
                angle += 180.0f;
                //angle = Mathf.Clamp(angle, 140.0f, 180.0f);
            }
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            var rotZ = transform.localRotation.eulerAngles.z;
            if (rotZ > 180.0f && rotZ < 360.0f)
            {
                rotZ = rotZ - 360.0f;
            }

            transform.localRotation = Quaternion.Euler(
                0.0f,
                0.0f,
                rotZ
                //Mathf.Clamp(transform.localRotation.eulerAngles.z, -20.0f, 20.0f)
                );
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 3.0f);
        }


        return;

        movement = CrossPlatformInputManager.GetAxis("Horizontal") * speed;

        if(!k & !t)
            rot = turret.rotation;

        if (Input.mousePosition.x < 1200 & Input.mousePosition.x > 900 & Input.mousePosition.y < 700 & Input.mousePosition.y > 350)
            t = true;

        if (Input.GetMouseButton(0) & t)
        {
            k = true;
        
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            
            if(facingRight)
            {
                rotZ = Mathf.Atan2(-difference.y, -difference.x ) * Mathf.Rad2Deg;

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
            endpos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
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

            for (int i = 0; i < numberofdots; i++)
                trajectorydots[i].transform.position = calculateposition(i * 0.1f);
        }
        if (Input.GetMouseButtonUp(0) & t)
        {
            t = false;
            if(facingRight)
                bullet = Instantiate(bulletprefab, shotpoint.position, Quaternion.identity);
            else
                bullet = Instantiate(bulletprefebaleft, shotpoint.position, Quaternion.identity);

            myRigidbody = bullet.GetComponent<Rigidbody2D>();
            myRigidbody.gravityScale = 1;
            myRigidbody.velocity = new Vector2(-forceatplayer.x * force, -forceatplayer.y * force);

            for (int i = 0; i < numberofdots; i++)
                Destroy(trajectorydots[i]);

            turret.rotation = rot;
        }

    }

    private void LateUpdate()
    {
        check_where_to_face();
    }

    private Vector2 calculateposition(float time)
    {
        return new Vector2(endpos.x, endpos.y) + new Vector2(-forceatplayer.x * force, -forceatplayer.y * force) * time + 0.5f * Physics2D.gravity * time * time;
    }

    private Vector3 calculateposition2(float time)
    {
        var multiplier = force * (transform.lossyScale.x > 0.0f ? 1.0f : -1.0f);
        return ((Vector2)(shotpoint.right * multiplier * time) + (0.5f * Physics2D.gravity * time * time));
        //return new Vector2(-forceatplayer.x * force, -forceatplayer.y * force) * time + 0.5f * Physics2D.gravity * time * time;
    }

    void check_where_to_face()
    {
        return;
        if (movement > 0)
            facingRight = true;
        else if (movement < 0)
            facingRight = false;
    }

}