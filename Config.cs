using System.ComponentModel;
using Exiled.API.Interfaces;

namespace GCD
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Enable or disable the timer system for the Grenade.")]
        public bool EnableGrenadeTimer { get; set; } = true;
        
        [Description("Enable or disable the timer system for the Flash.")]
        public bool EnableFlashTimer { get; set; } = true;
        
        [Description("The duration of the timer for the Grenade in seconds.")]
        public int GrenadeTimerDuration { get; set; } = 6;

        [Description("The duration of the timer for the Flash in seconds.")]
        public int FlashTimerDuration { get; set; } = 6;
        
        [Description("The text to display when the grenade will explode.")]
        public string GrenadeExplodeText { get; set; } = "<color=yellow>Grenade will explode in</color> <color=red>{0}</color> <color=yellow>seconds</color>";
        
        [Description("The text to display when the flashbang will explode.")]
        public string FlashExplodeText { get; set; } = "<color=yellow>Flash will explode in</color> <color=red>{0}</color> <color=yellow>seconds</color>";
        
        [Description("The time in seconds before the grenade explodes.")]
        public float GrenadeFuseTime { get; set; } = 0.1f;
        
        [Description("The time in seconds the grenade will concuss players.")]
        public float GrenadeConcussDuration { get; set; } = 5f;
        
        [Description("The time in seconds the grenade will burn players.")]
        public float GrenadeBurnDuration { get; set; } = 5f;
        
        [Description("The time in seconds before the flashbang explodes.")]
        public float FlashFuseTime { get; set; } = 0.1f;
    }
}