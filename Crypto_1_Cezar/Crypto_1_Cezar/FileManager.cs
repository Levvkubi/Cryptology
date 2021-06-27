using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Crypto_1_Cezar
{
    public class FileManager
    {
        public FileInfo file;

        public FileManager(string filename)
        {
            file = new FileInfo(filename);
        }

        public string ReadFromFile(string filename)
        {
            string fileContent;
            if (Path.GetExtension(file.FullName) != ".txt")
            {
                fileContent = Convert.ToBase64String(File.ReadAllBytes(file.FullName));
            }
            else
            {
                using (FileStream fs = File.OpenRead(file.FullName))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.Default))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            return fileContent;
        }

        public void SaveAsFile(string filename, string text)
        {
            if (Path.GetExtension(filename) != ".txt")
            {
                byte[] bytes = Convert.FromBase64String(text);
                var imageMemoryStream = new MemoryStream(bytes);
                Image imageFromStream = Image.FromStream(imageMemoryStream);
                imageFromStream.Save(filename, ImageFormat.Jpeg);
            }
            else
            {
                byte[] txtBytes = Encoding.ASCII.GetBytes(text);
                File.WriteAllBytes(filename, txtBytes);
            }
        }
    }
}
