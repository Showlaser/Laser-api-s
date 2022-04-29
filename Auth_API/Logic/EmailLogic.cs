using Auth_API.Enums;
using Auth_API.Models.Helper;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Auth_API.Logic
{
    public class EmailLogic
    {
        private static readonly string SmtpHost = Environment.GetEnvironmentVariable("SMTPHOST") ?? throw new NoNullAllowedException("Environment variable" +
                                                                                                                                      "SMTPHOST was empty. Set it using the SMTPHOST environment variable");

        private static readonly int SmtpPort = Convert.ToInt32(Environment.GetEnvironmentVariable("SMTPPORT") ?? throw new NoNullAllowedException("Environment variable" +
                                                                                                                                                   "SMTPPORT was empty. Set it using the SMTPPORT environment variable"));

        private static readonly string EmailAddress = Environment.GetEnvironmentVariable("EMAILADDRESS") ?? throw new NoNullAllowedException("Environment variable" +
                                                                                                                                             "EMAILADDRESS was empty. Set it using the EMAILADDRESS environment variable");

        private static readonly string EmailPassword = Environment.GetEnvironmentVariable("EMAILPASSWORD") ?? throw new NoNullAllowedException("Environment variable" +
                                                                                                                                               "EMAILPASSWORD was empty. Set it using the EMAILPASSWORD environment variable");

        private static string GetTemplatePathByName(EmailTemplatePath templateName)
        {
            Dictionary<EmailTemplatePath, string> dictionary = new()
            {
                { EmailTemplatePath.EmailValidation, "/EmailTemplates/EmailValidation.html" },
                { EmailTemplatePath.ForgotPassword, "/EmailTemplates/PasswordReset.html" }
            };

            return dictionary[templateName];
        }

        /// <summary>
        /// Finds an HTML template by the templateName parameter and replaces all @{} values with the value in the keyValueCollection parameter
        /// </summary>
        /// <param name="templateName">The path of the template</param>
        /// <param name="keyValueCollection">The key in the template to search and the value to replace it with</param>
        /// <returns>An string which contains HTML content</returns>
        public static string GetHtmlFormattedEmailBody(EmailTemplatePath templateName, IDictionary<string, string> keyValueCollection)
        {
            string templatePath = GetTemplatePathByName(templateName);
            string filePath = Environment.CurrentDirectory + templatePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(nameof(templateName));
            }

            string fileText = File.ReadAllText(filePath);
            StringBuilder sb = new(fileText);
            foreach ((string? key, string? value) in keyValueCollection)
            {
                sb.Replace("@{" + key + "}", value);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="email">The email to send</param>
        public static void Send(Email email)
        {
            if (string.IsNullOrEmpty(email?.Message) ||
                string.IsNullOrEmpty(email.EmailAddress) ||
                string.IsNullOrEmpty(email.Subject))
            {
                throw new NoNullAllowedException(nameof(email));
            }

            // client settings
            using SmtpClient client = new(SmtpHost, SmtpPort)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EmailAddress, EmailPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000
            };

            // mail settings
            using MailMessage message = new()
            {
                From = new MailAddress(EmailAddress)
            };
            message.To.Add(email.EmailAddress);
            message.Body = email.Message;
            message.Subject = email.Subject;
            message.IsBodyHtml = true;

            client.Send(message);
        }
    }
}
