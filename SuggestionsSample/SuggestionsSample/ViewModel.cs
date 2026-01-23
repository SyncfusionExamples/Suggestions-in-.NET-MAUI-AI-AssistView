using Syncfusion.Maui.AIAssistView;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace SuggestionsSample
{
    public class ViewModel : INotifyPropertyChanged
    {
        private IAssistItem? requestItem;
        private ObservableCollection<IAssistItem>? messages;
        private List<List<string>> suggestionlist = new List<List<string>>();
        public ICommand AssistViewRequestCommand { get; set; }
        public ObservableCollection<IAssistItem> Messages
        {
            get
            {
                return messages!;
            }

            set
            {
                messages = value;
                RaisePropertyChanged(nameof(Messages));
            }
        }

        public ObservableCollection<ISuggestion> Suggestions { get; set; }

        public ViewModel()
        {
            Suggestions = new ObservableCollection<ISuggestion>();
            AssistViewRequestCommand = new Command<object>(ExecuteRequestCommand);
            Messages = new ObservableCollection<IAssistItem>();
            PopulateDefaultSuggestions();
            GenerateSuggestions();
        }

        private void GenerateSuggestions()
        {
            List<string> firstHeaderSuggestion = new List<string> { "Initiative", "Responsibility", "Accountability" };
            List<string> secondHeaderSuggestion = new List<string> { "Different Perspective", "More Ideas" };
            List<string> thirdHeaderSuggestion = new List<string> { "Active Listening", "Passive Listening" };
            suggestionlist.Add(firstHeaderSuggestion);
            suggestionlist.Add(secondHeaderSuggestion);
            suggestionlist.Add(thirdHeaderSuggestion);
        }

        private ObservableCollection<string> HeaderSuggestions { get; set; } = new ObservableCollection<string>
        {
            "Ownership",
            "Brainstorming",
            "Listening",
            "Resilience"
        };

        private ObservableCollection<string> ImagesCollection { get; set; } = new ObservableCollection<string>
        {
            "ownership.png",
            "brainstorming.png",
            "listening.png",
            "resilience.png"
        };

        private async void ExecuteRequestCommand(object obj)
        {
            requestItem = (obj as RequestEventArgs)!.RequestItem;
            await GetResponseWithSuggestion(requestItem).ConfigureAwait(true);
        }

        private async Task GetResponseWithSuggestion(object inputQuery)
        {
            await Task.Delay(2000).ConfigureAwait(true);
            AssistItem request = (AssistItem)inputQuery;
            if (request != null)
            {
                var suggestion = this.GetSuggestion(request.Text);
                await Task.Delay(1000).ConfigureAwait(true);
                var responseText = GetSolutionText(request.Text);
                AssistItem responseItem = new AssistItem() { Text = responseText, Suggestion = suggestion };
                responseItem.RequestItem = request;
                Messages.Add(responseItem);
            }
        }

        private AssistItemSuggestion GetSuggestion(string prompt)
        {
            var promptSuggestions = new AssistItemSuggestion();
            if (string.IsNullOrWhiteSpace(prompt))
                return promptSuggestions;
            int index = HeaderSuggestions.ToList()
                         .FindIndex(s => string.Equals(s, prompt, StringComparison.OrdinalIgnoreCase));

            if (index >= 0 && index < suggestionlist.Count)
            {
                var suggestions = new ObservableCollection<ISuggestion>();
                foreach (var item in suggestionlist[index])
                {
                    suggestions.Add(new AssistSuggestion() { Text = item });
                }
                promptSuggestions.Items = suggestions;
                promptSuggestions.Orientation = SuggestionsOrientation.Horizontal;
            }

            return promptSuggestions;
        }

        private string GetSolutionText(string requestText)
        {
            if (string.IsNullOrWhiteSpace(requestText))
                return string.Empty;

            string cleaned = Regex.Replace(requestText, @"[^\w\s]", "");
            for (int i = 0; i < promptRequest.Length; i++)
            {
                string pattern = $@"\b{Regex.Escape(cleaned)}\b";
                if (Regex.IsMatch(promptRequest[i], pattern, RegexOptions.IgnoreCase))
                {
                    return promptResponse[i].Replace("\n", "<br>");
                }
            }
            return string.Empty;
        }

        internal async Task GetResult(object inputQuery)
        {
            await Task.Delay(2000).ConfigureAwait(true);
            AssistItem request = (AssistItem)inputQuery;
            if (request != null)
            {
                GetSolutionToPrompt(request);
            }
        }

        internal void GetSolutionToPrompt(AssistItem request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Text))
                return;

            for (int i = 0; i < promptRequest.Length; i++)
            {
                string cleaned = Regex.Replace(request.Text, @"[^\w\s]", "");
                string pattern = $@"\b{Regex.Escape(cleaned)}\b";
                var isMatch = Regex.IsMatch(promptRequest[i], pattern, RegexOptions.IgnoreCase);

                if (isMatch)
                {
                    string response = promptResponse[i].Replace("\n", "<br>");
                    if (!string.IsNullOrEmpty(response))
                    {
                        AssistItem responseItem = new AssistItem() { Text = response };
                        responseItem.RequestItem = request;
                        Messages.Add(responseItem);
                    }
                }
            }
        }

        private void PopulateDefaultSuggestions()
        {
            for (int i = 0; i < HeaderSuggestions.Count; i++)
            {
                var image = (i < ImagesCollection.Count) ? ImagesCollection[i] : string.Empty;
                Suggestions.Add(new AssistSuggestion() { Text = HeaderSuggestions[i], ImageSource = image });
            }
        }

        private string[] promptRequest = new string[]
        {
            "Ownership",
            "brainstorming",
            "Listening",
            "resilience",
            "initiative",
            "responsibility",
            "accountability",
            "different perspective",
            "more ideas",
            "active listening",
            "passive listening"
        };

        private string[] promptResponse = new string[]
        {
            "<b>Ownership</b><ol><li>&nbsp;Ownership is about taking initiative.</li><li>&nbsp;It’s an understanding that taking action is your responsibility, not someone else’s.</li><li>&nbsp;It is the fundamental principle that you, as an individual, are accountable for the delivery of an outcome, even though there may be others who have a role to play.</li></ol>",
            "<b>Brainstorming</b><br>When a group of people sit and discuss a problem and try to derive the solution, this is termed brainstorming. Brainstorming helps team members, pool ideas and bounce them off each other, narrowing them down and refining them into a plan. It's useful for gaining different perspectives and more ideas.",
            "<b>Listening</b><br>For a good communication, it is not only enough to convey the information efficiently, but it also needs to include good listening skill. Common types of Listening are Active listening and Passive listening.",
            "<b>Resilience</b><br>Resilience is the ability to bounce back from setbacks, adapt to difficult situations, and keep going despite adversity. It involves having the strength to confront challenges, learn from failures, and continue to grow without becoming overly discouraged. Essentially, resilience is about enduring tough times and emerging stronger.",
            "<b>Initiative</b><br>Taking the first step to start something or to bring new ideas and actions into motion. It means proactively beginning a task or project<ol><li><b>&nbsp;Definition:</b> Taking the first step to start a project, task, or initiative without waiting for direction or instruction.</li><li><b>&nbsp;Importance:</b> Demonstrates proactive behavior and leadership qualities. It shows an individual’s willingness to take charge and bring ideas to life.</li><li><b>&nbsp;Example:</b> An employee noticing a gap in the company's process and developing a plan to address it without being asked.</li></ol>",
            "<b>Responsibility</b><br>Being reliable and dependable in managing tasks or duties. It involves making decisions and completing tasks that one is assigned or has taken on.<ol><li><b>&nbsp;Definition:</b> Ability to be trusted with the duties and tasks associated with a particular role or project.</li><li><b>&nbsp;Importance:</b> Ensures that tasks are assigned and managed efficiently. Responsible individuals are committed to fulfilling their roles and meeting expectations.</li><li><b>&nbsp;Example:</b> A project manager overseeing the progress of a project, ensuring deadlines are met and resources are utilized effectively.</li></ol>",
            "<b>Accountability</b><br>Being answerable for the outcomes of your actions and decisions. It means acknowledging and accepting the consequences, whether they are successes or failures.<ol><li><b>&nbsp;Definition:</b> Being answerable for the outcomes of one's actions and decisions. This involves acknowledging both successes and failures.</li><li><b>&nbsp;Importance:</b> Encourages trust and transparency within a team or organization. It fosters a culture of continuous improvement and ethical behavior.</li><li><b>&nbsp;Example:</b> A team leader accepting responsibility for a project's failure, analyzing what went wrong, and developing strategies to avoid similar issues in the future.</li></ol>",
            "<b>Different Perspective</b><br>Each person has a different way of approaching a problem. It might differ with the way they see it, due to their unique understanding, way of thinking or experience. While brainstorming, participants can show each other different angles of a problem, giving everyone a better view of it.",
            "<b>More Ideas</b><br>When a group brainstorms, they can come up with a lot of ideas for a solution that don't stick to the usual approach. There are better chances of coming up with an innovative solution, as well as plan Bs and Cs. Sometimes, groups can try integrating the positives of each solution and can thus derive an effective solution for that problem.",
            "<b>Active Listening</b><br>Active listening is a two-way communication. The listener puts in active effort to understand the information conveyed by the speaker and react to it. ",
            "<b>Passive Listening</b><br>In passive listening, a person just tries to know what the speaker is trying to say. It is just like one way communication."
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
