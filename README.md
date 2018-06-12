# OpenStory

OpenStory is a ASP.NET MVC 5 Application. The idea behind this web application is that it is meant to be a online forum where any users 
can come in and post stories.   

To run this application you can open it in Visual Studio and just host it via IIS Express by running the application. There is a 
.mdf file used as the database when the application is ran in Visual Studio.   

The alernative is that you can generate a build using the publish tool in Visual Studio and put that build in a website directory in IIS.To do this, open up IIS Manager on your desktop and right click "Sites" and then click "Add Website".
Note that this application uses https at times, so make sure that in the application.config, you have https as an option. Also you
would need to set up database access for it as well. 

The database is created using Entity Framework with code first approach. You can see a list of migrations in the migrations folder 
where each update(migration) of the database is held. To create a database for the application, use the following command to generate
a sql script: 
```
update-database -script -sourcemigration:0
```
Note that the number after source migration specifies which migration you are starting from.
You can then take this generated SQL code and run it against a SQL server. 

Users can register and login in this application, this is done through ASP.Net Identity. 
Once logged in, users can post new topics or reply to someone elses topic. 

The view is creating using bootstrap, for a reference look at: https://bootswatch.com/3/superhero/

For more details on Entity Framework, refer to :https://docs.microsoft.com/en-us/ef/#pivot=ef6x&panel=ef6x1

If there is an intention to do some preformance profiling, look into Glimpse. For more information look into https://www.nuget.org/packages/Glimpse
