using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    Dictionary<string, AudioClip> allClips = new Dictionary<string, AudioClip>();
    public GameObject audioClipInWorldPrefab;

    [Header("GENERAL")]
    [Space]
    public AudioClip run;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip punch;
    public AudioClip hitPlayer;
    public AudioClip death;
    public AudioClip stunned;

    public AudioClip teleport;

    [Header("HUD")]
    [Space]
    public AudioClip navigationOnHud;
    public AudioClip pauseClip;
    public AudioClip winRound; //Manejada por la musica
    public AudioClip winGame; //manejada por la musica


    [Header("Caballero")]
    [Space]
    public AudioClip explosionKnight;
    public AudioClip fireJump;
    public AudioClip fire;

    [Header("Pirate")]
    [Space]
    public AudioClip throwHook;
    public AudioClip hookSomething;
    public AudioClip traveling;
    public AudioClip fireGun;

    [Header("Rogue")]
    [Space]
    public AudioClip throwBlade;
    public AudioClip throwHit;
    public AudioClip teleportAbility;

    [Header("Berserk")]
    [Space]
    public AudioClip chargingAttack;
    public AudioClip jumpingAttack;
    public AudioClip attackingAbility;
    public AudioClip runAbility;
    public AudioClip hitWithRun;

    [Header("Elf")]
    [Space]
    public AudioClip shootingMissileElf;
    public AudioClip incomingMissiles;
    public AudioClip explosionMissiles;
    public AudioClip teleportMissile;
    public AudioClip teleportingElf;
    public AudioClip markEffect;

    [Header("Yeti")]
    [Space]
    public AudioClip explosionYeti;
    public AudioClip hitWithIce;
    public AudioClip iceIdle; //Crear en el hielo
    public AudioClip creationOfPlatform;

    private void Awake()
    {
        Instance = this;
        CreateDictionary();
    }

    void CreateDictionary()
    {
        allClips.Add("Run", run);
        allClips.Add("Jump", jump);
        allClips.Add("Land", land);
        allClips.Add("Punch", punch);
        allClips.Add("HitPlayer", hitPlayer);
        allClips.Add("Death", death);
        allClips.Add("Teleport", teleport);
        allClips.Add("Stunned", stunned);

        allClips.Add("NavigationHUD", navigationOnHud);
        allClips.Add("Pause", pauseClip);

        allClips.Add("ExplosionKnight", explosionKnight);
        allClips.Add("FireJump", fireJump);
        allClips.Add("Fire", fire);

        allClips.Add("ThrowHook", throwHook);
        allClips.Add("HookSomething", hookSomething);
        allClips.Add("Traveling", traveling);
        allClips.Add("FirePirate", fireGun);

        allClips.Add("ThrowBlade", throwBlade);
        allClips.Add("ThrowHit", throwHit);
        allClips.Add("TeleportAbility", teleportAbility);

        allClips.Add("ChargingBerserk", chargingAttack);
        allClips.Add("JumpingBerserk", jumpingAttack);
        allClips.Add("AttackingAbilityBerserk", attackingAbility);
        allClips.Add("RunAbilityBerserk", runAbility);
        allClips.Add("HitRunningBerserk", hitWithRun);

        allClips.Add("ShootingMissilesElf", shootingMissileElf);
        allClips.Add("IncomingMissileElf", incomingMissiles);
        allClips.Add("ExplosionMissileElf", explosionMissiles);
        allClips.Add("TeleportMissileElf", teleportMissile);
        allClips.Add("TeleportingElf", teleportingElf);
        allClips.Add("MarkElf", markEffect);

        allClips.Add("ExplosionYeti", explosionYeti);
        allClips.Add("HitWithIce", hitWithIce);
        allClips.Add("CreationOfPlatform", creationOfPlatform);
    }

    public void CreateSound(string audioKey, float timeToDestroy = 3f)
    {
        if (allClips.ContainsKey(audioKey))
        {
            var audioClip = allClips[audioKey];
            var clip = GameObject.Instantiate(audioClipInWorldPrefab);
            clip.name = audioKey + " Sound";
            clip.GetComponent<AudioSource>().clip = audioClip;
            clip.GetComponent<DestroyableObject>().timeToDestroy = timeToDestroy;
        }
    }


}
