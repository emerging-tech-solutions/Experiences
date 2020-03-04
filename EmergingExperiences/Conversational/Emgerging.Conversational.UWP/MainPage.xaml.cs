using SharedServices;
using SharedServices.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Emgerging.Conversational.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaCapture mediaCapture;
        bool isCapturingAudio = false;
        bool capturingStoppedForAudioState = false;
        LowLagMediaRecording _mediaRecording;
        SpeechServices speechServices;
        StorageFile audioFile;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeMediaElements();
            speechServices = new SpeechServices();
            speechServices.Authenticate();
        }

        private async Task InitializeMediaElements()
        {
            mediaCapture = new MediaCapture();
            await mediaCapture.InitializeAsync();
            mediaCapture.Failed += MediaCapture_Failed;
        }

        private void MediaCapture_Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            throw new NotImplementedException();
        }

        private async void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!isCapturingAudio)
            {
                await InitializeMediaElements();
                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                mediaCapture.RecordLimitationExceeded += MediaCapture_RecordLimitationExceeded;

                audioFile = await localFolder.CreateFileAsync("audioPayload.wav", CreationCollisionOption.ReplaceExisting);
                _mediaRecording = await mediaCapture.PrepareLowLagRecordToStorageFileAsync(
                        MediaEncodingProfile.CreateWav(AudioEncodingQuality.Low), audioFile);
                isCapturingAudio = true;
                await _mediaRecording.StartAsync();
            }
            else
            {
                await _mediaRecording.StopAsync();
                await _mediaRecording.FinishAsync();
                SendAudioForTranslation(audioFile.Path);
                isCapturingAudio = false;
            }
        }

        private async void SendAudioForTranslation(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                var fileByteArray = ReadFully(stream);
                var selectedLanguage = ((ComboBoxItem)ComboLangSelection.SelectedItem).Content.ToString();
                var speechResult =  await speechServices.SynthesiseSpeech(fileByteArray, selectedLanguage);
                if (speechResult != null)
                  txtInputBox.Text = speechResult.DisplayText;
            }
        }

        public byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private void MediaCapture_RecordLimitationExceeded(MediaCapture sender)
        {
            throw new NotImplementedException();
        }
    }
}
