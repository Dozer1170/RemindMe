﻿using System.Collections.Generic;
using Dalamud.Game.ClientState.Objects.Types;
using Lumina.Excel.GeneratedSheets;
using Newtonsoft.Json;

namespace RemindMe.Config {
    public class StatusMonitor {
        public uint ClassJob = 0;
        public uint Status;
        public bool SelfOnly = false;
        public bool IsRaid = false;
        public bool Stacking = false;
        public bool SingleIcon = false;
        public float MaxDuration = 30;
        public ushort LimitedZone = 0;
        public bool AlwaysAvailable = false;
        public byte MinLevel = byte.MinValue;
        public byte MaxLevel = byte.MaxValue;
        public uint[] StatusList;
        [JsonIgnore] public Status StatusData { get; set; }

        public override bool Equals(object obj) {
            if (!(obj is StatusMonitor sm)) return false;
            return sm.Status == this.Status && sm.ClassJob == this.ClassJob && sm.SelfOnly == this.SelfOnly && sm.IsRaid == this.IsRaid && sm.AlwaysAvailable == this.AlwaysAvailable && sm.MinLevel == this.MinLevel && sm.MaxLevel == this.MaxLevel;
        }


        [JsonIgnore] private uint[] allStatusListCache;
        [JsonIgnore]
        public uint[] StatusIDs {
            get {
                if (allStatusListCache != null) return allStatusListCache;
                var buildingCache = new List<uint>() {Status};
                if (StatusList != null) {
                    buildingCache.AddRange(StatusList);
                }
                buildingCache.Sort();
                allStatusListCache = buildingCache.ToArray();
                return allStatusListCache;
            }
        }

        public void ClickHandler(RemindMe plugin, object param) {
            if (param is GameObject a) {
                Service.Targets.SetTarget(a);
            }
        }
    }
}
