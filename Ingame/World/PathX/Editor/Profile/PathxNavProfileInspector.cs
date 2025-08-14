using Corelib.SUI;

namespace PathX
{
    public static class PathxNavProfileInspector
    {
        private static bool fold = false;

        public static SUIElement Render(PathXNavProfile profileAgent)
        {
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.FoldGroup(profileAgent.domain.ToString(), fold)
                .OnValueChanged(value => fold = value)
                .Content(
                    PathXNavProfileConfigInspector.Render(profileAgent.appliedConfig, true)
                    + PathXNavProfileConfigInspector.Render(profileAgent.config)
                )
            );
        }
    }
}