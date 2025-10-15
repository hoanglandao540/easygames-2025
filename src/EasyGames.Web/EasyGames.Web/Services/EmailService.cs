using System.Diagnostics;

namespace EasyGames.Web.Services
{
    // student-style: fake sender writing to debug output
    public class EmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string body)
        {
            Debug.WriteLine($"[EmailService] To:{to} | Subject:{subject}\n{body}");
            return Task.CompletedTask;
        }
    }
}
