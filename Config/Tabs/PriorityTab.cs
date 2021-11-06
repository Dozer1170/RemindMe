using System;
using System.Numerics;
using ImGuiNET;
using RemindMe.Config;
using Action = Lumina.Excel.GeneratedSheets.Action;

namespace RemindMe 
{
    public partial class RemindMeConfig
    {
        public void DrawPriorityTab() 
        {
            ImGui.BeginChild("###priorityScroll", ImGui.GetWindowSize() - (ImGui.GetStyle().WindowPadding * 2) - new Vector2(0, ImGui.GetCursorPosY()));
            
            foreach (var display in MonitorDisplays.Values) {
                if (ImGui.CollapsingHeader($"{(display.Enabled ? "":"[Disabled] ")}{display.Name}###configDisplay{display.Guid}")) {
                    DrawDisplayPriority(display);
                }
            }
            ImGui.EndChild();
        }

        private void DrawDisplayPriority(MonitorDisplay display)
        {
            ImGui.Indent(10);
            ImGui.Checkbox("Enable priority sort", ref display.SortByPriority);

            if (!display.SortByPriority) return;
            
            ImGui.BeginListBox("", new Vector2(ImGui.GetWindowWidth() - 10f, 118f));
            
            for (var i = 0; i < display.Cooldowns.Count; i++)
            {
                var cooldown = display.Cooldowns[i];
                var action = plugin.ActionManager.GetAction(cooldown.ActionId);
                if (action == null) continue;

                if (!display.SortPriorities.ContainsKey(action.Name))
                {
                    display.SortPriorities[action.Name] = i;
                }
                
                var icon = plugin.IconManager.GetActionIcon(action);
                if (icon != null) {
                    ImGui.Image(icon.ImGuiHandle, new Vector2(25));
                } else {
                    ImGui.Dummy(new Vector2(24));
                }

                ImGui.SameLine();
                ImGui.Text(action.Name);

                ImGui.SameLine(ImGui.GetWindowWidth() - 80);
                if (ImGui.ArrowButton($"Up{i}", ImGuiDir.Up) && i > 0)
                {
                    // Move action up       
                    Swap(display, action, i, i - 1);
                    Save();
                }
                
                ImGui.SameLine(ImGui.GetWindowWidth() - 50);
                if (ImGui.ArrowButton($"Down{i}", ImGuiDir.Down) && i < display.Cooldowns.Count - 1)
                {
                    // Move action down
                    Swap(display, action, i, i + 1);
                    Save();
                }
            }
            ImGui.EndListBox();
        }

        private void SwapActionPriorities(MonitorDisplay display, Action action, int index, int targetIndex)
        {
            var targetSlotAction = plugin.ActionManager.GetAction(display.Cooldowns[targetIndex].ActionId);
            if (targetSlotAction == null)
                return;

            display.SortPriorities[action.Name] = targetIndex;
            display.SortPriorities[targetSlotAction.Name] = index;
        }

        private void Swap(MonitorDisplay display, Action action, int index, int targetIndex)
        {
            SwapActionPriorities(display, action, index, targetIndex);
            (display.Cooldowns[targetIndex], display.Cooldowns[index]) = (display.Cooldowns[index], display.Cooldowns[targetIndex]);
        }
    }
}
