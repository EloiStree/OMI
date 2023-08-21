
using UnityEngine;

public  class HIDClipboardQuickCodeMono : MonoBehaviour
{
    public void SetClipboardText(string text)
    {
#if UNITY_EDITOR
        UnityEditor.EditorGUIUtility.systemCopyBuffer = text;
#elif UNITY_ANDROID
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject clipboardManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "clipboard");
            AndroidJavaClass clipDataClass = new AndroidJavaClass("android.content.ClipData");
            string label = "text";
            AndroidJavaObject clipData = clipDataClass.CallStatic<AndroidJavaObject>("newPlainText", label, text);
            clipboardManager.Call("setPrimaryClip", clipData);
#elif UNITY_IOS
            SetClipboardText_iOS(text);
#else

        GUIUtility.systemCopyBuffer = text;
#endif
    }

#if UNITY_IOS
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void SetClipboardText_iOS(string text);
#endif
}