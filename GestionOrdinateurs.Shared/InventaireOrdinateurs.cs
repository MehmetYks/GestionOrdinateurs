namespace GestionOrdinateurs.Shared;

public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs{get; set;} = new();

    public Ordinateur? this[int index]{
        get{
            return Ordinateurs.FirstOrDefault(o => o.Id == index);
        }
    }

    public Ordinateur? this[string reference]
    {
        get
        {
            return Ordinateurs.FirstOrDefault(o => o.Nom == reference);
        }
    }
}