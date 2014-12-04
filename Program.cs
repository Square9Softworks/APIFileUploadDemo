using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestSharp;
using S9APIUploadDemo.Models;

namespace S9APIUploadDemo
{
    class Program
    {
        static RestClient ApiClient = new RestClient("http://localhost/Square9Api"); //path to the website where the api is hosted

        static void Main(string[] args)
        {
            //windows auth
            ApiClient.Authenticator = new NtlmAuthenticator();

            var databaseID = 1;  //Database ID
            var archiveID =  3;  //Archive ID

            var localFileName = "tester.pdf"; //file to be uploaded

            var token = "";
            try
            {
                //upload file
                string uploadedFileName = PostFile(localFileName); //api returns the name of the file on the server for indexing
                //valid SS license
                token = GetLicense();
                //some field data
                List<FieldItem> fieldData = new List<FieldItem>() { new FieldItem(1, "uploader1"), new FieldItem(2, "uploader2") };
                //index file
                IndexDocument(databaseID, archiveID, fieldData, uploadedFileName, token);

                Console.WriteLine(localFileName + " Uploaded Successfully.");
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to upload file:\n " + ex.Message);
                Console.Read();
            }
            finally
            {
                if (!String.IsNullOrEmpty(token))
                {
                    ReleaseLicense(token);
                }
            }

        }

        static String PostFile(String fileToPost)
        {
            String uploadedFileName = String.Empty;
            
            var request = new RestRequest("api/files/", Method.POST);
            request.AddFile("File", fileToPost);
            var response = ApiClient.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unable to upload file: " + response.Content);
            }
            //for some reason the restsharp deserializer is having trouble with this object
            var uploadedFiles = JsonConvert.DeserializeObject<UploadedFileList>(response.Content);
            uploadedFileName = uploadedFiles.files[0].name;

            return uploadedFileName;
        }

        static void IndexDocument(Int32 DatabaseID, Int32 ArchiveID, List<FieldItem> FieldData, String UploadedFileName, String Token)
        {
            //load up our data
            var indexData = new Indexer();
            indexData.files.Add(new File(UploadedFileName));

            if (FieldData != null && FieldData.Count > 0)
            {
                foreach (FieldItem field in FieldData)
                {
                    var fieldItem = new Field(field.ID.ToString(), field.VAL);
                    indexData.fields.Add(fieldItem);
                }
            }

            var request = new RestRequest("api/dbs/{db}/archives/{arch}?token={token}", Method.POST);
            request.AddParameter("token", Token, ParameterType.UrlSegment); //have to specifiy type on POST
            request.AddParameter("db", DatabaseID, ParameterType.UrlSegment);
            request.AddParameter("arch", ArchiveID, ParameterType.UrlSegment);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(indexData);

            var response = ApiClient.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unable to index document: " + response.Content);
            }
        }


        static String GetLicense()
        {
            var request = new RestRequest("api/licenses");
            var license = ApiClient.Execute<License>(request);
            if (license.StatusCode != HttpStatusCode.OK)
            {
                if (license.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("Unable to get a License: The passed user is Unauthorized.");
                }
                else if (license.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new Exception("Unable to get a License: " + license.Content);
                }
                else if (license.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception("Unable to get a License: Unable to connect to the license server, server not found.");
                }
                else
                {
                    throw new Exception("Unable to get a License: " + license.Content);
                }
            }
            return license.Data.Token;
        }
        static void ReleaseLicense(String Token)
        {
            var request = new RestRequest("api/licenses/" + Token);
            var response = ApiClient.Execute(request);
            if (response.ErrorException != null)
            {
                throw new Exception("Unable to release license token. ", response.ErrorException);
            }
            else if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("Unable to release license token. " + response.Content);
            }
        }
    }
}
