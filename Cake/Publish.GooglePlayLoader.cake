#addin nuget:?package=Google.Apis
#addin nuget:?package=Google.Apis.Core
#addin nuget:?package=Google.Apis.Auth
#addin nuget:?package=Google.Apis.AndroidPublisher.v2
#addin nuget:?package=Newtonsoft.Json&version=10.0.1

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.AndroidPublisher.v2;
using Google.Apis.AndroidPublisher.v2.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;


public class GooglePlayLoader
{
  private const string TrackAlpha = "alpha"; //Track for uploading the apk, can be 'alpha', beta', 'production' or 'rollout'.
  
  private const string ContentType = "application/octet-stream";
  private const string KeyPwd = "notasecret";
  
  private readonly string _packageName;

  private ServiceAccountCredential _credentials;
  
  private string _apkPath;
  private string _obbPath;
  private string _folderPath;
  private string _publishChannel;
  
  public GooglePlayLoader(string packageName)
  {
    if (string.IsNullOrEmpty(packageName))
      throw new Exception("Package Name is empty!");
    
    _packageName = packageName;
  }

  public GooglePlayLoader Authenticate(string keyPath, string serviceAccountEmail) => FluentSet(() => 
    _credentials = GetCredentials(keyPath, KeyPwd, serviceAccountEmail)
  );

  public GooglePlayLoader SetArtifactPathes(string apkPath, string obbPath = null) => FluentSet(() => {
    _apkPath = apkPath;
    _obbPath = obbPath;
  });

  public GooglePlayLoader SetArtifactsFolder(string folderPath) => FluentSet(() => 
    _folderPath = folderPath
  );
  
  public GooglePlayLoader SetPublishChannel(string channelName = TrackAlpha) => FluentSet(() =>
    _publishChannel = channelName
  );
  
  public void StartUpload()
  {
    if (_credentials == null)
      throw new Exception("App not authenticated!");
    if (string.IsNullOrEmpty(_publishChannel))
      throw new Exception("Publish Channel name is not set!");

    if (!string.IsNullOrEmpty(_folderPath))
    {
      UploadMultiple();
      return;
    }
      
    if (!string.IsNullOrEmpty(_apkPath))
    {
      Upload();
      return;
    }

    throw new Exception("APK path is not set!");
  }

  #region Helpers

  private GooglePlayLoader FluentSet(Action set)
  {
    set();
    return this;
  }
  
  private ServiceAccountCredential GetCredentials(string keyPath, string keyPwd, string serviceAccountEmail)
  {
    var certificate = new X509Certificate2(keyPath, keyPwd, X509KeyStorageFlags.Exportable);
    var scopes = new[] {AndroidPublisherService.Scope.Androidpublisher};
    var credInit = new ServiceAccountCredential.Initializer(serviceAccountEmail) { Scopes = scopes }
      .FromCertificate(certificate);
    
    return new ServiceAccountCredential(credInit);
  }
  
  private void Upload()
  {
    // Create the service.
    var service = new AndroidPublisherService(new BaseClientService.Initializer() { HttpClientInitializer = _credentials });
    var edits = service.Edits;
    
    // Create a new edit to make changes.
    var editRequest = edits.Insert(new AppEdit(), _packageName);
    var appEdit = editRequest.Execute();

    Console.WriteLine("Start upload");

    int apkVersionCode;
    
    TryUploadApk(_apkPath, edits, appEdit, out apkVersionCode);     
    Console.WriteLine($"APK (version {apkVersionCode}) was uploaded!");

    if (!string.IsNullOrEmpty(_obbPath))
    {
      TryUploadObb(_obbPath, apkVersionCode, edits, appEdit);   
      Console.WriteLine($"OBB for package {_packageName} was uploaded");
    }

    SaveChanges(new List<int?>{apkVersionCode}, edits, appEdit);
    Console.WriteLine("Upload finished.");
  }
  
    private void UploadMultiple()
  {
    // Create the service.
    var service = new AndroidPublisherService(new BaseClientService.Initializer() { HttpClientInitializer = _credentials });
    var edits = service.Edits;
    
    // Create a new edit to make changes.
    var editRequest = edits.Insert(new AppEdit(), _packageName);
    var appEdit = editRequest.Execute();

    if (!System.IO.Directory.Exists(_folderPath))
      throw new Exception(string.Format("Folder \"{0}\" not found!", _folderPath));
          
    Console.WriteLine("Start uploading from " + _folderPath);

    var versionCodes = new List<int?>();

    var apkFiles = System.IO.Directory.GetFiles(_folderPath, "*.apk");
    if (apkFiles.Length == 0)
      throw new Exception("Apk files not found.");

		foreach(string apkPath in apkFiles)
    {
      int apkVersionCode = 0;

      TryUploadApk(apkPath, edits, appEdit, out apkVersionCode);   

      versionCodes.Add(apkVersionCode);
      Console.WriteLine($"APK (version {apkVersionCode}) was uploaded!");
    }

    SaveChanges(versionCodes, edits, appEdit);
    Console.WriteLine("Upload finished.");
  }
  
  private void TryUploadApk(string path, EditsResource edits, AppEdit appEdit, out int apkVersionCode)
  {
    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
    {
      var size = (stream.Length / 1024f) / 1024f;
      Console.WriteLine($"Upload APK (size: {size} mb)");
      
      var uploadRequest = edits.Apks.Upload(_packageName, appEdit.Id, stream, ContentType);
      uploadRequest.Upload();

      if (uploadRequest.GetProgress().Status != UploadStatus.Completed)
      {
        Console.WriteLine($"Upload status: {uploadRequest.GetProgress().Status}");
        throw new Exception(uploadRequest.GetProgress().Exception.Message);
      }

      var versionCode = uploadRequest.ResponseBody.VersionCode;
      if (versionCode == null)
        throw new Exception("Error! Apk VersionCode is NULL");

      apkVersionCode = versionCode.Value;
    }
  }
  
  private void TryUploadObb(string path, int apkVersionCode, EditsResource edits, AppEdit appEdit)
  {
    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
    {
      var size = (stream.Length / 1024f) / 1024f;
      Console.WriteLine($"Upload OBB  (size: {size} mb)");
      
      var uploadResponse = edits.Expansionfiles.Upload(_packageName, appEdit.Id, apkVersionCode
        , EditsResource.ExpansionfilesResource.UploadMediaUpload.ExpansionFileTypeEnum.Main, stream,
        ContentType).Upload();

      if (uploadResponse.Status != UploadStatus.Completed)
      {
        Console.WriteLine($"Upload OBB status: {uploadResponse.Status}");
        throw new Exception($"Upload OBB result: {uploadResponse.Exception.Message}");
      }  
    }
  }

  private void SaveChanges(List<int?> apkVersionCodes, EditsResource edits, AppEdit appEdit)
  {
    var track = new Track()
    {
      TrackValue = _publishChannel,
      VersionCodes = apkVersionCodes
    };
    
    var updateTrackRequest = edits.Tracks.Update(track, _packageName, appEdit.Id, _publishChannel);
    var updatedTrack = updateTrackRequest.Execute();

    // Commit changes for edit.
    var commitRequest = edits.Commit(_packageName, appEdit.Id);
    commitRequest.Execute();
    
    Console.WriteLine($"Apk has been updated >> {updatedTrack.TrackValue}");
  }

  #endregion
}
