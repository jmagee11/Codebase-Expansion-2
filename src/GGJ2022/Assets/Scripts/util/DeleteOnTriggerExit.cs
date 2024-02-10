using System;
using System.ComponentModel;
using UnityEngine;

namespace util
{
   // Delete this game object, when it exists an trigger.
   public class DeleteOnTriggerExit : MonoBehaviour
   {
      [Description("May be null or a specific trigger.")]
      public new Collider2D collider;
      
      public void OnTriggerExit2D(Collider2D other)
      {
         if (null == collider || collider == other)
            Destroy(gameObject);
      }
   }
}
