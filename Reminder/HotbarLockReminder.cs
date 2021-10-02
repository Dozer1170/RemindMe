﻿using System;
using System.Linq;
using System.Runtime.InteropServices;
using Dalamud.Plugin;
using Newtonsoft.Json;
using RemindMe.Config;

namespace RemindMe.Reminder {
    internal unsafe class HotbarLockReminder : GeneralReminder {

        [JsonIgnore]
        public override string Name => "Hotbar Lock Reminder";

        [JsonIgnore]
        public override string Description => "Reminds you to lock your hotbar.\n(Cross Hotbar not included)";

        public override string GetText(DalamudPluginInterface pluginInterface, RemindMe plugin, MonitorDisplay display) {
            return "Lock Hotbar";
        }

        public override bool ShouldShow(DalamudPluginInterface pluginInterface, RemindMe plugin, MonitorDisplay display) {
            var actionBar = Service.GameGui.GetAddonByName("_ActionBar", 1);
            if (actionBar == IntPtr.Zero) return false;
            return Marshal.ReadByte(actionBar, 0x23F) == 0;
        }

        public override ushort GetIconID(DalamudPluginInterface pluginInterface, RemindMe plugin, MonitorDisplay display) {
            return 5; //60840;
        }

    }
}
