using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DropController : MonoBehaviour
{
    private GameController _gameController;
    private Button _magicButton;
    public GameObject Drop;
    public GameObject TimePanel;
    public Slider TimeSLider;
    [HideInInspector] public float CurrentTime;

    private void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        _magicButton = GameObject.FindGameObjectWithTag("MagicButton").GetComponent<Button>();
        
        CurrentTime = 0;
        TimePanel.SetActive(false);
        TimeSLider.maxValue = 5;
        TimeSLider.value = CurrentTime;
    }
    
    #region Interaction
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTakenDamageArea"))
        {
            StopCoroutine(DropPanelCloserTimer(0));
            TimePanel.SetActive(true);
            CurrentTime += Time.deltaTime;
            TimeSLider.value = CurrentTime;
            if (TimeSLider.value >= TimeSLider.maxValue)
            {
                if (_magicButton.interactable == true)
                {
                    int randomItemIndex = Random.Range(1, _gameController.Skils.Length);
                    Instantiate(_gameController.Skils[randomItemIndex],
                        new Vector3(transform.position.x, 1.75F, transform.position.z), Quaternion.identity);
                }
                else if (_magicButton.interactable == false)
                {
                    Instantiate(_gameController.Skils[0],
                        new Vector3(transform.position.x, 1.75F, transform.position.z), Quaternion.identity);
                }
                Drop.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("ZombieAttackTriggerArea"))
        {
            GameObject obj = other.gameObject.GetComponent<ZombieTriggerController>().Zombie;
            obj.GetComponent<ZombieOtherController>()
                .TakenDamage(obj.GetComponent<ZombieOtherController>().CurrentHealth);
            obj.GetComponent<ZombieOtherController>().FillHealth();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerTakenDamageArea"))
        {
            StartCoroutine(DropPanelCloserTimer(3));
        }
    }
    
    #endregion
    
    #region Events
    private IEnumerator DropPanelCloserTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        TimePanel.SetActive(false);
        CurrentTime = 0;
    }
    #endregion
}
