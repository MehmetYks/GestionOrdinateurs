using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionOrdinateurs.Shared;

[Table("Ordinateur")]
public class Ordinateur
{
    public int Id {get; set;}
    public string Nom {get; set;}
    public EtatOrdinateur Etat {get; set;}

    [DefaultValue(0)]
    public int Stock{get; set;}

    [DefaultValue(0)]
    public decimal Prix{get; set;}

    [DefaultValue(false)]
    public bool Vendu{get; set;}

    public Caracteristiques Caracteristiques{get; set;}

    public List<Reparation> Reparations{get; set;} = new();

    /*
    public Ordinateur(int id, string nom, string marque, int ram, string processeur, EtatOrdinateur etat)
    {
        Id = id;
        Nom = nom;
        Marque = marque;
        RAM = ram;
        Processeur = processeur;
        Etat = etat;
    }

    public Ordinateur()
    {
        
    }
    */

    //Met à jour les info modifiables d'un ordi (la méthode à été mise à jour par rapport aux caractéristiques)
    public void ModifPc(string nom, EtatOrdinateur etat, int stock, decimal prix, Caracteristiques caracteristiques)
    {
        Nom = nom;
        Etat = etat;
        Stock = stock;
        Prix = prix;
        Caracteristiques = caracteristiques;
    }
    
    
    //Pour retourner le nom de l'état sinon il retourne un chiffre, si l'état n'existe pas il affiche une chaîne de charactère vide
    public string EtatEnTexte()
    {
        switch (Etat)
        {
            case EtatOrdinateur.EnService: return "En service";
            case EtatOrdinateur.EnPanne: return "En panne";
            case EtatOrdinateur.EnMaintenance: return "En maintenance";
            case EtatOrdinateur.HorsService: return "Hors Service";
            case EtatOrdinateur.Lent: return "Lent";
            case EtatOrdinateur.Reparable: return "Réparable";
            default: return "";
        }
    }
    

}
