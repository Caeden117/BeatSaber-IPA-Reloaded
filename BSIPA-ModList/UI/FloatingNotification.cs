﻿using CustomUI.BeatSaber;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BSIPA_ModList.UI
{
    internal class FloatingNotification : MonoBehaviour
    {
        private Canvas _canvas;
        private TMP_Text _authorNameText;
        private TMP_Text _pluginNameText;
        private TMP_Text _headerText;
        private Image _loadingBackg;
        private Image _loadingBar;

        private static readonly Vector3 Position = new Vector3(2.25f, 2.3f, 1.55f);
        private static readonly Vector3 Rotation = new Vector3(0, 60, 0);
        private static readonly Vector3 Scale = new Vector3(0.01f, 0.01f, 0.01f);

        private static readonly Vector2 CanvasSize = new Vector2(100, 50);

        private const string AuthorNameText = "BSIPA";
        private const float AuthorNameFontSize = 7f;
        private static readonly Vector2 AuthorNamePosition = new Vector2(10, 31);

        private const string PluginNameText = "Mod Updater";
        private const float PluginNameFontSize = 9f;
        private static readonly Vector2 PluginNamePosition = new Vector2(10, 23);

        private static readonly Vector2 HeaderPosition = new Vector2(10, 15);
        private static readonly Vector2 HeaderSize = new Vector2(100, 20);
        private const string HeaderText = "Checking for updates...";
        private const float HeaderFontSize = 15f;

        private static readonly Vector2 LoadingBarSize = new Vector2(100, 10);
        private static readonly Color BackgroundColor = new Color(0, 0, 0, 0.2f);

        private bool _showingMessage;

        public static FloatingNotification Create()
        {
            return new GameObject("Mod List Floating Notification").AddComponent<FloatingNotification>();
        }

        public void ShowMessage(string message, float time)
        {
            StopAllCoroutines();
            _showingMessage = true;
            _headerText.text = message;
            _headerText.alignment = TextAlignmentOptions.Left;
            _loadingBar.enabled = false;
            _loadingBackg.enabled = false;
            _canvas.enabled = true;
            StartCoroutine(DisableCanvasRoutine(time));
        }

        public void ShowMessage(string message)
        {
            StopAllCoroutines();
            _showingMessage = true;
            _headerText.text = message;
            _headerText.alignment = TextAlignmentOptions.Left;
            _loadingBar.enabled = false;
            _loadingBackg.enabled = false;
            _canvas.enabled = true;
        }

        protected void OnEnable()
        {
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
            DownloadController.Instance.OnDownloadStateChanged += DownloaderStateChanged;
            DownloadController.Instance.OnCheckForUpdates += CheckForUpdatesStart;
            DownloadController.Instance.OnCheckForUpdatesComplete += CheckForUpdatesDone;
        }

        protected void OnDisable()
        {
            SceneManager.activeSceneChanged -= SceneManagerOnActiveSceneChanged;
            DownloadController.Instance.OnDownloadStateChanged -= DownloaderStateChanged;
            DownloadController.Instance.OnCheckForUpdates -= CheckForUpdatesStart;
            DownloadController.Instance.OnCheckForUpdatesComplete -= CheckForUpdatesDone;
        }

        private void CheckForUpdatesStart()
        {
            StopAllCoroutines();
            _showingMessage = false;
            _headerText.text = HeaderText;
            _headerText.alignment = TextAlignmentOptions.Left;
            _loadingBar.enabled = false;
            _loadingBackg.enabled = false;
            _canvas.enabled = true;
        }

        private bool updatesZero = false;
        private void CheckForUpdatesDone(int count)
        {
            if (count == 0) updatesZero = true;
            else updatesZero = false;

            StopAllCoroutines();
            _showingMessage = false;
            _headerText.text = $"{count} updates found";
            _headerText.alignment = TextAlignmentOptions.Left;
            _loadingBar.enabled = false;
            _loadingBackg.enabled = false;
            _canvas.enabled = true;

            StartCoroutine(DisableCanvasRoutine(5f));
        }

        private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.name == "MenuCore")
            {
                if (_showingMessage)
                {
                    _canvas.enabled = true;
                }
            }
            else
            {
                _canvas.enabled = false;
            }
        }

        private void DownloaderStateChanged()
        {
            if (DownloadController.Instance.IsDownloading)
            {
                StopAllCoroutines();
                _showingMessage = false;
                _headerText.text = "Downloading updates...";
                _headerText.alignment = TextAlignmentOptions.Left;
                _loadingBar.enabled = false;
                _loadingBackg.enabled = false;
                _canvas.enabled = true;
            }
            if (DownloadController.Instance.IsDone && !updatesZero)
            {
                StopAllCoroutines();
                _showingMessage = false;
                _headerText.text = "Update complete. Restart to finish installation.";
                _headerText.alignment = TextAlignmentOptions.Center;
                _loadingBar.enabled = false;
                _loadingBackg.enabled = false;
            }
        }

        internal void Close()
        {
            StartCoroutine(DisableCanvasRoutine(0f));
        }

        private IEnumerator DisableCanvasRoutine(float time)
        {
            yield return new WaitForSecondsRealtime(time);
            _canvas.enabled = false;
            _showingMessage = false;
        }

        internal static FloatingNotification instance;

        protected void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this;

            gameObject.transform.position = Position;
            gameObject.transform.eulerAngles = Rotation;
            gameObject.transform.localScale = Scale;

            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.enabled = false;
            var rectTransform = _canvas.transform as RectTransform;
            rectTransform.sizeDelta = CanvasSize;

            _authorNameText = BeatSaberUI.CreateText(_canvas.transform as RectTransform, AuthorNameText, AuthorNamePosition);
            rectTransform = _authorNameText.transform as RectTransform;
            rectTransform.SetParent(_canvas.transform, false);
            rectTransform.anchoredPosition = AuthorNamePosition;
            rectTransform.sizeDelta = HeaderSize;
            _authorNameText.text = AuthorNameText;
            _authorNameText.fontSize = AuthorNameFontSize;

            _pluginNameText = BeatSaberUI.CreateText(_canvas.transform as RectTransform, PluginNameText, PluginNamePosition);
            rectTransform = _pluginNameText.transform as RectTransform;
            rectTransform.SetParent(_canvas.transform, false);
            rectTransform.sizeDelta = HeaderSize;
            rectTransform.anchoredPosition = PluginNamePosition;
            _pluginNameText.text = PluginNameText;
            _pluginNameText.fontSize = PluginNameFontSize;

            _headerText = BeatSaberUI.CreateText(_canvas.transform as RectTransform, HeaderText, HeaderPosition);
            rectTransform = _headerText.transform as RectTransform;
            rectTransform.SetParent(_canvas.transform, false);
            rectTransform.anchoredPosition = HeaderPosition;
            rectTransform.sizeDelta = HeaderSize;
            _headerText.text = HeaderText;
            _headerText.fontSize = HeaderFontSize;

            _loadingBackg = new GameObject("Background").AddComponent<Image>();
            rectTransform = _loadingBackg.transform as RectTransform;
            rectTransform.SetParent(_canvas.transform, false);
            rectTransform.sizeDelta = LoadingBarSize;
            _loadingBackg.color = BackgroundColor;

            _loadingBar = new GameObject("Loading Bar").AddComponent<Image>();
            rectTransform = _loadingBar.transform as RectTransform;
            rectTransform.SetParent(_canvas.transform, false);
            rectTransform.sizeDelta = LoadingBarSize;
            var tex = Texture2D.whiteTexture;
            var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f, 100, 1);
            _loadingBar.sprite = sprite;
            _loadingBar.type = Image.Type.Filled;
            _loadingBar.fillMethod = Image.FillMethod.Horizontal;
            _loadingBar.color = new Color(1, 1, 1, 0.5f);

            DontDestroyOnLoad(gameObject);
        }

        /*private void Update()
        {
            if (!_canvas.enabled) return;
            _loadingBar.fillAmount = SongLoader.LoadingProgress;
        }*/
    }
}
