using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using EFT;
using Comfort.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoliageDesolation
{
    // first string below is your plugin's GUID, it MUST be unique to any other mod. Read more about it in BepInEx docs. Be sure to update it if you copy this project.
    [BepInPlugin("BoredUltimatum.FoliageDesolation", "FoliageDesolation", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
        public static ConfigEntry<KeyboardShortcut> DecimateFoliageShortCutConfig;
        public static ConfigEntry<int> DesolationAmountConfig;
        public static ConfigEntry<KeyboardShortcut> ResetFoliageShortCutConfig;
        public static ConfigEntry<KeyboardShortcut> ToggleSlownessShortCutConfig;

        const string headerLabelMain = "Main";

        private int testId = 0;

        // BaseUnityPlugin inherits MonoBehaviour, so you can use base unity functions like Awake() and Update()
        private void Awake()
        {
            // save the Logger to variable so we can use it elsewhere in the project
            LogSource = Logger;
            LogSource.LogInfo("plugin loaded!");

            this.enabled = true;

            DecimateFoliageShortCutConfig = Config.Bind(headerLabelMain, "Decimate Foliage", new KeyboardShortcut(UnityEngine.KeyCode.F2, UnityEngine.KeyCode.LeftControl, UnityEngine.KeyCode.LeftAlt));
            DesolationAmountConfig = Config.Bind(headerLabelMain, "Decimation Percentage", 100,
                new ConfigDescription("Percent of foliage to decimate", new AcceptableValueRange<int>(0, 100)));
            ResetFoliageShortCutConfig = Config.Bind(headerLabelMain, "Reset Foliage", new KeyboardShortcut(UnityEngine.KeyCode.F2, UnityEngine.KeyCode.LeftControl, UnityEngine.KeyCode.LeftShift));
            ToggleSlownessShortCutConfig = Config.Bind(headerLabelMain, "Toggle Slowness", new KeyboardShortcut(UnityEngine.KeyCode.F2, UnityEngine.KeyCode.LeftAlt, UnityEngine.KeyCode.LeftShift));
        }

        private void DecimateFoliage()
        {
            Random rand = new Random();
            float configPercent = (float)(DesolationAmountConfig.Value) / 100f;
            EFT.Interactive.TreeInteractive[] treeInteractives = (EFT.Interactive.TreeInteractive[])UnityEngine.GameObject.FindObjectsOfType<EFT.Interactive.TreeInteractive>();
            byte[] randBytes;
            int randIndex;
            randBytes = new byte[treeInteractives.Length];
            randIndex = 0;
            if (DesolationAmountConfig.Value < 100)
            {
                rand.NextBytes(randBytes);
            }
            foreach (EFT.Interactive.TreeInteractive tree in treeInteractives)
            {
                if (DesolationAmountConfig.Value < 100)
                {
                    float percent = (float)(randBytes[randIndex++]) / (float)(byte.MaxValue);
                    if (percent > configPercent)
                    {
                        continue;
                    }
                }
                tree.gameObject.SetActive(false);
            }
            EFT.SpeedTree.TreeWind[] treeWinds = (EFT.SpeedTree.TreeWind[])UnityEngine.GameObject.FindObjectsOfType<EFT.SpeedTree.TreeWind>();
            randBytes = new byte[treeWinds.Length];
            randIndex = 0;
            if (DesolationAmountConfig.Value < 100)
            {
                rand.NextBytes(randBytes);
            }
            foreach (EFT.SpeedTree.TreeWind tree in treeWinds)
            {
                if (DesolationAmountConfig.Value < 100)
                {
                    float percent = (float)(randBytes[randIndex++]) / (float)(byte.MaxValue);
                    if (percent > configPercent)
                    {
                        continue;
                    }
                }
                tree.gameObject.SetActive(false);
            }
        }

        private void ResetFoliage()
        {
            EFT.Interactive.TreeInteractive[] treeInteractives = (EFT.Interactive.TreeInteractive[])UnityEngine.GameObject.FindObjectsOfType<EFT.Interactive.TreeInteractive>(true);
            foreach (EFT.Interactive.TreeInteractive tree in treeInteractives)
            {
                tree.gameObject.SetActive(true);
            }
            EFT.SpeedTree.TreeWind[] treeWinds = (EFT.SpeedTree.TreeWind[])UnityEngine.GameObject.FindObjectsOfType<EFT.SpeedTree.TreeWind>(true);
            foreach (EFT.SpeedTree.TreeWind tree in treeWinds)
            {
                tree.gameObject.SetActive(true);
            }
        }

        private void ToggleSlowness()
        {
            EFT.Interactive.ObstacleCollider[] obstacleColliders = (EFT.Interactive.ObstacleCollider[])UnityEngine.GameObject.FindObjectsOfType<EFT.Interactive.ObstacleCollider>(true);
            foreach (EFT.Interactive.ObstacleCollider obstacleCollider in obstacleColliders)
            {
                obstacleCollider.gameObject.SetActive(!obstacleCollider.gameObject.activeSelf);
            }
        }

        private void ToggleFoliage()
        {
            EFT.Interactive.TreeInteractive[] treeInteractives = (EFT.Interactive.TreeInteractive[])UnityEngine.GameObject.FindObjectsOfType<EFT.Interactive.TreeInteractive>(true);
            foreach (EFT.Interactive.TreeInteractive tree in treeInteractives)
            {
                tree.gameObject.SetActive(!tree.gameObject.activeSelf);
            }
            EFT.SpeedTree.TreeWind[] treeWinds = (EFT.SpeedTree.TreeWind[])UnityEngine.GameObject.FindObjectsOfType<EFT.SpeedTree.TreeWind>(true);
            foreach (EFT.SpeedTree.TreeWind tree in treeWinds)
            {
                tree.gameObject.SetActive(!tree.gameObject.activeSelf);
            }
        }

        private void PlayerCollisionTesting()
        {
            if ((!Singleton<GameWorld>.Instantiated) || (Singleton<GameWorld>.Instance.MainPlayer == null))
            {
                return;
            }
            GameWorld gameWorld = Singleton<GameWorld>.Instance;
            Player player = gameWorld.MainPlayer;
            BaseLocalGame<EftGamePlayerOwner> baseLocalGame = (BaseLocalGame<EftGamePlayerOwner>)Singleton<AbstractGame>.Instance;
        }

        void Update()
        {
            if (DecimateFoliageShortCutConfig.Value.IsDown())
            {
                DecimateFoliage();
            }
            if (ResetFoliageShortCutConfig.Value.IsDown())
            {
                ResetFoliage();
            }
            if (ToggleSlownessShortCutConfig.Value.IsDown())
            {
                ToggleSlowness();
            }
            testId = 0;
            int nonce = testId + 0;
            if (testId > 0)
            {
                if (testId == 1)
                {
                    ToggleFoliage();
                }
                if (testId == 100)
                {
                    PlayerCollisionTesting();
                }
            }
        }
    }
}
