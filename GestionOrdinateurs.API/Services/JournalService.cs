using System.Formats.Asn1;

namespace GestionOrdinateurs.API;

public class JournalService
{
    public void Ecrire(string message)
    {

        // J'utilise un chemin relatif donc les logs vont dans le dossier d'execution des tests pendant les tests et dans le dossier Logs du projet quand je lance l'API.
        Directory.CreateDirectory("Logs/Reparation_Logs");
        using var writer = new StreamWriter("Logs/Reparations_Logs/reparations.log", append: true);

        writer.WriteLine(message);
    }

    public void EcrireOrdinateur(string message)
    {

        // J'utilise un chemin relatif donc les logs vont dans le dossier d'execution des tests pendant les tests et dans le dossier Logs du projet quand je lance l'API.
        Directory.CreateDirectory("Logs/Ordinateurs");
        using var writer = new StreamWriter("Logs/Ordinateurs/ordinateurs.log", append: true);

        writer.WriteLine(message);
    }

    
}