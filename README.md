# Read/Write Cookie HttpModule
An HttpModule that reads and writes cookies for any watched query string keys coming through the request pipeline. Includes the ability to create an anonymous user cookie that contains a unique ID as well as an example key vault for the retrieval of your watched keys. Also employs a means of inversion of control. Some assembly required ;)

# Setting Up
Register service componenets with the UnityConfig container in the App_Start folder:
```c#
public static class UnityConfig
{
    private static UnityContainer _container;
    public static UnityContainer GetContainer()
    {
        return _container;
    }
    public static void RegisterComponents(HttpConfiguration config)
    {
        // register all your components with the container here
        // it is NOT necessary to register your controllers
        UnityContainer container = new UnityContainer();

        container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
        container.RegisterType<ICookiesService, CookiesService>(new ContainerControlledLifetimeManager());
        container.RegisterType<IConfigService, SiteConfig>(new ContainerControlledLifetimeManager());

        DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        _container = container;
    }
}
```
Set up your query string keys to watch & register the http module via Web.config
```c#
<appSettings>
    <add key="WatchedKeys" value="example,example1,example2" />
    <add key="Example.ApiKey" value="example1293jD_nWo.example94keLdm" />
</appSettings>
<modules>
      <add name="CookieModule" type="Example.Namespace.CookeHttpModule.CookieModule" preCondition="managedHandler" />
</modules>
```
