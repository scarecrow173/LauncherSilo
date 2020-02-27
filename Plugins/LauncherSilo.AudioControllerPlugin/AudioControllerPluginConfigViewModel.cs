using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Windows.Markup;
using System.Windows.Input;

using System.Windows.Interop;
using System.Windows.Media;
using NAudio;


namespace LauncherSilo.AudioControllerPlugin
{
    public class ChannelViewModel : INotifyPropertyChanged
    {
        public ChannelViewModel(double VolumeValue, double PeekValue)
        {
            _Volume = VolumeValue;
            _Peek = PeekValue;
        }
        public double Volume
        {
            get
            {
                return _Volume;
            }
            set
            {
                if (_Volume != value)
                {
                    _Volume = value;
                    OnPropertyChanged("Volume");
                    OnPropertyChanged("DisplayVolume");
                }
            }
        }
        private double _Volume = 0.0;

        public double DisplayVolume
        {
            get
            {
                return _Volume;
            }
        }
        public double Peek
        {
            get
            {
                return _Peek;
            }
            set
            {
                if (_Peek != value)
                {
                    _Peek = value;
                    OnPropertyChanged("Peek");
                    OnPropertyChanged("DisplayPeek");
                }
            }
        }
        private double _Peek = 0.0;

        public double DisplayPeek
        {
            get
            {
                return Math.Round(_Peek * 100.0);
            }
            set
            {

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    public class AudioDeviceViewModel : INotifyPropertyChanged
    {
        private readonly SynchronizationContext synchronizationContext;
        public AudioDeviceViewModel(NAudio.CoreAudioApi.MMDevice audioDevice)
        {
            synchronizationContext = SynchronizationContext.Current;
            Device = audioDevice;
            if (Device.State == NAudio.CoreAudioApi.DeviceState.Active && Device.DataFlow == NAudio.CoreAudioApi.DataFlow.Render)
            {
                float MaxDecibels = Device.AudioEndpointVolume.VolumeRange.MaxDecibels;
                float MinDecibels = Device.AudioEndpointVolume.VolumeRange.MinDecibels;
                Misc.LogStatics.Debug("Device:{0} MasterLevel={1} MasterScalar={2} MaxDecibels={3} MinDecibels={4}", 
                    Device.FriendlyName,
                    Device.AudioEndpointVolume.MasterVolumeLevel, 
                    Device.AudioEndpointVolume.MasterVolumeLevelScalar,
                    MaxDecibels,
                    MinDecibels);
                try
                {                    
                    int ChannelCount = Math.Max(Device.AudioEndpointVolume.Channels.Count, Device.AudioMeterInformation.PeakValues.Count);
                    _Master.Volume = Math.Round(Device.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0);
                    _Master.Peek = Device.AudioMeterInformation.MasterPeakValue;
                    _Channels = new ChannelViewModel[ChannelCount];

                    for (int iChannel = 0; iChannel < ChannelCount; ++iChannel)
                    {
                        double ChVol = 0.0;
                        double ChPeek = 0.0;
                        if (iChannel < Device.AudioEndpointVolume.Channels.Count)
                        {
                            NAudio.CoreAudioApi.AudioEndpointVolumeChannel Channel = Device.AudioEndpointVolume.Channels[iChannel];
                            ChVol = Math.Round(Channel.VolumeLevelScalar * 100.0);
                            Misc.LogStatics.Debug("\tCH:{0} Level={1} Scalar={2}", iChannel, Channel.VolumeLevel, Channel.VolumeLevelScalar);
                        }
                        if (iChannel < Device.AudioEndpointVolume.Channels.Count)
                        {
                            ChPeek = Device.AudioMeterInformation.PeakValues[iChannel];
                        }
                        _Channels[iChannel] = new ChannelViewModel(ChVol, ChPeek);
                    }
                }
                catch (Exception ex)
                {
                    Misc.LogStatics.Debug("{0}", ex);
                }
                try
                {
                    Device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                    PeekUpdateTimer.Elapsed += PeekUpdateTimer_Elapsed;
                    PeekUpdateTimer.Start();
                }
                catch (Exception ex)
                {
                    Misc.LogStatics.Debug("{0}", ex);
                }
            }
        }

        private void AudioEndpointVolume_OnVolumeNotification(NAudio.CoreAudioApi.AudioVolumeNotificationData data)
        {
            _Master.Volume = data.MasterVolume;
            int ChannelCount = data.ChannelVolume.Count();
            for (int iChannel = 0; iChannel < ChannelCount; ++iChannel)
            {
                NAudio.CoreAudioApi.AudioEndpointVolumeChannel Channel = Device.AudioEndpointVolume.Channels[iChannel];
                _Channels[iChannel].Volume = Math.Round(Channel.VolumeLevelScalar * 100.0);
            }
            OnPropertyChanged("Master");
            OnPropertyChanged("Channels");
        }

        private void PeekUpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                synchronizationContext.Post(s => 
                {
                    int PeekCount = Device.AudioMeterInformation.PeakValues.Count;
                    _Master.Peek = Device.AudioMeterInformation.MasterPeakValue;
                    for (int iPeek = 0; iPeek < PeekCount; ++iPeek)
                    {
                        _Channels[iPeek].Peek = Device.AudioMeterInformation.PeakValues[iPeek];
                    }
                }
                ,null);
                OnPropertyChanged("Master");
                OnPropertyChanged("Channels");

            }
            catch (Exception ex)
            {
                Misc.LogStatics.Debug("{0}", ex);
                _HasException = true;
            }
            finally
            {
                if (!_HasException)
                {
                    PeekUpdateTimer.Start();
                }
            }
        }
        private bool _HasException = false;
        private System.Timers.Timer PeekUpdateTimer = new System.Timers.Timer(16) { AutoReset = false };

        public NAudio.CoreAudioApi.MMDevice Device { get; private set; }
        public string FullName
        {
            get
            {
                if (Device != null)
                {
                    return Device.FriendlyName;
                }
                return string.Empty;
            }
        }

        public ImageSource IconPath
        {
            get
            {
                if (Device != null)
                {
                    string[] IconAddress = Device.IconPath.Split(',');
                    return Misc.IconImageStorage.FindIconImage(IconAddress[0], int.Parse(IconAddress[1]));
                }
                return null;
            }
        }

        public bool IsDefaultDevice
        {
            get
            {
                if (Device != null)
                {
                    return Device.State == NAudio.CoreAudioApi.DeviceState.Active;
                }
                return false;
            }
            set
            {
                //if (Device != null && Device.IsDefaultDevice != value)
                //{
                //    if (value)
                //    {
                //        Device.SetAsDefault();
                //        OnPropertyChanged("IsDefaultDevice");
                //    }
                //}
            }
        }

        public string Name
        {
            get
            {
                if (Device != null)
                {
                    return Device.DeviceFriendlyName;
                }
                return string.Empty;
            }
        }
        public bool IsActive
        {
            get
            {
                //if (Device != null)
                //{
                //    return Device.State != DeviceState.Active;
                //}
                return false;
            }
        }
        public ChannelViewModel Master
        {
            get
            {
                //if (Device != null)
                //{
                //    return Math.Round(Device.Volume);
                //}
                return _Master;
            }
            set
            {
                //if (Device != null && Device.Volume != value)
                //{
                //    Device.SetVolumeAsync(value);
                //}
            }
        }
        private ChannelViewModel _Master = new ChannelViewModel(0.0, 0.0);

        public ChannelViewModel[] Channels
        {
            get
            {
                return _Channels;
            }
        }
        private ChannelViewModel[] _Channels = null;


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }
    }
    public class AudioControllerPluginConfigViewModel : Core.ViewModels.PluginConfigViewModel
    {
        public AudioControllerPluginConfigViewModel(PluginSystem.IPlugin Plugin, PluginSystem.PluginConfig PluginConfig)
            : base(Plugin, PluginConfig)
        {
            Uri uri = new Uri("pack://application:,,,/LauncherSilo.AudioControllerPlugin;component/AudioControllerPluginResource.xaml");
            StreamResourceInfo info = Application.GetResourceStream(uri);
            XamlReader reader = new XamlReader();
            var dictionary = reader.LoadAsync(info.Stream) as ResourceDictionary;
            PluginConfigControlTemplate = dictionary["AudioControllerPluginConfigView"] as ControlTemplate;

            NAudio.CoreAudioApi.MMDeviceEnumerator enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
            NAudio.CoreAudioApi.MMDeviceCollection endPoints = enumerator.EnumerateAudioEndPoints(NAudio.CoreAudioApi.DataFlow.All, NAudio.CoreAudioApi.DeviceState.All);
            foreach (NAudio.CoreAudioApi.MMDevice endPoint in endPoints)
            {
                if (endPoint.State == NAudio.CoreAudioApi.DeviceState.NotPresent)
                {
                    continue;
                }
                if (endPoint.DataFlow == NAudio.CoreAudioApi.DataFlow.Render)
                {
                    AudioPlaybackDeviceVM.Add(new AudioDeviceViewModel(endPoint));
                }
                else if (endPoint.DataFlow == NAudio.CoreAudioApi.DataFlow.Capture)
                {
                    AudioCaptureDeviceVM.Add(new AudioDeviceViewModel(endPoint));
                }
            }
        }
        public AudioControllerPluginConfig SwitcherPluginConfig { get { return _PluginConfig as AudioControllerPluginConfig; } set { _PluginConfig = value; } }
        //public ShowMeterCommand ShowMeter { get; private set; } = new ShowMeterCommand();
        NAudio.Wave.WasapiLoopbackCapture _capture = null;
        private NAudio.Wave.WaveFileWriter _waveFileWriter = null;
        private NAudio.CoreAudioApi.MMDevice DefaultRenderDevice = null;
        public bool IsCapture
        {
            get
            {
                return _IsCapture;
            }
            set
            {
                if (value != _IsCapture)
                {
                    _IsCapture = value;
                    OnPropertyChanged("IsCapture");
                    if (_IsCapture)
                    {
                        NAudio.CoreAudioApi.MMDeviceEnumerator enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator();
                        DefaultRenderDevice = enumerator.GetDefaultAudioEndpoint(NAudio.CoreAudioApi.DataFlow.Render, NAudio.CoreAudioApi.Role.Multimedia);
                        _capture = new NAudio.Wave.WasapiLoopbackCapture();
                        _waveFileWriter = new NAudio.Wave.WaveFileWriter("recoded.wav", _capture.WaveFormat);
                        _capture.DataAvailable += Capture_DataAvailable;
                        _capture.RecordingStopped += Capture_RecordingStopped;
                        _capture.StartRecording();
                    }
                    else
                    {
                        _capture?.StopRecording();
                    }
                }
            }
        }

        private void Capture_RecordingStopped(object sender, NAudio.Wave.StoppedEventArgs e)
        {
            _waveFileWriter?.Dispose();
            _waveFileWriter = null;
            _capture.Dispose();
        }

        private void Capture_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            _waveFileWriter?.Write(e.Buffer, 0, e.BytesRecorded);
        }

        private bool _IsCapture = true;
        public ObservableCollection<AudioDeviceViewModel> AudioPlaybackDeviceVM { get; set; } = new ObservableCollection<AudioDeviceViewModel>();
        public ObservableCollection<AudioDeviceViewModel> AudioCaptureDeviceVM { get; set; } = new ObservableCollection<AudioDeviceViewModel>();


    }
}
