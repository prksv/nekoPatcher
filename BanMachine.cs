using Pastel;
using PSHostsFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace nekoPatcher
{
    internal class BanMachine
    {
        string[] API_URLS = { };
        string[] IP_ADRESSSES = { };
        string PROGRAM_PATH = "";
        string SERVER_IP = "127.0.0.1";

        public BanMachine(string[] apiUrls, string[] ipAdresses, string programPath) 
        {
            this.API_URLS = apiUrls;
            this.IP_ADRESSSES = ipAdresses;
            this.PROGRAM_PATH = programPath;
        }

        private void BanIP(string RuleName, string IPAddress, string Port, string Protocol)
        {
            if (!string.IsNullOrEmpty(RuleName) && !string.IsNullOrEmpty(IPAddress) && !string.IsNullOrEmpty(Port) && !string.IsNullOrEmpty(Protocol) && new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                using (Process RunCmd = new Process())
                {
                    RunCmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    RunCmd.StartInfo.FileName = "cmd.exe";
                    RunCmd.StartInfo.UseShellExecute = true;
                    RunCmd.StartInfo.Arguments = "/C netsh advfirewall firewall add rule name=\"" + RuleName + "\" dir=OUT program=" + this.PROGRAM_PATH + " action=block remoteip=" + IPAddress + " remoteport=" + Port + " protocol=" + Protocol;
                    RunCmd.Start();
                    RunCmd.StartInfo.Arguments = "/C netsh advfirewall firewall add rule name=\"" + RuleName + "\" dir=IN action=block program=" + this.PROGRAM_PATH + " remoteip=" + IPAddress + " remoteport=" + Port + " protocol=" + Protocol;
                    RunCmd.Start();
                }
            }
        }

        private void UnbanIP(string RuleName)
        {
            if (!string.IsNullOrEmpty(RuleName))
            {
                using (Process RunCmd = new Process())
                {
                    RunCmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    RunCmd.StartInfo.FileName = "cmd.exe";
                    RunCmd.StartInfo.UseShellExecute = true;
                    RunCmd.StartInfo.Arguments = "/C netsh advfirewall firewall delete rule name=\"" + RuleName + "\"";
                    RunCmd.Start();
                }
            }
        }

        public void BanAPI()
        {
            try
            {
                foreach (string url in this.API_URLS)
                {
                    HostsFile.Set(url, this.SERVER_IP);
                }

                Console.WriteLine($"Доступ к API был заблокирован!".Pastel(Color.GreenYellow));

                foreach (string ip_address in this.IP_ADRESSSES)
                {
                    this.BanIP("NekoBan" + ip_address, ip_address, "443", "TCP");

                    Console.WriteLine($"[{ip_address}] IP был заблокирован!".Pastel(Color.GreenYellow));
                }
            }
            catch
            {
                Console.WriteLine("Не удалось заблокировать сервисы Neko".Pastel(Color.Red));
            }

        }
        public void UnbanAPI()
        {
            try
            {
                foreach (string url in this.API_URLS)
                {
                    HostsFile.Remove(url);
                };
                Console.WriteLine($"Доступ к API был разблокирован!".Pastel(Color.Orange));

                foreach (string ip_address in this.IP_ADRESSSES)
                {
                    UnbanIP("NekoBan" + ip_address);

                    Console.WriteLine($"[{ip_address}] IP был разблокирован!".Pastel(Color.OrangeRed));
                }
            }
            catch
            {
                Console.WriteLine("Не удалось разблокировать сервисы Neko".Pastel(Color.Red));
            }
        }
    }
}
