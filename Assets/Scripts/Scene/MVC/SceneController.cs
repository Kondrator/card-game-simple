﻿using Kondrat.MVC;
using System.Collections;
using System.Collections.Generic;
using UIWindowManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Controller<SceneModel, SceneView>, IListenerLoaderSceneCompleted {


    public const string NOTIFY_LOADED = "scene.loaded";
    public const string NOTIFY_DESTROYED = "scene.destroyed";
    public const string NOTIFY_RELOAD = "scene.reload";





#if UNITY_EDITOR
    void Update() {
        if( Input.GetKeyDown( KeyCode.F1 ) ) {
            SettingsManager.Language = SystemLanguage.English;
        }
        if( Input.GetKeyDown( KeyCode.F2 ) ) {
            SettingsManager.Language = SystemLanguage.Russian;
        }
    }
#endif




    protected void OnDestroy() {
        Notify( SceneController.NOTIFY_DESTROYED );
    }



    protected override void PreInitiate() {
        SettingsManager.CheckLanguage();
    }

    protected override void Initiate() {

        Add(
            SceneController.NOTIFY_RELOAD,
            ( NotifyData data ) => {
                UILoaderBetweenScenes.LoadScene( SceneManager.GetActiveScene().name );
            }
        );

    }



    public void OnLoadSceneCompleted() {
        Notify( SceneController.NOTIFY_LOADED );
    }

}
