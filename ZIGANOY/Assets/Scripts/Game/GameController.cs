using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
   public GameObject[] Objects;
   public GameObject[] Skils;
   [HideInInspector] public GameObject[] InstantiatedZombies=new GameObject[1];
   [HideInInspector] public GameObject[] InstantiatedDrop = new GameObject[1];
   [HideInInspector] public GameObject[] InstantiatedXP=new GameObject[1];
   [HideInInspector] public GameObject[] InstantiatedMushroom=new GameObject[1];
   public GameObject[] InterfaceObjects;
   public GameObject[] SpawnTransforms;
   [HideInInspector] public GameObject Player;
   [HideInInspector] public bool ZombieAttackLock = false;
   [HideInInspector] public bool ZombieMovementLock = false;
   [HideInInspector] public float TimeValue = 0;

   [Header("Instantiate Amount Settings")]
   public int FirstlyZombieInstantiateAmount = 0;
   public int FirstlyDropInstantiateAmount = 0;
   public int FirstlyXPInstantiateAmount = 0; 
   public int FirstlyMushroomInstantiateAmount=0;
   public int FirstlySkillInstantiateAmount = 0;

   [Header("Instantiate Duration Settings")]
   public int InstantiateZombieDuration = 4;
   public int InstantiateDropDuration = 59;
   public int InstantiateXPDuration = 2;
   public int InstantiateSkilDuration = 39;
   public int InstantiateMushroomDuration=9;
   
   [Header("Other Component")]
   public Text TimerText;
    public Slider XPLevelSlider;
    public Text XPLevelText;
    public Text ScorText;
    
   private float magicSkillReloadTimer;
   private float enemySlowDownSkillReloadTimer=20;
   [HideInInspector] public float CurrentXP;
   [HideInInspector] public int XPLevel;
   private bool MagicSkiilReloadTimerLock = true;
   private bool EnemySlowDownSkillReloadTimerLock = false;
   
   private void Awake()
   {
       SetXpLevel();
       SpawnTransforms = GameObject.FindGameObjectsWithTag("ZombieInstantiateTransform");
    }
    private void Start()
       {
           InstantiatingPlayer();
           for (int i = 0; i < FirstlyDropInstantiateAmount; i++)
           {
               InstantiatingDrop();
           }
           for (int i = 0; i < FirstlyZombieInstantiateAmount; i++)
           {
               InstantiatingZombie();
           }
           for (int i = 0; i < FirstlyXPInstantiateAmount; i++)
           {
               InstantiatingXP();
           }
           for (int i = 0; i < FirstlySkillInstantiateAmount; i++)
           {
               InstantiatingSkills();
           }
           for (int i = 0; i < FirstlyMushroomInstantiateAmount; i++)
           {
               InstantiatiateMushroom();
           }
           
           StartCoroutine(InstantiateZombieByTimer(InstantiateZombieDuration));
           StartCoroutine(InstantiateDropByTimer(InstantiateDropDuration));
           StartCoroutine(InstantiateXPByTimer(InstantiateXPDuration));
           StartCoroutine(InstantiateSkilByTimer(InstantiateSkilDuration));
           StartCoroutine(InstantiateMushroomByTimer(InstantiateMushroomDuration));

           StartCoroutine(ZombieWawe(60));
       }

   
   private void Update()
   {
      Player = Objects[0];
      DisplayTime();
      MagicSkillReloadTimer();
      EnemySkillReloadTimer();
   }

   #region Interface
   
   public void MagicSkillButtonClick()
   {
      InterfaceObjects[1].GetComponent<Button>().interactable = false;
      InterfaceObjects[2].SetActive(true);
      MagicSkiilReloadTimerLock = false;
      magicSkillReloadTimer = 20;
      GameObject.FindGameObjectWithTag("MagicSkillArea").GetComponent<PlayerSkillsController>().MagicSkill();
   }

   public void EnemySlowDownSkillButtonClick()
   {
       InterfaceObjects[3].GetComponent<Button>().interactable = false;
       InterfaceObjects[4].SetActive(true);
       EnemySlowDownSkillReloadTimerLock = false;
       enemySlowDownSkillReloadTimer = 20;
       GameObject.FindGameObjectWithTag("MagicSkillArea").GetComponent<PlayerSkillsController>().EnemySlowDown(InstantiatedZombies);
   }

   private void MagicSkillReloadTimer()
   {
      if (MagicSkiilReloadTimerLock != true)
      {
         magicSkillReloadTimer -= Time.deltaTime;
         InterfaceObjects[2].GetComponent<Text>().text = Convert.ToString(Math.Round(magicSkillReloadTimer));
         if (magicSkillReloadTimer <= 0)
         {
            InterfaceObjects[1].GetComponent<Button>().interactable = true;
            InterfaceObjects[2].SetActive(false);
            MagicSkiilReloadTimerLock = true;
         }
      }
   }

   private void EnemySkillReloadTimer()
   {
       if (EnemySlowDownSkillReloadTimerLock != true)
       {
           enemySlowDownSkillReloadTimer -= Time.deltaTime;
           InterfaceObjects[4].GetComponent<Text>().text = Convert.ToString(Math.Round(enemySlowDownSkillReloadTimer));
           if (enemySlowDownSkillReloadTimer <= 0)
           {
               InterfaceObjects[3].GetComponent<Button>().interactable = true;
               InterfaceObjects[4].SetActive(false);
               EnemySlowDownSkillReloadTimerLock = true;
           }
       }
   }

   public void ResetMagicSkillReloadTimer()
   {
      magicSkillReloadTimer = 0;
      InterfaceObjects[2].GetComponent<Text>().text = Convert.ToString(magicSkillReloadTimer);
   }

   public void ScorIncrease()
   {
      int scor = Convert.ToInt16(ScorText.text);
      scor++;
      ScorText.text = Convert.ToString(scor);
   }
   private void DisplayTime()
   {
      TimeValue += Time.deltaTime;
      float minutes=Mathf.FloorToInt(TimeValue / 60);
      float seconds = Mathf.FloorToInt(TimeValue % 60);
      TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
   }
   public void LoadScene(int index)
   {
      SceneManager.LoadScene(index);
   }

   public void ChangeSceneTime(int index)
   {
       Time.timeScale = index;
   }
   #endregion
   #region XP

   
   
   public void SetXpLevel()
   {
      CurrentXP = 0;
      XPLevel = 1;
      XPLevelSlider.maxValue = 300;
      XPLevelSlider.value = CurrentXP;
      XPLevelText.text = "1";
   }

   public void IncreaseXP(int increase)
   {
      CurrentXP += increase;
      float remainderXP = XPLevelSlider.maxValue - CurrentXP;
      if (remainderXP < 0)
      {
         CurrentXP = remainderXP*-1;
         XPLevel++;
         XPLevelText.text = Convert.ToString(XPLevel);
         XPLevelSlider.value = CurrentXP;
         XPLevelSlider.maxValue = 300+(XPLevel * 600);
         remainderXP = XPLevelSlider.maxValue - CurrentXP;
         if (remainderXP < 0)
         {
            IncreaseXP(0);
         }
      }
      else if (remainderXP >= 0)
      {
         XPLevelSlider.value = CurrentXP;
      }
   }
   #endregion
   #region Events
   
 
    private void InstantiatingPlayer()
    {
        int randomIndex = Random.Range(0,SpawnTransforms.Length);
        Instantiate(Objects[0], new Vector3(
            SpawnTransforms[randomIndex].transform.position.x, 0.6F,
            SpawnTransforms[randomIndex].transform.position.z), Quaternion.identity);
    }
    public void InstantiatingZombie()
    {
        for (int i = 0; i <InstantiatedZombies.Length; i++)
        {
            GameObject Zombie =InstantiatedZombies[i];
            if (InstantiatedZombies[i]!=null &&Zombie.activeSelf==false)
            {
                Zombie.SetActive(true);
                Zombie.GetComponent<ZombieOtherController>().FillHealth();
                int randomIndex = Random.Range(0, SpawnTransforms.Length);
                Zombie.transform.position = new Vector3(SpawnTransforms[randomIndex].transform.position.x, 1.2F,
                    SpawnTransforms[randomIndex].transform.position.z);
                break;
            }
            if(i==InstantiatedZombies.Length-1)
            {
                int randomIndex = Random.Range(0, SpawnTransforms.Length);
                Zombie =  Instantiate(Objects[1], new Vector3(SpawnTransforms[randomIndex].transform.position.x, 1.2F,
                        SpawnTransforms[randomIndex].transform.position.z),
                    Quaternion.identity);
                InstantiatedZombies = InstantiatedZombies.Concat(new GameObject[]{Zombie}).ToArray();
                break;
            }
        }
    }
    private void InstantiatingDrop()
    {
        for (int i = 0; i < InstantiatedDrop.Length; i++)
        {
            GameObject Drop = InstantiatedDrop[i];
            if (InstantiatedDrop[i]!=null &&Drop.activeSelf==false)
            {
                Drop.SetActive(true);
                int randomIndex = Random.Range(0, SpawnTransforms.Length);
                Drop.transform.position = new Vector3(SpawnTransforms[randomIndex].transform.position.x, 4F,
                    SpawnTransforms[randomIndex].transform.position.z);
                Drop.transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            }
            if(i==InstantiatedDrop.Length-1)
            {
                int randomIndex = Random.Range(0, SpawnTransforms.Length);
                Drop =  Instantiate(Objects[3], new Vector3(SpawnTransforms[randomIndex].transform.position.x, 4F,
                        SpawnTransforms[randomIndex].transform.position.z),
                    Quaternion.identity);
                InstantiatedDrop = InstantiatedDrop.Concat(new GameObject[]{Drop}).ToArray();
                Drop.transform.rotation = Quaternion.Euler(-90, 0, 0);
                break;
            }
        }
    }
    private void InstantiatingXP()
    {
        for (int i = 0; i < InstantiatedXP.Length; i++)
        {
            GameObject XP = InstantiatedXP[i];
            if (InstantiatedXP[i]!=null &&XP.activeSelf==false)
            {
                XP.SetActive(true);
                int randomIndex = Random.Range(0, SpawnTransforms.Length);
                XP.transform.position = new Vector3(SpawnTransforms[randomIndex].transform.position.x+2, 1F,
                    SpawnTransforms[randomIndex].transform.position.z+2);
                break;
            }
            if(i==InstantiatedXP.Length-1)
            {
                int randomIndex = Random.Range(0, SpawnTransforms.Length);
                XP =  Instantiate(Objects[2], new Vector3(SpawnTransforms[randomIndex].transform.position.x+2, 1F,
                        SpawnTransforms[randomIndex].transform.position.z),
                    Quaternion.identity);
                InstantiatedXP = InstantiatedXP.Concat(new GameObject[]{XP}).ToArray();
                break;
            }
        }
    }

    private void InstantiatingSkills()
    {
        int randomIndex = Random.Range(0, Skils.Length);
        int randomIndex2 = Random.Range(0, SpawnTransforms.Length);
        int randomIndexForX = Random.Range(-4, 5);
        int randomIndexForZ = Random.Range(-4, 5);
        Instantiate(Skils[randomIndex], new Vector3(SpawnTransforms[randomIndex2].transform.position.x+randomIndexForX, 1.75F,
                SpawnTransforms[randomIndex2].transform.position.z+randomIndexForZ),
            Quaternion.identity);
    }

    private void InstantiatiateMushroom()
    {
        for (int i = 0; i < InstantiatedMushroom.Length; i++)
        {
            GameObject XP = InstantiatedMushroom[i];
            if (InstantiatedMushroom[i]!=null &&XP.activeSelf==false)
            {
                XP.SetActive(true);
                int randomIndex= Random.Range(0, SpawnTransforms.Length);
                int randomIndexForX = Random.Range(-4, 5);
                int randomIndexForZ = Random.Range(-4, 5);
                XP.transform.position = new Vector3(SpawnTransforms[randomIndex].transform.position.x+randomIndexForX, 0.15F,
                    SpawnTransforms[randomIndex].transform.position.z+randomIndexForZ);
                break;
            }
            if(i==InstantiatedMushroom.Length-1)
            {
                int randomIndex= Random.Range(0, SpawnTransforms.Length);
                int randomIndexForX = Random.Range(-4, 5);
                int randomIndexForZ = Random.Range(-4, 5);
                XP =  Instantiate(Objects[5], new Vector3(SpawnTransforms[randomIndex].transform.position.x+randomIndexForX, 0.15F,
                        SpawnTransforms[randomIndex].transform.position.z+randomIndexForZ),
                    Quaternion.identity);
                InstantiatedMushroom = InstantiatedMushroom.Concat(new GameObject[]{XP}).ToArray();
                break;
            }
        }
    }
    
    private IEnumerator InstantiateZombieByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        InstantiatingZombie();
        StartCoroutine(InstantiateZombieByTimer(time));
    }
    private IEnumerator InstantiateDropByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        InstantiatingDrop();
        StartCoroutine(InstantiateDropByTimer(time));
    }

    private IEnumerator InstantiateXPByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        InstantiatingXP();
        StartCoroutine(InstantiateXPByTimer(time));
    }

    private IEnumerator InstantiateSkilByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        InstantiatingSkills();
    }
    
    private IEnumerator InstantiateMushroomByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        InstantiatiateMushroom();
    }

    private IEnumerator ZombieWawe(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        for (int i = 0; i < 20 * XPLevel; i++)
        {
            InstantiatingZombie();
        }
        StartCoroutine(ZombieWawe(time));
    }
   #endregion
}