#if UNITY_EDITOR
using SimpleDialogue;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleDialogueEditor
{
    [CustomEditor(typeof(Dialogue))]
    public class DialogueEditor : Editor
    {
        private Dialogue dialogueTarget;
        private DialogueSystemStaticOptions dialogueOptions;
       // private byte languageIndex;

        private void OnEnable()
        {
            dialogueOptions = AssetDatabase.LoadAssetAtPath<DialogueSystemStaticOptions>("Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
            if (dialogueOptions == null)
            {
                DialogueSystemStaticOptions newOptions = new DialogueSystemStaticOptions();
                AssetDatabase.CreateAsset(newOptions, "Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
                dialogueOptions = AssetDatabase.LoadAssetAtPath<DialogueSystemStaticOptions>("Assets/SimpleDialogue/Runtime/DialogueSystemOptionsFile.asset");
            }
           // languageIndex = 0;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            dialogueTarget = (Dialogue)target;
            OnGUI();
            serializedObject.ApplyModifiedProperties();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnGUI()
        {
            if (dialogueTarget.Messages.Length == 0)
            {
                dialogueTarget.Messages = new DialogueMessage[1] { new DialogueMessage((byte)dialogueOptions.LanguageNames.Length) };
            }

            DrawSimpleMode();
        }

        private const byte c_simpleModeRawSize = 54;
        private void DrawSimpleMode()
        {
            if (dialogueTarget.Messages == null || dialogueTarget.Messages.Length == 0)
            {
                dialogueTarget.Messages = new DialogueMessage[0];
            }

            for (int i = 0; i < dialogueTarget.Messages.Length; i++)
            {
                GUILayout.BeginHorizontal();

                dialogueTarget.Messages[i].SpeakerIcon = SpriteFieldWithSquarePreview(dialogueTarget.Messages[i].SpeakerIcon, c_simpleModeRawSize, i);

                if (dialogueTarget.Messages.Length > 0)
                {
                    string text = dialogueTarget.Messages[i].Text[dialogueOptions.CurrentLanguageIndex];
                    dialogueTarget.Messages[i].Text[dialogueOptions.CurrentLanguageIndex] = DrawSimpleTextArea(text);
                }

                dialogueTarget.Messages[i].SpeakerSound = AudioClipFieldWithSquarePreview(dialogueTarget.Messages[i].SpeakerSound, c_simpleModeRawSize, byte.MaxValue - i);

                GUILayout.EndHorizontal();
                GUILayout.Space(3f);
            }

            if (GUILayout.Button("NEW"))
            {
                List<DialogueMessage> messages = new(dialogueTarget.Messages);

                DialogueMessage newMessage = null;

                if (messages.Count > 0)
                {
                    newMessage = new(messages[messages.Count-1]);
                }
                else
                {
                    newMessage = new((byte)dialogueOptions.LanguageNames.Length);
                }

                messages.Add(newMessage);
    
                dialogueTarget.Messages = messages.ToArray();
            }
        }

        private string DrawSimpleTextArea(string text)
        {
            GUIStyle areaStyle = new(GUI.skin.textField);
            areaStyle.fontSize = 13;
            areaStyle.normal.textColor = new(0.7f, 0.7f, 0.7f);
            areaStyle.hover.textColor = new(0.7f, 0.7f, 0.7f);
            return EditorGUILayout.TextArea(text, areaStyle, GUILayout.Height(c_simpleModeRawSize), GUILayout.ExpandWidth(true));
        }

        public static Sprite SpriteFieldWithSquarePreview(Rect position, Sprite obj, GUIStyle style, int controlID)
        {
            if (obj != null)
            {
                Texture2D previewTexture = AssetPreview.GetAssetPreview(obj);
                if (previewTexture != null)
                {
                    GUI.DrawTexture(position, previewTexture, ScaleMode.ScaleToFit);
                }
            }
            else
            {
                EditorGUI.LabelField(position, "None\n(Sprite)", style);
            }

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown && position.Contains(currentEvent.mousePosition))
            {
                EditorGUIUtility.ShowObjectPicker<Sprite>(obj, false, "", controlID);
                currentEvent.Use();
            }

            if (Event.current.commandName == "ObjectSelectorUpdated" &&
                EditorGUIUtility.GetObjectPickerControlID() == controlID)
            {
                Object selectedObject = EditorGUIUtility.GetObjectPickerObject();
                if (selectedObject is Sprite selectedSprite)
                {
                    obj = selectedSprite;
                }
                else if (selectedObject == null)
                {
                    obj = null;
                }
            }

            return obj;
        }

        public static AudioClip AudioClipFieldWithSquarePreview(Rect position, AudioClip obj, GUIStyle style, int controlID)
        {
            if (obj != null)
            {
                Texture2D previewTexture = AssetPreview.GetAssetPreview(obj);
                if (previewTexture != null)
                {
                    GUI.DrawTexture(position, previewTexture, ScaleMode.ScaleToFit);
                }
            }
            else
            {
                EditorGUI.LabelField(position, "None\n(AudioClip)", style);
            }

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown && position.Contains(currentEvent.mousePosition))
            {
                EditorGUIUtility.ShowObjectPicker<AudioClip>(obj, false, "", controlID);
                currentEvent.Use();
            }

            if (Event.current.commandName == "ObjectSelectorUpdated" &&
                EditorGUIUtility.GetObjectPickerControlID() == controlID)
            {
                Object selectedObject = EditorGUIUtility.GetObjectPickerObject();
                if (selectedObject is AudioClip selectedAudioClip)
                {
                    obj = selectedAudioClip;
                }
                else if (selectedObject == null)
                {
                    obj = null;
                }
            }

            return obj;
        }

        public static Sprite SpriteFieldWithSquarePreview(Sprite obj, int size, int controlID)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, size, GUILayout.Width(size), GUILayout.Height(size));
            GUIStyle style = new(EditorStyles.textField);
            style.normal.textColor = new(0.7f, 0.7f, 0.7f);
            style.fontStyle = FontStyle.Normal;
            style.fontSize = 9;
            style.alignment = TextAnchor.UpperCenter;
            return SpriteFieldWithSquarePreview(rect, obj, style, controlID);
        }

        public static AudioClip AudioClipFieldWithSquarePreview(AudioClip obj, int size, int controlID)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, size, GUILayout.Width(size), GUILayout.Height(size));
            GUIStyle style = new(GUI.skin.textField);
            style.normal.textColor = new(0.7f, 0.7f, 0.7f);
            style.fontStyle = FontStyle.Normal;
            style.fontSize = 9;
            style.alignment = TextAnchor.UpperCenter;
            return AudioClipFieldWithSquarePreview(rect, obj, style, controlID);
        }

    }
}
#endif