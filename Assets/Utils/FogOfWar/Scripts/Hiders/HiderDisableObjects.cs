using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FOW
{
    public class HiderDisableObjects : HiderBehavior
    {
        public GameObject[] ObjectsToHide;
        public GameObject[] ObjectsToHideInLayer;
        public Image[] ImagesToHide;
        public CanvasGroup[] CanvasGroup;
        public GameObject eyeVisibilityIcon;

        // New variables for handling layers using LayerMask
        public LayerMask defaultLayer;
        public LayerMask hiddenLayer;

        protected override void OnHide()
        {
            foreach (GameObject o in ObjectsToHide)
            {
                if (o != null) o.SetActive(false);
            }

            foreach (Image o in ImagesToHide)
            {
                if (o != null) o.enabled = false;
            }

            foreach (CanvasGroup o in CanvasGroup)
            {
                if (o != null) o.alpha = 0;
            }

            foreach (var o in ObjectsToHideInLayer)
            {
                if (o != null)
                {
                    SetLayerRecursively(o, LayerMaskToLayer(hiddenLayer));
                }
            }

            if (eyeVisibilityIcon != null)
            {
                eyeVisibilityIcon.SetActive(true);
            }
        }

        protected override void OnReveal()
        {
            foreach (GameObject o in ObjectsToHide)
            {
                if (o != null) o.SetActive(true);
            }

            foreach (Image o in ImagesToHide)
            {
                if (o != null) o.enabled = true;
            }

            foreach (CanvasGroup o in CanvasGroup)
            {
                if (o != null) o.alpha = 1;
            }

            foreach (var o in ObjectsToHideInLayer)
            {
                if (o != null)
                {
                    SetLayerRecursively(o, LayerMaskToLayer(defaultLayer));
                }
            }

            if (eyeVisibilityIcon != null)
            {
                eyeVisibilityIcon.SetActive(false);
            }
        }

        public void ModifyHiddenObjects(GameObject[] newObjectsToHide)
        {
            OnReveal();
            ObjectsToHide = newObjectsToHide;
            if (!enabled)
                return;

            if (!IsEnabled)
                OnHide();
            else
                OnReveal();
        }

        // Helper method to convert LayerMask to Layer index
        private int LayerMaskToLayer(LayerMask layerMask)
        {
            int layer = 0;
            int layerValue = layerMask.value;
            while (layerValue > 1)
            {
                layerValue = layerValue >> 1;
                layer++;
            }
            return layer;
        }

        // Helper method to set layer recursively
        private void SetLayerRecursively(GameObject obj, int newLayer)
        {
            if (obj == null)
                return;

            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                if (child == null)
                    continue;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
