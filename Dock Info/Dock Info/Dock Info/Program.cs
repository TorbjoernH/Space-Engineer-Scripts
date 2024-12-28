using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // Go to:
        // https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
        //
        // to learn more about ingame scripts.

        public Program()
        {
            InitDocks();
        }


        public void Main(string argument, UpdateType updateSource)
        {


        }
        public class Dock
        {

            public string Name { get; }
            public List<Ship> Ships { get; } = new List<Ship>();
            public List<IMyTerminalBlock> Connectors { get; } = new List<IMyTerminalBlock>();
            public Dock(string name)
            {
                Name = name;
            }

        }
        public class Ship
        {
            public string Name { get; set; }
            public string Owner { get; set; }
            public Ship(string name, string owner)
            {
                Name = name;
                Owner = owner;
            }
        }

        List<Dock> Docks = new List<Dock>();
        public void InitDocks()
        {
            string connectorGroupName = "Base Hanger 1 Connectors";
            List<IMyTerminalBlock> connectors = new List<IMyTerminalBlock>();
            var connectorGroup = GridTerminalSystem.GetBlockGroupWithName(connectorGroupName);
            connectorGroup.GetBlocks(connectors);

            Docks.Add(new Dock("White"));
            Docks.Add(new Dock("Red"));
            Docks.Add(new Dock("Purple"));
            Docks.Add(new Dock("Yellow"));
            Docks.Add(new Dock("Green"));
            Docks.Add(new Dock("Blue"));

            foreach (var dock in Docks)
            {
                Echo(dock.Name);
                GridTerminalSystem.SearchBlocksOfName($"[Base] Connector {dock.Name}", dock.Connectors);
                Echo(dock.Connectors[0].CustomName);
                Echo(dock.Connectors[1].CustomName);
            }
        }
        
        public void CheckDock()
        {

        }

    }
}
