using GTRMod.Survivors.GTRaptor.SkillStates;
namespace GTRMod.Survivors.GTRaptor
{
    public static class GTRaptorStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Vulcans));

            Modules.Content.AddEntityState(typeof(Blasting));

            Modules.Content.AddEntityState(typeof(Dash));

            Modules.Content.AddEntityState(typeof(ActivateScanner));
        }
    }
}
