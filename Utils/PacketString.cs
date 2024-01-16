using Digimon_Project.Database.Results;
using Digimon_Project.Game;
using System;
using System.Diagnostics;
using System.IO;

namespace Digimon_Project.Utils
{
    public static class PacketString
    {
        public static string Read(string file)
        {
            string result = "00 00 00 00 00 00 00 00 ";
            // Read entire text file content in one string  
            string textFile = Directory.GetCurrentDirectory()+ "/"+file+".txt";
            string[] lines = File.ReadAllLines(textFile);

            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                foreach(string s in split)
                {
                    if (s.Length == 2 && !s.Contains("."))
                        result += s + " ";
                }
            }

            //Debug.Print(result);

            return result;
        }

        public static string ReadOnly(string file)
        {
            string result = "";
            // Read entire text file content in one string  
            string textFile = Directory.GetCurrentDirectory() + "/" + file + ".txt";
            string[] lines = File.ReadAllLines(textFile);

            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                foreach (string s in split)
                {
                    if (s.Length == 2 && !s.Contains("."))
                        result += s + " ";
                }
            }

            //Debug.Print(result);

            return result;
        }

        // Lendo pacotes com criptografia
        public static string CryptoRead(string file, string key)
        {
            string result = "";
            // Read entire text file content in one string  
            string textFile = Directory.GetCurrentDirectory() + "/" + file + ".txt";
            string[] lines = File.ReadAllLines(textFile);
            AlphaMap alpha = new AlphaMap();

            // Chaves
            byte e = StringHex.Hex2Binary(key[0]);
            byte d = StringHex.Hex2Binary(key[1]);

            bool append = false;

            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                foreach (string s in split)
                {
                    if (s.Length == 2 && !s.Contains("."))
                    {
                        string rs = s;
                        // Descriptografando os caracteres
                        if (alpha.Alpha.Length >= e && alpha.Alpha.Length >= d)
                        {
                            int ei = 0, di = 0;
                            for(int i = 0; i < 16; i++)
                            {
                                // Byte da esquerda
                                if (alpha.Alpha[e][i] == s[0])
                                {
                                    ei = i;
                                }
                                // Byte da direita
                                if (alpha.Alpha[d][i] == s[1])
                                {
                                    di = i;
                                }
                            }
                            rs = alpha.Alpha[0][ei].ToString() + alpha.Alpha[0][di].ToString();
                        }

                        result += rs + " ";
                    }
                }

                // Escrevendo a linha no arquivo
                Debug.Print("Line: {0}", result);
                string tFile = Directory.GetCurrentDirectory() + "/crypto.txt";
                using (StreamWriter outputFile = new StreamWriter(tFile, append: append))
                {
                    outputFile.WriteLine(result);
                }

                result = "";
                append = true;
            }

            //Debug.Print(result);

            return result;
        }
    }
}
