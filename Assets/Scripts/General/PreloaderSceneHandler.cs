﻿using Features.SceneManagement.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General
{
    public class PreloaderSceneHandler : MonoBehaviour
    {
        private const float WaitSeconds = 0.5f;

        private void Start()
        {
            StartCoroutine(LoadAsyncScene());
        }

        private IEnumerator LoadAsyncScene()
        {
            yield return new WaitForSeconds(WaitSeconds);

            SceneManager.LoadScene(SceneType.Startup.ToString());
        }
    }
}