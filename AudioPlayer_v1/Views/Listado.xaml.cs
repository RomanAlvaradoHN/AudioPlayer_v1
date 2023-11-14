using Plugin.Maui.Audio;
using System.Windows.Input;

namespace AudioPlayer_v1.Views;

public partial class Listado : ContentPage
{
    IAudioPlayer player;



	public Listado() { 
		InitializeComponent();
	}



    protected override async void OnAppearing() {
        base.OnAppearing();
        viewListado.ItemsSource = await App.db.SelectAll();
    }




    public ICommand SwPlay => new Command<byte[]>(async (audio) => {
        try {
            Stream stream = new MemoryStream(audio);
            player = AudioManager.Current.CreatePlayer(stream);
            player.Play();

        } catch (Exception ex) {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }

    });


}