#addin Cake.Slack

SlackChatMessageSettings _SlackSettings = null;

void ConfigureSlack(string userName, string iconUrl, string webHookUrl) =>
  _SlackSettings =
    new SlackChatMessageSettings
    {
      UserName = userName,
      IconUrl = new Uri(iconUrl),
      IncomingWebHookUrl = webHookUrl,
      LinkNames = true
    };

bool SendSlackMessage(string to, string message, SlackChatMessageAttachment[] attachments = null)
{
  if (_SlackSettings == null)
    throw new InvalidOperationException("Please call ConfigureSlack method before sending messages. You can do this outside of task - just on your script initialization. This will not take precious script time, initialization itself is fast.");

  if(attachments != null)
    return Slack.Chat.PostMessage(to, message, attachments, _SlackSettings).Ok;
  else
    return Slack.Chat.PostMessage(to, message, _SlackSettings).Ok;
}