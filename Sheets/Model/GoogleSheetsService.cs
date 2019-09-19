using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Sheets.Core;
using static Google.Apis.Services.BaseClientService;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum;

namespace Sheets.Model
{
  public class GoogleSheetsService : ISheetsService
  {
    private readonly SheetsService _service;
    
    public ISpreadsheets Spreadsheets => new Spreadsheets(_service);

    public GoogleSheetsService(string clientSecret) => 
      _service = InitializedService(clientSecret);

    private static SheetsService InitializedService(string clientSecret) =>
      new SheetsService(new Initializer
      {
        HttpClientInitializer = Credential(clientSecret),
        ApplicationName = "StaticDataAPI",
      });

    private static UserCredential Credential(string clientSecret)
    {
      const string path = "Credentials";
      
      using (var stream = new FileStream(clientSecret, FileMode.Open, FileAccess.Read))
      {
        return GoogleWebAuthorizationBroker.AuthorizeAsync(
          GoogleClientSecrets.Load(stream).Secrets,
          new[] { SheetsService.Scope.Spreadsheets },
          "User",
          CancellationToken.None,
          new FileDataStore(path, true)).Result;
      }
    }
  }
}