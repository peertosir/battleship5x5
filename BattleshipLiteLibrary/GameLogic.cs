using BattleshipLiteLibrary.Models;
using BattleshipLiteLibrary.Enums;
using System;
using System.Collections.Generic;

namespace BattleshipLiteLibrary
{
    public static class GameLogic
    {

        private static List<string> gridLetters = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E"
        };

        private static List<int> gridNumbers = new List<int>
        {
            1,
            2,
            3,
            4,
            5
        };

        public static void InitializeGrid(PlayerInfoModel model)
        {
            foreach (string letter in gridLetters)
            {
                foreach (int number in gridNumbers)
                {
                    AddGridSpot(model, letter, number);
                }
            }
        }

        public static bool PlayerStillActive(PlayerInfoModel opponent)
        {
            bool isActive = false;

            foreach (var ship in opponent.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }
            return isActive;
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNumber = number,
                Status = GridSpotStatus.Empty
            };
            model.ShotsGrid.Add(spot);
        }

        public static int GetShotCount(PlayerInfoModel winner)
        {
            return winner.ShotsCount;
        }

        public static bool PlaceShip(PlayerInfoModel model, string location)
        {
            string row = "";
            int column = -1;

            (row, column) = SplitIntoRowAndColumns(location);

            bool isValidPlaceForShip = ValidateLocation(row, column);
            if (!isValidPlaceForShip) return false;

            if (GetSpotFromList(model.ShipLocations, row, column) != null)
            {
                return false;
            }

            GridSpotModel shipModel = new GridSpotModel { 
                Status = GridSpotStatus.Ship,
                SpotLetter = row,
                SpotNumber = column
            };

            model.ShipLocations.Add(shipModel);

            return true;
        }

        public static bool ValidateLocation(string row, int column)
        {
            if (gridNumbers.Contains(column) && gridLetters.Contains(row))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateShot(PlayerInfoModel activePlayer, string row, int column)
        {
            bool isValidShotLocation = ValidateLocation(row, column);
            if (!isValidShotLocation) return false;

            GridSpotModel output = GetSpotFromList(activePlayer.ShotsGrid, row, column);

            if ((output != null) && (output.Status == GridSpotStatus.Empty))
            {
                return true;
            }
            return false;
        }

        public static (string, int) SplitIntoRowAndColumns(string location)
        {
            if (location.Length != 2)
            {
                return ("", -1);
            }

            string row = location[0].ToString();
            string columnText = location[1].ToString();

            bool isValidInt = int.TryParse(columnText, out int rowNumber);
            if (isValidInt)
            {
                return (row, rowNumber);
            } else
            {
                return ("", -1);
            }
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            GridSpotModel enemyShip = GetSpotFromList(opponent.ShipLocations, row, column);
            if (enemyShip != null)
            {
                enemyShip.Status = GridSpotStatus.Sunk;
                return true;
            }
            return false;
        }

        public static void MarkShotResult(PlayerInfoModel activePlayer, string row, int column, bool isAHit)
        {
            GridSpotModel shopGridSpot =  GetSpotFromList(activePlayer.ShotsGrid, row, column);
            if (isAHit)
            {
                shopGridSpot.Status = GridSpotStatus.Hit;
            } 
            else
            {
                shopGridSpot.Status = GridSpotStatus.Miss;
            }
            activePlayer.ShotsCount++;
        }

        public static GridSpotModel GetSpotFromList(List<GridSpotModel> listOfGrids, string row, int column)
        {
            foreach (GridSpotModel spotModel in listOfGrids)
            {
                if (row == spotModel.SpotLetter && column == spotModel.SpotNumber)
                {
                    return spotModel;
                }
            }
            return null;
        }
    }
}
