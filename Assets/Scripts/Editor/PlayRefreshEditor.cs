using UnityEditor;

namespace Editor
{
    [InitializeOnLoad]
    public static class PlayRefreshEditor
    {
        private const string MENU_ITEM_NAME = "Tools/AutoRefreshOnPlay";
        private const string AUTO_REFRESH_PREFERENCE_KEY = "EditorAutoRefreshOnPlayEnabled";

        private static bool _isAutoRefreshOnPlay;

        static PlayRefreshEditor()
        {
            EditorApplication.update += Initialize;
        }

        private static void Initialize()
        {
            EditorApplication.update -= Initialize;

            _isAutoRefreshOnPlay = EditorPrefs.GetBool(AUTO_REFRESH_PREFERENCE_KEY, false);
            Menu.SetChecked(MENU_ITEM_NAME, _isAutoRefreshOnPlay);

            if (_isAutoRefreshOnPlay)
            {
                EnableAutoRefreshOnPlay();
            }
        }

        private static void EnableAutoRefreshOnPlay()
        {
            EditorApplication.playModeStateChanged += PlayRefresh;
        }

        private static void PlayRefresh(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport);
            }
        }

        [MenuItem(MENU_ITEM_NAME)]
        private static void ToggleAutoRefreshOnPlay()
        {
            _isAutoRefreshOnPlay = _isAutoRefreshOnPlay == false;
            EditorPrefs.SetBool(AUTO_REFRESH_PREFERENCE_KEY, _isAutoRefreshOnPlay);
            Menu.SetChecked(MENU_ITEM_NAME, _isAutoRefreshOnPlay);

            if (_isAutoRefreshOnPlay)
            {
                EnableAutoRefreshOnPlay();
            }
            else
            {
                DisableAutoRefreshOnPlay();
            }
        }

        private static void DisableAutoRefreshOnPlay()
        {
            EditorApplication.playModeStateChanged -= PlayRefresh;
        }
    }
}
