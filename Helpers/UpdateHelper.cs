using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Squirrel;

namespace Ruminoid.Dashboard.Helpers
{
    public class UpdateHelper : INotifyPropertyChanged
    {
        #region Current

        public static UpdateHelper Current { get; } = InitializeUpdateHelper();

        #endregion

        #region Constructor

        private UpdateHelper()
        {

        }

        public static UpdateHelper InitializeUpdateHelper()
        {
            UpdateHelper helper = new UpdateHelper();
            helper.ReconstructUpdateManager();
            helper.TriggerProcess();
            return helper;
        }

        #endregion

        #region Manager

        private UpdateManager _updateManager;

        #endregion

        #region Processors

        private void ReconstructUpdateManager()
        {
            _updateManager?.Dispose();
            _updateManager = new UpdateManager(Config.Current.UpdateServer + Config.Current.UpdateChannel);
            UpdateMode = "ready";
        }

        public void TriggerProcess()
        {
            switch (UpdateMode)
            {
                case "ready":
                    break;
                case "restart":
                    break;
            }
        }

        #endregion

        #region DataContext

        private string _updateMode = "ready";

        /// <summary>
        /// The Update Mode.
        /// Possible values:
        /// ready | search | down | inst | restart
        /// </summary>
        public string UpdateMode
        {
            get => _updateMode;
            set
            {
                _updateMode = value;
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
