using Digimon_Project.Database.Results;
using System;
using System.IO;

namespace Digimon_Project.Utils
{
    public static class MakeKeys
    {
        public static void Read()
        {
            // Read entire text file content in one string  
            string textFile = System.IO.Directory.GetCurrentDirectory()+ "/mails.txt";
            string[] lines = File.ReadAllLines(textFile);

            foreach (string line in lines)
            {
                VerificationResult result = Emulator.Enviroment.Database.Select<VerificationResult>(
                                "id", "closed_keys", "WHERE email=@email"
                                , new Database.QueryParameters() { {"email", line } });
                if (!result.HasRows)
                {
                    Random r = new Random();
                    int chave = r.Next();
                    int ID = Emulator.Enviroment.Database.Insert<int>("closed_keys", new Database.QueryParameters() {
                        { "email", line }, { "chave", chave } });
                    Console.WriteLine("{0} Key: {1}", line, chave);
                }
            }
        }
    }
}
