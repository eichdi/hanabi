using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using hanabi.Controller;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;
using System.IO;
namespace hanabi
{
   
    class Program
    {
        static void Main(string[] args)
        {
            var Bot = new Api("225115203:AAH_vGJDopLajGzNSK16YkQLjGBCZzVUT10");

            Run(Bot).Wait();
        }

        static async Task Run(Api Bot)
        {


            var me = await Bot.GetMe();

            Console.WriteLine("Hello my name is {0}", me.Username);

            var offset = 0;

            while (true)
            {
                var updates = await Bot.GetUpdates(offset);
                

                foreach (var update in updates)
                {
                    if (update.Message.Type == MessageType.TextMessage)
                    {
                        await Bot.SendChatAction(update.Message.Chat.Id, ChatAction.Typing);
                        //await Task.Delay(2000);
                        var t = await Bot.SendTextMessage(update.Message.Chat.Id, update.Message.Text);
                        Console.WriteLine("Echo Message: {0}", update.Message.Text);
                    }

                    if (update.Message.Type == MessageType.PhotoMessage)
                    {
                        var file = await Bot.GetFile(update.Message.Photo.LastOrDefault().FileId);

                        Console.WriteLine("Received Photo: {0}", file.FilePath);

                        var filename = file.FileId+"."+file.FilePath.Split('.').Last();

                        using (var profileImageStream = File.Open(filename, FileMode.Create))
                        {
                            await file.FileStream.CopyToAsync(profileImageStream);
                        }
                    }

                    offset = update.Id + 1;
                }

                //await Task.Delay(1000);
            }
        }
        //static void Main(string[] args)
        //{

           
        //    int level = 2;
        //    Command com = new Command(level);
        //    string result;
        //    var bot = new Api("your token");
        //    //string scommand = Console.ReadLine();
        //    //while (scommand != null)
        //    //{
        //    //    com.DoFunc(scommand);
        //    //    result = com.GetResultGame();
        //    //    if (result != null)
        //    //    {
        //    //        Console.WriteLine(result);
        //    //    }
        //    //    scommand = Console.ReadLine();
        //    //}
        //}
    }
}
