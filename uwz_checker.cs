using System;
using System.Threading;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace wgetImage
{
    /// <summary>
    /// Zweck dieses Programms:
    ///     Ausgabe der aktuellen Unwettersituation fuer ein Gebiet (Daten von www.uwz.at)
    /// </summary>
    class Program
    {
        // Zwischendatei (Download der Grafik von www.uwz.at)
        private const string filename = @"C:\temp\warn.png";
        // Die URL zu der Grafik auf www.uwz.at
        private const string url = "http://www.uwz.at/at/de/incoming/maps_warnings/at/AT_all_warnings_484x242.png";

        static void Main(string[] args)
        {
            // Es soll periodisch aktualisiert werden
            Timer t = new Timer(performCheck, null, 0, 15000);
            
            Console.ReadKey();
        }

        /// <summary>
        /// Diese Methode laedt die Grafik der aktuellen Unwettersituation in Oesterreich herunter und wertet dieses aus
        /// </summary>
        /// <param name="o">Dieser Parameter wird nicht verwendet, ist jedoch fuer den Timer vorgeschrieben</param>
        private static void performCheck(Object o)
        {
            try
            {
                // Wenn temporaere Datei (vorheriger Grafikdownload von uwz) bereits existiert -> loeschen
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                
                // Download der Grafik durchfuehren, wenn nicht erfolgreich: Fehlermeldung anzeigen und abbrechen
                if (!DownloadRemoteImageFile(url, filename))
                {
                    Console.WriteLine("Error while downloading image from the internet!");
                    return;
                }

                // Mittels GetPixel wird der Farbwert fuer eine Position der Karte (Grafik von www.uwz.at) gelesen
                Bitmap myBitmap = (Bitmap)Image.FromFile(filename);
                Color pixelColor = myBitmap.GetPixel(227, 126); //Hallein

                // Gelesenen Pixelwert auswerten und Information auf Console ausgeben
                printState(pixelColor);

                myBitmap.Dispose();
            }
            catch (Exception ex)
            {
                // Bei einem Fehler muss Fehlertext auf der Console ausgegeben werden
                Console.WriteLine(ex.Message);
            } finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Wertet den Farbwert von pixelColor aus und gibt die entsprechende Information auf der Console aus
        /// Die genauen Bedeutungen der Farben kann auf der Webseite www.uwz.at eingesehen werden
        /// </summary>
        /// <param name="pixelColor"></param>
        private static void printState(Color pixelColor)
        {
            if (pixelColor.R == 0 && pixelColor.G == 255 && pixelColor.B == 0) // Gruen
            {
                Console.WriteLine("Weather OK - " + pixelColor.ToString());
            }
            else if (pixelColor.R == 255 && pixelColor.G == 0 && pixelColor.B == 255) // Lila
            {
                Console.WriteLine("!!! *** Heftiges Unwetter *** !!! - " + pixelColor.ToString());
            }
            else if (pixelColor.R == 255 && pixelColor.G == 0 && pixelColor.B == 0) // Rot
            {
                Console.WriteLine("!!! *** Markantes Unwetter *** !!! - " + pixelColor.ToString());
            }
            else if (pixelColor.R == 255 && pixelColor.G == 180 && pixelColor.B == 0) // Orange
            {
                Console.WriteLine("!!! *** Unwetter *** !!! - " + pixelColor.ToString());
            }
            else if (pixelColor.R == 255 && pixelColor.G == 255 && pixelColor.B == 0) // Gelb
            {
                Console.WriteLine("!!! *** Unwetter-Vorwarnung *** !!! - " + pixelColor.ToString());
            }
            else //neue oder andere Farbe?
            {
                Console.WriteLine("!!! *** Wert nicht definiert! *** !!! - " + pixelColor.ToString());
            }
        }

        /// <summary>
        /// Diese Funktion fuehrt einen Bild-Dateidown von einer Webseite durch (aehnlich wie wget)
        /// </summary>
        /// <param name="uri">Die URL zu der Webressource (Image), welche auf das lokale System heruntergeladen werden soll</param>
        /// <param name="fileName">Zielpfad fuer den Dateidownload</param>
        /// <returns>Wenn erfolgreich wird true zurueck gegeben, sonst false</returns>
        private static bool DownloadRemoteImageFile(string uri, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Ueberprueft, ob die Datei Remote gefunden wurde
            // Das ist noetig, da eine 404-page Fehlermeldung den Status-Code OK zurueck liefert
            // obwohl die Bild-Datei nicht herunter geladen werden konnte
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // Wenn die Datei gefunden wurde, download durchfuehren
                using (Stream inputStream = response.GetResponseStream())
                using (Stream outputStream = File.OpenWrite(fileName))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    do
                    {
                        bytesRead = inputStream.Read(buffer, 0, buffer.Length);
                        outputStream.Write(buffer, 0, bytesRead);
                    } while (bytesRead != 0);
                }
                // Dateidownload erfolgreich
                return true;
            }
            // Dateidownload _nicht_ erfolgreich (existiert nicht)
            return false;
        }
    }
}
