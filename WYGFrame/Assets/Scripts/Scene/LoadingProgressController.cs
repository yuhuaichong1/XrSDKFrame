using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XrCode
{
    public class LoadingProgressController
    {
        private float currentProgress = 0;
        private float targetProgress = 0;
        private float smoothSpeed = 50f;
        private Action<float> onProgressChanged;

        public LoadingProgressController(Action<float> progressCallback)
        {
            onProgressChanged = progressCallback;
        }

        public void SetTargetProgress(float target)
        {
            targetProgress = Mathf.Clamp(target, 0f, 100f);
        }

        public bool UpdateProgress()
        {
            if (Mathf.Approximately(currentProgress, targetProgress))
                return false;

            currentProgress = Mathf.MoveTowards(currentProgress, targetProgress, Time.deltaTime * smoothSpeed);
            onProgressChanged?.Invoke(currentProgress);
            return true;
        }
    }

}