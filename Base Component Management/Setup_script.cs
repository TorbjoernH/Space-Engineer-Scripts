public Program()
{
    Setup();
    PrintAll();
}

public void Save()
{
    SaveSettings();
}

public void Main(string argument, UpdateType updateSource)
{
    int c = Int32.Parse(argument);
    Control((Controls)c);
    PrintAll();
}

public class Comp
{
    public string Name {get;}
    public string Id {get;}
    public int MinAmount {get; set;}
    public bool Selected {get; set;}
    
    public Comp(string name, string id, int minAmount, bool selected)
    {
        Name = name;
        Id = id;
        MinAmount = minAmount;
        Selected = selected;
    }
}

public List<Comp> CompMinAmount = new List<Comp>();
public void Setup()
{
    CompMinAmount.Add(new Comp("Steel Plates", "SteelPlate", 10000, true));
    CompMinAmount.Add(new Comp("Interior Plates", "InteriorPlate", 5000, false));
    CompMinAmount.Add(new Comp("Construction Components", "Construction", 5000, false));
    CompMinAmount.Add(new Comp("Displays", "Display", 500, false));
    CompMinAmount.Add(new Comp("Computers", "Computer", 5000, false));
    CompMinAmount.Add(new Comp("Girders", "Girder", 500, false));
    CompMinAmount.Add(new Comp("Large Steel Tubes", "LargeTube", 1000, false));
    CompMinAmount.Add(new Comp("Small Steel Tubes", "SmallTube", 1000, false));
    CompMinAmount.Add(new Comp("Metal Grids", "MetalGrid", 500, false));
    CompMinAmount.Add(new Comp("Motors", "Motor", 1000, false));
    CompMinAmount.Add(new Comp("Thruster Comp.", "Thrust", 0, false));
    CompMinAmount.Add(new Comp("Bulletproof Glass", "BulletproofGlass", 500, false));
    CompMinAmount.Add(new Comp("Power Cells", "PowerCell", 100, false));
    CompMinAmount.Add(new Comp("Solar Cells", "SolarCell", 0, false));
    CompMinAmount.Add(new Comp("Detector Components", "Detector", 0, false));
    CompMinAmount.Add(new Comp("Medical Comp.", "Medical", 0, false));
    CompMinAmount.Add(new Comp("Radio-Communication Comp.", "RadioCommunication", 0, false));
    CompMinAmount.Add(new Comp("Reactor Comp.", "Reactor", 0, false));
    CompMinAmount.Add(new Comp("Gravity Comp.", "GravityGenerator", 0, false));
    CompMinAmount.Add(new Comp("Superconductors", "Superconductor", 0, false));
}

public void Output(string text, bool append)
{
    IMyTextPanel screen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("[Base] Component Setup Screen");
    screen.WriteText(text + "\n", append);
}

public enum Controls
{
    Up, Down, Reset, Save, ButtonSetup
    Increase10 = 10,
    Increase100 = 100,
    Increase1000 = 1000,
    Increase10000 = 10000,
    Decrease10 = -10,
    Decrease100 = -100,
    Decrease1000 = -1000,
    Decrease10000 = -10000,
}

public void Increase(Comp comp, int amount)
{
    comp.MinAmount += amount;
}

public void Decrease(Comp comp, int amount)
{
    if ((comp.MinAmount + amount) < 0)
        comp.MinAmount = 0;
    else
        comp.MinAmount += amount;
}

public void Control(Controls c)
{
    int i = CompMinAmount.FindIndex(a => a.Selected == true);
    switch(c)
    {
        case Controls.Up:
        {
            
            CompMinAmount[i].Selected = false;
            if (i == 0)
                i = CompMinAmount.Count-1;
            else
                i--;
            CompMinAmount[i].Selected = true;
            break;
        }
        case Controls.Down:
        {
            
            CompMinAmount[i].Selected = false;
            if (i == (CompMinAmount.Count-1))
                i = 0;
            else
                i++;
            CompMinAmount[i].Selected = true;
            break;
        }
        case Controls.Reset:
            foreach (var comp in CompMinAmount)
            {
                comp.MinAmount = 0;
            }
            break;
        case Controls.Save:
        {
            SaveSettings();
            break;
        }
        case Controls.Increase10: case Controls.Increase100: case Controls.Increase1000: case Controls.Increase10000:
            Increase(CompMinAmount[i], (int)c);
            break;
        case Controls.Decrease10: case Controls.Decrease100: case Controls.Decrease1000: case Controls.Decrease10000:
            Decrease(CompMinAmount[i], (int)c);
            break;
            
    }
}

public void PrintAll()
{
    Output("Minimum amount:         Component:", false);

    int length = 35;
    foreach (var comp in CompMinAmount)
    {
        if (comp.Selected)
            Output($"{comp.MinAmount}{("-->"+comp.Name+"<--").PadLeft(length).Substring(0, length)}", true);
        else
            Output($"{comp.MinAmount }{comp.Name.PadLeft(length).Substring(0, length)}", true);
            
    }
}

public void SaveSettings()
{
    Echo("Saving Settings");

    var self = GridTerminalSystem.GetBlockWithName(Me.CustomName);
    string[] storage = new string[CompMinAmount.Count()];
    for(int i = 0; i<CompMinAmount.Count(); i++)
    {
        storage[i] = $"{CompMinAmount[i].Id},{CompMinAmount[i].MinAmount}" ;
    }

    string s = string.Join(";", storage);
    self.CustomData = s;
}

public void ReadSettings()
{
    //var settingsBlock = GridTerminalSystem.GetBlockWithName(settingsBlockName);
    string readString = Me.CustomData;

    if (readString != null)
    {
        Echo("Reading settings");
        string[] storage = readString.Split(';');
        int lineCount = storage.Count();
        for (int i = 0; i < lineCount; i++)
        {
            string[] item = storage[i].Split(',');
            CompMinAmount[i].MinAmount = Int32.Parse(item[1]);
        }
    }
    else
    {
        Echo("Storage settings not found");
    }
}