using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalSoundManager : MonoBehaviour {


    public void LandSoundPlay() { AudioManager.Instance.CreateSound("Land"); }
    public void GetHitSoundPlay() {  }
    public void JumpSoundPlay() { AudioManager.Instance.CreateSound("Jump"); }
    public void HitSoundPlay() { }
    public void DeathSoundPlay() { AudioManager.Instance.CreateSound("Death"); }
    public void ImpactSoundPlay() {  }
    public void RunSoundPlay() { AudioManager.Instance.CreateSound("Run"); }
    public void SkillSoundPlay() { }
    public void DashSoundPlay() {  }
    public void StunSoundPlay() { AudioManager.Instance.CreateSound("Stunned"); }
}
