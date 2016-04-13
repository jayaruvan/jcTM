﻿using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Windows.UI.Xaml;

using jcTM.PCL.Global;
using jcTM.PCL.Transports;

using Newtonsoft.Json;

namespace jcTM.UWP.ViewModels {
    public class MainPageModel : INotifyPropertyChanged {
        private string _recordedTime;

        public string RecordedTime {
            get { return _recordedTime; }
            set { _recordedTime = value; OnPropertyChanged(); }
        }

        private string _temperature;

        public string Temperature {
            get { return _temperature; }
            set { _temperature = value; OnPropertyChanged(); }
        }

        private double _lowTemperature;

        public double LowTemperature {  get { return _lowTemperature; } set { _lowTemperature = value; OnPropertyChanged(); } }

        private double _highTemperature;

        public double HighTemperature { get { return _highTemperature; } set { _highTemperature = value; OnPropertyChanged(); } }

        private Visibility _showProgress;

        public Visibility ShowProgress {
            get {  return _showProgress; }
            set { _showProgress = value; OnPropertyChanged(); }
        }

        public async Task<T> GET<T>(string urlArguments) {
            var handler = new HttpClientHandler();
            var client = new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(1) };
            var str = await client.GetStringAsync($"{Constants.WEBAPI_ADDRESS}{urlArguments}");

            return JsonConvert.DeserializeObject<T>(str);
        }

        public async Task<bool> LoadData() {
            ShowProgress = Visibility.Visible;

            var result = await GET<DashboardResponseItem>("Temperature");

            if (result == default(DashboardResponseItem)) {
                return false;
            }

            RecordedTime = $"As of {result.Latest_RecordedTime.AddHours(-4)}";
            Temperature = $"{Math.Round(result.Latest_Temperature, 2)}'F";
            HighTemperature = result.CurrentDay_HighTemperature;
            LowTemperature = result.CurrentDay_LowTemperature;

            ShowProgress = Visibility.Collapsed;
            
            return true;
        }

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        #endregion
    }
}