using System;
using UnityEngine;

namespace SimpleDialogue
{
    public static class DialogueSystem
    {
        public static event Action OnKeysUpdate;

        public static event Action OnSettingsUpdate;

        public static KeyCode ActiveSubmitKey
        {
            get { return _activeSubmitKey; }
            set { _activeSubmitKey = value; OnKeysUpdate?.Invoke(); }
        }

        public static KeyCode AltSubmitKey
        {
            get { return _altSubmitKey; }
            set { _altSubmitKey = value; OnKeysUpdate?.Invoke(); }
        }

        public static KeyCode ActiveSkipKey
        {
            get { return _activeSkipKey; }
            set { _activeSkipKey = value; OnKeysUpdate?.Invoke(); }
        }

        public static KeyCode AltSkipKey
        {
            get { return _altSkipKey; }
            set { _altSkipKey = value; OnKeysUpdate?.Invoke(); }
        }

        public static KeyCode ActiveSkipToAllKey
        {
            get { return _activeSkipToAllKey; }
            set { _activeSkipToAllKey = value; OnKeysUpdate?.Invoke(); }
        }

        public static KeyCode AltSkipToAllKey
        {
            get { return _altSkipToAllKey; }
            set { _altSkipToAllKey = value; OnKeysUpdate?.Invoke(); }
        }

        private static KeyCode _activeSubmitKey = KeyCode.Return;
        private static KeyCode _altSubmitKey = KeyCode.Z;
        private static KeyCode _activeSkipKey = KeyCode.RightShift;
        private static KeyCode _altSkipKey = KeyCode.X;
        private static KeyCode _activeSkipToAllKey = KeyCode.RightControl;
        private static KeyCode _altSkipToAllKey = KeyCode.C;

        public static class DefaultSettings
        {
            public static float TickTime
            {
                get { return _tickTime; }
                set { _tickTime = value; OnSettingsUpdate?.Invoke(); }
            }

            public static Font Font
            {
                get { return _font; }
                set { _font = value; OnSettingsUpdate?.Invoke(); }
            }

            public static Color Color
            {
                get { return _color; }
                set { _color = value; OnSettingsUpdate?.Invoke(); }
            }

            private static float _tickTime = 0.0432f;
            private static Font _font;
            private static Color _color = Color.white;
        }

        public static Char[] SoundlessChars = { ' ', '.', ',', '?' };

        public static IntervalChar[] IntervalChars = { new IntervalChar(',', 5f), new IntervalChar('.', 8f), new IntervalChar('!', 8f) };

        private static DialogueSystemBrain _mainSceneBehaviour;

        public static DialogueSystemBrain Brain
        {
            get { return _mainSceneBehaviour; }
        }

        public static Boolean IsPlaying
        {
            get
            {
                if (_mainSceneBehaviour != null)
                {
                    return _mainSceneBehaviour.IsPlaying;
                }
                else
                {
                    Debug.LogError("There no DialogueSystemBrain in scene");
                    return false;
                }
            }
        }

        public static void SetAllOptions(DialogueSystemStaticOptions options)
        {
            _activeSubmitKey = options.ActiveSubmitKey;
            _altSubmitKey = options.AltSubmitKey;
            _activeSkipKey = options.ActiveSkipKey;
            _altSkipKey = options.AltSkipKey;
            _activeSkipToAllKey = options.ActiveSkipToAllKey;
            _altSkipToAllKey = options.AltSkipToAllKey;
            DefaultSettings.TickTime = options.DefaultTickTime;
            if (options.DefaultFont != null)
            DefaultSettings.Font = options.DefaultFont;
            DefaultSettings.Color = options.DefaultColor;
            SoundlessChars = options.SoundlessChars;
            IntervalChars = options.IntervalChars;
        }

        public static void SetAllOptions(DialogueSystemBrain dialogueSystem)
        {
            _mainSceneBehaviour = dialogueSystem;
        }
    }
}