public Program()
{
    Runtime.UpdateFrequency = UpdateFrequency.Update100;
    InitScreens();
    InitComp();
    InitContainers();
    InitAssemblers();
}

public string TAG = "[Base] ";

public void Main(string argument, UpdateType Update100)
{
    FindItemAmount();
    CheckItemAmount();
    ReadSettings();
}

public abstract class Item
{
    public string Name { get; set; }
    public string Id { get; set; }
    public string DefinitionId { get; set; }
    public int Amount { get; set; }
    public int MinAmount { get; set; }
    public MyItemType ItemType { get; set; }
    public MyDefinitionId Blueprint { get; set; }
}

public class Component : Item
{
    public Component(string name, string id, int amount, int minAmount)
    {
        Name = name;
        Id = id;
        Amount = amount;
        MinAmount = minAmount;
        DefinitionId = id;
    }

    public Component(string name, string id, string definitionId, int amount, int minAmount)
    {
        Name = name;
        Id = id;
        DefinitionId = definitionId;
        Amount = amount;
        MinAmount = minAmount;
    }
}

public class Ammo : Item
{
    public Ammo(string name, string id, string definitionId, int amount, int minAmount)
    {
        Name = name;
        Id = id;
        DefinitionId = definitionId;
        Amount = amount;
        MinAmount = minAmount;
    }
}


Dictionary<string, Item> CompList = new Dictionary<string, Item>();

public void InitComp()
{
    // CHANGE MINIMUN VALUES HERE BY CHANGING THE LAST NUMBER
    CompList.Add("SteelPlate", new Component("Steel Plates", "SteelPlate", 0, 10000));
    CompList.Add("InteriorPlate", new Component("Interior Plates", "InteriorPlate", 0, 5000));
    CompList.Add("Construction", new Component("Construction Components", "Construction", "ConstructionComponent", 0, 5000));
    CompList.Add("Display", new Component("Displays", "Display", 0, 500));
    CompList.Add("Computer", new Component("Computers", "Computer", "ComputerComponent", 0, 5000));
    CompList.Add("Girder", new Component("Girders", "Girder", "GirderComponent", 0, 500));
    CompList.Add("LargeTube", new Component("Large Steel Tubes", "LargeTube", 0, 1000));
    CompList.Add("SmallTube", new Component("Small Steel Tubes", "SmallTube", 0, 1000));
    CompList.Add("MetalGrid", new Component("Metal Grids", "MetalGrid", 0, 500));
    CompList.Add("Motor", new Component("Motors", "Motor", "MotorComponent", 0, 1000));
    CompList.Add("Thrust", new Component("Thruster Comp.", "Thrust", "ThrustComponent", 0, 0));
    CompList.Add("BulletproofGlass", new Component("Bulletproof Glass", "BulletproofGlass", 0, 500));
    CompList.Add("PowerCell", new Component("Power Cells", "PowerCell", 0, 100));
    CompList.Add("SolarCell", new Component("Solar Cells", "SolarCell", 0, 0));
    CompList.Add("Detector", new Component("Detector Components", "Detector", "DetectorComponent", 0, 0));
    CompList.Add("Medical", new Component("Medical Comp.", "Medical", "MedicalComponent", 0, 0));
    CompList.Add("RadioCommunication", new Component("Radio-Communication Comp.", "RadioCommunication", "RadioCommunicationComponent", 0, 0));
    CompList.Add("Reactor", new Component("Reactor Comp.", "Reactor", "ReactorComponent", 0, 0));
    CompList.Add("GravityGenerator", new Component("Gravity Comp.", "GravityGenerator", "GravityGeneratorComponent", 0, 0));
    CompList.Add("Superconductor", new Component("Superconductors", "Superconductor", 0, 0));
    CompList.Add("Explosives", new Component("Explosives", "Explosives", "ExplosivesComponent", 0, 500));

    // Ammo 
    CompList.Add("Missile200mm", new Ammo("Rocket", "Missile200mm", "Position0100_Missile200mm", 0, 0));
    CompList.Add("NATO_25x184mm", new Ammo("Gatling Ammo Box", "NATO_25x184mm", "Position0080_NATO_25x184mmMagazine", 0, 0));
    CompList.Add("LargeCalibreAmmo", new Ammo("Artillery Shell", "LargeCalibreAmmo", "Position0120_LargeCalibreAmmo", 0, 0));
    CompList.Add("MediumCalibreAmmo", new Ammo("Assault Cannon Shell", "MediumCalibreAmmo", "Position0110_MediumCalibreAmmo", 0, 0));
    CompList.Add("AutocannonClip", new Ammo("Autocannon Mag.", "AutocannonClip", "Position0090_AutocannonClip", 0, 0));


    string myObjectBuilderType;
    foreach (var item in CompList.Values)
    {
        myObjectBuilderType = "MyObjectBuilder_Component";
        
        if (item is Ammo)
            myObjectBuilderType = "MyObjectBuilder_AmmoMagazine";

        MyItemType itemType = new MyItemType(myObjectBuilderType, item.Id);
        string s = "MyObjectBuilder_BlueprintDefinition/" + item.DefinitionId;
        MyDefinitionId blueprint = MyDefinitionId.Parse(s);
        item.Blueprint = blueprint;
        item.ItemType = itemType;
    }
}

List<IMyTerminalBlock> Containers = new List<IMyTerminalBlock>();
public void InitContainers()
{
    string containerGroupName = "Base Component Containers";
    var containerGroup = GridTerminalSystem.GetBlockGroupWithName(containerGroupName);
    containerGroup.GetBlocks(Containers);

    Echo($"Initializing containers");

    foreach (var container in Containers)
    {
        Echo($"container -{container.CustomName}- found");
    }

    if (Containers.Count > 0)
        Echo($"{Containers.Count} containers found");
    else
        Echo($"No containers found");
}


List<IMyTerminalBlock> Assemblers = new List<IMyTerminalBlock>();
public void InitAssemblers()
{
    Echo("Initializing assemblers");
    string assemblerGroupName = "Base Assemblers";
    var assemblerGroup = GridTerminalSystem.GetBlockGroupWithName(assemblerGroupName);
    assemblerGroup.GetBlocks(Assemblers);
    Containers.AddRange(Assemblers);
}

public void InitTurrets()
{
    Echo("Initializing turrets");
    List<IMyLargeTurretBase> turrets = new List<IMyLargeTurretBase>();
    GridTerminalSystem.GetBlocksOfType<IMyLargeTurretBase>(turrets, x => x.IsSameConstructAs(Me));
    Containers.AddRange(turrets);
}


List<IMyTerminalBlock> Screens = new List<IMyTerminalBlock>();
public void InitScreens()
{
    /*
    string screenGroupName = "Base Output Screens";
    var screenGroup = GridTerminalSystem.GetBlockGroupWithName(screenGroupName);
    screenGroup.GetBlocks(Screens);
    */
    var compListScreen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName($"{TAG}Component List Screen");
    var compQueueScreen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName($"{TAG}Component Queue Screen");
    Screens.Add(compListScreen);
    Screens.Add(compQueueScreen);

    Echo($"Initializing screens");

    foreach (var screen in Screens)
    {
        Echo($"Screen -{screen.CustomName}- found");
    }

    if (Screens.Count > 0)
        Echo($"{Screens.Count} screens found");
    else
        Echo($"No screens found");
}

/* Output function
*   Input: Message to printed as a string
*   Append: Boolean determinging whether to override current message
*   ScreenNumber: Which screen to print to. 0 for list of components, 1 for queue screen
*/
public void PrintToScreen(string input, bool append, int screenNumber)
{
    IMyTextPanel outputScreen = (IMyTextPanel)Screens[screenNumber];
    outputScreen.WriteText(input + "\n", append);
}

// Finds all amounts of items in the base cargo containers and assemblers
public void FindItemAmount()
{
    int amount = 0;

    foreach (var comp in CompList.Values) { comp.Amount = 0; }

    foreach (var block in Containers)
    {
        var inventory = block.GetInventory();
        if (block.CustomName.Contains("Assembler"))
        {
            IMyProductionBlock assembler = (IMyProductionBlock)block;
            inventory = assembler.OutputInventory;
        }

        foreach (var item in CompList.Values)
        {
            amount = (int)inventory.GetItemAmount(item.ItemType);
            item.Amount += amount;
        }
    }

    PrintToScreen("Total item amount including Assemblers:", false, 0);
    foreach (var item in CompList.Values)
    {
        PrintToScreen($"{item.Name}: {item.Amount}", true, 0);
    }
}

// Checks the amount of components compared to their minimun amount and queues new components
public void CheckItemAmount()
{
    List<MyProductionItem> queue = new List<MyProductionItem>();
    Dictionary<string, int> queueAmount = new Dictionary<string, int>();
    int assemblerCount = Assemblers.Count();
    for (int i = 0; i < assemblerCount; i++)
    {
        IMyProductionBlock assembler = (IMyProductionBlock)Assemblers[i];
        assembler.GetQueue(queue);
        foreach (var item in queue)
        {
            string[] name = item.BlueprintId.ToString().Split('/');

            if (queueAmount.ContainsKey(name[1]))
                queueAmount[name[1]] += (int)item.Amount;
            else
                queueAmount.Add(name[1], (int)item.Amount);
        }
    }
    PrintToScreen($"\nCurrently queued components:", false, 1);
    foreach (var item in queueAmount)
    {
        PrintToScreen($"{item.Key}: {item.Value}", true, 1);
    }

    PrintToScreen($"\nComponents below minimun amount:", true, 1);
    foreach (var comp in CompList.Values)
    {
        int itemQueueAmount = queueAmount.ContainsKey(comp.DefinitionId) ? queueAmount[comp.DefinitionId] : 0;
        if ((comp.Amount + itemQueueAmount) < comp.MinAmount)
        {
            int diff = comp.MinAmount - (comp.Amount + itemQueueAmount);
            PrintToScreen($"{comp.Name} by {diff}", true, 1);

            IMyAssembler assembler = FindAvailableAssembler();

            assembler.AddQueueItem(comp.Blueprint, (MyFixedPoint)diff);
        }
    }
}

// Function for finding assembler with lowest queue
public IMyAssembler FindAvailableAssembler()
{
    IMyAssembler assembler = null;
    List<MyProductionItem> queue = new List<MyProductionItem>();
    int prevMax = 0;
    int index = 0;
    int assemblerCount = Assemblers.Count();
    for (int i = 0; i < assemblerCount; i++)
    {
        assembler = (IMyAssembler)Assemblers[i];
        if (assembler.IsQueueEmpty)
            return assembler;

        assembler.GetQueue(queue);
        int currentMax = 0;
        foreach (var item in queue)
        {
            currentMax += (int)item.Amount;
        }

        if (currentMax < prevMax || prevMax == 0)
        {
            prevMax = currentMax;
            index = i;
        }
    }

    return (IMyAssembler)Assemblers[index];
}

/* Function for reading settings from setup script.
*  Attempts to find setup block named "Base Component Setup" and read settings from customdata property  
*/
public void ReadSettings()
{
    string settingsBlockName = $"{TAG}Base Component Setup";
    if (GridTerminalSystem.GetBlockWithName(settingsBlockName) != null)
    {
        var settingsBlock = GridTerminalSystem.GetBlockWithName(settingsBlockName);
        string readString = settingsBlock.CustomData;

        if (readString != null)
        {
            Echo("Reading settings");
            string[] storage = readString.Split(';');
            int lineCount = storage.Count();
            for (int i = 0; i < lineCount; i++)
            {
                string[] item = storage[i].Split(',');
                CompList[item[0]].MinAmount = Int32.Parse(item[1]);
            }
        }
        else
        {
            Echo("Storage settings not found");
        }
    }
    else
    {
        Echo("Settings block not found");
    }
}