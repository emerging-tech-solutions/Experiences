﻿using Newtonsoft.Json;
using SharedServices;
using SharedServices.Models;
using SharedServices.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
        DispatcherTimer Timer = new DispatcherTimer();
        MediaCapture mediaCapture;
        bool isAudioCapture = true;
        bool isCapturingAudio = false;
        bool capturingStoppedForAudioState = false;
        LowLagMediaRecording _mediaRecording;
        SpeechServices speechServices;
        StorageFile audioFile;
        DateTime lastInteraction;

        public MainPage()
        {
            this.InitializeComponent();
            InitializeMediaElements();
            speechServices = new SpeechServices();
            speechServices.Authenticate();
            InitializeScreen();
        }


        public void InitializeScreen()
        {
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();

            SpeechBubbleFromMe.Visibility = Visibility.Collapsed;
            SpeechBubbleFromAi.Visibility = Visibility.Collapsed;
            BlankPlaceHolderFromMe.Visibility = Visibility.Visible;
            DateTime time = DateTime.Now;             // Use current time.
            string format = "dddd, MMMM d, yyyy";   // Use this format.
            DateField.Text = time.ToString(format);
            isVoiceEntry();
            Pbar.Visibility = Visibility.Collapsed;
            lastInteraction = DateTime.Now;
        }

        private void Timer_Tick(object sender, object e)
        {
            //Clean up screen after 15 seconds
            TimeSpan delay = DateTime.Now- lastInteraction;

            if (delay > TimeSpan.FromSeconds(15))
            {
                BlankPlaceHolderFromMe.Visibility = Visibility.Visible;
                SpeechBubbleFromMe.Visibility = Visibility.Collapsed;
                SpeechBubbleFromAi.Visibility = Visibility.Collapsed;
                Pbar.Visibility = Visibility.Collapsed;
            }
        }

        public void isKeyEntry()
        {
            isAudioCapture = false;
            KeyboardTextInbox.Text = "";
            EnableKeyboardIcon.Visibility = Visibility.Collapsed;
            EnableMicrophoneIcon.Visibility = Visibility.Visible;
            KeyboardInputBox.Visibility = Visibility.Visible;
            MicrophoneIcon.Visibility = Visibility.Collapsed;
        }

        public void isVoiceEntry()
        {
            isAudioCapture = true;
            MicrophoneIcon.Visibility = Visibility.Visible;
            EnableKeyboardIcon.Visibility = Visibility.Visible;
            EnableMicrophoneIcon.Visibility = Visibility.Collapsed;
            KeyboardInputBox.Visibility = Visibility.Collapsed;
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
                    if (speechResult.DisplayText == null)
                        return;
                    
                    if (isAudioCapture)
                    {
                        if (speechResult.DisplayText.Length <= 50)
                            GetBotIntent(txtInputBox.Text);
                        else
                            MetalDetectorDemo(txtInputBox.Text);

                        GetBotIntent(speechResult.DisplayText);
                    }
                    else
                    {
                        if (txtInputBox.Text.Length <= 50)
                            GetBotIntent(txtInputBox.Text);
                        else
                            MetalDetectorDemo(txtInputBox.Text);
                    }
                }
            }
        }

        private void MetalDetectorDemo(string message)
        {
            //HttpClient client = new HttpClient();
            //var postURI = $"https://emergingtech-api.azurewebsites.net/api/Conversational?message={message}";
            //HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, postURI);
            //httpRequest.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            //var response = await client.PostAsync(postURI, httpRequest.Content);
            //if (response.IsSuccessStatusCode)
            //{
            //    var responseString = await response.Content.ReadAsStringAsync();
            //    var botResponse = JsonConvert.DeserializeObject<BotResponse>(responseString);
            //    txtOutputBox.Text = botResponse.TextResponse;

            //    var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //    Byte[] b = Convert.FromBase64String(botResponse.Base64Audio);

            //    Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //    var filePath = localFolder.Path + "\\AudioResponse.mp3";
            //    Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("AudioResponse.mp3"
            //        , CreationCollisionOption.ReplaceExisting);

            //}
        }
        private async void GetBotIntent(string message)
        {
            HttpClient client = new HttpClient();
            var postURI = $"https://emergingtech-api.azurewebsites.net/api/Conversational?message={message}";
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, postURI);
            httpRequest.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(postURI, httpRequest.Content);
            if(response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var botResponse = JsonConvert.DeserializeObject<BotResponse>(responseString);
                txtOutputBox.Text = botResponse.TextResponse;

                var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Byte[] b = Convert.FromBase64String(botResponse.Base64Audio);

                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                var filePath = localFolder.Path + "\\AudioResponse.mp3";
                Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("AudioResponse.mp3"
                    , CreationCollisionOption.ReplaceExisting);

                txtInputBox.Text = message;
                BlankPlaceHolderFromMe.Visibility = Visibility.Collapsed;
                SpeechBubbleFromMe.Visibility = Visibility.Visible;
                SpeechBubbleFromAi.Visibility = Visibility.Visible;
                Pbar.Visibility = Visibility.Collapsed;
                lastInteraction = DateTime.Now;
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
                SpeechBubbleFromMe.Visibility = Visibility.Collapsed;
                SpeechBubbleFromAi.Visibility = Visibility.Collapsed;
                BlankPlaceHolderFromMe.Visibility = Visibility.Visible;
                Pbar.Visibility = Visibility.Visible;
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

        private void KeyboardTextInbox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Pbar.Visibility = Visibility.Visible;
                GetBotIntent(KeyboardTextInbox.Text);
                KeyboardTextInbox.Text = "";
            }
        }

        private void EnableKeyboardIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            isKeyEntry();
        }

        private void EnableMicrophoneIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            isVoiceEntry();
        }
    }
}
