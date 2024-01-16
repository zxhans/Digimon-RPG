using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Digimon_Project.Game.Entities;

namespace Digimon_Project.Database.Results
{
    // Classe que transforma um resultado de Query no banco em uma lista de objetos Tamer
    public class ClosedBetaKeysResult : ISelectResult
    {
        public void OnExecuted(MySqlDataReader reader)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string mail = reader.GetString("email");
                    int key = reader.GetInt32("chave");

                    string link = "<a href = 'https://drive.google.com/open?id=1A7Rb4G9t95Jv4TK38-Ta10biqEo4dLmB'>TAOSetup</a>";

                    string msg = "Português:< br > "
                    + "<br>"
                    + "Olá Domares, estou trazendo aqui para vocês o link do download para jogar o Tamers Adventure Online, mas antes, quero deixar totalmente explicado a situação atual do jogo para que não haja nenhum problema ao decorrer do tempo. O jogo se encontra atualmente com bugs, normal para um closed beta, estamos tentando arrumar tudo ao nosso alcance, para isso, vocês teriam que nos ajudar a reportar bugs que ocorrerem durante o jogo, para isso, mande detalhamente para o privado da página do Tamers Adventure Online, explicando como ocorre o bug para corrigir. E queria também falar sobre funções inativas no jogo, como npcs, itens, cards e entre outras coisas não estão funcionando, mas está desativado para ser ativado depois certinho. E também tem a questão do drop que está dando apenas cake e fries, algumas coisas, mas iremos corrigir isso também. Queremos que nos ajude também e espero que não haja nenhum problema durante o closed beta e toda a equipe estará lá para ajudar e auxiliar. Agradecemos pela sua participação ao closed beta. Sua aventura está só começando, sejam todos Bem Vindos ao Closed Beta do Tamers Adventure Online!<br>"
                    + "<br>"
                    + "<br>"
                    + "Antes de fazer o download, leia o texto acima!!!!<br>"
                    + "<br>"
                    + "Link do download: " + link + "<br>"
                    + "<br>"
                    + "<br>"
                    + "<br>"
                    + "English:<br>"
                    + "<br>"
                    + "Hello Domares, I am bringing here the download link to play Tamers Adventure Online, but first, I want to fully explain the current situation of the game so that there is no problem over time.  The game is currently buggy, normal for a closed beta, we are trying to fix everything in our power, so you would have to help us report bugs that occur during the game, so please send it in detail to the private page.  Tamers Adventure Online, explaining how the bug to fix occurs.  And I would also like to talk about inactive functions in the game, such as npcs, items, cards and other things are not working, but it is disabled to be activated later.  And there is also the issue of drop that is only giving cake and fries, some things, but we will fix that too.  We want you to help us too and hope there is no problem during the closed beta and the whole team will be there to help and assist.  Thank you for your participation in the closed beta.  Your adventure is just beginning, welcome to the Tamers Adventure Online Closed Beta!<br>"
                    + "<br>"
                    + "<br>"
                    + " Before downloading, read the text above !!!!<br>"
                    + "<br>"
                    + " Download Link: " + link + "<br>"
                    + "<br>"
                    + "<br>"
                    + "<br>"
                    + "Español:<br>"
                    + "<br>"
                    + "Hola Domares, traigo aquí el enlace de descarga para jugar Tamers Adventure Online, pero primero, quiero explicar completamente la situación actual del juego para que no haya ningún problema con el tiempo.  El juego actualmente tiene errores, es normal para una versión beta cerrada, estamos tratando de arreglar todo lo que está a nuestro alcance, por lo que debería ayudarnos a informar los errores que ocurren durante el juego, así que envíelo en detalle a la página privada.  Tamers Adventure Online, explicando cómo se produce el error para corregir.  Y también me gustaría hablar sobre funciones inactivas en el juego, como npcs, elementos, tarjetas y otras cosas que no funcionan, pero está desactivado para ser activado más adelante.  Y también está el problema de la caída, que solo da pastel y papas fritas, algunas cosas, pero también lo arreglaremos.  Queremos que también nos ayudes y esperamos que no haya ningún problema durante la beta cerrada y que todo el equipo estará allí para ayudar y ayudar.  Gracias por su participación en la beta cerrada.  Su aventura recién comienza, ¡bienvenido a la Beta cerrada en línea de Tamers Adventure!<br>"
                    + "<br>"
                    + "<br>"
                    + "  Antes de descargar, lea el texto de arriba !!!!<br>"
                    + "<br>"
                    + "  Enlace de descarga: " + link + "";

                    Utils.Email.SendMail(mail, "Tamers Adventure Online Closed Beta", msg);
                }
            }
        }
    }
}
