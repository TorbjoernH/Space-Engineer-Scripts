public Program()
{
    Setup();
    PrintAll();
}

public void Save()
{
    
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
    public int MinAmount {get; set;}
    public bool Selected {get; set;}
    
    public Comp(string name, int minAmount, bool selected)
    {
        Name = name;
        MinAmount = minAmount;
        Selected = selected;
    }
}

public List<Comp> CompMinAmount = new List<Comp>();
public void Setup()
{
    CompMinAmount.Add(new Comp("Steel Plates", 0, true));
    CompMinAmount.Add(new Comp("Interior Plates", 0, false)); 
    CompMinAmount.Add(new Comp("Construction Components", 0, false)); 
    CompMinAmount.Add(new Comp("Displays", 0, false));
    CompMinAmount.Add(new Comp("Computers", 0, false));
    CompMinAmount.Add(new Comp("Girders", 0, false)); 
    CompMinAmount.Add(new Comp("Large Steel Tubes", 0, false));                                        
    CompMinAmount.Add(new Comp("Small Steel Tubes", 0, false)); 
    CompMinAmount.Add(new Comp("Metal Grids", 0, false));
    CompMinAmount.Add(new Comp("Motors", 0, false)); 
    CompMinAmount.Add(new Comp("Thruster Components", 0, false)); 
    CompMinAmount.Add(new Comp("Bulletproof Glass", 0, false));
    CompMinAmount.Add(new Comp("Power Cells", 0, false));                                          
    CompMinAmount.Add(new Comp("Solar Cells", 0, false));
    CompMinAmount.Add(new Comp("Detector Components", 0, false));
    CompMinAmount.Add(new Comp("Medical Components", 0, false));
    CompMinAmount.Add(new Comp("Radio-Communication Comp.", 0, false));                                           
    CompMinAmount.Add(new Comp("Reactor Components", 0, false));
    CompMinAmount.Add(new Comp("Gravity Components", 0, false));
    CompMinAmount.Add(new Comp("Superconductors", 0, false));
}

public void Output(string text, bool append)
{
    IMyTextPanel screen = (IMyTextPanel)GridTerminalSystem.GetBlockWithName("Setup Screen");
    screen.WriteText(text + "\n", append);
}

public enum Controls
{
    Up, Down, Reset, Save,
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
    Output("Component:         Minimum amount:", false);

    int length = 35;
    foreach (var comp in CompMinAmount)
    {
        if (comp.Selected)
            Output($"{("-->"+comp.Name+"<--").PadRight(length).Substring(0, length)}{comp.MinAmount}", true);
        else
            Output($"{comp.Name.PadRight(length).Substring(0, length)}{comp.MinAmount}", true);
            
    }
}

public void SaveSettings()
{
    Echo("Saving Settings");

    var self = GridTerminalSystem.GetBlockWithName(Me.CustomName);
    string[] storage = new string[CompMinAmount.Count];
    for(int i; i<CompMinAmount.Count; i++;)
    {
        string[i] = $"{CompMinAmount[i].Name},{CompMinAmount[i].MinAmount}" ;
    }

    string.Join(';', storage);
    self.CustomData = storage;
}