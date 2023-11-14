using Plugin.Maui.Audio;
using AudioPlayer_v1.Controllers;
using AudioPlayer_v1.Views;

namespace AudioPlayer_v1 {
    public partial class App : Application {
        public static readonly DBController db = new DBController();
        public static readonly string audiosDirectory = Path.Combine(FileSystem.AppDataDirectory, "Audios");



        public App() {
            InitializeComponent();

            //valida existencia del directorio de los audios, si no existe, lo crea.
            if (!Directory.Exists(audiosDirectory)) {
                Directory.CreateDirectory(audiosDirectory);
            }

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new RecorderScreen(AudioManager.Current));
        }
    }
}