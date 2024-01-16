namespace Digimon_Project
{
    // Classe principal, primeiro código a ser iniciado
    class Emulator
    {
        public static Enviroment Enviroment;

        static void Main(string[] args)
        {
            Enviroment = new Enviroment();

            if (Enviroment.Start())
            {
                while (Enviroment.IsRunning)
                    Enviroment.Run();
            }
           
            Enviroment.Stop();
        }
    }
}
