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
            await RunTelePrompter();
        }

        private static async Task ShowTeleprompter(TelePrompterConfig config)
        {
            var words = ReadFrom("sampleQuotes.txt");
            foreach (var word in words)
            {
                Console.WriteLine(word);
                if (!string.IsNullOrWhiteSpace(word))
                {
                    await Task.Delay(config.DelayInMilliseconds);
                }
            }
        }

        private static async Task GetInput(TelePrompterConfig config)
        {
            Action work = () =>
            {
                do
                {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '>')
                    {
                        config.UpdateDelay(-10);
                    }
                    else if (key.KeyChar == '<')
                    {
                        config.UpdateDelay(10);                    }
                    else if (key.KeyChar is 'X' or 'x')
                    {
                        break;
                    }


                } while (!config.Done);

            };
            await Task.Run(work);
        }

        private static async Task RunTelePrompter()
        {
            var config = new TelePrompterConfig();
            var displayTask = ShowTeleprompter(config);

            var speedTask = GetInput(config);
            await Task.WhenAny(displayTask, speedTask);


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
                    lineLength += word.Length + 1;
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
