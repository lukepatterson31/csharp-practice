using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TeleprompterConsole
{
    class Program
    {
        static async Task Main()
        {
            await ShowTeleprompter();
        }

        private static async Task ShowTeleprompter()
        {
            var words = ReadFrom("sampleQuotes.txt");
            foreach (var word in words)
            {
                Console.WriteLine(word);
                if (!string.IsNullOrWhiteSpace(word))
                {
                    await Task.Delay(200);
                }
            }
        }

        private static async Task Getinput()
        {
            var delay = 200;
            Action work = () =>
            {
                do
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '>')
                    {
                        delay -= 10;
                    }
                    else if (key.KeyChar == '<')
                    {
                        delay += 10;
                    }
                    else if (key.KeyChar is 'X' or 'x')
                    {
                        break;
                    }


                } while (true);

            };
            await Task.Run(work);
        }
        
        static  IEnumerable<string> ReadFrom(string file)
        {
            using var reader = File.OpenText(file);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var words = line.Split();
                var lineLength = 0;

                foreach (var word in words)
                {
                    yield return word + ' ';
                    if (lineLength > 70)
                    {
                        yield return Environment.NewLine;
                        lineLength = 0;
                    }
                }

                yield return Environment.NewLine;
            }
        }
    }
}
