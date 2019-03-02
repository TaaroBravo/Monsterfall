using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance { get; private set; }

    Dictionary<string, AudioClip> allClips = new Dictionary<string, AudioClip>();
    public GameObject audioClipInWorldPrefab;
    public AudioMixerGroup mixer;

    [Header("GENERAL")]
    [Space]
    public AudioClip runL;
    public AudioClip runR;
    public AudioClip jumpA;
    public AudioClip jumpB;
    public AudioClip jumpC;
    public AudioClip jumpD;
    public AudioClip land;
    public AudioClip punch;
    public AudioClip hitPlayer;
    public AudioClip death;
    public AudioClip stunned;
    public AudioClip charginAttack;
    public AudioClip hitRay;
    public AudioClip kill;

    public AudioClip teleport;

    [Header("HUD")]
    [Space]
    public AudioClip navigationOnHud;
    public AudioClip onGame;
    public AudioClip pauseClip;
    public AudioClip readyClip;
    public AudioClip fightClip;
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
    public AudioClip chargingAttackBerserk;
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
    public AudioClip iceDestruction;

    private void Awake()
    {
        Instance = this;
        CreateDictionary();
    }

    void CreateDictionary()
    {
        //Algunos deberian tener referencia para borrarlos en momentos especificos

        allClips.Add("RunL", runL); //
        allClips.Add("RunR", runR); //
        allClips.Add("JumpA", jumpA); //
        allClips.Add("JumpB", jumpB); //
        allClips.Add("JumpC", jumpC); //
        allClips.Add("JumpD", jumpD); //
        allClips.Add("Land", land); //
        allClips.Add("Punch", punch); //
        allClips.Add("ChargingAttack", charginAttack); //
        allClips.Add("HitPlayer", hitPlayer); //
        allClips.Add("Death", death); //
        allClips.Add("Teleport", teleport); //
        allClips.Add("Stunned", stunned); //
        allClips.Add("HitRay", hitRay); //
        allClips.Add("Kill", kill); //

        allClips.Add("NavigationHUD", navigationOnHud);
        allClips.Add("OnGame", onGame);
        allClips.Add("Pause", pauseClip); //
        allClips.Add("Ready", readyClip); //
        allClips.Add("Fight", fightClip); //

        allClips.Add("ExplosionKnight", explosionKnight); //
        allClips.Add("FireJump", fireJump); //
        allClips.Add("Fire", fire); //

        allClips.Add("ThrowHook", throwHook); //
        allClips.Add("HookSomething", hookSomething); //
        allClips.Add("Traveling", traveling); // Cuidado con que se este instanciando siempre
        allClips.Add("FirePirate", fireGun); //

        allClips.Add("ThrowBlade", throwBlade); //
        allClips.Add("ThrowHit", throwHit); //
        allClips.Add("TeleportAbility", teleportAbility); //

        allClips.Add("ChargingBerserk", chargingAttackBerserk); //
        allClips.Add("JumpingBerserk", jumpingAttack); //
        allClips.Add("AttackingAbilityBerserk", attackingAbility); //
        allClips.Add("RunAbilityBerserk", runAbility); //
        allClips.Add("HitRunningBerserk", hitWithRun); //

        allClips.Add("ShootingMissilesElf", shootingMissileElf); //
        allClips.Add("IncomingMissileElf", incomingMissiles); //
        allClips.Add("ExplosionMissileElf", explosionMissiles); //
        allClips.Add("TeleportMissileElf", teleportMissile); // 
        allClips.Add("TeleportingElf", teleportingElf); // 
        allClips.Add("MarkElf", markEffect); //

        allClips.Add("ExplosionYeti", explosionYeti); //
        allClips.Add("HitWithIce", hitWithIce); //
        allClips.Add("IceCreation", iceIdle); //
        allClips.Add("CreationOfPlatform", creationOfPlatform); //
        allClips.Add("IceDestruction", iceDestruction); //
    }

    public GameObject CreateSound(string audioKey, float timeToDestroy = 3f, bool fadeIn = false)
    {
        if (allClips.ContainsKey(audioKey))
        {
            var audioClip = allClips[audioKey];
            var clip = GameObject.Instantiate(audioClipInWorldPrefab);
            if (audioKey.Equals("RunL") || audioKey.Equals("RunR") || audioKey.Equals("Land"))
                clip.GetComponent<AudioSource>().volume = 0.2f;
            else if(audioKey.Equals("JumpA") || audioKey.Equals("JumpB") || audioKey.Equals("JumpC") || audioKey.Equals("JumpD"))
                clip.GetComponent<AudioSource>().volume = 0.5f;
            clip.name = audioKey + " Sound";
            clip.GetComponent<AudioSource>().clip = audioClip;
            clip.GetComponent<AudioSource>().outputAudioMixerGroup = mixer;
            clip.GetComponent<DestroyableObject>().timeToDestroy = timeToDestroy;
            clip.GetComponent<AudioSource>().Play();
            if (fadeIn)
            {
                float maxVolume = clip.GetComponent<AudioSource>().volume;
                clip.GetComponent<AudioSource>().volume = 0;
                FadeIn(clip, 0.1f, maxVolume);
            }
            return clip;
        }
        else
            return null;
    }

    public void FadeIn(GameObject myObj, float speed, float maxVolume)
    {
        StartCoroutine(FadeInCoroutine(myObj, speed, maxVolume));
    }

    public void FadeOut(GameObject myObj, float speed)
    {
        if (!myObj)
            return;
        StartCoroutine(FadeOutCoroutine(myObj, speed));
    }

    IEnumerator FadeInCoroutine(GameObject myObj, float speed, float maxVolume)
    {
        var audioSource = myObj.GetComponent<AudioSource>();

        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += speed;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeOutCoroutine(GameObject myObj, float speed)
    {
        if (myObj)
        {
            var audioSource = myObj.GetComponent<AudioSource>();

            while (audioSource != null && audioSource.volume > 0)
            {
                audioSource.volume -= speed;
                yield return new WaitForSeconds(0.1f);
            }
            if (myObj)
                Destroy(myObj);
        }
    }


}
