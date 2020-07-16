using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ruminoid.Dashboard.Models
{
    public sealed class Product : INotifyPropertyChanged
    {
        #region DataContext

        private string id = "Unknown";

        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }

        private string title = "Unknown Product";

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string category = "Unknown";

        public string Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged();
            }
        }

        private string description = "";

        public string Description
        {
            get => description;
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
