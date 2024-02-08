
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Klasse Programm wird definiert, dass ist der Kopf für den Haupteil des Codes was bedeutet dass es notwendig ist
class Program
{
    // Hauptteil des Codes das heißt es ist das erste was ausgegeben wird wenn das Programm gestartet wird. 
    static void Main(string[] args)
    {
        // Einlesen des Raumplans
        Dictionary<int, int> raumPlan = new Dictionary<int, int>(); // Dictionary wird erstellt damit der Raumplan gespeichert wird
        string[] raumPlanZeilen = File.ReadAllLines("raumplan.csv"); // Wird durchgegangen und in Array wie dargestellt angegeben
        foreach (string zeile in raumPlanZeilen) // Jede Zeile von Raumplan wird durchlaufen
        {
            string[] teile = zeile.Split('\t'); // Durch T werden die Zeilen gespaltet in kleinere Teile
            int vonRaum = teile[0] == "E" ? -1 : int.Parse(teile[0]); // Der erste Raum wird durchgegangen und falls E rauskommt wird es in -1 gesetzt und wenn nicht dann zu einem Integer
            int zuRaum = int.Parse(teile[1]); // Der Zielraum wird durchgegangen
            raumPlan[vonRaum] = zuRaum; // von und zu raum werden von dictionary aufgenommen 
        }

        // Einlesen von Zeitprotokoll
        string[] logZeilen = File.ReadAllLines("zeiten.csv"); // Die Zeilen von zeiten.csv werden durchgegangen und auch im Array gespeichert
        List<int[]> raumZaehlungenListe = new List<int[]>(); // Neue Liste wird erstellt um die Raumzählungen zu zählen
        foreach (string zeile in logZeilen) // Von dem Zeitprotokoll werden alle Zeilen durchgegangen 
        {
            if (!string.IsNullOrWhiteSpace(zeile)) // Hier wird überprüft ob die Zeile leer ist 
            {
                string[] teile = zeile.Split(';'); // Zeile wird wegen dem Semikolon gesplitet in kleinere Teile
                if (teile.Length >= 2) // Überprüft ob genug von den Teilen da sind
                {
                    int[] raumZaehlungen = teile.Skip(1).Select(int.Parse).ToArray(); // Das Zählen der Räume wird aus den Teilen herausgenommen und zu einem Integer Array gemacht (enthält nur Elemente vom Typ int) 
                    raumZaehlungenListe.Add(raumZaehlungen); // Die Zählungen werden der Liste geaddet also hinzugefügt
                }
            }
        }

        // Durchlauf des Protokolls, um Eindringlinge zu finden
        for (int i = 1; i < raumZaehlungenListe.Count; i++) // Liste mit den Zählungen welche hinzugefügt wurden werden durchgegangen
        {
            int[] vorherigeRaumZaehlungen = raumZaehlungenListe[i - 1]; // Das Zählen von der Zeile davor wird einfach gesichert
            int[] aktuelleRaumZaehlungen = raumZaehlungenListe[i]; // Jetzt von der aktuellen 
            DateTime zeit = DateTime.Parse(logZeilen[i].Split(';')[0]); // Die Zeit aus der Zeile wird herausgenommen und zu einem DateTime gesetzt also zu einem Objekt

            if (PruefeAufEindringling(vorherigeRaumZaehlungen, aktuelleRaumZaehlungen)) // Methode wird aufgerufen um auf Eindringlinge zu prüfen
            {
                Console.WriteLine($"Alarm: Eindringling erkannt um {zeit}"); // Falls einer erkannt wird ausgegeben die Zeit wann es war 
            }
        }

        Console.ReadKey();
    }

    // Methode zur Überprüfung auf Eindringlinge
    static bool PruefeAufEindringling(int[] vorherigeRaumZaehlungen, int[] aktuelleRaumZaehlungen)
    {
        for (int i = 1; i < vorherigeRaumZaehlungen.Length; i++) // Durchlaufen von Zählen der Räume
        {
            // Überprüfen ob sich die Personenzahl im Raum geändert hat
            if (vorherigeRaumZaehlungen[i] > 0 && aktuelleRaumZaehlungen[i] > 0 && vorherigeRaumZaehlungen[i] != aktuelleRaumZaehlungen[i])
            {
                return true; // True soll ausgegeben werden wenn echt ein Eindrnigling erkannt wird
            }
        }

        return false; // False soll ausgegeben werden wenn keiner erkannt wurde 
    }
}