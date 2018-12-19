using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalSoundManager : MonoBehaviour {

    public AudioSource gethitsound;
    public AudioSource landsound;
    public AudioSource jumpsound;
    public AudioSource hitsound;
    public AudioSource deathsound;
    public AudioSource impactsound;
    public AudioSource runsound;
    public AudioSource skillsound;
    public AudioSource dashsound;
    public AudioSource stunsound;

    public void LandSoundPlay() { landsound.Play(); }
    public void GetHitSoundPlay() { /*gethitsound.Play();*/ }
    public void JumpSoundPlay() { jumpsound.Play(); }
    public void HitSoundPlay() { /*hitsound.Play();*/ }
    public void DeathSoundPlay() { /*deathsound.Play();*/ }
    public void ImpactSoundPlay() { /*impactsound.Play();*/ }
    public void RunSoundPlay() { /*runsound.Play();*/ }
    public void SkillSoundPlay() { /*skillsound.Play();*/ }
    public void DashSoundPlay() { /*dashsound.Play();*/ }
    public void StunSoundPlay() { /*stunsound.Play();*/ }
}
