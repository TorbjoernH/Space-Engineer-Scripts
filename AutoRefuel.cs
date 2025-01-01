//Battery List
List<IMyBatteryBlock> batteryBlocks = new List<IMyBatteryBlock>();
//Thruster List
List<IMyThrust> thrusterBlocks = new List<IMyThrust>();
//HydroTanks
List<IMyGasTank> tankBlocks = new List<IMyGasTank>();
//Antennas
List<IMyRadioAntenna> antennas = new List<IMyRadioAntenna>();
//Ore detector
List<IMyOreDetector> oreDetectors = new List<IMyOreDetector>();

public Program()
{

    InitBatteries();

    InitThrust();

    InitTanks();

    InitAntennas();

    InitOreDetectors();
    
}

public void Main(string argument, UpdateType updateSource)
{
    if(argument=="connect")
        OnConnect();
    else if(argument=="disconnect")
        OnDisconnect();
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

    if (antennas.Count > 0)
    {
        foreach (IMyRadioAntenna antenna in antennas)
        {
            antenna.Enabled = false;
        }
    }

    if (oreDetectors.Count > 0)
    {
        foreach (IMyOreDetector oreDetector in oreDetectors)
        {
            oreDetector.Enabled = false;
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

    if (antennas.Count > 0)
    {
        foreach (IMyRadioAntenna antenna in antennas)
        {
            antenna.Enabled = true;
        }
    }

    if (oreDetectors.Count > 0)
    {
        foreach (IMyOreDetector oreDetector in oreDetectors)
        {
            oreDetector.Enabled = true;
        }
    }
}

public void InitBatteries()
{
    GridTerminalSystem.GetBlocksOfType<IMyBatteryBlock>(batteryBlocks);

    if (batteryBlocks.Count > 0)
    {
        batteryBlocks.RemoveAll(x => x.IsSameConstructAs(Me) == false);
    }
}

public void InitThrust()
{
    GridTerminalSystem.GetBlocksOfType<IMyThrust>(thrusterBlocks);

    if (thrusterBlocks.Count > 0)
    {
        thrusterBlocks.RemoveAll(x => x.IsSameConstructAs(Me) == false);
    }
}

public void InitTanks()
{
    GridTerminalSystem.GetBlocksOfType<IMyGasTank>(tankBlocks);

    if (tankBlocks.Count > 0)
    {
        tankBlocks.RemoveAll(x => x.IsSameConstructAs(Me) == false);
    }
}

public void InitAntennas()
{
    GridTerminalSystem.GetBlocksOfType<IMyRadioAntenna>(antennas);

    if (antennas.Count > 0)
    {
        antennas.RemoveAll(x => x.IsSameConstructAs(Me) == false);
    }
}

public void InitOreDetectors()
{
    GridTerminalSystem.GetBlocksOfType<IMyOreDetector>(oreDetectors);

    if (oreDetectors.Count > 0)
    {
        oreDetectors.RemoveAll(x => x.IsSameConstructAs(Me) == false);
    }
}