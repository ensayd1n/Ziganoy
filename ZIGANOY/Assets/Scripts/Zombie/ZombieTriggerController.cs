using System.Collections;
using UnityEngine;

public class ZombieTriggerController : MonoBehaviour
{
    public GameObject Zombie;
    
    #region Interaction
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("MagicSkillArea") && GameObject.FindGameObjectWithTag("MagicSkillArea").GetComponent<PlayerSkillsController>().GivenMagicDamageLock != true)
        {
            Zombie.GetComponent<ZombieAnimationController>().SetWallkingAnimation(false);
            other.gameObject.GetComponent<PlayerSkillsController>().UsedMagicSkill(Zombie);
        }

        if (other.gameObject.CompareTag("Potion"))
        {
            StartCoroutine(SetActiveParticleSytemDuration(0.1F));
        }
    }
    #endregion
    
    #region Events
    private IEnumerator SetActiveParticleSytemDuration(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Zombie.GetComponent<ZombieOtherController>().TakenDamageParticleSystem.Stop();
    }
    #endregion
}
