//Battery List
List<IMyBatteryBlock> batteryBlocks = new List<IMyBatteryBlock>();
//Thruster List
List<IMyThrust> thrusterBlocks = new List<IMyThrust>();
//HydroTanks
List<IMyGasTank> tankBlocks = new List<IMyGasTank>();
//Connector
IMyShipConnector connector;

public Program()
{

    InitBatteries();

    InitThrust();

    InitTanks();

    connector = GridTerminalSystem.GetBlockWithName("[" + Me.CubeGrid.CustomName + "] " + "Connector (Main)") as IMyShipConnector;

}

public void Main(string argument, UpdateType updateSource)
{
    if (connector == null) { return; }

    if (connector.IsConnected)
    {
        OnConnect();
    }
    else
    {
        OnDisconnect();
    }
}

public void OnConnect()
{
    if (batteryBlocks.Count > 0)
    {
        foreach (IMyBatteryBlock battery in batteryBlocks)
        {
            battery.ChargeMode = ChargeMode.Recharge;
        }
    }

    if (tankBlocks.Count > 0)
    {
        foreach (IMyGasTank tank in tankBlocks)
        {
            tank.Stockpile = true;
        }
    }

    if (thrusterBlocks.Count > 0)
    {
        foreach (IMyThrust thruster in thrusterBlocks)
        {
            thruster.Enabled = false;
        }
    }

}

public void OnDisconnect()
{
    if (batteryBlocks.Count > 0)
    {
        foreach (IMyBatteryBlock battery in batteryBlocks)
        {
            battery.ChargeMode = ChargeMode.Auto;
        }
    }

    if (tankBlocks.Count > 0)
    {
        foreach (IMyGasTank tank in tankBlocks)
        {
            tank.Stockpile = false;
        }
    }

    if (thrusterBlocks.Count > 0)
    {
        foreach (IMyThrust thruster in thrusterBlocks)
        {
            thruster.Enabled = true;
        }
    }
}

public void InitBatteries()
{
    GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteryBlocks);

    if (batteryBlocks.Count > 0)
    {
        foreach (IMyBatteryBlock battery in batteryBlocks)
        {
            if (battery.IsSameConstructAs(Me)) { continue; }

            batteryBlocks.Remove(battery);
        }
    }
}

public void InitThrust()
{
    GridTerminalSystem.GetBlocksOfType<IMyThrust>(thrusterBlocks);

    if (thrusterBlocks.Count > 0)
    {
        foreach (IMyThrust thrust in thrusterBlocks)
        {
            if (thrust.IsSameConstructAs(Me)) { continue; }

            thrusterBlocks.Remove(thrust);
        }
    }
}

public void InitTanks()
{
    GridTerminalSystem.GetBlocksOfType<IMyGasTank>(tankBlocks);

    if (tankBlocks.Count > 0)
    {
        foreach (IMyGasTank tank in tankBlocks)
        {
            if (tank.IsSameConstructAs(Me)) { continue; }

            tankBlocks.Remove(tank);
        }
    }
}