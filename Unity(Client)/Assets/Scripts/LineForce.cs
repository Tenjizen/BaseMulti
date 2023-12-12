using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineForce : MonoBehaviour
{
    [SerializeField] float _power = 150;
    [SerializeField] float _velocityForShoot = 0.05f;
    [SerializeField] private LineRenderer _lr;

    [SerializeField] ParticleSystem _particleSystemInHole;
    public bool InHole = false;
    
    public int _idClient;

    private bool _canShoot;
    private bool _aim;

    [SerializeField] Rigidbody _rb;
    [SerializeField] SphereCollider _sphereC;
    [SerializeField] MeshRenderer _mr;

    private Vector3 _startPos;
    private Vector3 _oldPos;

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        _sphereC = GetComponent<SphereCollider>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        Init();
    }
    private void Update()
    {
        Server();

        PlayerMove();
    }
    void PlayerMove()
    {
        if (GameManager.Instance.IDClient != GameManager.Instance.PlayerIDTurn) { return; }

        if (GameManager.Instance.IDClient == _idClient)
        {
            if (_rb.velocity.magnitude < _velocityForShoot && _canShoot == false)
            {
                SetCanShootToTrue();
            }
            Aim();
        }
    }
    void Server()
    {
        if (InHole == true || GameManager.Instance.IDClient != _idClient) { return; }
        var multiplicateur = 1000f;

        var oldPos = _oldPos;
        oldPos.x = Mathf.Round(oldPos.x * multiplicateur) / multiplicateur;
        oldPos.y = Mathf.Round(oldPos.y * multiplicateur) / multiplicateur;
        oldPos.z = Mathf.Round(oldPos.z * multiplicateur) / multiplicateur;

        var current = transform.localPosition;
        current.x = Mathf.Round(current.x * multiplicateur) / multiplicateur;
        current.y = Mathf.Round(current.y * multiplicateur) / multiplicateur;
        current.z = Mathf.Round(current.z * multiplicateur) / multiplicateur;

        if (oldPos != current)
        {
            GameManager.Instance.PlayerIORef.SendMove(current);
            _oldPos = transform.localPosition;
        }
    }
  
    public void SetId(int id)
    {
        _idClient = id;
        _oldPos = transform.position;
    }
    private void SetCanShootToTrue()
    {
        _startPos = transform.position;
        _canShoot = true;

        GameManager.Instance.PlayerIORef.SendEndMove();
    }
    public void SetInHole()
    {
        InHole = true;

        _particleSystemInHole.Play();
        
        _mr.enabled = false;
        _sphereC.enabled = false;
        _aim = false;
        _lr.enabled = false;

        GameManager.Instance.CheckIfNextLevel();
    }

    private void ResetPlayer()
    {
        InHole = false;
        _mr.enabled = true;
        _sphereC.enabled = true;
        _aim = false;
        _lr.enabled = false;
        _canShoot = true;
    }
    public void Init()
    {
        InHole = false;
        _mr.enabled = true;
        _sphereC.enabled = true;
        _aim = false;
        _lr.enabled = false;
        _canShoot = true;

        GameManager.Instance.PlayersScript.Add(this);
    }
    public void ResetOldPos()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.position = _startPos;

        GameManager.Instance.PlayerIORef.SendEndMove();
    }


    private void OnMouseDown()
    {
        if (_canShoot)
        {
            _aim = true;
        }
    }

    private void Aim()
    {
        if (!_aim || !_canShoot) { return; }

        Vector3? worldPt = CastMouseClickRay();

        if (!worldPt.HasValue) { return; }

        DrawLine(worldPt.Value);

        if (Input.GetMouseButtonUp(0))
        {
            Shoot(worldPt.Value);
        }

    }

    private void Shoot(Vector3 value)
    {
        _aim = false;
        _lr.enabled = false;
        Vector3 horizWorldPt = new Vector3(value.x, transform.position.y, value.z);
        Vector3 dir = (horizWorldPt - transform.position).normalized;
        float strength = Vector3.Distance(transform.position, horizWorldPt);

        _rb.AddForce(-dir * strength * _power);
        _canShoot = false;
    }

    private void DrawLine(Vector3 point)
    {
        Vector3[] pos =
        {
            transform.position,
            point };
        _lr.SetPositions(pos);
        _lr.enabled = true;
    }
    private Vector3? CastMouseClickRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

        Vector3 wolrdMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 wolrdMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

        RaycastHit hit;
        if (Physics.Raycast(wolrdMousePosNear, wolrdMousePosFar - wolrdMousePosNear, out hit, float.PositiveInfinity))
        {
            return hit.point;
        }
        else
        {
            return null;
        }
    }
}

