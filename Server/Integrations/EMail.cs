using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Search;

namespace Server.Integrations {
    public class EMail {

        public class Mail
        {
            public string Subject;
            public string SendDate;
            public string ReceiveDate;
            public string Id;
        }

        public static Mail[] GetMail()
        {
            List<Mail> messages = new List<Mail>();
            int inboxCount = 0;
            using (var client = new ImapClient())
            {
                client.Connect("imap.yandex.ru",993,true);
                client.Authenticate("somelogin", "somepassword");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var query = SearchQuery.DeliveredAfter(DateTime.Now - TimeSpan.FromDays(2));
                var uids = client.Inbox.Search(query);

                var fields = new HashSet<string>();
                fields.Add("Received");
                var f = inbox.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.InternalDate | MessageSummaryItems.UniqueId, fields);

                foreach (var ff in f)
                {
                    var m = new Mail();
                    var mm = inbox.GetMessage(ff.UniqueId);

                    m.Subject = mm.Subject;
                    m.SendDate = ff.Date.ToString();
                    m.ReceiveDate = ff.InternalDate.ToString();
                    m.Id = mm.MessageId;

                    messages.Add(m);
                }

                /*for (int i = inbox.Count - 1; i >= (inbox.Count - 10); i--) {
                    var message = inbox.GetMessage(i);
                    message.da
                    messages.Add(m);
                    //Console.WriteLine("Subject: {0}", message.Subject);
                }*/

                client.Disconnect(true);
            }

            return messages.ToArray();

        }
    }
}
