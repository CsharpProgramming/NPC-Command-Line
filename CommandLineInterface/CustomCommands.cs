using NPC_Terminal.Properties;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;

namespace CommandLineInterface
{
    class CustomCommands
    {
        public static Action<string, Color> Append;

        public static Dictionary<string, Action<string>> Commands = new Dictionary<string, Action<string>>
        {
            { "about", About },
            { "version", Version },
            { "code", Code },
            { "github", Code },
            { "alias", CreateAlias },
            { "createalias", CreateAlias },
            { "weather", Weather },
            { "time", Time },
            { "calc", Calc },
            { "qr", QR },
            { "hash", Hash },
            { "encode", Encode },
            { "uuid", UUID },
            { "ip", IP },
            { "sys", Sys },
            { "network", Network },
            { "password", Password },
            { "rickroll", RickRoll },
            { "sus", Sus },
            { "doge", Doge },
            { "nyan", Nyan },
            { "matrix", Matrix },
            { "cowsay", CowSay },
            { "lenny", Lenny },
            { "ascii", ASCII },
            { "yeet", Yeet },
            { "sigma", Sigma },
            { "skibidi", Skibidi },
            { "ohio", Ohio },
            { "rizz", Rizz },
            { "gyatt", Gyatt },
            { "bussin", Bussin },
            { "sussy", Sussy },
            { "lowkey", LowKey },
            { "nocap", NoCap },
            { "periodt", Periodt },
            { "slaps", Slaps },
        };

        public static void About(string arguments)
        {
            Append(" Welcome to ", Color.White);
            Append("NPC Terminal", Color.MediumSpringGreen);
            Append($" v.{Settings.Default.Version}!\n", Color.White);
            Append(" NPC Terminal is a clone of Windows Terminal but better. With Quality of Life features, better design, and more.\n Supports adding custom commands. Made by youtube.com/@CsharpProgramming\n", Color.White);
        }

        public static void Version(string arguments)
        {
            Append(" NPC Terminal", Color.MediumSpringGreen);
            Append($" v.{Settings.Default.Version}!\n", Color.White);
        }

        public static void Code(string arguments)
        {
            Append(" NPC Terminal", Color.MediumSpringGreen);
            Append(" code available at github.com/CsharpProgramming/NPC-Command-Line\n", Color.White);
        }

        public static void CreateAlias(string arguments)
        {
            try
            {
                var notepad = new Process();
                notepad.StartInfo = new ProcessStartInfo("notepad.exe", Path.Combine(Application.StartupPath, "aliases.txt"));
                notepad.EnableRaisingEvents = true;
                notepad.Exited += (s, e) => { Form1.Instance.Invoke(() => { Form1.Instance.LoadAliasesFromFile(); }); };
                notepad.Start();
            }

            catch (Exception e)
            {
                Append(" Error opening the alias file. Please report it:\n", Color.Yellow);
                Append($" Error: {e}", Color.Yellow);
            }
        }

        public static void Weather(string arguments)
        {
            Append("bro just check outside your window gng\n", Color.Yellow);
        }

        public static void Time(string arguments)
        {
            var now = DateTime.Now;
            Append($" Current time: {now:HH:mm:ss}\n Date: {now:dddd, MMMM dd, yyyy}\n", Color.Cyan);
        }

        public static void Calc(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                Append(" Usage: calc <expression> (like calc 2+2)\n", Color.Yellow);
                return;
            }
            try
            {
                var table = new System.Data.DataTable();
                var result = table.Compute(arguments, null);
                Append($" {arguments} = {result}\n", Color.LightGreen);
            }
            catch
            {
                Append(" That math ain't mathing chief\n", Color.Red);
            }
        }

        public static void QR(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                Append(" Usage: qr <text to encode>\n", Color.Yellow);
                return;
            }
            Append(" QR code generated in your imagination because this is a terminal\n", Color.Magenta);
            Append($" Text: {arguments}\n", Color.White);
        }

        public static void Hash(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                Append(" Usage: hash <text>\n", Color.Yellow);
                return;
            }
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(arguments));
            var hashString = Convert.ToHexString(hash).ToLower();
            Append($" MD5: {hashString}\n", Color.LightBlue);
        }

        public static void Encode(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                Append(" Usage: encode <text>\n", Color.Yellow);
                return;
            }
            var bytes = Encoding.UTF8.GetBytes(arguments);
            var encoded = Convert.ToBase64String(bytes);
            Append($" Base64: {encoded}\n", Color.LightBlue);
        }

        public static void UUID(string arguments)
        {
            var guid = Guid.NewGuid().ToString();
            Append($" Fresh UUID: {guid}\n", Color.Cyan);
        }

        public static async void IP(string arguments)
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var localIP = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                Append($" Local IP: {localIP}\n", Color.LightGreen);

                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(5);
                var publicIP = await client.GetStringAsync("https://api.ipify.org");
                Append($" Public IP: {publicIP.Trim()}\n", Color.LightBlue);
            }
            catch (HttpRequestException)
            {
                Append(" service down bruv \n", Color.Red);
            }
            catch (TaskCanceledException)
            {
                Append(" request timed out\n", Color.Red);
            }
            catch
            {
                Append(" Network said NUH UH\n", Color.Red);
            }
        }

        public static void Sys(string arguments)
        {
            Append($" OS: {Environment.OSVersion}\n", Color.White);
            Append($" Machine: {Environment.MachineName}\n", Color.White);
            Append($" User: {Environment.UserName}\n", Color.White);
            Append($" Processors: {Environment.ProcessorCount}\n", Color.White);
            Append($" RAM: {GC.GetTotalMemory(false) / 1024 / 1024} MB used by this app\n", Color.White);
        }

        public static void Network(string arguments)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send("8.8.8.8", 1000);
                if (reply.Status == IPStatus.Success)
                {
                    Append($" Google DNS ping: {reply.RoundtripTime}ms\n", Color.LightGreen);
                }
                else
                {
                    Append(" Network might be dead for real\n", Color.Red);
                }
            }
            catch
            {
                Append(" Bro dont have internet access\n", Color.Red);
            }
        }

        public static void Password(string arguments)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
            var random = new Random();
            var length = 16;
            if (!string.IsNullOrEmpty(arguments) && int.TryParse(arguments, out int customLength))
                length = Math.Min(Math.Max(customLength, 4), 128);
            var password = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            Append($" gen password: {password}\n", Color.LightGreen);
        }

        public static void RickRoll(string arguments)
        {
            Append("         ⠀⠀⠀⠀⣠⣶⡾⠏⠉⠙⠳⢦⡀⠀⠀⠀⢠⠞⠉⠙⠲⡀⠀\n", Color.Red);
            Append("         ⠀⠀⠀⣴⠿⠏⠀⠀⠀⠀⠀⠀⢳⡀⠀⡏⠀⠀⠀⠀⠀⢷\n", Color.Red);
            Append("         ⠀⠀⢠⣟⣋⡀⢀⣀⣀⡀⠀⣀⡀⣧⠀⢸⠀⠀⠀⠀⠀⠀⣿\n", Color.Red);
            Append("         ⠀⠀⢸⣯⡭⠁⠸⣛⣟⠆⡴⣻⡲⣿⠀⣸ Never gonna give you up\n", Color.White);
            Append("         ⠀⠀⣟⣿⡭⠀⠀⠀⠀⠀⢱⠀⠀⣿⠀⢹ Never gonna let you down\n", Color.White);
            Append("         ⠀⠀⠙⢿⣯⠄⠀⠀⠀⢀⡀⠀⠀⡿⠀⠀⡇ Never gonna run around\n", Color.White);
            Append("         ⠀⠀⠀⠀⠹⣶⠆⠀⠀⠀⠀⠀⡴⠃⠀⠀⠘⠤⣄ and desert you\n", Color.White);
            Append(" You just got rickrolled in ASCII form lol\n", Color.Yellow);
        }

        public static void Sus(string arguments)
        {
            Append("      ⠀⠀⠀⠀⠀⢀⣴⡾⠿⠿⠿⠿⢶⣦⣄⠀⠀⠀\n", Color.Red);
            Append("      ⠀⠀⠀⠀⢠⣿⠁⠀⠀⠀⣀⣀⣀⣈⣻⣷⡄⠀\n", Color.Red);
            Append("      ⠀⠀⠀⠀⣾⡇⠀⠀⣾⣟⠛⠋⠉⠉⠙⠛⢷⣄⠀\n", Color.Red);
            Append("      ⠀⠀⠀⠀⣿⠀⠀⠀⠶⠿⠭⠭⠭⠭⠭⠭⠭⠬⠭⠀\n", Color.Red);
            Append("      ⠀⠀⠀⠀⣿⣷⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n", Color.Red);
            Append("      ⠀⠀⠀⠀⢿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\n", Color.Red);
            Append("      ⠀⠀⠀⠀⠀⠿⣷⣶⣤⣤⣤⣤⣭⣭⣭⣭⣭⣭⡀\n", Color.Red);
            var suspiciousness = new Random().Next(0, 101);
            Append($" That's pretty sus ngl... Suspicion level: {suspiciousness}%\n", Color.Yellow);
            if (suspiciousness > 75) Append(" EMERGENCY MEETING!\n", Color.Red);
        }

        public static void Doge(string arguments)
        {
            Append("                       ▄              ▄\n", Color.Yellow);
            Append("                      ▌▒█           ▄▀▒▌\n", Color.Yellow);
            Append("                      ▌▒▒█        ▄▀▒▒▒▐\n", Color.Yellow);
            Append("                     ▐▄▀▒▒▀▀▀▀▄▄▄▀▒▒▒▒▒▐\n", Color.Yellow);
            Append("                   ▄▄▀▒░▒▒▒▒▒▒▒▒▒█▒▒▄█▒▐\n", Color.Yellow);
            Append("                 ▄▀▒▒▒░░░▒▒▒░░░▒▒▒▀██▀▒▌\n", Color.Yellow);
            Append("                ▐▒▒▒▄▄▒▒▒▒░░░▒▒▒▒▒▒▒▀▄▒▒▌\n", Color.Yellow);
            var phrases = new[] { "much terminal", "so command", "very code", "wow technology", "such programming", "many features" };
            var random = new Random();
            for (int i = 0; i < 3; i++)
            {
                Append($" {phrases[random.Next(phrases.Length)]}\n", Color.Cyan);
            }
        }

        public static void Nyan(string arguments)
        {
            Append(" ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░\n", Color.Gray);
            Append(" ░░░░░░░░░░▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄░░░░░░░\n", Color.Gray);
            Append(" ░░░░░░░░▄▀░░░░░░░░░░░░▄░░░░░░░▀▄░░░░░░\n", Color.Gray);
            Append(" ░░░░░░░░█░░▄░░░░▄░░░░░░░░▄░░░░░█░░░░░░\n", Color.Gray);
            Append(" ░░░░░░░░█░░░░░░░░░░▄█▄▄░░▄░░░░░█░▄▄▄░░\n", Color.Gray);
            Append(" ░▄▄▄▄▄░░█░░░░░░▀░░░░▀█░░░░░░░░█▀▀░██░░\n", Color.Gray);
            Append(" ░██▄▀██▄█░░░▄░░░░░░░██░░░░▄░░░█░░░░▀▀░░\n", Color.Gray);
            Append(" ░░▀██▄▀██░░░░░░░░▀░██▀░░░░░░░░█░░░░░░░░\n", Color.Gray);
            Append(" ░░░░▀████░▀░░░░▄░░░██░░░▄░░░░▄█░░░░░░░░\n", Color.Gray);
            Append(" ~=[,,_,,]:3 NYAN NYAN NYAN!\n", Color.Magenta);
        }

        public static void Matrix(string arguments)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789ｦｱｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃ";
            var random = new Random();
            for (int i = 0; i < 15; i++)
            {
                var line = "";
                for (int j = 0; j < 50; j++)
                {
                    if (random.Next(0, 4) == 0)
                        line += chars[random.Next(chars.Length)];
                    else
                        line += " ";
                }
                Append($" {line}\n", Color.LimeGreen);
            }
            Append(" Welcome to the Matrix, Neo\n", Color.Red);
        }

        public static void CowSay(string arguments)
        {
            var message = string.IsNullOrEmpty(arguments) ? "Moo!" : arguments;
            var border = new string('-', message.Length + 2);
            Append($" {border}\n", Color.White);
            Append($"< {message} >\n", Color.White);
            Append($" {border}\n", Color.White);
            Append("        \\   ^__^\n", Color.White);
            Append("         \\  (oo)\\_______\n", Color.White);
            Append("            (__)\\       )\\/\\\n", Color.White);
            Append("                ||----w |\n", Color.White);
            Append("                ||     ||\n", Color.White);
        }

        public static void Lenny(string arguments)
        {
            var faces = new[] {
                "( ͡° ͜ʖ ͡°)", "( ͠° ͟ʖ ͡°)", "ᕦ( ͡° ͜ʖ ͡°)ᕤ", "( ͡~ ͜ʖ ͡°)",
                "( ͡ᵔ ͜ʖ ͡ᵔ )", "( ͡⊙ ͜ʖ ͡⊙)", "( ͡◉ ͜ʖ ͡◉)"
            };
            var random = new Random();
            var face = faces[random.Next(faces.Length)];
            Append($" {face}\n", Color.Yellow);
            Append(" You know what that means...\n", Color.Gray);
        }

        public static void ASCII(string arguments)
        {
            if (string.IsNullOrEmpty(arguments))
            {
                Append(" Usage: ascii <text>\n", Color.Yellow);
                return;
            }
            Append($" ╔══════════════════════════╗\n", Color.Cyan);
            Append($" ║  {arguments.PadRight(20)}  ║\n", Color.Cyan);
            Append($" ╚══════════════════════════╝\n", Color.Cyan);
            Append(" There's your fancy ASCII box mate\n", Color.White);
        }

        public static void Joke(string arguments)
        {
            var jokes = new[] {
                "why do devs love dark mode? cuz light attracts bugs and we got enough of those already bruv :pray:",
                "how many devs does it take to change a lightbulb? zero thats hardware's problem lol",
                "why do java devs wear glasses? because they literally cannot c sharp",
                "sql query walks into a bar and asks two tables... yo can i join you guys",
                "why dont programmers go outside? too many bugs and zero documentation",
                "wheres a programmers favorite place to hang? foo bar obviously",
                "why did the dev go broke? spent all his cache money",
                "how do you make a js bug feel better? you console it",
                "programming is just googling errors and pretending you know whats happening"
            };
            var random = new Random();
            var joke = jokes[random.Next(jokes.Length)];
            Append($" {joke}\n", Color.LightGreen);
        }

        public static void Yeet(string arguments)
        {
            var message = string.IsNullOrEmpty(arguments) ? "YEET!" : arguments;
            Append("    ⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡠⠤⠖⠒⠋⠉⠉⠉⠉⠓⠲⢤⡀⠀⠀⠀⠀⠀\n", Color.Yellow);
            Append("    ⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡴⠊⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠙⢦⡀⠀⠀⠀\n", Color.Yellow);
            Append("    ⠀⠀⠀⠀⠀⠀⠀⠀⡰⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠙⢆⠀⠀\n", Color.Yellow);
            Append("    ⠀⠀⠀⠀⠀⠀⠀⡼⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢧⠀⠀\n", Color.Yellow);
            Append($"   Y E E T ! ! !   {message}\n", Color.Red);
            Append(" Maximum power achieved! 🚀\n", Color.White);
        }

        public static void Sigma(string arguments)
        {
            var quotes = new[] {
                "keep grinding king the bag don't stop",
                "we don't do mid around here",
                "hustle hits different when you're sigma",
                "success or nothing bro no in between",
                "grind now flex later that's the move",
                "different breed mentality fr",
                "alpha energy only we don't settle"
            };
            var random = new Random();
            var quote = quotes[random.Next(quotes.Length)];
            Append($" {quote}\n", Color.Gold);
            Append(" sigma rule #47: never stop the grind\n", Color.Gray);
        }

        public static void Skibidi(string arguments)
        {
            Append("    ╭─────────────────────────╮\n", Color.Cyan);
            Append("    │    SKIBIDI TOILET    │\n", Color.Cyan);
            Append("    │         🚽           │\n", Color.Cyan);
            Append("    ╰─────────────────────────╯\n", Color.Cyan);
            var phrases = new[] { "bop bop yes yes", "skibidi toilet sigma", "ohio moment fr fr", "this hits different" };
            var random = new Random();
            Append($" {phrases[random.Next(phrases.Length)]}\n", Color.Yellow);
            Append(" Gen Alpha approved ✅\n", Color.LightGreen);
        }

        public static void Ohio(string arguments)
        {
            Append("    🌽 ONLY IN OHIO 🌽\n", Color.Yellow);
            var scenarios = new[] {
                "bro's code actually compiled without errors",
                "stack overflow went down but you knew the answer anyway",
                "deployed to prod and nothing broke wtf",
                "client said they loved it on the first try",
                "git merge actually worked with zero drama",
                "your time estimate was spot on for once"
            };
            var random = new Random();
            var scenario = scenarios[random.Next(scenarios.Length)];
            Append($" {scenario}... ONLY IN OHIO 💀\n", Color.Red);
            Append(" this state just hits different ngl\n", Color.Gray);
        }

        public static void Rizz(string arguments)
        {
            var rizzLevel = new Random().Next(1, 101);
            Append($"    📊 RIZZ LEVEL: {rizzLevel}% 📊\n", Color.Pink);

            if (rizzLevel < 30)
                Append(" Negative rizz detected 💀\n", Color.Red);
            else if (rizzLevel < 60)
                Append(" Mid rizz energy ⚡\n", Color.Yellow);
            else if (rizzLevel < 85)
                Append(" Solid rizz game 🔥\n", Color.Orange);
            else
                Append(" UNSPOKEN RIZZ 👑\n", Color.Gold);

            var tips = new[] { "be yourself", "confidence is key", "respectful vibes only", "genuine conversation wins" };
            var random = new Random();
            Append($" Pro tip: {tips[random.Next(tips.Length)]}\n", Color.LightBlue);
        }

        public static void Gyatt(string arguments)
        {
            var intensity = new Random().Next(1, 6);
            var gyatt = "G" + new string('Y', intensity) + "ATT";
            Append($"  {gyatt}! 🗿\n", Color.Magenta);

            if (intensity <= 2)
                Append(" Mid reaction tbh\n", Color.Gray);
            else if (intensity <= 4)
                Append(" Decent energy right there\n", Color.Yellow);
            else
                Append(" MAXIMUM GYATT ACHIEVED 💯\n", Color.Red);

            Append(" Respectfully noticing things since 2023\n", Color.White);
        }

        public static void Bussin(string arguments)
        {
            var foods = new[] { "pizza", "burgers", "tacos", "ramen", "cookies", "your code", "this terminal", "the vibes" };
            var random = new Random();
            var item = string.IsNullOrEmpty(arguments) ? foods[random.Next(foods.Length)] : arguments;

            Append($" {item} is straight up BUSSIN fr 🔥\n", Color.Orange);
            var reactions = new[] { "no cap this slaps different", "pure fire no printer", "this hits every single time", "the squad definitely approves" };
            Append($" {reactions[random.Next(reactions.Length)]}\n", Color.Yellow);
        }

        public static void Sussy(string arguments)
        {
            var sussLevel = new Random().Next(1, 101);
            Append($"    🔍 SUSSY METER: {sussLevel}% 🔍\n", Color.Red);

            if (sussLevel < 25)
                Append(" Pretty clean, no sus detected\n", Color.LightGreen);
            else if (sussLevel < 50)
                Append(" Slightly sus but we'll let it slide\n", Color.Yellow);
            else if (sussLevel < 75)
                Append(" That's pretty sus ngl 👀\n", Color.Orange);
            else
                Append(" MAXIMUM SUSSINESS DETECTED! 🚨\n", Color.Red);

            Append(" When the impostor is sus 😳\n", Color.Gray);
        }

        public static void LowKey(string arguments)
        {
            var phrases = new[] {
                "lowkey this terminal hits different",
                "lowkey your code is clean",
                "lowkey the best command ever",
                "lowkey impressed by this",
                "lowkey want to use this more",
                "lowkey obsessed with terminals now"
            };
            var random = new Random();
            var phrase = phrases[random.Next(phrases.Length)];
            Append($" {phrase}\n", Color.Purple);

            var flip = random.Next(0, 2);
            if (flip == 0)
                Append(" But also highkey amazing 📈\n", Color.LightBlue);
            else
                Append(" Keeping it lowkey though 🤫\n", Color.Gray);
        }

        public static void NoCap(string arguments)
        {
            Append("    🧢 NO CAP DETECTED 🧢\n", Color.Blue);
            var statements = new[] {
                "This terminal is actually fire",
                "Programming is lowkey fun",
                "Your code skills are improving",
                "This command system slaps",
                "Terminal life chose you",
                "You're becoming a dev legend"
            };
            var random = new Random();
            var statement = statements[random.Next(statements.Length)];
            Append($" {statement}, no cap! 💯\n", Color.LightGreen);
            Append(" Verified truth only, we don't do lies here\n", Color.White);
        }

        public static void Periodt(string arguments)
        {
            var statements = new[] {
                "this terminal is literally the best periodt",
                "your coding game is straight fire periodt",
                "we're done with bugs in prod periodt",
                "stack overflow is basically our religion periodt",
                "coffee is not optional its required periodt",
                "daily git commits or we riot periodt"
            };
            var random = new Random();
            var statement = statements[random.Next(statements.Length)];
            Append($" {statement} 💅\n", Color.HotPink);
            Append(" and thats the final word bestie\n", Color.White);
        }

        public static void Slaps(string arguments)
        {
            var subject = string.IsNullOrEmpty(arguments) ? "this command" : arguments;
            Append($" {subject} absolutely SLAPS different 🎯\n", Color.Lime);

            var reactions = new[] {
                "hit me right in the feels ngl",
                "this goes so hard fr fr",
                "literally cannot describe how good this is",
                "pure excellence has been detected",
                "chefs kiss level quality right here",
                "this is what peak performance looks like bestie"
            };
            var random = new Random();
            Append($" {reactions[random.Next(reactions.Length)]} 🔥\n", Color.Yellow);
        }
    }
}