using UnityEngine;

namespace VRMM{

    public class AttachToHand : MonoBehaviour
    {
        public GameObject handAttachPoint;
        private bool isAttached;

        private void Update()
        {
            if (!isAttached && handAttachPoint != null)
            {
                Attach();
            }
        }

        private void Attach()
        {
            transform.position = Vector3.zero;
            transform.SetParent(handAttachPoint.transform, false);
            isAttached = true;
        }
    }
}
