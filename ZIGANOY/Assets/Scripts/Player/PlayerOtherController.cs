using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOtherController : MonoBehaviour
{
    private PlayerAnimationController _playerAnimationController;
    private GameController _gameController;
    
    public Player PlayerType;
    
    [HideInInspector] public float CurrentyHealty;
    [HideInInspector] public float XPLevel;
    
    private Slider _healthSlider;
    public Gradient Gradient;
    private Image _fill;
    public ParticleSystem DebufParticleSystem;

    private void Start()
    {
        _playerAnimationController = GetComponent<PlayerAnimationController>();
        _gameController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameController>();
        
        _healthSlider = GameObject.FindGameObjectWithTag("HealthSlider").GetComponent<Slider>();
        _fill = GameObject.FindGameObjectWithTag("HealthFill").GetComponent<Image>();
        FillHealth();
    }

    #region Health

    private void FillHealth()
    {
        CurrentyHealty = PlayerType.Health;
        _healthSlider.maxValue = PlayerType.Health;
        _healthSlider.value = PlayerType.Health;
        _fill.color = Gradient.Evaluate(_healthSlider.normalizedValue);
    }
    public void TakenDamage(float damage)
    {
        CurrentyHealty -= damage;
        _healthSlider.value = CurrentyHealty;
        _fill.color = Gradient.Evaluate(_healthSlider.normalizedValue);
        CheckCurrentHealth();
    }
    public void IncreaseHealth(float increaseIndex)
    {
        if (CurrentyHealty + increaseIndex <= PlayerType.Health)
        {
            CurrentyHealty += increaseIndex;
            _healthSlider.value = CurrentyHealty;
            _fill.color = Gradient.Evaluate(_healthSlider.normalizedValue);
        }
        else
        {
            FillHealth();
        }
    }
    public void CheckCurrentHealth()
    {
        if (CurrentyHealty <= 0)
        {
            _playerAnimationController.SetWallkingAnimation(false);
            _playerAnimationController.SetDamageAnimation(false);
            _playerAnimationController.SetHeallingAnimation(false);
            _playerAnimationController.SetVictoryAnimation(false);
            _playerAnimationController.SetMagicAttackAnimation(false);
            _playerAnimationController.SetDieAnimation(true);
            _gameController.ZombieAttackLock = true;
            _gameController.ZombieMovementLock = true;
            
        }
    }
    #endregion
    #region HealthEvent
    private IEnumerator DieScreenByTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        
    }
    #endregion
}
