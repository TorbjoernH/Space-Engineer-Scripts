List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();

public Program()
{
    GridTerminalSystem.GetBlocks(allBlocks);
}

public void Main(string argument, UpdateType updateSource)
{
    foreach (IMyTerminalBlock block in allBlocks)
    {
        if (!block.IsSameConstructAs(Me)) { continue; }

        if (block.CustomName.Contains(block.CubeGrid.CustomName)) { continue; }

        if (block.CustomName.Contains("[") && block.CustomName.Contains("]")) { continue; }

        block.CustomName = "[" + block.CubeGrid.CustomName + "] " + block.CustomName;
    }
}