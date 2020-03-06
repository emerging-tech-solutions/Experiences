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
using Windows.UI.Xaml.Media.Imaging;
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
            SpeechBubbleFromMe.Visibility = Visibility.Collapsed;
            SpeechBubbleFromAi.Visibility = Visibility.Collapsed;
            BlankPlaceHolderFromMe.Visibility = Visibility.Visible;
            DateTime time = DateTime.Now;             // Use current time.
            string format = "dddd, MMMM d, yyyy";   // Use this format.
            DateField.Text = time.ToString(format);
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

        private async void SendAudioForTranslation(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                var fileByteArray = ReadFully(stream);
                var selectedLanguage = ((ComboBoxItem)ComboLangSelection.SelectedItem).Content.ToString();
                var speechResult =  await speechServices.SynthesiseSpeech(fileByteArray, selectedLanguage);
                if (speechResult != null)
                {
                    txtInputBox.Text = speechResult.DisplayText;
                    BlankPlaceHolderFromMe.Visibility = Visibility.Collapsed;
                    SpeechBubbleFromMe.Visibility = Visibility.Visible;
                    SpeechBubbleFromAi.Visibility = Visibility.Visible;
                    txtOutputBox.Text = "That's a good one. Try this: How much wood would a woodchuck chuck if a woodchuck could chuck wood? A woodchuck would chuck as much wood as a woodchuck could chuck if a woodchuck could chuck wood";

                }
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


        private async void MicrophoneIcon_Tapped(object sender, TappedRoutedEventArgs e)
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
                MicrophoneGreen.Visibility = Visibility.Collapsed;
                MicrophoneRed.Visibility = Visibility.Visible;
                //MicrophoneButtonImage.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/InterfaceIcons/chatbot_speak.png"));
                await _mediaRecording.StartAsync();
            }
            else
            {
                await _mediaRecording.StopAsync();
                await _mediaRecording.FinishAsync();
                SendAudioForTranslation(audioFile.Path);
                MicrophoneGreen.Visibility = Visibility.Visible;
                MicrophoneRed.Visibility = Visibility.Collapsed;
                //MicrophoneButtonImage.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/InterfaceIcons/chatbot_listen.png"));
                isCapturingAudio = false;
            }
        }
    }
}
