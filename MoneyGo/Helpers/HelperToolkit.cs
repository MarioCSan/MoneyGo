using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyGo.Helpers
{
    public class HelperToolkit
    {
        public static object TempData { get; private set; }

        public static bool CompararArrayBytes(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i].Equals(b[i]) == false)
                {
                    iguales = false;
                    break;
                }
            }
            return iguales;
        }

        public static String Normalize(String filename)
        {
            String extension = System.IO.Path.GetExtension(filename).Trim('.');

            if (extension != "jpg")
            {
                return "La extensión de la imagen no es válida. Los formatos válidos son: .jpg, .png y .gif";
            }
            else
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(filename);

                HashSet<char> removeChars = new HashSet<char>(" ?&^$#@!()+´`^`·¨-,:;<>’\'-_*=");
                StringBuilder result = new StringBuilder(name.Length);
                foreach (char c in name)
                {
                    if (!removeChars.Contains(c))
                    {
                        result.Append(c);
                    } 
                }

                return result.ToString() + '.' + extension;
            }
        }

    }
}
