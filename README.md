API File Upload Demo
=================

This is a quick windows console application that shows how to upload a file to Square-9's GlobalSearch API.  

To get started, in the `main(String[] args)` method of Program.cs, update the following to match your GlobalSearch Installation.  

1. The website target in `RestClient("");`  
2. `var databaseID = ` the databse you want to upload to.  
3. `var archiveID = ` the archive you want to index the document into.  
4. `var localFileName = ""` the name of the file on your local machine you want to upload and index.  

NOTE: Requires .NET v4 and a version of VisualStudio running NuGet with NuGet Package Restore enabled.  

Run the application!  

The upload happens in 4 steps: 

1. POST a file to the server.  
2. Obtain a license.  
3. Index the document using the file that was uploaded, and some provided field data into a database/archive.  
4. Release the license.  


