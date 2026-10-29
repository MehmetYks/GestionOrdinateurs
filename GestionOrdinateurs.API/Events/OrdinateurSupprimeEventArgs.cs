namespace GestionOrdinateurs.API.Events;


public class OrdinateurSupprimeEventArgs : EventArgs
{
    public string Nom {get; set;}
}