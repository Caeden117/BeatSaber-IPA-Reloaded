﻿using UnityEngine.SceneManagement;
// ReSharper disable CheckNamespace

namespace IPA
{
    /// <summary>
    /// Interface for Beat Saber plugins. Every class that implements this will be loaded if the DLL is placed at
    /// data/Managed/Plugins.
    /// </summary>
    public interface IBeatSaberPlugin
    {
        /// <summary>
        /// Gets invoked when the application is started.
        /// 
        /// THIS EVENT WILL NOT BE GUARANTEED TO FIRE. USE Init OR <see cref="IDisablablePlugin.OnEnable"/> INSTEAD.
        /// </summary>
        void OnApplicationStart();

        /// <summary>
        /// Gets invoked when the application is closed.
        /// </summary>
        void OnApplicationQuit();

        /// <summary>
        /// Gets invoked on every graphic update.
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// Gets invoked on ever physics update.
        /// </summary>
        void OnFixedUpdate();
        
        /// <summary>
        /// Gets invoked whenever a scene is loaded.
        /// </summary>
        /// <param name="scene">The scene currently loaded</param>
        /// <param name="sceneMode">The type of loading</param>
        void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode);

        /// <summary>
        /// Gets invoked whenever a scene is unloaded
        /// </summary>
        /// <param name="scene">The unloaded scene</param>
        void OnSceneUnloaded(Scene scene);

        /// <summary>
        /// Gets invoked whenever a scene is changed
        /// </summary>
        /// <param name="prevScene">The Scene that was previously loaded</param>
        /// <param name="nextScene">The Scene being loaded</param>
        void OnActiveSceneChanged(Scene prevScene, Scene nextScene);
    }
}
