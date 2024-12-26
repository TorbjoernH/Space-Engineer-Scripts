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
    
    // if (connector.IsConnected)
    // {
    //     OnConnect();
    // }
    // else
    // {
    //     OnDisconnect();
    // }


    /* Proposed change to make it work with an event block
       To make this work, have an event block trigger on connector connect
       then make the first slot in the event block tool bar call the programmable block with argument "connect"
       and the second slot call the programmable block with the argument "disconnect"
    */
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
        // foreach (IMyBatteryBlock battery in batteryBlocks)
        // {
        //     if (battery.IsSameConstructAs(Me)) { continue; }

        //     batteryBlocks.Remove(battery);
        // }

        /* As far as I have read/understand, manipulating a collection you are iterating over using a foreach loop
           is either considered very bad practice or not possible at all, possible the former as it script worked so far.

           Proposed change does the same as the foreach loop, but instead uses the list method RemoveAll,
           which removes all members of the list that meets the predicate. 
           Probably needs testing first
        */

        batteryBlocks.RemoveAll(IsSameConstructAs);
    }
}

public void InitThrust()
{
    GridTerminalSystem.GetBlocksOfType<IMyThrust>(thrusterBlocks);

    if (thrusterBlocks.Count > 0)
    {
        /*
        foreach (IMyThrust thrust in thrusterBlocks)
        {
            if (thrust.IsSameConstructAs(Me)) { continue; }

            thrusterBlocks.Remove(thrust);
        }
        */
        thrusterBlocks.RemoveAll(IsSameConstructAs);
    }
}

public void InitTanks()
{
    GridTerminalSystem.GetBlocksOfType<IMyGasTank>(tankBlocks);

    if (tankBlocks.Count > 0)
    {
        /*
        foreach (IMyGasTank tank in tankBlocks)
        {
            if (tank.IsSameConstructAs(Me)) { continue; }

            tankBlocks.Remove(tank);
        }
        */
        tankBlocks.RemoveAll(IsSameConstructAs);
    }
}

public bool IsSameConstructAs(var block)
{
    return block.IsSameConstructAs(Me);
}