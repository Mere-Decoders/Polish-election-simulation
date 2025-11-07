# Backend

## Running the project using docker via terminal
```bash
cd backend
docker build -f backend/Dockerfile -t <image-tag> .
docker run --name <container-name> -p 8080:<port1> -p 8081:<port2> <image-tag>
```
where \<container-name\> and \<image-tag\> are arbitrary names and
\<port1\>, \<port2\> are ports you want to map to the ports used inside the container. 

After that the container will serve the API on your localhost

## Documentation
We follow a conventional ASP.NET Core layout commonly used in tutorials and Microsoft documentation:

`backend/Models` - folder for simple data structures which could be structs but are classes for convenience (heap memory and DI)

`backend/Data` - code relating to persistent data, ex. a database (might be unused)

`backend/Services` - business logic, paired with Interfaces for DI container

`backend/Program.cs` - running the app and Dependency Injection setup
