using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tesseract;

namespace QPil.Procesos
{
    class AutomatizacionOCR
    {
        public Bitmap procesarImagen(Bitmap source, int umb) // Metodo que procesa imagen a escla de Blanco y Negro
        {
            Bitmap target = new Bitmap(source.Width, source.Height, source.PixelFormat);
            // Recorrer pixel de la imagen
            for (int i = 0; i < source.Width; i++)
            {
                for (int e = 0; e < source.Height; e++)
                {
                    // Color del pixel
                    Color col = source.GetPixel(i, e);
                    // Escala de grises
                    byte gris = (byte)(col.R * 0.3f + col.G * 0.59f + col.B * 0.11f);
                    // Blanco o negro
                    byte value = 0;
                    if (gris > umb)
                    {
                        value = 255;
                    }
                    // Asginar nuevo color
                    Color newColor = System.Drawing.Color.FromArgb(value, value, value);
                    target.SetPixel(i, e, newColor);

                }
            }
            target.Save(Directory.GetCurrentDirectory() + @"\file_name_string3.Png", System.Drawing.Imaging.ImageFormat.Png);
            return (target);
        }

        public string reconocerTextoImagenSinFiltro(Image img) // Método que reconoce Captcha sin usar Filtros 
        {
            Bitmap imagen = new Bitmap(img);
            string reconocido = OCR((Bitmap)imagen);
            return reconocido;
        }
        public string reconocerTextoImagenConFiltro(Image img) // Método que reconoce Captcha usando Filtros 
        {
            Bitmap imagen = new Bitmap(img);
            imagen = imagen.Clone(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb); //
            Erosion erosion = new Erosion();
            Dilatation dilatation = new Dilatation();
            Invert inverter = new Invert();
            ColorFiltering cor = new ColorFiltering();
            cor.Blue = new AForge.IntRange(200, 255);
            cor.Red = new AForge.IntRange(200, 255);
            cor.Green = new AForge.IntRange(200, 255);
            Opening open = new Opening();
            BlobsFiltering bc = new BlobsFiltering();
            Closing close = new Closing();
            GaussianSharpen gs = new GaussianSharpen();
            ContrastCorrection cc = new ContrastCorrection();
            bc.MinHeight = 10;
            FiltersSequence seq = new FiltersSequence(gs, inverter, open, inverter, bc, inverter, open, cc, cor, bc, inverter);
            imagen = seq.Apply(imagen); //
            string reconocido = OCR((Bitmap)imagen);
            return reconocido;
        }
        public string OCR(Bitmap b) // Motor OCR
        {
            string res = "";
            using (var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZqwertyuiopasdfghjklñzxcvbnm.áéíóúÁÉÍÓÚ");
                engine.SetVariable("tessedit_unrej_any_wd", true);

                using (var page = engine.Process(b, PageSegMode.Auto))
                    res = page.GetText();
            }
            return res;
        }
        public string ocrRectanguloSinProcesar(string ruta, int x, int y, int w, int h) // Procesa con OCR haciendo ractangulo a una imagen sin usar Filtros
        {
            Bitmap bitmap = new Bitmap(ruta);
            string valorcaptcha = "";
            try
            {
                Rectangle cloneRect = new Rectangle(x, y, w, h);
                PixelFormat format = bitmap.PixelFormat;
                Bitmap clone = bitmap.Clone(cloneRect, format);
                bitmap.Dispose();
                clone.Save(Directory.GetCurrentDirectory() + @"\file_name_string2.png", System.Drawing.Imaging.ImageFormat.Png);
                //Image imagen = procesarImagen(clone, 220);
                valorcaptcha = reconocerTextoImagenSinFiltro(clone);
                clone.Dispose();
                valorcaptcha = valorcaptcha.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
                return valorcaptcha;
            }
            catch (System.OutOfMemoryException)
            {
                MessageBox.Show("Coordenadas excedieron la memoria " + x + " " + y + " " + w + " " + h);
                //return ocrRectanguloSinProcesar(ruta,x - 200, y - 200, w - 200, h-200);

            }
            catch (System.ArgumentException)
            {
                MessageBox.Show("Coordenadas excedieron la memoria " + x + " " + y + " " + w + " " + h);
                //return ocrRectanguloSinProcesar(ruta, x-200, y - 200, w - 200, h - 200);

            }



            return valorcaptcha;


        }
        public string ocrRectanguloProcesado(string ruta, int x, int y, int w, int h) // Procesa con OCR haciendo ractangulo a una imagen usando Filtros
        {
            Bitmap bitmap = new Bitmap(ruta);
            string valorcaptcha = "";
            try
            {
                Rectangle cloneRect = new Rectangle(x, y, w, h);
                PixelFormat format = bitmap.PixelFormat;
                Bitmap clone = bitmap.Clone(cloneRect, format);
                bitmap.Dispose();
                clone.Save(Directory.GetCurrentDirectory() + @"\file_name_string2.png", System.Drawing.Imaging.ImageFormat.Png);
                Image imagen = procesarImagen(clone, 220);
                valorcaptcha = reconocerTextoImagenConFiltro(imagen);
                imagen.Dispose();
                valorcaptcha = valorcaptcha.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace(" ", "");
            }
            catch (System.OutOfMemoryException)
            {
                MessageBox.Show("Coordenadas excedieron la memoria" + x + " " + y + " " + w + " " + h);
               // return ocrRectanguloSinProcesar(ruta, x - 200, y - 200, w - 200, h - 200);

            }
            catch (System.ArgumentException)
            {
                MessageBox.Show("Coordenadas excedieron la memoria" + x + " " + y + " " + w + " " + h);
                //return ocrRectanguloSinProcesar(ruta, x - 200, y - 200, w - 200, h - 200);

            }
            return valorcaptcha;
        }


    }

}
