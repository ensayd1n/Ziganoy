using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestController : MonoBehaviour
{
   public GameObject[] ToolOptions;

   #region Interaction
   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.CompareTag("PlayerTakenDamageArea"))
      {
         int randomIndex = Random.Range(0, ToolOptions.Length);
         Instantiate(ToolOptions[randomIndex],
            new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1,
               gameObject.transform.position.z), Quaternion.identity);
         gameObject.transform.DOMoveY(transform.position.y - 2, 2);
      }
   }
   #endregion
}
