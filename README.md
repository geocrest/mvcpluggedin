![Nuget version](https://img.shields.io/nuget/v/Geocrest.Web.Mvc.svg)

# MvcPluggedIn

MvcPluggedIn extends the ASP.NET MVC/Web API framework with a dynamic plug-and-play system for developing new areas/modules. The framework incorporates aspects of Entity Framework Code First, Ninject, ELMAH, and the ArcGIS REST API.

## Usage
The compiled libraries can be consumed in an application using Nuget. Several packages are available ([https://www.nuget.org/packages?q=Geocrest](https://www.nuget.org/packages?q=Geocrest)).

Once installed, allow your `global.asax.cs` class to inherit from the `Geocrest.Web.Mvc.BaseApplication` class. Additionally, verify that no `NinjectWebCommon.cs` class exists in the *App_Start* folder. This file gets installed with Ninject and duplicates functionality contained in `BaseApplication`. It is necessary to delete from your project.

## Registering services
MvcPluggedIn utilizes Ninject from it's dependency injection. In order to register your services, you can add a class to your project (typically in *App_Start*) that inherits from `Ninject.Modules.NinjectModule`. Override the `Load` method and register your services there. 

```csharp
public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            // Bind your contracts to their implementations here
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["Context"].ConnectionString);
            Bind<IRepository>().To<Repository>();
            Bind<Context>().ToSelf().InRequestScope()
                .WithConstructorArgument("connectionstring", builder.ToString())
                .WithConstructorArgument("schema", "dbo");
            Bind<System.Data.Entity.DbContext>().To<Context>();
            Bind<IMailSender>().To<MailSender>();
        }
    }
```