public namespace GasLevel // Don't Copy the Namespace
{
    int counter = 0;
    string ICE_SCREEN_NAME = "Ice Screen"; // IF YOU ARE USING A LCD SCREEN, TYPE THE NAME OF THE SCREEN HERE, OTHERWISE LEAVE EMPTY
    int SCREEN_NUMBER = 0; // IF YOU ARE USING AN LCD DISPLAY IN COCKPIT OR OTHER BLOCK, CHANGE THE SCREENNUMBER HERE
    string COCKPIT_NAME = "Cab Cockpit"; // NAME OF COCKPIT OR BLOCK, WHERE THE DISPLAY IS
    string ORE_SCREEN_NAME = "Ore Screen";
    string INGOT_SCREEN_NAME = "Ingot Screen";

    IMyTextSurface ICE_SCREEN = null;
    IMyTextSurface ORE_SCREEN = null;
    IMyTextSurface INGOT_SCREEN = null;
    List<IMyTerminalBlock> INVENTORY_LIST = new List<IMyTerminalBlock>();
    List<IMyGasTank> OXYGEN_TANKS = new List<IMyGasTank>();
    List<IMyGasTank> HYDROGEN_TANKS = new List<IMyGasTank>();
    Dictionary<string, Resource> RESOURCES = new Dictionary<string, Resource>();
    public Program()
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update100;
        SetupTanks();
        SetupResources();
        INVENTORY_LIST = SetupInventories();
        ICE_SCREEN = ICE_SCREEN_NAME.Length > 0 ? SetupScreen(ICE_SCREEN_NAME) : SetupScreen(COCKPIT_NAME, SCREEN_NUMBER);
        ORE_SCREEN = SetupScreen(ORE_SCREEN_NAME);
        INGOT_SCREEN = SetupScreen(INGOT_SCREEN_NAME);
    }

    public void Main(string argument, UpdateType Update100)
    {
        counter++;
        GasInfo o2Info = GetTankLevel(OXYGEN_TANKS);
        GasInfo hydrogenInfo = GetTankLevel(HYDROGEN_TANKS);
        int iceLevel = GetItemAmount("MyObjectBuilder_Ore", RESOURCES["Ice"].Id, INVENTORY_LIST);
        PrintOutput(o2Info, hydrogenInfo, iceLevel, ICE_SCREEN);
        PrintOres(ORE_SCREEN);
        PrintIngots(INGOT_SCREEN);


        if (counter % 10 == 0)
        {
            SetupTanks();
        }

    }

    public List<IMyTerminalBlock> SetupInventories()
    {
        List<IMyTerminalBlock> temp = new List<IMyTerminalBlock>();
        GridTerminalSystem.GetBlocksOfType(temp, x => x.HasInventory && x.IsSameConstructAs(Me));
        
        return temp;
    }
    public IMyTextSurface SetupScreen(string screenName)
    {
        return (IMyTextSurface)GridTerminalSystem.GetBlockWithName(screenName);
    }

    public IMyTextSurface SetupScreen(string cockpitName, int screenNumber)
    {
        IMyCockpit cockpit = (IMyCockpit)GridTerminalSystem.GetBlockWithName(cockpitName);
        return (IMyTextSurface)cockpit.GetSurface(screenNumber);
    }


    public void SetupTanks()
    {
        GridTerminalSystem.GetBlocksOfType<IMyGasTank>(OXYGEN_TANKS, x => x.IsSameConstructAs(Me)
                                                                                                 && x.DisplayNameText.Contains("Oxygen"));
        GridTerminalSystem.GetBlocksOfType<IMyGasTank>(HYDROGEN_TANKS, x => x.IsSameConstructAs(Me)
                                                                                                 && x.DisplayNameText.Contains("Hydrogen"));


    }

    public void SetupResources()
    {
        RESOURCES.Add("Iron",      new Resource("Iron", "Iron", 0, 0));
        RESOURCES.Add("Nickel",    new Resource("Nickel", "Nickel", 0, 0));
        RESOURCES.Add("Cobalt",    new Resource("Cobalt", "Cobalt", 0, 0));
        RESOURCES.Add("Silicon",   new Resource("Silicon", "Silicon", 0, 0));
        RESOURCES.Add("Magnesium", new Resource("Magnesium", "Magnesium", 0, 0));
        RESOURCES.Add("Silver",    new Resource("Silver", "Silver", 0, 0));
        RESOURCES.Add("Gold",      new Resource("Gold", "Gold", 0, 0));
        RESOURCES.Add("Platinum",  new Resource("Platinum", "Platinum", 0, 0));
        RESOURCES.Add("Uranium",   new Resource("Uranium", "Uranium", 0, 0));
        RESOURCES.Add("Stone", new Resource("Stone", "Stone", 0, 0));
        RESOURCES.Add("Ice",   new Resource("Ice", "Ice", 0, 0));
    }
    public class Resource
    {
        public string Name { get; }
        public string Id { get; }
        public int OreAmount { get; set; }
        public int IngotAmount { get; set; }
        public Resource(string name, string id, int oreAmount, int ingotAmount)
        {
            Name = name;
            Id = id;
            OreAmount = oreAmount;
            IngotAmount = ingotAmount;
        }
    };
    public class GasInfo
    {
        public double GasLevel { get; set; }
        public double MaxCapacity { get; set; }

        public GasInfo(double gasLevel, double maxCapacity)
        {
            GasLevel = gasLevel;
            MaxCapacity = maxCapacity;
        }
    };

    public GasInfo GetTankLevel(List<IMyGasTank> tanks)
    {
        double gasLevel = 0;
        double maxCapacity = 0;

        foreach (IMyGasTank tank in tanks)
        {
            maxCapacity += tank.Capacity;
            gasLevel += tank.Capacity * tank.FilledRatio;
        }

        return new GasInfo(gasLevel, maxCapacity);
    }

    public int GetItemAmount(string typeId, string subTypeId, List<IMyTerminalBlock> inventoryBlocks)
    {
        int amount = 0;
        MyItemType ice = new MyItemType(typeId, subTypeId);
        foreach (var block in inventoryBlocks)
        {
            for (int i = 0; i < block.InventoryCount; i++)
            {
                var inv = block.GetInventory(i);
                amount += inv.GetItemAmount(ice).ToIntSafe();
            }
        }

        return amount;
    }

    public void PrintOutput(GasInfo oxygen, GasInfo hydrogen, int iceLevel, IMyTextSurface screen)
    {
        int barLength = 75;

        string edgeBar = new string('=', barLength);

        double oxygenLevel = oxygen.GasLevel;
        double maxOxygen = oxygen.MaxCapacity;

        double hydrogenLevel = hydrogen.GasLevel;
        double maxHydrogen = hydrogen.MaxCapacity;

        int oxygenPercentage = maxOxygen > 0 ? (int)((oxygenLevel / maxOxygen) * barLength) : 0;
        int hydrogenPercentage = maxHydrogen > 0 ? (int)((hydrogenLevel / maxHydrogen) * barLength) : 0;

        string oxygenOutput = "[" + new string('|', oxygenPercentage) + new string('\'', barLength - oxygenPercentage) + "]";
        string hydrogenOutput = "[" + new string('|', hydrogenPercentage) + new string('\'', barLength - hydrogenPercentage) + "]";

        screen.WriteText(edgeBar + "\n", false);

        screen.WriteText($"Oxygen:    {(int)oxygen.GasLevel} L / {(int)oxygen.MaxCapacity} L\n", true);
        screen.WriteText(oxygenOutput, true);

        screen.WriteText($"\nHydrogen:     {(int)hydrogen.GasLevel} L /{(int)hydrogen.MaxCapacity} L\n", true);
        screen.WriteText(hydrogenOutput, true);

        screen.WriteText($"\nIce:   {iceLevel} kg", true);

        screen.WriteText("\n" + edgeBar + "\n", true);
    }

    public void PrintOres(IMyTextSurface screen)
    {
        int barLength = 75;
        string edgeBar = new string('=', barLength);
        string typeId = "MyObjectBuilder_Ore";

        screen.WriteText(edgeBar + "\n", false);
        screen.WriteText("ORES: \n", true);
        foreach (var ore in RESOURCES.Values)
        {
            ore.OreAmount = GetItemAmount(typeId, ore.Id, INVENTORY_LIST);


            screen.WriteText($"\n{ore.Name}:     {ore.OreAmount} kg", true);
            
        }
        screen.WriteText("\n" + edgeBar + "\n", true);
    }

public void PrintIngots(IMyTextSurface screen)
    {
        int barLength = 75;
        string edgeBar = new string('=', barLength);
        string typeId = "MyObjectBuilder_Ingot";

        screen.WriteText(edgeBar + "\n", ORE_SCREEN_NAME == INGOT_SCREEN_NAME);
        screen.WriteText("INGOTS: \n", true);
        foreach (var ingot in RESOURCES.Values)
        {
            if (ingot.Name == "Ice") { continue; }

            ingot.IngotAmount = GetItemAmount(typeId, ingot.Id, INVENTORY_LIST);

            screen.WriteText($"\n{ingot.Name}:     {ingot.IngotAmount} kg", true);

        }
        screen.WriteText("\n" + edgeBar + "\n", true);
    }
}