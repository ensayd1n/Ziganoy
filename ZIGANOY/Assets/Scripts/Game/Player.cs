using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player")]
public class Player : ScriptableObject
{
    public new string Name;
    
    public float Damage;
    public float DamageDuration;
    public float Health;
    public float MoveSpeed;

}
