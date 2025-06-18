﻿namespace Backend.DataAbstraction
{
  public interface IEmailSender
  {
    void SendEmail(string to, string subject, string htmlBody);
  }
}
