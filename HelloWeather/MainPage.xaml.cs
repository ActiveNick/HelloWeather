using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace HelloWeather
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        OpenWeatherMapService owms;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            owms = new OpenWeatherMapService();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void ButtonLookup_Click(object sender, RoutedEventArgs e)
        {
            string location = txtLocation.Text.Trim();

            var wr = await owms.GetWeather(location);
            if (wr != null)
            {
                var weatherText = "The current temperature in {0} is {1}°F, with a high today of {2}° and a low of {3}°.";
                string weatherMessage = string.Format(weatherText, wr.Name, (int)wr.MainWeather.Temp, (int)wr.MainWeather.MaximumTemp, (int)wr.MainWeather.MinimumTemp);
                lblMessage.Text = weatherMessage;
                lblTemp.Text = string.Format("{0}°", (int)wr.MainWeather.Temp);
                ReadText(weatherMessage);
            }
        }

        private async void ButtonHelp_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Windows.UI.Popups.MessageDialog("This feature is not implemented yet.", "Help");
            await dlg.ShowAsync();
        }

        private async void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Windows.UI.Popups.MessageDialog("This feature is not implemented yet.", "Settings");
            await dlg.ShowAsync();
        }


        private async void ReadText(string mytext)
        {
            //Reminder: You need to enable the Microphone capabilitiy in Windows Phone projects
            //Reminder: Add this namespace in your using statements
            //using Windows.Media.SpeechSynthesis;

            // The media object for controlling and playing audio.
            MediaElement mediaplayer = new MediaElement();

            // The object for controlling the speech synthesis engine (voice).
            using (var speech = new SpeechSynthesizer())
            {
                //Retrieve the first female voice
                speech.Voice = SpeechSynthesizer.AllVoices
                    .First(i => (i.Gender == VoiceGender.Female && i.Description.Contains("United States")));
                // Generate the audio stream from plain text.
                SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(mytext);

                // Send the stream to the media object.
                mediaplayer.SetSource(stream, stream.ContentType);
                mediaplayer.Play();
            }
        }
    }
}
