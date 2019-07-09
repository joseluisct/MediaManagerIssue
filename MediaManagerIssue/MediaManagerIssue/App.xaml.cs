using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MediaManagerIssue
{
    public partial class App : Application
    {
        public static string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MediaManagerIssue");
        public static string cover = Path.Combine(directory, "Cover.jpg");
        public const string mp3 = "SampleAudio_0.4mb.mp3";
        public static string play = Path.Combine(directory, "ic_play.png");
        public static string pause = Path.Combine(directory, "ic_pause.png");
        public static string stop = Path.Combine(directory, "ic_stop.png");

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                var assembly = Assembly.GetExecutingAssembly();

                ExtractResource(assembly, mp3);
                ExtractResource(assembly, "Cover.jpg");
                ExtractResource(assembly, "ic_pause.png");
                ExtractResource(assembly, "ic_play.png");
                ExtractResource(assembly, "ic_stop.png");
            }
        }

        static void ExtractResource(Assembly assembly, string name)
        {
            string file = Path.Combine(directory, name);
            Stream stream = assembly.GetManifestResourceStream("MediaManagerIssue.Assets." + name);

            using (var fileStream = File.Create(file))
            {
                stream.CopyTo(fileStream);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
