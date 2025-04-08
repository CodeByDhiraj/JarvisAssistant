/*using System;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Text;
using System.Diagnostics;

namespace GeminiJarvis
{
    public partial class MainWindow : Window
    {
     
        private const string GeminiApiKey = "AIzaSyCUkLDnt77mUAhNJGBS0t5j4_SItCgCQBc"; 
        private const string GeminiEndpoint = "https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent";
        private SpeechRecognitionEngine speechRecognizer;
        private SpeechSynthesizer speechSynthesizer;
        private bool isListening = false;
        private readonly HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpeechSystem();
        }

        private void InitializeSpeechSystem()
        {
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();

            speechRecognizer = new SpeechRecognitionEngine();
            var grammar = new DictationGrammar();
            speechRecognizer.LoadGrammar(grammar);
            speechRecognizer.SpeechRecognized += SpeechRecognizedHandler;
            speechRecognizer.SetInputToDefaultAudioDevice();
        }

        private async Task<string> GetGeminiResponse(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                Console.WriteLine("Request JSON: " + json); 
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var apiUrl = $"{GeminiEndpoint}?key={GeminiApiKey}";
                var response = await httpClient.PostAsync(apiUrl, content);

                // Check for successful response
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return $"Error: {response.StatusCode} - {errorContent}";
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<GeminiResponse>(responseJson);

                return responseObject?.candidates?[0]?.content?.parts?[0]?.text?.Trim() ?? "Sorry, I couldn't process that.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private async void SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            var userInput = e.Result.Text;
            Dispatcher.Invoke(() => AddMessage($"You: {userInput}", true));
            await ProcessUserInput(userInput);
        }

        private async Task ProcessUserInput(string input)
        {
            try
            {
                input = input.ToLower();

                if (ExecuteOfflineCommand(input))
                {
                    return;
                }

                string response = await GetGeminiResponse(input);
                Dispatcher.Invoke(() => AddMessage($"Jarvis: {response}", false));
                SpeakResponse(response);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AddMessage($"Error: {ex.Message}", false));
            }
        }

        private bool ExecuteOfflineCommand(string input)
        {
            try
            {
                if (input.Contains("open notepad"))
                {
                    Process.Start("notepad.exe");
                    SpeakResponse("Opening Notepad");
                    return true;
                }
                else if (input.Contains("open calculator"))
                {
                    Process.Start("calc.exe");
                    SpeakResponse("Opening Calculator");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AddMessage($"Error: {ex.Message}", false));
                return false;
            }
        }

        private void AddMessage(string message, bool isUser)
        {
            var messageBlock = new TextBlock
            {
                Text = message,
                Margin = new Thickness(5),
                Foreground = isUser ? System.Windows.Media.Brushes.Blue : System.Windows.Media.Brushes.Green
            };

            ChatPanel.Children.Add(messageBlock);
            ChatScrollViewer.ScrollToEnd();
        }

        private void SpeakResponse(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                speechSynthesizer.SpeakAsync(text);
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputBox.Text))
            {
                var input = InputBox.Text;
                InputBox.Clear();
                AddMessage($"You: {input}", true);
                await ProcessUserInput(input);
            }
        }

        private void InputBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private void VoiceToggle_Click(object sender, RoutedEventArgs e)
        {
            if (!isListening)
            {
                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
                StatusLight.Fill = System.Windows.Media.Brushes.Green;
                isListening = true;
            }
            else
            {
                speechRecognizer.RecognizeAsyncStop();
                StatusLight.Fill = System.Windows.Media.Brushes.Red;
                isListening = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            speechRecognizer?.Dispose();
            speechSynthesizer?.Dispose();
            httpClient?.Dispose();
        }
    }

    public class GeminiResponse
    {
        public Candidate[] candidates { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public Part[] parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }
}*/



/*using System;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Text;
using System.Diagnostics;
using System.Net;


namespace GeminiJarvis
{
    public partial class MainWindow : Window
    {
        // Gemini API Configuration
        private const string GeminiApiKey = "AIzaSyCpK0T34Rb2LqQP5eOuO9mGldGJ1lLp51o";
        private const string GeminiEndpoint = "https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent";



        // Speech components
        private SpeechRecognitionEngine speechRecognizer;
        private SpeechSynthesizer speechSynthesizer;
        private bool isListening = false;
        private readonly HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpeechSystem();
        }

        private void InitializeSpeechSystem()
        {
            // Speech synthesis
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();

            // Speech recognition
            speechRecognizer = new SpeechRecognitionEngine();
            var grammar = new DictationGrammar();
            speechRecognizer.LoadGrammar(grammar);
            speechRecognizer.SpeechRecognized += SpeechRecognizedHandler;
            speechRecognizer.SetInputToDefaultAudioDevice();
        }

        private async Task<string> GetGeminiResponse(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var apiUrl = $"{GeminiEndpoint}?key={GeminiApiKey}";
                var response = await httpClient.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<GeminiResponse>(responseJson);

                return responseObject?.candidates?[0]?.content?.parts?[0]?.text?.Trim() ?? "Sorry, I couldn't process that.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private async void SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            var userInput = e.Result.Text;
            Dispatcher.Invoke(() => AddMessage($"You: {userInput}", true));
            await ProcessUserInput(userInput);
        }

        private async Task ProcessUserInput(string input)
        {
            try
            {
                input = input.ToLower();

                // Check if it's an offline command
                if (ExecuteOfflineCommand(input))
                {
                    return; // Stop execution if offline command was executed
                }

                // Otherwise, process with Gemini AI
                string response = await GetGeminiResponse(input);
                Dispatcher.Invoke(() => AddMessage($"Jarvis: {response}", false));
                SpeakResponse(response);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AddMessage($"Error: {ex.Message}", false));
            }
        }

        private bool ExecuteOfflineCommand(string input)
        {
            try
            {
                if (input.Contains("open notepad"))
                {
                    Process.Start("notepad.exe");
                    SpeakResponse("Opening Notepad");
                    return true;
                }
                else if (input.Contains("open calculator"))
                {
                    Process.Start("calc.exe");
                    SpeakResponse("Opening Calculator");
                    return true;
                }
                else if (input.Contains("open file explorer"))
                {
                    Process.Start("explorer.exe");
                    SpeakResponse("Opening File Explorer");
                    return true;
                }
                else if (input.Contains("open instagram"))
                {
                    Process.Start("cmd.exe", "/c start https://www.instagram.com");
                    SpeakResponse("Opening Instagram");
                    return true;
                }
                else if (input.Contains("open browser"))
                {
                    Process.Start("cmd.exe", "/c start https://www.google.com");
                    SpeakResponse("Opening Browser");
                    return true;
                }
                else if (input.StartsWith("search for"))
                {
                    string query = input.Replace("search for", "").Trim();
                    string searchUrl = $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";
                    Process.Start("cmd.exe", $"/c start {searchUrl}");
                    SpeakResponse($"Searching Google for {query}");
                    return true;
                }
                else if (input.StartsWith("open website"))
                {
                    // Predefined website list
                    Dictionary<string, string> websites = new Dictionary<string, string>()
            {
                { "fallmodz", "https://FallModz.in" },
                { "youtube", "https://youtube.com" },
                { "instagram", "https://instagram.com" },
                { "twitter", "https://twitter.com" },
                { "video", "https://youtu.be/FqBrf-Fs808?si=i5g5Z-00Q_NPAk6M" },
                { "video2", "https://youtu.be/W8x6Dwyj0-A?si=9DkbrK0dNyWFEM4z" },
                { "playsong", "https://youtu.be/t8yVk0bm684?si=JGPC0gk6vggcCZFp" },
                { "song", "https://youtu.be/t8yVk0bm684?si=JGPC0gk6vggcCZFp" }


            };

                    // Extract the requested site name
                    string siteName = input.Replace("open website", "").Trim().ToLower();

                    if (websites.ContainsKey(siteName))
                    {
                        string url = websites[siteName];
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });

                        SpeakResponse($"Opening {siteName}.");
                    }
                    else
                    {
                        SpeakResponse("Website not found. Available options are: fallmodz, youtube, instagram, twitter, video, video1, playsong, song.");
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AddMessage($"Error: {ex.Message}", false));
                return false;
            }
        }

        private void AddMessage(string message, bool isUser)
        {
            var messageBlock = new TextBlock
            {
                Text = message,
                Margin = new Thickness(5),
                Foreground = isUser ? System.Windows.Media.Brushes.Blue : System.Windows.Media.Brushes.Green
            };

            ChatPanel.Children.Add(messageBlock);
            ChatScrollViewer.ScrollToEnd();
        }

        private void SpeakResponse(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                // Remove markdown symbols (** for bold, * for italics)
                string cleanText = text.Replace("**", "").Replace("*", "");

                // Speak the cleaned response
                speechSynthesizer.SpeakAsync(cleanText);
            }
        }


        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputBox.Text))
            {
                var input = InputBox.Text;
                InputBox.Clear();
                AddMessage($"You: {input}", true);
                await ProcessUserInput(input);
            }
        }

        private void InputBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private void VoiceToggle_Click(object sender, RoutedEventArgs e)
        {
            if (!isListening)
            {
                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
                StatusLight.Fill = System.Windows.Media.Brushes.Green;
                isListening = true;
            }
            else
            {
                speechRecognizer.RecognizeAsyncStop();
                StatusLight.Fill = System.Windows.Media.Brushes.Red;
                isListening = false;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            speechRecognizer?.Dispose();
            speechSynthesizer?.Dispose();
            httpClient?.Dispose();
        }
    }

    public class GeminiResponse
    {
        public Candidate[] candidates { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public Part[] parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }
}*/


using System;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Text;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Net;

namespace GeminiJarvis
{
    public partial class MainWindow : Window
    {
        // Gemini API Configuration
        private const string GeminiApiKey = "AIzaSyCUkLDnt77mUAhNJGBS0t5j4_SItCgCQBc";
        private const string GeminiEndpoint = "https://generativelanguage.googleapis.com/v1/models/gemini-1.5-flash:generateContent";

        // Speech components
        private SpeechRecognitionEngine speechRecognizer;
        private SpeechSynthesizer speechSynthesizer;
        private bool isListening = false;
        private readonly HttpClient httpClient = new HttpClient();
        private string userName;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpeechSystem();
            GreetUser();
        }
        private void GreetUser()
        {
            userName = Microsoft.VisualBasic.Interaction.InputBox("Hello! What is your name?", "Welcome to Jarvis", "");
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = "User";
            }
            string greetingMessage = $"Hello {userName}, how can I assist you today?";
            AddMessage(greetingMessage, false);
            SpeakResponse(greetingMessage);
        }

        private void InitializeSpeechSystem()
        {
            // Speech synthesis
            speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.SetOutputToDefaultAudioDevice();

            // Speech recognition
            speechRecognizer = new SpeechRecognitionEngine();
            var grammar = new DictationGrammar();
            speechRecognizer.LoadGrammar(grammar);
            speechRecognizer.SpeechRecognized += SpeechRecognizedHandler;
            speechRecognizer.SetInputToDefaultAudioDevice();
        }

        private async Task<string> GetGeminiResponse(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var apiUrl = $"{GeminiEndpoint}?key={GeminiApiKey}";
                var response = await httpClient.PostAsync(apiUrl, content);

                // Check for successful response
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return $"Error: {response.StatusCode} - {errorContent}";
                }

                var responseJson = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<GeminiResponse>(responseJson);

                return responseObject?.candidates?[0]?.content?.parts?[0]?.text?.Trim() ?? "Sorry, I couldn't process that.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private async void SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            var userInput = e.Result.Text;
            Dispatcher.Invoke(() => AddMessage($"You: {userInput}", true));
            await ProcessUserInput(userInput);
        }

        private async Task ProcessUserInput(string input)
        {
            try
            {
                input = input.ToLower();

                // Check if it's an offline command
                if (ExecuteOfflineCommand(input))
                {
                    return; // Stop execution if offline command was executed
                }

                // Otherwise, process with Gemini AI
                string response = await GetGeminiResponse(input);
                Dispatcher.Invoke(() => AddMessage($"Jarvis: {response}", false));
                SpeakResponse(response);
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AddMessage($"Error: {ex.Message}", false));
            }
        }

        private bool ExecuteOfflineCommand(string input)
        {
            try
            {
                if (input.Contains("open notepad"))
                {
                    Process.Start("notepad.exe");
                    SpeakResponse("Opening Notepad");
                    return true;
                }
                else if (input.Contains("open calculator"))
                {
                    Process.Start("calc.exe");
                    SpeakResponse("Opening Calculator");
                    return true;
                }
                else if (input.Contains("open file explorer"))
                {
                    Process.Start("explorer.exe");
                    SpeakResponse("Opening File Explorer");
                    return true;
                }
                else if (input.Contains("open instagram"))
                {
                    Process.Start("cmd.exe", "/c start https://www.instagram.com");
                    SpeakResponse("Opening Instagram");
                    return true;
                }
                else if (input.Contains("open browser"))
                {
                    Process.Start("cmd.exe", "/c start https://www.google.com");
                    SpeakResponse("Opening Browser");
                    return true;
                }
                else if (input.StartsWith("search for"))
                {
                    string query = input.Replace("search for", "").Trim();
                    string searchUrl = $"https://www.google.com/search?q={Uri.EscapeDataString(query)}";
                    Process.Start("cmd.exe", $"/c start {searchUrl}");
                    SpeakResponse($"Searching Google for {query}");
                    return true;
                }
                else if (input.StartsWith("open "))
                {
                    // Predefined website list
                    Dictionary<string, string> open = new Dictionary<string, string>()
                    {
                        { "fallmodz", "https://FallModz.in" },
                        { "youtube", "https://youtube.com" },
                        { "instagram", "https://instagram.com" },
                        { "twitter", "https://twitter.com" },
                        { "video", "https://youtu.be/vZ4vwUxX-Bk" },
                        { "video2", "https://youtu.be/AETFvQonfV8" },
                        { "playsong", "https://youtu.be/HH_a6aRO1TE" },
                        { "song", "https://youtu.be/fdD4_lexi7s" }
                    };

                    // Extract the requested site name
                    string siteName = input.Replace("open ", "").Trim().ToLower();

                    if (open.ContainsKey(siteName))
                    {
                        string url = open[siteName];
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });

                        SpeakResponse($"Opening {siteName}.");
                    }
                    else
                    {
                        SpeakResponse("Website not found. Available options are: fallmodz, youtube, instagram, twitter, video, video1, playsong, song.");
                    }

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() => AddMessage($"Error: {ex.Message}", false));
                return false;
            }
        }

        private void AddMessage(string message, bool isUser)
        {
            var messageBlock = new Border
            {
                BorderThickness = new Thickness(3),
                BorderBrush = isUser ? System.Windows.Media.Brushes.Aqua : System.Windows.Media.Brushes.White,
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(7),
                Margin = new Thickness(7),
                Child = new TextBlock
                {
                    Text = message,
                    FontSize = 16,
                    Foreground = isUser ? System.Windows.Media.Brushes.Aqua : System.Windows.Media.Brushes.White,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(5)
                }
            };

            ChatPanel.Children.Add(messageBlock);
            ChatScrollViewer.ScrollToEnd();
        }

        private void SpeakResponse(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                // Remove markdown symbols (** for bold, * for italics)
                string cleanText = text.Replace("*", "");

                // Speak the cleaned response
                speechSynthesizer.SpeakAsync(cleanText);
            }
        }
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(InputBox.Text))
            {
                var input = InputBox.Text;
                InputBox.Clear();
                AddMessage($"You: {input}", true);
                await ProcessUserInput(input);
            }
        }

        private void InputBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendButton_Click(sender, e);
            }
        }

        private void VoiceToggle_Click(object sender, RoutedEventArgs e)
        {
            if (!isListening)
            {
                speechRecognizer.RecognizeAsync(RecognizeMode.Multiple);
                StatusLight.Fill = System.Windows.Media.Brushes.Green;
                isListening = true;
            }
            else
            {
                speechRecognizer.RecognizeAsyncStop();
                StatusLight.Fill = System.Windows.Media.Brushes.Red;
                isListening = false;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            speechSynthesizer.SpeakAsyncCancelAll();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            speechRecognizer?.Dispose();
            speechSynthesizer?.Dispose();
            httpClient?.Dispose();
        }
    }


    public class GeminiResponse
    {
        public Candidate[] candidates { get; set; }
    }

    public class Candidate
    {
        public Content content { get; set; }
    }

    public class Content
    {
        public Part[] parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }
}


