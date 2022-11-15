using DataAccessLibrary.Interfaces;
using DTOsLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Business_Entity
{
    public class MailBusinessEntity
    {

        private readonly IConfiguration configuration;
        public MailBusinessEntity(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SendRetrainEmail(string body)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(configuration["Mail:AutoMail"]);
            mailMessage.To.Add(new MailAddress(configuration["Mail:AI_Dept"]));
            mailMessage.Subject = "Biển báo cần được Re-train";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body =
                "<div style=\"width: 60%; font-family: 'Google Sans'\">" +
                    "<div style=\"background-color: #003399; text-align: center; padding: 2%\">" +
                        "<span style=\"color: #E5EAF5; font-weight: bold; font-size: 1.5rem\">" +
                            $"{body.Split("\n")[0]}" +
                        "</span>" +
                    "</div>" +
                    "<div style=\"background-color: #FFFFFF; text-align: center; padding:2%\">" +
                        "<span style=\"font-size: 1.2rem\">" +
                            $"{body.Split("\n")[1]}" +
                        "</span>" +
                    "</div>" +
                    "<div style=\"background-color: #FFFFFF; text-align: center; padding: 3%\">" +
                        $"<a href=\"{body.Split("\n")[2]}\"" +
                        "style=\"box-sizing: border-box;text-decoration: none;text-align: center;" +
                        "color: #FFFFFF; background-color: #ff6600; border-radius: 4px; width: 7rem; padding: 2%\">" +
                            "<span style=\"padding:2%\">" +
                                "<strong><span style=\"font-size:1.2rem\">Firebase Link</span></strong>" +
                            "</span>" +
                        "</a>" +
                    "</div>" +
                    "<div style=\"background-color: #cfe2ed; text-align: center; padding-top: 2rem; padding-bottom: 1rem\">" +
                        "<p style=\"font-size: 1.2rem; color: #003399; padding: 0; margin: 0\">" +
                            "<strong>VNRDNTAI</strong>" +
                        "</p>" +
                        "<p style=\"font-size: 1rem; color: #003399; padding: 0; margin: 0\">" +
                            "<span>Email tự động từ vnrdntaiautomail@gmail.com</span>" +
                        "</p>" +
                    "</div>" +
                    "<div style=\"background-color: #003399; text-align: center; padding: 2%\">" +
                        "<p style=\"font-size: 1.1rem; color: #FAFAFA\">" +
                            "Copyrights © VNRDNTAI" +
                        "</p>" +
                    "</div>" +
                "</div>";

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = 
                new NetworkCredential(
                    configuration["Mail:AutoMail"], 
                    configuration["Mail:AutoMailAppPwd"]
                );
            smtpClient.Send(mailMessage);
        }
    }
}
