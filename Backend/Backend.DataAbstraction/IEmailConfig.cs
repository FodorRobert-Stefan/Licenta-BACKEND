﻿namespace Backend.Services
{
  public interface IEmailConfig
  {
    public string From { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }
}
