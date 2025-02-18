﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SceneManager : MonoSingleton<SceneManager> 
{
    UnityAction<float> onProgress = null; // 通知进度事件
    internal Action onSceneLoadDone;

    protected override void OnStart()
    {
        
    }

    void Update () {
		
 	}

    public void LoadScene(string name)
    {
        StartCoroutine(LoadLevel(name)); // 启用协程异步加载场景
    }

    IEnumerator LoadLevel(string name)
    {
        Debug.LogFormat("LoadLevel: {0}", name);
        AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = true;

        async.completed += LevelLoadCompleted;
        while (!async.isDone)
        {
            if (onProgress != null)
                onProgress(async.progress);
            yield return null;
        }
    }

    private void LevelLoadCompleted(AsyncOperation obj)
    {
        if (onProgress != null)
            onProgress(1f);
        if (onSceneLoadDone != null)
            onSceneLoadDone();
        Debug.Log("LevelLoadCompleted:" + obj.progress);
    }
}
