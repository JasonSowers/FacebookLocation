using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FacebookLocation.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            dynamic data = JObject.Parse(activity.ChannelData.ToString());

            string longitude = data.message.attachments[0].payload.coordinates["long"].ToString();
            string latitude = data.message.attachments[0].payload.coordinates["lat"].ToString();
           
            // return our reply to the user
            await context.PostAsync(longitude + "\n\n" + latitude);
            
            context.Wait(MessageReceivedAsync);
        }
    }


    public class FacebookJSON
    {
        public Sender sender { get; set; }
        public Recipient recipient { get; set; }
        public long timestamp { get; set; }
        public Message message { get; set; }
    }

    public class Sender
    {
        public string id { get; set; }
    }

    public class Recipient
    {
        public string id { get; set; }
    }

    public class Message
    {
        public string mid { get; set; }
        public int seq { get; set; }
        public bool is_echo { get; set; }
        public Attachment[] attachments { get; set; }
    }

    public class Attachment
    {
        public string type { get; set; }
        public Payload payload { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Payload
    {
        public Coordinates coordinates { get; set; }
    }

    public class Coordinates
    {
        public float lat { get; set; }
        public float _long { get; set; }
    }

}