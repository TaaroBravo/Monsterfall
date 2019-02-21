using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RogueDashHability : IHability
{

    Collider _hitArea;

    float _speed;
    float _power;

    float _currentTime;
    float _activeTime;

    bool active;

    Vector3 damage;
    Vector3 finalPos;
    Vector3 _dir;

    Vector3 startPos;

    ParticleSystem _hability;
    ParticleSystem.EmissionModule emissionModule;

    List<PlayerController> playersHitted = new List<PlayerController>();

    public RogueDashHability(PlayerController p, CdHUDChecker _cooldownHUD, Collider hitArea, ParticleSystem hability, float power, float dashingTime, float speed, float _timerCoolDown = 0)
    {
        player = p;
        _speed = speed;
        _power = power;
        timerCoolDown = _timerCoolDown;
        coolDown = _timerCoolDown;
        _activeTime = dashingTime;
        cooldownHUD = _cooldownHUD;
        _hitArea = hitArea;
        _hability = hability;
        startPos = player.transform.position;
        emissionModule = _hability.emission;
        _hability.gameObject.SetActive(false);
        ((Rogue)p).OnTeleportRogue += Teleporting;
    }

    void Teleporting()
    {
        player.StartCoroutine(ParticlesTeleporting());
    }

    IEnumerator IsDashingTimer(float x)
    {
        while (true)
        {
            emissionModule.rateOverDistance = 7;
            active = true;
            player.isDashing = true;
            _hability.gameObject.SetActive(true);
            _hability.Play();
            player.myAnim.Play("Dash");
            player.myAnim.SetBool("Dashing", true);
            yield return new WaitForSeconds(x);
            player.isDashing = false;
            active = false;
            player.StartCoroutine(OffParticles());
            emissionModule.rateOverDistance = 0;
            ResetValues();
            break;
        }
    }

    IEnumerator OffParticles()
    {
        yield return new WaitUntil(() => _hability.particleCount == 0);
        _hability.gameObject.SetActive(false);
    }

    IEnumerator ParticlesTeleporting()
    {
        emissionModule.rateOverDistance = 0;
        yield return new WaitForSeconds(0.5f);
        emissionModule.rateOverDistance = 7;
    }

    public override void Update()
    {
        base.Update();

        if (active)
        {

            //player.transform.position += _dir * _speed * Time.deltaTime;
            player.controller.Move(_dir * _speed * Time.deltaTime);
            Collider[] cols = Physics.OverlapSphere(_hitArea.bounds.center, _hitArea.bounds.extents.y, LayerMask.GetMask("Hitbox"));
            foreach (Collider c in cols)
            {
                if (CheckParently(c.transform))
                    continue;
                PlayerController target = TargetScript(c.transform);
                if (target != null && !playersHitted.Contains(target))
                {
                    playersHitted.Add(target);
                    target.SetDamage(10);
                    target.ReceiveImpact(damage * _power, player);
                    player.whoIHited = target;
                }
            }
        }
        //if ((finalPos - player.transform.position).magnitude < 3f)
        //{
        //    player.transform.position = finalPos;
        //    ResetValues();
        //}
    }

    public override void Hability()
    {
        if (timerCoolDown < 0)
        {
            float x = player.GetComponent<PlayerInput>().MainHorizontal();
            float y = player.GetComponent<PlayerInput>().MainVertical();
            if (x + y == 0)
                x = Mathf.Sign(player.transform.localScale.z);
            player.ResetVelocity();
            float distance = _speed * _activeTime;
            _dir = new Vector3(x, y, 0);
            finalPos = CalculateFinalPos(_dir, distance);
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, _dir, out hit, 30))
            {
                if(hit.collider.gameObject.layer == 17 || (hit.collider.gameObject.layer == 19 && hit.collider.gameObject.tag == "Limit"))
                {
                    GameManager.Instance.RegisterLastPos(player, hit.point);
                    finalPos = hit.point;
                }
                else if (hit.collider.gameObject.tag == "Borders" && (hit.point - player.transform.position).magnitude < 6f)
                {
                    GameManager.Instance.RegisterLastPos(player, hit.point);
                    finalPos = hit.point;
                }
            }
            damage = _dir;
            _hability.Play();
            //player.controller.enabled = false;
            OffCollisions();
            player.StartCoroutine(IsDashingTimer(_activeTime));
            timerCoolDown = coolDown;
            player.usingHability = true;
            AudioManager.Instance.CreateSound("TeleportAbility");
            player.lifeHUD.ActivateDashCD();
        }
    }

    void OffCollisions()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Platform"))
        {
            foreach(Collider col in item.GetComponents<Collider>())
                Physics.IgnoreCollision(player.GetComponent<Collider>(), col, true);
        }
    }

    void OnCollisions()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("Platform"))
        {
            foreach (Collider col in item.GetComponents<Collider>())
                Physics.IgnoreCollision(player.GetComponent<Collider>(), col, false);
        }
    }

    Vector3 CalculateFinalPos(Vector3 dir, float distance)
    {
        finalPos = player.transform.position + (dir * distance);
        return finalPos;
    }

    void ResetValues()
    {
        finalPos = Vector3.zero;
        player.moveVector.x = 0;
        player.StopCoroutine(IsDashingTimer(_activeTime));
        player.canInteract = true;
        player.myAnim.SetBool("Dashing", false);
        //player.controller.enabled = true;
        OnCollisions();
        player.usingHability = false;
        playersHitted.Clear();
    }

    public override void Release()
    {

    }

    #region Extra Calculation
    public bool CheckParently(Transform t)
    {
        if (t.parent == null)
            return false;
        if (t.parent == player.transform)
            return true;
        return CheckParently(t.parent);
    }

    public PlayerController TargetScript(Transform t)
    {
        if (t.GetComponent<PlayerController>())
            return t.GetComponent<PlayerController>();
        if (t.parent == null)
            return null;
        return TargetScript(t.parent);
    }
    #endregion
}
