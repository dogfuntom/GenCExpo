using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GenC
{
    public class DebugWelcomer : MonoBehaviour
    {
        public TMP_Text text;

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }

        void Start()
        {
            text.text += $@"{
                Environment.NewLine
                }Application.isMobilePlatform = {Application.isMobilePlatform}{
                Environment.NewLine
                }Input.touchSupported = {Input.touchSupported}{
                Environment.NewLine
                }SystemInfo.graphicsDeviceType = {SystemInfo.graphicsDeviceType}{
                Environment.NewLine
                }Graphics.activeTier = {Graphics.activeTier}";
        }
    }
}
