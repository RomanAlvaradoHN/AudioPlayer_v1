using AudioPlayer_v1.Models;
using Microsoft.Maui.Controls.Compatibility;
using Plugin.Maui.Audio;

namespace AudioPlayer_v1.Views;

public partial class RecorderScreen : ContentPage
{
    private PermissionStatus permiso;
	private readonly IAudioRecorder audioRecorder;
	private byte[] audioArray;



	public RecorderScreen(IAudioManager audioManager){
		InitializeComponent();
		this.audioRecorder = audioManager.CreateRecorder();
	}



    protected override async void OnAppearing() {
        base.OnAppearing();

        permiso = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (permiso == PermissionStatus.Granted) {
            return;
        } else {
            permiso = await Permissions.RequestAsync<Permissions.Microphone>();
        }
    }





    private async void OnBtnStartRecordingClicked(object sender, EventArgs args) {
        if (permiso == PermissionStatus.Granted) {
            if (!audioRecorder.IsRecording) {
                await audioRecorder.StartAsync();
                SetButtonRecordingStyle();

            } else {
                var audio = await audioRecorder.StopAsync();
                using (MemoryStream ms = new MemoryStream()) {
                    Stream st = audio.GetAudioStream();
                    await st.CopyToAsync(ms);
                    audioArray = ms.ToArray();
                }

                SetButtonRecordedStyle();
                //var player = AudioManager.Current.CreatePlayer(recordedAudio.GetAudioStream());
                //player.Play();
            }
        } else {
            await DisplayAlert("Grabar", "No se concedieron permisos para grabar", "Aceptar");
        }
	}





    public async void OnBtnGuardarClicked(object sender, EventArgs args) {
        Audios audio = new Audios(
            audioArray,
            txtDescripcion.Text
        );

        //Guardado en SQLite y almacenamiento. (Si uno de los datos no es valido, se lanza el alert)
        if (!audio.GetDatosInvalidos().Any()) {
            await App.db.Insert(audio);

            //Guardado del archivo de imagen en fisico.
            string path = Path.Combine(App.audiosDirectory, audio.Nombre);
            using (FileStream audioFile = File.OpenWrite(path)) {
                Stream st = new MemoryStream(audioArray);
                await st.CopyToAsync(audioFile);
            }

            LimpiarCampos();
            await DisplayAlert("Guardar", "Datos guardados", "Aceptar");

        } else {
            string msj = string.Join("\n", audio.GetDatosInvalidos());
            await DisplayAlert("Datos Invalidos:", msj, "Acepar");
        }
    }




    public async void OnBtnListaClicked(object sender, EventArgs args) {
        LimpiarCampos();
        await Navigation.PushAsync(new Listado());
    }



    

    private void SetButtonNormalStyle() {
        btnBtnStartRecording.BackgroundColor = Colors.Black;
        btnBtnStartRecording.BorderColor = Colors.YellowGreen;
        btnBtnStartRecording.ImageSource = new FileImageSource().File = "microphone_ico.png";
        //btnBtnStartRecording.Text = "Grabar";
    }

    private void SetButtonRecordingStyle() {
        btnBtnStartRecording.BackgroundColor = Colors.Red;
        btnBtnStartRecording.BorderColor = Colors.Red;
        btnBtnStartRecording.ImageSource = new FileImageSource().File = "stop_ico.png";
        //btnBtnStartRecording.Text = "Detener";
    }

    private void SetButtonRecordedStyle() {
        btnBtnStartRecording.BackgroundColor = Colors.DeepSkyBlue;
        btnBtnStartRecording.BorderColor = Colors.Cyan;
        btnBtnStartRecording.ImageSource = new FileImageSource().File = "done_ico.png";
        //btnBtnStartRecording.Text = "Detener";
    }


    public void OnBtnLimpiarClicked(object sender, EventArgs args) {
        LimpiarCampos();
    }

    private async void LimpiarCampos() {
        if (audioRecorder.IsRecording) {
            await audioRecorder.StopAsync();
        }
        SetButtonNormalStyle();
        audioArray = new byte[0];
        txtDescripcion.Text = string.Empty;
    }

}