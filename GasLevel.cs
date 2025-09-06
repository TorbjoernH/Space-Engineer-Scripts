public namespace GasLevel
{
    
    
    public Program()
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update100;
        SetupTanks();
    }
    
    public void Main(string argument, UpdateType Update100)
    {
        GasInfo o2Info = GetTankLevel(OXYGEN_TANKS);
        GasInfo hydrogenInfo = GetTankLevel(HYDROGEN_TANKS);
        PrintOutput(o2Info, hydrogenInfo);


      
    }

    List<IMyGasTank> OXYGEN_TANKS = new List<IMyGasTank>();
    List<IMyGasTank> HYDROGEN_TANKS = new List<IMyGasTank>();
    public void SetupTanks()
    {
        GridTerminalSystem.GetBlocksOfType<IMyGasTank>(OXYGEN_TANKS, x => x.IsSameConstructAs(Me) 
                                                                                                 && x.DisplayNameText.Contains("Oxygen"));
        GridTerminalSystem.GetBlocksOfType<IMyGasTank>(HYDROGEN_TANKS, x => x.IsSameConstructAs(Me) 
                                                                                                 && x.DisplayNameText.Contains("Hydrogen"));
    
        
    }
    
    public class GasInfo
    {
        public float GasLevel;
        public float MaxCapacity;

        public GasInfo(float gasLevel, float maxCapacity)
        {
            GasLevel = gasLevel;
            MaxCapacity = maxCapacity;
        }
    }

    public GasInfo GetTankLevel(List<IMyGasTank> tanks)
    {
        float gasLevel = 0;
        float maxCapacity = 0;

        foreach (IMyGasTank tank in tanks)
        {
            Echo(tank.CustomName);
            maxCapacity += tank.Capacity;
            gasLevel += (float)(tank.Capacity * tank.FilledRatio);
        }

        return new GasInfo(gasLevel, maxCapacity);
    }

    public void PrintOutput(GasInfo oxygen, GasInfo hydrogen)
    {
        IMyTextPanel screen = GridTerminalSystem.GetBlockWithName("testpanel") as IMyTextPanel;
        int barLength = 50;

        float oxygenLevel = oxygen.GasLevel;
        float maxOxygen = oxygen.MaxCapacity;

        float hydrogenLevel = hydrogen.GasLevel;
        float maxHydrogen = hydrogen.MaxCapacity;

        int oxygenPercentage = maxOxygen > 0 ? (int)((oxygenLevel / maxOxygen) * barLength) : 0;
        int hydrogenPercentage = maxHydrogen > 0 ? (int)((hydrogenLevel / maxHydrogen) * barLength) : 0;
        
        string oxygenOutput = "[" + new string('|', oxygenPercentage) + new string('\'', barLength - oxygenPercentage) + "]";
        string hydrogenOutput = "[" + new string('|', hydrogenPercentage) + new string('\'', barLength - hydrogenPercentage) + "]";
        
        screen.WriteText($"Total Oxygen \n", false);
        screen.WriteText(oxygenOutput, true);
        
        screen.WriteText($"\nTotal Hydrogen \n", true);
        screen.WriteText(hydrogenOutput, true);
    }
}