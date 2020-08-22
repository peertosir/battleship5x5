using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary.Models
{
    public class PlayerInfoModel
    {
        public string UserName { get; set; }
        public int ShotsCount { get; set; } = 0;
        public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();
        public List<GridSpotModel> ShotsGrid { get; set; } = new List<GridSpotModel>();
    }
}
