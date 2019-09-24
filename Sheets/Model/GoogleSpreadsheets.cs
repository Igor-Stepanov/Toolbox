using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Sheets.Core;
using static Google.Apis.Services.BaseClientService;

namespace Sheets.Model
{
  public class GoogleSpreadsheets : ISpreadsheets
  {
    private readonly SheetsService _service;
    private readonly Dictionary<string, ISpreadsheet> _spreadsheets;

    public GoogleSpreadsheets(string clientSecret)
    {
      _service = InitializedService(clientSecret);
      _spreadsheets = new Dictionary<string, ISpreadsheet>();
    }

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

    public ISpreadsheet Spreadsheet(string name)
    {
      if (!_spreadsheets.TryGetValue(name, out var spreadsheet))
        _spreadsheets[name] = spreadsheet = new Spreadsheet(_service, name);

      return spreadsheet;
    }
  }
}