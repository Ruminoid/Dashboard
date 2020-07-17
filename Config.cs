using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ruminoid.Common.Helpers;

namespace Ruminoid.Dashboard
{
    [RuminoidProduct("Dashboard")]
    [JsonObject(MemberSerialization.OptIn)]
    public class Config
    {
        #region Current

        public static Config Current { get; set; } = ConfigHelper<Config>.OpenConfig();

        #endregion

        #region Recent Products

        [JsonProperty]
        private Dictionary<string, int> recentProducts = new Dictionary<string, int>();

        public Dictionary<string, int> RecentProducts
        {
            get => recentProducts;
            set
            {
                recentProducts = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Update

        [JsonProperty] private string updateChannel = "";

        public string UpdateChannel
        {
            get => updateChannel;
            set
            {
                updateChannel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
