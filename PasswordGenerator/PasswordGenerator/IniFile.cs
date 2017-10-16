using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    class IniFile
    {
        private static string path = ""; //Имя файла.
        public static string Path { get { return path; } set { path = value; } }
        [DllImport("kernel32")] // Подключаем kernel32.dll и описываем его функцию WritePrivateProfilesString
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int Section, string Key,
               string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
               int Size, string FileName);
        // С помощью конструктора записываем пусть до файла и его имя.
        public static void InitFile(string IniPath)
        {
           path = new FileInfo(IniPath).FullName.ToString();
        }
        public static string[] GetSectionNames()
        {
            List<string> sections = new List<string>();
            foreach (string line in File.ReadAllLines(Path))
            {
                string reg = Regex.Match(line, @"(?<=\[)(.*)(?=\])").ToString();
                if (!string.IsNullOrEmpty(reg))
                {
                    sections.Add(reg);
                }
            }
            return sections.ToArray();
        }
        //Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        public static string ReadINI(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }
        //Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        public static void Write(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        //Удаляем ключ из выбранной секции.
        public static void DeleteKey(string Key, string Section = null)
        {
            Write(Section, Key, null);
        }
        //Удаляем выбранную секцию
        public static void DeleteSection(string Section = null)
        {
            Write(Section, null, null);
        }
        //Проверяем, есть ли такой ключ, в этой секции
        public static bool KeyExists(string Key, string Section = null)
        {
            return ReadINI(Section, Key).Length > 0;
        }
    }

}
