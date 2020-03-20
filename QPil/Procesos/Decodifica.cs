
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QPil.Utilidades
{
    class Decodifica
    {
           public static string decodifica(string original)
        {
            string[] strings = { original };
            Encoding asciiEncoding = Encoding.ASCII;

            // Create array of adequate size.
            byte[] bytes = new byte[49];
            // Create index for current position of array.
            int index = 0;

            //MessageBox.Show("Strings to encode:");
            foreach (var stringValue in strings)
            {
                //MessageBox.Show(stringValue);

                int count = asciiEncoding.GetByteCount(stringValue);
                if (count + index >= bytes.Length)
                    Array.Resize(ref bytes, bytes.Length + 50);

                int written = asciiEncoding.GetBytes(stringValue, 0,
                                                     stringValue.Length,
                                                     bytes, index);

                index = index + written;
            }
            //MessageBox.Show("Encoded bytes:");
            string encoder = ShowByteValues(bytes, index);
            //MessageBox.Show( ShowByteValues(bytes, index));
            

            // Decode Unicode byte array to a string.
            string newString = asciiEncoding.GetString(bytes, 0, index);
            //MessageBox.Show("Decoded: "+ newString);
            return encoder;
        }

        private static string ShowByteValues(byte[] bytes, int last)
        {
            string returnString = "   ";
            for (int ctr = 0; ctr <= last - 1; ctr++)
            {
                if (ctr % 20 == 0)
                    returnString += "\n   ";
                returnString += String.Format("{0:X2} ", bytes[ctr]);
            }
            return returnString;
        }
    }
}
