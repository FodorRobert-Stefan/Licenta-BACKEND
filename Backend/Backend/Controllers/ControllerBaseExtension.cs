// <copyright file="ControllerBaseExtension.cs" company="DVSE GmbH">
//   Copyright (c) by DVSE Gesellschaft für Datenverarbeitung, Service und Entwicklung mbH. All rights reserved.
// </copyright>

namespace TM.Translate.Service.Api.Extensions
{
  using System.Net;
  using Backend.BusinessLogic.Response;
  using Microsoft.AspNetCore.Mvc;

  public static class ControllerBaseExtension
  {
    public static IActionResult ReturnResponse(this ControllerBase input, Response response)
    {
      IActionResult result;

      switch (response.HttpStatusCode)
      {
        case HttpStatusCode.OK:
          result = input.Ok(response);
          break;

        case HttpStatusCode.BadRequest:
          result = input.BadRequest(response);
          break;

        case HttpStatusCode.Unauthorized:
          result = input.Unauthorized(response);
          break;

        case HttpStatusCode.NotFound:
          result = input.NotFound(response);
          break;

        case HttpStatusCode.Created:
          result = input.Created(string.Empty, response);
          break;

        case HttpStatusCode.NotModified:
          result = input.StatusCode((int)HttpStatusCode.NotModified);
          break;
        case HttpStatusCode.InternalServerError:
          result = input.StatusCode((int)HttpStatusCode.InternalServerError);
          break;
        default:
          result = input.StatusCode((int)response.HttpStatusCode, "Unhandled status code.");
          break;
      }

      return result;
    }
  }
}
