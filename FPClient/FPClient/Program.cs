using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FPClient
{
    class Program
    {
        public static bool exit = false;

        enum playerMove
        {
            Rock, Paper, Scissors, Exit, noInput
        }

        struct Player
        {
            public playerMove move;
        }

        static string game()
        {
            Player player;
            int choose;
            string input = "0";

            player.move = playerMove.noInput;
            Console.WriteLine("1. Rock");
            Console.WriteLine("2. Paper");
            Console.WriteLine("3. Scissors");
            Console.WriteLine("0. Exit");
            Console.WriteLine("Choose : ");
            choose = Convert.ToInt32(Console.ReadLine());

            switch(choose)
            {
                case 0:
                    player.move = playerMove.Exit;
                    input = Convert.ToString(player.move);
                    break;

                case 1:
                    player.move = playerMove.Rock;
                    input = Convert.ToString(player.move);
                    break;

                case 2:
                    player.move = playerMove.Paper;
                    input = Convert.ToString(player.move);
                    break;

                case 3:
                    player.move = playerMove.Scissors;
                    input = Convert.ToString(player.move);
                    break;

                default:
                    Console.WriteLine("Error! check your input and restart the game");
                    player.move = playerMove.noInput;
                    input = Convert.ToString(playerMove.noInput);
                    break;
            }

            return input;
        }

        //Create the Client
        static void aClient()
        {
            int port = 1302;
            string ipaddress = "127.0.0.1";
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipaddress), port);

            clientSocket.Connect(ep);
            Console.WriteLine("Connected to Other Player!");
            Program p = new Program();

            while(!exit)
            {
                //Player Turn
                Console.WriteLine("Your Turn to Choose : ");
                string yourAnswer = null;
                yourAnswer = game();

                //Send Player Turn
                clientSocket.Send(System.Text.Encoding.ASCII.GetBytes(yourAnswer), 0, yourAnswer.Length, SocketFlags.None);

                //Get Data
                byte[] message = new byte[1024];
                int size = clientSocket.Receive(message);
                string opponent = null;
                opponent = System.Text.Encoding.ASCII.GetString(message, 0, size);

                //check if player disconnected
                if (opponent == Convert.ToString(playerMove.Exit))
                {
                    Console.WriteLine();
                    Console.WriteLine("Another Player Disconnected...");
                    Console.WriteLine();
                    exit = true;
                    break;
                }

                //check
                check(opponent, yourAnswer);
            }
        }

        public static void check(string opponentAnswer, string yourAnswer)
        {
            //if opponent choose rock
            if (opponentAnswer == Convert.ToString(playerMove.Rock))
            {
                //if you choose paper
                if (yourAnswer == Convert.ToString(playerMove.Paper))
                {
                    Console.WriteLine();
                    Console.WriteLine("You win, Paper win againts Rock");
                    Console.WriteLine();
                }
                //if you choose scissor
                else if (yourAnswer == Convert.ToString(playerMove.Scissors))
                {
                    Console.WriteLine();
                    Console.WriteLine("You lose, Rock win againts scissors");
                    Console.WriteLine();
                }
                //if you choose rock too
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("It's a Tie!");
                    Console.WriteLine();
                }
            }
            //if opponent choose paper
            if (opponentAnswer == Convert.ToString(playerMove.Paper))
            {
                //  if you choose scissor
                if (yourAnswer == Convert.ToString(playerMove.Scissors))
                {
                    Console.WriteLine();
                    Console.WriteLine("You win, Scissors win againts Paper");
                    Console.WriteLine();
                }
                //if you choose rock
                else if (yourAnswer == Convert.ToString(playerMove.Rock))
                {
                    Console.WriteLine();
                    Console.WriteLine("You lose, Paper win againts Rock");
                    Console.WriteLine();
                }
                //if you choose paper too
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("It's a Tie!");
                    Console.WriteLine();
                }
            }
            //if opponent choose scissor
            if (opponentAnswer == Convert.ToString(playerMove.Scissors))
            {
                //  if you choose rock
                if (yourAnswer == Convert.ToString(playerMove.Rock))
                {
                    Console.WriteLine();
                    Console.WriteLine("You win, Rock win againts Scissors");
                    Console.WriteLine();
                }
                //  if you choose paper
                else if (yourAnswer == Convert.ToString(playerMove.Paper))
                {
                    Console.WriteLine();
                    Console.WriteLine("You lose, Scissors win againts Paper");
                    Console.WriteLine();
                }
                //  if you choose scissor too
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("It's a Tie!");
                    Console.WriteLine();
                }
            }

            //  no input from player 
            if (opponentAnswer == Convert.ToString(playerMove.noInput))
            {
                Console.WriteLine();
                Console.WriteLine("False no Player input...");
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            aClient();
        }
    }
}
