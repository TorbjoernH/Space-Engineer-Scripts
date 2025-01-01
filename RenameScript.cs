List<IMyTerminalBlock> allBlocks = new List<IMyTerminalBlock>();

public Program()
{
    GridTerminalSystem.GetBlocks(allBlocks);
}

public void Main(string argument, UpdateType updateSource)
{
    foreach (IMyTerminalBlock block in allBlocks)
    {
        if (block.IsSameConstructAs(Me) && !block.CustomName.Contains($"[{block.CubeGrid.CustomName}]"))
        {
            block.CustomName = "[" + block.CubeGrid.CustomName + "] " + block.CustomName;
        }

    }
}