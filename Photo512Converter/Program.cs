using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Photo512Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {                
                Start();
                Console.WriteLine();
            }
        }

        static void Start()
        {
            Console.Write("Введите макс. размер одной из сторон:");
            int max_new_image_side = GetInt();

            DirectoryInfo dir;
            while (true)
            {
                Console.Write("Введите путь к папке с файлами: ");
                string dir_path = Console.ReadLine();
                dir = new DirectoryInfo(dir_path);
                if (dir.Exists)
                    break;
                else
                    Console.WriteLine("Путь не найден");
            }
            string FormatMode;
            while (true)
            {
                Console.WriteLine("Выберите формат сохранения 1 - jpeg, 2 - png, 3 - bmp");
                FormatMode = Console.ReadLine();
                if (FormatMode == "1" || FormatMode == "2" || FormatMode == "3")
                    break;
                FormatMode = String.Empty;
                    
            }

            DirectoryInfo directory = new DirectoryInfo(dir.FullName + @"\Result");
            for (int i = 1; directory.Exists; i++)
                directory = new DirectoryInfo(dir.FullName + @"\Result_" + i.ToString());
            directory.Create();

            List<string> files = GetAllFile(dir.FullName);
            foreach (string file in files)
            {
                Bitmap image = new Bitmap(file);
                int max_image_side = Math.Max(image.Height, image.Width);
                double coeff = (double)max_new_image_side / max_image_side;
                int new_height = (int)(image.Height * coeff);
                int new_width = (int)(image.Width * coeff);
                Bitmap new_image = new Bitmap(image, new Size(new_width, new_height));
                switch (FormatMode) {
                    case "1":
                        new_image.Save(directory.FullName + @"\" + new FileInfo(file).Name + ".jpeg" , ImageFormat.Jpeg);
                        break;
                    case "2":
                        new_image.Save(directory.FullName + @"\" + new FileInfo(file).Name + ".png" , ImageFormat.Png);
                        break;
                    case "3":
                        new_image.Save(directory.FullName + @"\" + new FileInfo(file).Name + ".bmp", ImageFormat.Bmp);
                        break;

                }
            }
        }

        static int GetInt()
        {
            int value = 0;
            while ( ! int.TryParse(Console.ReadLine(), out value))
                Console.Write(Environment.NewLine + "Некорректный ввод:");
            return value;
        }

        static List<string> GetAllFile(string dir_path)
        {
            string searchPatternExpression = @"\.jpeg|\.jpg|\.png|\.bmp";
            Regex reSearchPattern = new Regex(searchPatternExpression, RegexOptions.IgnoreCase);
            var files = Directory.EnumerateFiles(dir_path, "*", SearchOption.AllDirectories).Where
            (
                file => reSearchPattern.IsMatch(Path.GetExtension(file))
            );

            Console.WriteLine("Список файлов:" + Environment.NewLine);
            foreach (string file in files)
                Console.WriteLine(Path.GetFileName(file));
            return files.ToList();
        }
    }
}
