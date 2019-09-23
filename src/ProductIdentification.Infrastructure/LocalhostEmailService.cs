using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProductIdentification.Infrastructure
{
    public class LocalhostEmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var filePath = Path.Combine(".//", $"{Guid.NewGuid()}.html");
            using (var outputFile = new StreamWriter(filePath))
            {
                outputFile.WriteLine($"<h2>TO: {email}</h2>");
                outputFile.WriteLine($"<h2>SUBJECT: {subject}</h2>");
                outputFile.WriteLine(htmlMessage);
            }
            
            OpenUrl(filePath); 
        }
        
        private static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}