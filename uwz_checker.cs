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
    class Program
    {
        static void Main(string[] args)
        {
            Timer t = new Timer(performCheck, null, 0, 15000);
            
            Console.ReadKey();
        }

        private static void performCheck(Object o)
        {
            string filename = @"C:\temp\warn.png";
            string url = "http://www.uwz.at/at/de/incoming/maps_warnings/at/AT_all_warnings_484x242.png";

            try
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                if (!DownloadRemoteImageFile(url, filename))
                {
                    Console.WriteLine("Error while downloading image from the internet!");
                    return;
                }

                // Create a Bitmap object from an image file.
                Bitmap myBitmap = (Bitmap)Image.FromFile(filename);
                // Get the color of a pixel within myBitmap.
                Color pixelColor = myBitmap.GetPixel(227, 126); //Hallein
                //Color pixelColor = myBitmap.GetPixel(215, 168); //Beispiel fuer Gelb (Gebiet hatte zu diesem Zeitpunkt einen Warnung - Gelb)
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
                else
                {
                    Console.WriteLine("!!! *** Wert nicht definiert! *** !!! - " + pixelColor.ToString());
                }

                myBitmap.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            } finally
            {
                GC.Collect();
            }
        }

        private static bool DownloadRemoteImageFile(string uri, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Check that the remote file was found. The ContentType
            // check is performed since a request for a non-existent
            // image file might be redirected to a 404-page, which would
            // yield the StatusCode "OK", even though the image was not
            // found.
            if ((response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.Moved ||
                response.StatusCode == HttpStatusCode.Redirect) &&
                response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
            {

                // if the remote file was found, download oit
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
                return true;
            }
            return false;
        }
    }
}
