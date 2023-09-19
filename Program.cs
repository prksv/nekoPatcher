using Pastel;
using System.Diagnostics;
using System.Drawing;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace nekoPatcher
{
    class Program
    {
        static string TELEGRAM_TOKEN = "6250036960:AAHX37D4y-LmjHjPANHtF9qRF7jRfHn2dsg";   

        static string NEKO_LAUNCHER_PATH = "NekoSolution.exe";
        static string CHAT_ID = "-807578986";
        static string[] NEKO_API_URLS = { "api.neko-solution.com" };
        static string[] IP_ADDRESSES = { 
            "64.233.164.94", 
            "104.21.49.14", 
            "172.67.158.8"
        };
        static string FULL_NEKO_LAUNCHER_PATH = Path.GetFullPath(NEKO_LAUNCHER_PATH);

        static string LOGO = "    _   __________ ______     ____  ___  ______________  ____________ \r\n   / | / / ____/ //_/ __ \\   / __ \\/   |/_  __/ ____/ / / / ____/ __ \\\r\n  /  |/ / __/ / ,< / / / /  / /_/ / /| | / / / /   / /_/ / __/ / /_/ /\r\n / /|  / /___/ /| / /_/ /  / ____/ ___ |/ / / /___/ __  / /___/ _, _/ \r\n/_/ |_/_____/_/ |_\\____/  /_/   /_/  |_/_/  \\____/_/ /_/_____/_/ |_|  \r\n                                                                      ";


        static void Main(string[] args)
        {

            var banMachine = new BanMachine(NEKO_API_URLS, IP_ADDRESSES, FULL_NEKO_LAUNCHER_PATH);
            var bot = new TelegramBotClient(TELEGRAM_TOKEN);

            void sendLog(string text)
            {
                try
                {
                    string messageText = $"🔑 *[{System.Environment.MachineName}]* {text}.";
                    Message message = bot.SendTextMessageAsync(CHAT_ID, messageText, Telegram.Bot.Types.Enums.ParseMode.Markdown).GetAwaiter().GetResult();
                    Console.WriteLine("[TELEGRAM-BOT] Лог отправлен.".Pastel(Color.Aqua));
                }
                catch
                {
                    Console.WriteLine("Не удалось отправить лог!".Pastel(Color.Red));
                }
            }

            Console.WriteLine(LOGO.Pastel(Color.Purple));
            Console.WriteLine("made by @snakePattern".Pastel(Color.Purple));
            Console.WriteLine(" ");
            try
            {
                banMachine.UnbanAPI();
            } catch
            {
                Console.WriteLine("Ошибка при определении правил.".Pastel(Color.Red));
            }

            var browserPath = Utils.GetPathToDefaultBrowser();
            var pathParts = browserPath.Split("\\");
            string browserName = pathParts[pathParts.Length - 1];

            Console.WriteLine($"[{browserName}] Браузер обнаружен.".Pastel(Color.GreenYellow));
            sendLog("Запущен патчер");

            Console.WriteLine("Запускаем Neko...");

            try
            {
                var nekoProcess = new ProcessStartInfo(NEKO_LAUNCHER_PATH);
                nekoProcess.UseShellExecute = true;
                Process.Start(nekoProcess);
                Console.WriteLine("Neko запущен!".Pastel(Color.Green));
            }
            catch
            {
                Console.WriteLine("Не удалось запустить Neko!".Pastel(Color.Red));
            }

            Thread.Sleep(1000 * 60);

            banMachine.BanAPI();

            Thread.Sleep(5000);
            sendLog("Патчер закрыт");

            Environment.Exit(1);

            Console.ReadLine();
        }
    }
}