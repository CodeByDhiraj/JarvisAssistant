# JarvisAssistant

**JarvisAssistant** is a smart desktop AI assistant built using **C#** and **Windows Forms**. It combines the power of **Google Gemini API** with desktop automation and web control. Instead of voice commands, users type their prompts, and Jarvis responds with voice using the system's speech synthesizer.

---

## Features

### 1. AI-Powered Chat (Google Gemini API)
- Accepts text input and processes it using the **Google Gemini AI API**.
- Replies are spoken aloud using **System.Speech.Synthesis**.
- Offers a human-like conversational experience for questions, prompts, and creative tasks.

### 2. System Utility Integration
- Launches essential Windows applications like:
  - Notepad
  - Calculator
  - Command Prompt
  - Control Panel
- Easily extendable to support more system tools.

### 3. Web & Google Search
- Opens predefined websites like:
  - YouTube
  - Instagram
  - Google
- Built-in **“Search for command”** that automatically searches user queries on Google.

---
## Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/CodeByDhiraj/JarvisAssistant.git


2. Open in Visual Studio

Open JarvisAssistant.sln in Visual Studio.



3. Install Dependencies

Make sure .NET Framework is installed (preferably 4.7 or above).

Install System.Speech if not already included.



4. Add Your Gemini API Key

Open the main code file and replace the placeholder with your actual API key:

string apiKey = "YOUR_GEMINI_API_KEY";



5. Build and Run

Press F5 or click Start to build and run the assistant.

Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you'd like to change.

Author

Developed by CodeByDhiraj

Let me know if you want me to create a version with live links, badges, or markdown enhancements like collapsible sections. I can also generate placeholder screenshots if you want.
