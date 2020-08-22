using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Enums;
using BattleshipLiteLibrary.Models;
using System;

namespace BattleshipLite
{
    class Program
    {
        static void Main(string[] args)
        {
            MainGame();
        }

        private static void MainGame()
        {
            WelcomeMessage();
            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                Console.WriteLine($"Player {activePlayer.UserName} turn!\n");
                DisplayShotGrid(activePlayer);

                bool shouldNotSwapPlayers = RecordPlayerShot(activePlayer, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);
                if (doesGameContinue)
                {
                    if (shouldNotSwapPlayers == false)
                    {
                        (activePlayer, opponent) = (opponent, activePlayer);
                        Console.Clear();
                    }
                }
                else
                {
                    // Identify winner as an active player
                    winner = activePlayer;
                }

            } while (winner == null);


            IdentifyWinner(winner);

            Console.ReadLine();
        }


        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations for {winner.UserName}!");
            Console.WriteLine($"{ winner.UserName } took { GameLogic.GetShotCount(winner) } shots");
            Console.Read();
        }

        private static bool RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;
            string row = "";
            int column = 0;
            do
            {
                string shot = AskForShot();
                (row, column) = GameLogic.SplitIntoRowAndColumns(shot);
                isValidShot = GameLogic.ValidateShot(activePlayer, row, column);

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location. Try Again!");
                }

            } while (!isValidShot);

            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);
           
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);
            if (isAHit)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private static string AskForShot()
        {
            Console.WriteLine("Please enter your shot:");
            Console.WriteLine("Valid inputs are: 'A1' or 'C5'");
            string output = Console.ReadLine();
            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotsGrid[0].SpotLetter;

            foreach (GridSpotModel shotSpot in activePlayer.ShotsGrid)
            {
                if (shotSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = shotSpot.SpotLetter;
                }
                

                if (shotSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {shotSpot.SpotLetter}{shotSpot.SpotNumber}");
                }
                else if (shotSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X ");
                } 
                else if (shotSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O ");
                } else
                {
                    //Should not be possible
                    //Only for debug purpose
                    Console.Write(" ? ");
                }
            }
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to BattleShipLite");
            Console.WriteLine("created by peertosir, in 2020\n");
        }

        private static PlayerInfoModel CreatePlayer(string playerLabel)
        {
            PlayerInfoModel output = new PlayerInfoModel();
            Console.WriteLine($"Player information for {playerLabel}");
            output.UserName = AskForUserName();
            GameLogic.InitializeGrid(output);

            AskForPlaceShip(output);
            Console.Clear();
            return output;
        }

        private static string AskForUserName()
        {
            Console.WriteLine("What is your name?");
            string output = "";
            do
            {
                Console.WriteLine("Name should be at least 2 characters long");
                output = Console.ReadLine();
            } while (output.Length < 2);
            return output;
        }

        private static void AskForPlaceShip(PlayerInfoModel model)
        {
           while(model.ShipLocations.Count < 5)
            {
                Console.WriteLine($"Where do you want to place a ship? { model.ShipLocations.Count + 1 }/5");
                Console.WriteLine("Valid inputs are: 'A1' or 'C5'");
                string location = Console.ReadLine();
                bool isValidLocation = GameLogic.PlaceShip(model, location);
                if (!isValidLocation)
                {
                    Console.WriteLine("That was not a valid location. Try again!");
                }
            }
        }

    }
}
