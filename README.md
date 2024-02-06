# Digital_Matter
 Assignment

------------------------------------------------------------------------------------------------------------------------------------------
Project dependencies

Framework:
Microsoft.AspNetCore.App(8.0.1)
Microsoft.NETCore.App(8.0.1)

Packages:
System.Data.SqlClient(4.8.6)
Microsoft.Entity.FrameworkCore(8.0.1)
------------------------------------------------------------------------------------------------------------------------------------------

Please note below, to connect the application to the database.

Change the ConnectionStrings in the appsettings.json file with key value pair below.

"ConnectionStrings": {
  "DefaultConnection": "Data Source=.\\SQLEXPRESS;Initial Catalog=DigitalMatter;Integrated Security=True;TrustServerCertificate=True"
}

The key,value "ConnectionStrings": {"DefaultConnection":" should be kept the same but everything from data source can be changed.
note that the format should be kept the same, specifically, the double back slash"\" which is not shown above but is in the appsettings.json
and no spaces between "TrustServerCertificate=True"

------------------------------------------------------------------------------------------------------------------------------------------
The sql scripts can be found in the sql script folder.
"full_sql.sql" is a script that runs to make the database, tables and insert values into tables.
------------------------------------------------------------------------------------------------------------------------------------------
The stack utilizes SQL server express with AspNetCore (C# css JS).
