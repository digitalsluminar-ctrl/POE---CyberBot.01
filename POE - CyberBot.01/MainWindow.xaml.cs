using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Speech.Synthesis;
using static System.Formats.Asn1.AsnWriter;

namespace POE___CyberBot._01
{
    public partial class MainWindow : Window
    {
        private int currentQuestionIndex = 0;
        private int score = 0;
        private bool isQuizActive = false;

        private List<(string Question, List<string> Options, string Answer, string Explanation)> quizQuestions;
        private List<string> activityLog = new List<string>();


        private SpeechSynthesizer synthesizer;

       // private string pendingTaskTitle = "";
        //private bool waitingForReminder = false;


        private bool waitingForTask = false;
        private bool waitingForReminderText = false;
        private string pendingReminder = "";


        private TaskWindow taskWindow;

        private string pendingTaskTitle = "";
        private bool waitingForReminder = false;

        public MainWindow()
        {

        InitializeComponent();
            synthesizer = new SpeechSynthesizer();

        

        quizQuestions = new List<(string, List<string>, string, string)>

        {
             (
        "What should you do if you receive an email asking for your password?",
        new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
        "C",
        "Correct! Reporting phishing emails helps prevent scams."
            ),
            (
        "Which of the following is the strongest password?",
        new List<string> { "A) Password123", "B) mydogname", "C) P@55w0rD!", "D) abcdefgh" },
        "C",
        "Correct! A strong password uses symbols, numbers, and capital letters."
            ),
            (
        "What is phishing?",
        new List<string> { "A) A type of malware", "B) A fake website", "C) A social engineering attack", "D) A firewall bypass" },
        "C",
        "Correct! Phishing tricks users into giving away personal info."
            ),
            (
        "True or False: It's safe to use public Wi-Fi for banking.",
        new List<string> { "A) True", "B) False" },
        "B",
        "Correct! Public Wi-Fi is risky for sensitive data like banking."
            ),
            (
        "What is two-factor authentication?",
        new List<string> { "A) Two passwords", "B) Password + another verification step", "C) A fingerprint only", "D) Backup password" },
        "B",
        "Correct! 2FA adds a second layer of security beyond passwords."
            ),
 
            (
        "What is the purpose of a firewall?",
        new List<string> { "A) Encrypts data", "B) Prevents overheating", "C) Filters incoming and outgoing traffic", "D) Backs up files" },
        "C",
        "Correct! Firewalls help filter and control network traffic."
            ),
            (
        "True or False: It's safe to use the same password for multiple accounts.",
        new List<string> { "A) True", "B) False" },
        "B",
        "Correct! Reusing passwords makes you vulnerable to credential stuffing attacks."
            ),
            (
        "Which of the following is a sign of a phishing email?",
        new List<string> { "A) Poor spelling", "B) Urgent request", "C) Unknown sender", "D) All of the above" },
        "D",
        "Correct! All are red flags commonly found in phishing emails."
            ),
            (
        "What does 2FA stand for?",
        new List<string> { "A) Two-Factor Authentication", "B) Two-Firewall Access", "C) File Authorization", "D) Fast Access" },
        "A",
        "Correct! Two-Factor Authentication adds extra security beyond your password."
            ),
            (
        "True or False: Antivirus software guarantees full protection against malware.",
        new List<string> { "A) True", "B) False" },
        "B",
        "Correct! No solution is perfect. Stay cautious even with antivirus installed."
            )

        };


            // 1. Voice Greeting
            PlayVoiceGreeting("Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online.");

            // 2. ASCII Logo Equivalent
            lstChatLog.Items.Add("   POE___CyberBot._01");
            lstChatLog.Items.Add("-----------------------------");

            // 3. Text-based Greeting
            lstChatLog.Items.Add("Bot: Welcome to the Cybersecurity Awareness Bot!");
            lstChatLog.Items.Add("Bot: What's your name?");
            //lstChatLog.Items.Add("Bot: Type 'exit' to - exit ");//
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)           // 1.

        {
            string input = txtUserInput.Text.Trim();

            if (string.IsNullOrWhiteSpace(input)) return;

            lstChatLog.Items.Add("You: " + input);
            txtUserInput.Clear();

            // First response: capture user's name
            if (lstChatLog.Items.Count == 5) // assuming name comes as 5th item
            {
                lstChatLog.Items.Add($"Bot: Nice to meet you, {input}!");
                ShowTopics();
                return;
            }

            string lowerInput = input.ToLower();

            // Handle task entry after NLP trigger
            if (waitingForTask)
            {
                lstChatLog.Items.Add($"Bot: Task added: '{input}'. Would you like to set a reminder for this task?");
                pendingReminder = input;
                waitingForTask = false;
                waitingForReminderText = true;
                return;
            }

            // Handle reminder entry after NLP trigger
            if (waitingForReminderText)
            {
                DateTime? reminderDate = ParseReminderDate(input);
                if (reminderDate.HasValue)
                {
                    lstChatLog.Items.Add($"Bot: Reminder set for: '{pendingReminder}' on {reminderDate.Value.ToShortDateString()}.");
                    // TODO: Save task + reminderDate if you want persistent storage
                }
                else
                {
                    lstChatLog.Items.Add("Bot: Sorry, I couldn't understand that date. Please enter a date like 'in 3 days' or '2025-07-01'.");
                    return; // Keep waiting for a proper date
                }

                waitingForReminderText = false;
                pendingReminder = "";
                return;
            }



            // Handle quiz answer if quiz is active
            if (isQuizActive)
            {
                string answerLetter = input.ToUpper();
                var currentQuestion = quizQuestions[currentQuestionIndex];

                if (answerLetter == "A" || answerLetter == "B" || answerLetter == "C" || answerLetter == "D")
                {
                    if (answerLetter == currentQuestion.Answer.ToUpper())
                    {
                        lstChatLog.Items.Add("Bot: ✅ Correct!");
                        score++;
                        activityLog.Add($"Answered question {currentQuestionIndex + 1} correctly");
                        activityLog.Add($"Answered question {currentQuestionIndex + 1} incorrectly");
                    }
                    else
                    {
                        lstChatLog.Items.Add("Bot: ❌ Incorrect.");
                    }

                    lstChatLog.Items.Add("Bot: " + currentQuestion.Explanation);

                    currentQuestionIndex++;

                    if (currentQuestionIndex < quizQuestions.Count)
                    {
                        DisplayNextQuestion();
                    }
                    else
                    {
                        lstChatLog.Items.Add($"Bot: Quiz complete! You scored {score}/{quizQuestions.Count}.");
                        if (score >= 8)
                            lstChatLog.Items.Add("Bot: 🎉 Great job! You're a cybersecurity pro!");
                        else if (score >= 5)
                            lstChatLog.Items.Add("Bot: 👍 Not bad! Keep learning to stay safe.");
                        else
                            lstChatLog.Items.Add("Bot: 📚 Keep practicing and improving your knowledge.");

                        isQuizActive = false;
                        currentQuestionIndex = 0;
                        score = 0;
                    }

                    return;
                }
            }


            // If the bot is waiting for reminder confirmation
            if (waitingForReminder)
            {
                if (lowerInput.Contains("yes") || lowerInput.Contains("remind"))
                {
                    DateTime reminderDate = DateTime.Now.AddDays(3);

                    // Add task to TaskWindow
                    if (taskWindow != null && taskWindow.IsLoaded)
                    {
                        TaskItem task = new TaskItem
                        {
                            Title = pendingTaskTitle,
                            Description = "Added via chatbot",
                            ReminderDate = reminderDate,
                            IsCompleted = false
                        };

                        taskWindow.lstTasks.Items.Add(task);
                    }

                    lstChatLog.Items.Add($"Bot: Got it! I'll remind you about '{pendingTaskTitle}' on {reminderDate.ToShortDateString()}.");

                    pendingTaskTitle = null;
                    waitingForReminder = false;
                }
                else if (lowerInput.Contains("no"))
                {
                    if (taskWindow != null && taskWindow.IsLoaded)
                    {
                        TaskItem task = new TaskItem
                        {
                            Title = pendingTaskTitle,
                            Description = "Added via chatbot",
                            ReminderDate = null,
                            IsCompleted = false
                        };

                        taskWindow.lstTasks.Items.Add(task);
                    }

                    lstChatLog.Items.Add($"Bot: No problem! Task '{pendingTaskTitle}' added without a reminder.");

                    pendingTaskTitle = null;
                    waitingForReminder = false;
                }
                else
                {
                    lstChatLog.Items.Add("Bot: Please say 'yes' to set a reminder or 'no' to skip.");
                }

                return;
            }

            if (lowerInput.StartsWith("add task"))
            {
                string taskDescription = input.Substring(8).Trim();
                if (string.IsNullOrEmpty(taskDescription))
                {
                    lstChatLog.Items.Add("Bot: Please provide a task description after 'add task'.");
                }
                else
                {
                    // Open TaskWindow if needed
                    if (taskWindow == null || !taskWindow.IsLoaded)
                    {
                        taskWindow = new TaskWindow();
                        taskWindow.Show();
                    }

                    pendingTaskTitle = taskDescription;
                    activityLog.Add($"Task '{taskDescription}' added, awaiting reminder confirmation");
                    waitingForReminder = true;

                    lstChatLog.Items.Add($"Bot: Task added with the description \"{taskDescription}\". Would you like a reminder?");
                }
                return;
            }


            if (lowerInput.Contains("task") || lowerInput.Contains("reminder"))
            {
                OpenTaskWindow();
                return;
            }

            if (lowerInput.StartsWith("add task"))
            {
                string description = input.Substring(8).Trim(); // get the text after "add task"
                lstChatLog.Items.Add($"Bot: Task added with the description \"{description}\". Would you like a reminder?");

                // Store it for reminder flow
                pendingTaskTitle = description;
                waitingForReminder = true;
                return;
            }

            // Check if bot is waiting for reminder confirmation
            if (waitingForReminder)
            {
                if (lowerInput.Contains("yes") || lowerInput.Contains("remind me"))
                {
                    // Example: remind in 3 days — you can improve by parsing actual number from input
                    DateTime reminderDate = DateTime.Now.AddDays(3);

                    lstChatLog.Items.Add($"Bot: Got it! I'll remind you about '{pendingTaskTitle}' on {reminderDate.ToShortDateString()}.");

                    // Create the task with reminder
                    TaskItem task = new TaskItem
                    {
                        Title = pendingTaskTitle,
                        Description = "Added via chatbot",
                        ReminderDate = reminderDate,
                        IsCompleted = false
                    };

                    activityLog.Add($"Task '{pendingTaskTitle}' added with reminder on {reminderDate.ToShortDateString()}");


                    // TODO: Add task to your task collection or TaskWindow list
                    // For now, you can just keep it local or connect with your TaskWindow

                    // Reset flags
                    waitingForReminder = false;
                    pendingTaskTitle = null;
                }
                else if (lowerInput.Contains("no"))
                {
                    lstChatLog.Items.Add($"Bot: No problem! Task '{pendingTaskTitle}' added without a reminder.");

                    TaskItem task = new TaskItem
                    {
                        Title = pendingTaskTitle,
                        Description = "Added via chatbot",
                        ReminderDate = null,
                        IsCompleted = false
                    };

                    activityLog.Add($"Task '{pendingTaskTitle}' added without reminder");


                    // TODO: Add task to task collection

                    waitingForReminder = false;
                    pendingTaskTitle = null;
                }
                else
                {
                    lstChatLog.Items.Add("Bot: Please respond with 'yes' or 'no' to set a reminder.");
                }

                return; // Prevent further processing until resolved
            }

            // NLP Simulation - Basic keyword detection
            else if (lowerInput.Contains("add") && lowerInput.Contains("task"))
            {
                lstChatLog.Items.Add("Bot: Sure! What task would you like to add?");
                waitingForTask = true;
                return;
            }
            else if (lowerInput.Contains("remind") || lowerInput.Contains("reminder"))
            {
                lstChatLog.Items.Add("Bot: What would you like me to remind you about?");
                waitingForReminderText = true;
                return;
            }



            else if (lowerInput.Contains("quiz") || lowerInput.Contains("game") || lowerInput.Contains("test"))
            {
                lstChatLog.Items.Add("Bot: Great! Let's begin the quiz.");
                isQuizActive = true;
                activityLog.Add("Started the cybersecurity quiz");
                currentQuestionIndex = 0;
                score = 0;
                DisplayNextQuestion();
                return;
            }

            if (lowerInput.StartsWith("add task") || lowerInput.StartsWith("remind me to"))
            {
                pendingTaskTitle = input; // Save the user's task
                waitingForReminder = true;

                lstChatLog.Items.Add($"Bot: Task added: '{pendingTaskTitle}'. Would you like a reminder?");
                activityLog.Add($"Task added: {pendingTaskTitle}");

                return;
            }


            if (lowerInput.Contains("password") || lowerInput == "1")
            {
                TellAboutPasswords();
            }
            else if (lowerInput.Contains("phishing") || lowerInput == "2")
            {
                TellAboutPhishing();
            }
            else if (lowerInput.Contains("browsing") || lowerInput == "3")
            {
                TellAboutSafeBrowsing();
            }
            else if (lowerInput.Contains("Firewalls") || lowerInput == "4")
            {
                TellAboutFirewalls();
            }
            else if (lowerInput.Contains("Hacking") || lowerInput == "5")
            {
                TellAboutHacking();
            }
            else if (lowerInput == "exit")
            {
                lstChatLog.Items.Add("Bot: Goodbye! Stay safe online!");
                this.Close(); // or Application.Current.Shutdown();
            }

            else if (lowerInput.Contains("activity log") || lowerInput.Contains("what have you done") || lowerInput.Contains("show log"))
            {
                lstChatLog.Items.Add("Bot: Here’s a summary of recent actions:");

                var recentActions = activityLog.TakeLast(5).ToList();

                if (recentActions.Count == 0)
                {
                    lstChatLog.Items.Add("Bot: No actions logged yet.");
                }
                else
                {
                    int count = 1;
                    foreach (var action in recentActions)
                    {
                        lstChatLog.Items.Add($"{count}. {action}");
                        count++;
                    }
                }
                return;
            }

            else if (waitingForReminder)
            {
                if (lowerInput.Contains("day") || lowerInput.Contains("days"))
                {
                    // Try to extract a number
                    int days = ExtractNumberFromInput(lowerInput);
                    if (days > 0)
                    {
                        lstChatLog.Items.Add($"Bot: Got it! I'll remind you to '{pendingTaskTitle}' in {days} days.");
                        activityLog.Add($"Reminder set: '{pendingTaskTitle}' in {days} days.");
                    }
                    else
                    {
                        lstChatLog.Items.Add("Bot: I couldn't understand how many days. Please try again.");
                    }
                }
                else
                {
                    lstChatLog.Items.Add($"Bot: Okay, no reminder set for '{pendingTaskTitle}'.");
                    activityLog.Add($"Task added without reminder: {pendingTaskTitle}");
                }

                // Reset state
                pendingTaskTitle = "";
                waitingForReminder = false;

                return;
            }


            else
            {
                lstChatLog.Items.Add("Bot: I didn't quite understand that. Could you rephrase it?");
            }

        }

        private void btnStartQuiz_Click(object sender, RoutedEventArgs e)
        {
            if (!isQuizActive)
            {
                isQuizActive = true;
                score = 0;
                currentQuestionIndex = 0;
                lstChatLog.Items.Add("Bot: Starting Cybersecurity Quiz! Type A, B, C or D.");
                AskNextQuestion();
            }
            else
            {
                lstChatLog.Items.Add("Bot: You're already taking the quiz.");
            }
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            lstChatLog.Items.Add("Bot: Goodbye! Stay safe online!");
            this.Close(); // or Application.Current.Shutdown();
        }




        private void ShowTopics()
        {
            lstChatLog.Items.Add("Bot: Here are some topics you can ask me about:");
            lstChatLog.Items.Add("1. Passwords");
            lstChatLog.Items.Add("2. Phishing");
            lstChatLog.Items.Add("3. Safe Browsing");
            lstChatLog.Items.Add("4. Firewalls");
            lstChatLog.Items.Add("5. Hacking");

            lstChatLog.Items.Add("Type the number or the keyword.");

            lstChatLog.Items.Add("To exit the chatbot, click the 'Exit' button or type 'exit'.");

            lstChatLog.Items.Add("To manage tasks or reminders, type 'task' or 'reminder'.");

        }

        private void TellAboutPasswords()
        {
            lstChatLog.Items.Add("Bot: Passwords are a critical part of online security.");
            lstChatLog.Items.Add("- Use strong, unique passwords for each of your accounts.");
            lstChatLog.Items.Add("- A strong password is at least 12 characters long with a mix of letters, numbers, and symbols.");
            lstChatLog.Items.Add("- Consider using a password manager.");
            lstChatLog.Items.Add("- Never share your passwords.");
            lstChatLog.Items.Add("- Enable two-factor authentication (2FA) whenever possible.");
        }

        private void TellAboutPhishing()
        {
            lstChatLog.Items.Add("Bot: Phishing uses deceptive messages to trick you.");
            lstChatLog.Items.Add("- Be cautious of unsolicited messages.");
            lstChatLog.Items.Add("- Verify the sender’s identity.");
            lstChatLog.Items.Add("- Watch for typos and suspicious links.");
            lstChatLog.Items.Add("- Never enter credentials on an untrusted site.");
        }

        private void TellAboutSafeBrowsing()
        {
            lstChatLog.Items.Add("Bot: Safe browsing helps you avoid online threats.");
            lstChatLog.Items.Add("- Keep your browser and antivirus up to date.");
            lstChatLog.Items.Add("- Don’t click unknown links.");
            lstChatLog.Items.Add("- Avoid downloading from untrusted sources.");
            lstChatLog.Items.Add("- Use private search engines.");
            lstChatLog.Items.Add("- Use a VPN on public Wi-Fi.");
        }

        private void TellAboutFirewalls()
        {
            lstChatLog.Items.Add("Bot: Firewalls help you to scan the header information of data packets coming into a system.");
            lstChatLog.Items.Add("- They deicde whether to drop the data packet or to let it through based on their configurations");
            lstChatLog.Items.Add("- The Differnt types of firewalls are :Packet-Filtering Firewall");
            lstChatLog.Items.Add("- Stateful Inspection Firewall");
            lstChatLog.Items.Add("- Static Filtering Firewall");
            lstChatLog.Items.Add("- Application Layer Firewall");
        }

        private void TellAboutHacking()
        {
            lstChatLog.Items.Add("Bot: Hacking is the act of gaining unauthorized access to computer systems, networks, or devices.");
            lstChatLog.Items.Add("- Two types of hackers are:");
            lstChatLog.Items.Add("- Expert Hackers");
            lstChatLog.Items.Add("- Unskilled Hackers");
            lstChatLog.Items.Add("- Expert Hackers design malware scripts adn they know the systems that they hack.");
            lstChatLog.Items.Add("- Unskilled Hackers use scripts from expert hackers and they are not familiar with system their hacking.");
        }

        private void OpenTaskWindow()
        {
            TaskWindow taskWindow = new TaskWindow();
            taskWindow.ShowDialog();
        }


        private void PlayVoiceGreeting(string message)
        {
            try
            {
                synthesizer.SelectVoice("Microsoft David Desktop");
                synthesizer.SpeakAsync(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Voice greeting error: " + ex.Message);
            }
        }

        private void AskNextQuestion()
        {
            var q = quizQuestions[currentQuestionIndex];
            lstChatLog.Items.Add($"Question {currentQuestionIndex + 1}: {q.Question}");
            foreach (var option in q.Options)
            {
                lstChatLog.Items.Add(option);
            }
        }

        private void DisplayNextQuestion()
        {
            var question = quizQuestions[currentQuestionIndex];
            lstChatLog.Items.Add($"Bot: {question.Question}");
            foreach (var option in question.Options)
            {
                lstChatLog.Items.Add(option);
            }
            lstChatLog.Items.Add("Please enter your answer (A, B, C, or D):");
        }

        private DateTime? ParseReminderDate(string input)
        {
            input = input.ToLower().Trim();

            if (input.StartsWith("in "))
            {
                // Example: "in 3 days"
                string[] parts = input.Split(' ');
                if (parts.Length >= 3 && int.TryParse(parts[1], out int days))
                {
                    if (parts[2].StartsWith("day")) // covers "day" or "days"
                    {
                        return DateTime.Now.AddDays(days);
                    }
                }
            }
            else if (DateTime.TryParse(input, out DateTime specificDate))
            {
                return specificDate;
            }

            return null; // could not parse
        }

        private int ExtractNumberFromInput(string input)
        {
            string[] words = input.Split(' ');
            foreach (string word in words)
            {
                if (int.TryParse(word, out int number))
                {
                    return number;
                }
            }
            return 0;
        }


    }

}



