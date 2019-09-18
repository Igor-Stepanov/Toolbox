using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Sheets.Core;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum;

namespace Sheets.Model
{
  public class GoogleSheetsService : ISheetsService
  {
    private readonly SheetsService _service;

    public GoogleSheetsService(string clientSecret) => 
      _service = InitializedService(clientSecret);

    public IEnumerable<IRow> FetchRows(string spreadsheetId, string sheetName) =>
      _service.Spreadsheets.Values
       .Get(spreadsheetId, sheetName)
       .Execute()
       .Values
       .Select(Row.Create);
    
    public void Update(string spreadsheetId, string sheetName, IEnumerable<IRow> rows)
    {
      var valueRange = new ValueRange
      {
        Values = rows.Select(x => x.Raw).ToList(),
      };
      
      var updateRequest = _service.Spreadsheets.Values.Update(valueRange, spreadsheetId, sheetName);
      updateRequest.ValueInputOption = RAW;
      var result = updateRequest.Execute();
    }

    private static SheetsService InitializedService(string clientSecret) =>
      new SheetsService(new BaseClientService.Initializer
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