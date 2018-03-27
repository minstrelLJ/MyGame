using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common
{
    public delegate void LodingValueChangeEvent(int v);

    public class AsyncLoadScene : UnitySingleton<AsyncLoadScene>
    {
        public static event LodingValueChangeEvent ValueChangeEvent;
        private AsyncOperation operation;

        public void ChangeLevel(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void ChangeLevelAsync(int index, Action action)
        {
            StartCoroutine(StartLoadLevel(index, action));
        }

        private IEnumerator StartLoadLevel(int index, Action action)
        {
            operation = SceneManager.LoadSceneAsync(index);
            operation.allowSceneActivation = false;
            int currentProgress = 0;
            int targetProgress = 0;

            //unity 加载90%  
            while (operation.progress < 0.9f)
            {
                targetProgress = (int)operation.progress * 100;
                //平滑过渡  
                while (currentProgress < targetProgress)
                {
                    ++currentProgress;
                    yield return new WaitForEndOfFrame();
                }
            }

            //自行加载剩余的10%  
            targetProgress = 100;
            while (currentProgress < targetProgress)
            {
                ++currentProgress;
                if (ValueChangeEvent != null)
                {
                    ValueChangeEvent(currentProgress);
                }
                yield return new WaitForEndOfFrame();
            }

            if (action != null)
            {
                action();
            }
            operation.allowSceneActivation = true;
        }
    }
}
