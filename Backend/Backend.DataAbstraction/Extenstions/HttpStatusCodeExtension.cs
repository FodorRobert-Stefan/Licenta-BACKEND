// <copyright file="HttpStatusCodeExtension.cs" company="DVSE GmbH">
//   Copyright (c) by DVSE Gesellschaft für Datenverarbeitung, Service und Entwicklung mbH. All rights reserved.
// </copyright>

namespace TM.Translate.Service.Api.Extensions
{
  using System.Net;

  public static class HttpStatusCodeExtension
  {
    public static string GetDescription(this HttpStatusCode code)
    {
      return code switch
      {
        HttpStatusCode.OK => "OK - The request has succeeded.",
        HttpStatusCode.Created => "Created - The request has been fulfilled and has resulted in one or more new resources being created.",
        HttpStatusCode.NoContent => "No Content - The server has successfully fulfilled the request and there is no content to send.",
        HttpStatusCode.BadRequest => "Bad Request - The server could not understand the request due to invalid syntax.",
        HttpStatusCode.Unauthorized => "Unauthorized - The client must authenticate itself to get the requested response.",
        HttpStatusCode.Forbidden => "Forbidden - The client does not have access rights to the content.",
        HttpStatusCode.NotFound => "Not Found - The server can not find the requested resource.",
        HttpStatusCode.MethodNotAllowed => "Method Not Allowed - The request method is known by the server but is not supported by the target resource.",
        HttpStatusCode.Conflict => "Conflict - The request could not be completed due to a conflict with the current state of the resource.",
        HttpStatusCode.InternalServerError => "Internal Server Error - The server encountered an unexpected condition that prevented it from fulfilling the request.",
        HttpStatusCode.BadGateway => "Bad Gateway - The server was acting as a gateway or proxy and received an invalid response from the upstream server.",
        HttpStatusCode.ServiceUnavailable => "Service Unavailable - The server is not ready to handle the request. Common causes might be the server is down for maintenance or overloaded.",
        HttpStatusCode.GatewayTimeout => "Gateway Timeout - The server was acting as a gateway or proxy and did not receive a timely response from the upstream server.",

        _ => "Unknown Status Code"
      };
    }
  }
}