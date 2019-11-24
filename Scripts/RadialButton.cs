using UnityEngine;
using UnityEngine.Events;

namespace VRMM{

   [System.Serializable]
   public class RadialButton : MonoBehaviour
   {
      [HideInInspector]public UnityEvent onButtonPress;
      
      public void SetOnButtonPress(UnityEvent value) => onButtonPress = value;
}
}

