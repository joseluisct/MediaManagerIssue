using MediaManager;
using MediaManager.Playback;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MediaManagerIssue
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        string file = Path.Combine(App.directory, App.mp3);
        IMediaManager MediaManager => CrossMediaManager.Current;

        public MainPage()
        {
            InitializeComponent();
            image.Source = ImageSource.FromFile(App.cover);
            stop.ImageSource = ImageSource.FromFile(App.stop);
            play.ImageSource = ImageSource.FromFile(App.play);
        }

        private async void Play_Clicked(object sender, EventArgs e)
        {
            if (MediaManager.State == MediaPlayerState.Stopped || MediaManager.State == MediaPlayerState.Failed)
            {              
                await MediaManager.Play(file);
            }
            else
            {
                await MediaManager.PlayPause();
            }
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            MediaManager.PositionChanged += (send, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (args != null)
                    {
                        if(MediaManager.Duration.TotalMilliseconds > 0) bar.Value = args.Position.TotalMilliseconds * 100 / MediaManager.Duration.TotalMilliseconds;
                        actual.Text = args.Position.ToString(@"mm\:ss");
                    }
                });
            };

            MediaManager.StateChanged += (send, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    switch (args.State)
                    {
                        case MediaPlayerState.Stopped:
                            play.ImageSource = ImageSource.FromFile(App.play);
                            Initialize();
                            break;
                        case MediaPlayerState.Paused:
                            play.ImageSource = ImageSource.FromFile(App.play);
                            break;
                        case MediaPlayerState.Playing:
                            play.ImageSource = ImageSource.FromFile(App.pause);
                            break;
                        case MediaPlayerState.Loading:
                            play.ImageSource = ImageSource.FromFile(App.pause);
                            break;
                        case MediaPlayerState.Buffering:
                            play.ImageSource = ImageSource.FromFile(App.pause);
                            break;
                        case MediaPlayerState.Failed:
                            play.ImageSource = ImageSource.FromFile(App.play);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (MediaManager.Duration != default(TimeSpan) && args.State != MediaPlayerState.Stopped && total.Text == "00:00")
                    {
                        total.Text = MediaManager.Duration.ToString(@"mm\:ss");
                    }
                });
            };

            MediaManager.MediaItemChanged += (send, args) =>
            {
                /*var duracion = args.File.Metadata.Duration;
                if(duracion > 0)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(duracion);
                    total.Text = ts.ToString(@"mm\:ss");
                }*/
                // Access any other metadata property through the file

                Initialize();
            };

            MediaManager.MediaItemFinished += (send, args) =>
            {
                //var fichero = args.File;
                MediaManager.Stop();
            };
        }

        private void Bar_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.WriteLine("New value: " + e.NewValue + "\tOld value: " + e.OldValue);
            if (Math.Abs(e.NewValue - e.OldValue) > 2)
            {
                if (MediaManager.State != MediaPlayerState.Stopped && MediaManager.State != MediaPlayerState.Failed)
                {
                    var time = e.NewValue * MediaManager.Duration.TotalMilliseconds / 100;
                    MediaManager.SeekTo(TimeSpan.FromMilliseconds(time));
                }
                else bar.Value = 0;
            }
        }

        private void Stop_Clicked(object sender, EventArgs e)
        {
            MediaManager.Stop();
        }

        private void Initialize()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                total.Text = "00:00";
                actual.Text = "00:00";
                bar.Value = 0;
            });
        }
    }
}
