namespace Game.Cloud
{
    public class JsonWriter
    {
        
    }
    
    // En class som lokalt holder på hvor mange baner er klaret for hver bane. 
    // Klassen opdateres lokalt når en base klares og opdaterer filen i cloud. 
    // Det gør den ved at generer en JSON baseret på sine fields, som den gemmer. 
    // Når en spiller logger ind, skal klassen starte med at hente den korrekte save json file i cloud.
    // Filen læses, og dataene læses ind i den lokale klasse. 
    // Lav den lokale klasse som en monobehaviour inkl. et editor-script. 
    // Editor script bruges til at teste generation af JSON-fil og læsning af selv samme, samt upload af data.
}