using Godot;
using Godot.Collections;

public class PlayerStatsResource : Resource
{
    [Export] public Array<Enums.Dinos> dinosUnlocked = new Array<Enums.Dinos>() { Enums.Dinos.Mega, Enums.Dinos.Warrior };
    [Export] public Array<Enums.Genes> genesFound = new Array<Enums.Genes>() { Enums.Genes.Fire };

    public void SaveResource()
    {
        ResourceSaver.Save(this.ResourcePath, this);
    }

}