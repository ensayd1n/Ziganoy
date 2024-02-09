using UnityEngine;

[CreateAssetMenu(fileName = "New Zombie", menuName = "Zombie")]
public class Zombie : ScriptableObject
{
    public new string Name;
    
    public float Damage;
    public float Health;
    public float MoveSpeed;
    

}
