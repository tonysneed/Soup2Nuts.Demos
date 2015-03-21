Soup to Nuts Final Demo: After ReadMe

Prerequisites:

1. Visual Studio 2013 Community or Professional with Update 4 or later.
   https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx
2. EF 6.x Tools for Visual Studio
   http://www.microsoft.com/en-us/download/details.aspx?id=40762
3. Entity Framework Power Tools
   http://visualstudiogallery.msdn.microsoft.com/72a60b14-1581-4b9b-89f2-846072eff19d
4. Trackable Entities for VS 2013 v2.5 or later
   https://visualstudiogallery.msdn.microsoft.com/74e6d323-c827-48be-90da-703a9fa8f530
5. Simple MVVM Toolkit v5.5 or later
   https://visualstudiogallery.msdn.microsoft.com/b2d3cf62-61e0-49a9-a12e-a3be24d0431f
   - Update NuGet packages to 5.5.1 or later
6. Scaled down version of the Northwind sample database:
   http://bit.ly/northwindslim.

The Before solution contains separate service and client entities, with change tracking
that is propagated aross service boundaries and persisted using Entity Framework. There
is a console client which exists to show how to use HttpClient and client change tracking,
and to verify that the service is functioning as expected.  This solution demonstrates
the application of the Model-View-ViewModel pattern using a portable class library to
contain view models for any client platform, as well as a WPF application which binds
to the view models.  Feel free to extend the sample by adding a Xamarin Forms client.
Note that unit and integration tests can also be added.

Steps to re-create the 'After' solution:

1. Create a new Portable Class Library project: SoupToNuts.Final.Client.Common
   - Target (Uncheck Silverlight 5, Windows Phone 8.1):
     .NET 4.5
	 Windows 8
	 Windows Phone Silverlight 8
   - Add the following Nuget packages:
     SimpleMvvmToolkit.Portable
	 TrackableEntities.Client
   - Add a reference to the SoupToNuts.Final.Entities.Client.Portable project
   - Add the following interfaces to a Services folder:
     > ICustomerService, IProductService, IOrderService
   - Add the following classes to a ViewModels folder:
     > MainPageViewModel, CustomerOrdersViewModel, OrderViewModelDetail

2. Add a new WPF project: SoupToNuts.Final.WpfClient
   - Add the following references:
     System.Net.Http
	 System.Windows
   - Add the following packages:
     SimpleMvvmToolkit
	 TrackableEntities.Client
	 Microsoft.AspNet.WebApi.Client
	 AspNetWebApi2Helpers.Serialization
	 SimpleInjector
   - Add a reference to the following projects:
     SoupToNuts.Final.Client.Common
     SoupToNuts.Final.Entities.Client.Portable
   - Add a reference to SimpleMvvmToolkit-Portable (browse to dll in packages)
   - Add the following folders:
     > Locators, Services, Views

3. Set up services
   - Add static Constants class with web client host and port number
   - Add static ServiceProxy class with following properties:
     > Instance (HttpClient) and Formatter (MediaTypeFormatter)
	 > Config formatter to handle cycles
   - Add the following classes to the Services folder:
     > CustomerService, ProductService, CustomerService, OrderService
	 > Implement services interfaces

4. Set up dependenecy injection   
   - Add a ViewModelLocator class to the Locators folder
   - Add SimpleInjector Container field
   - Init in ctor and Bootstrap method
   - Register services, view models (Lifestyle.Transient)
   - Add view model properties:
     > Read-only, call _container.GetInstance
     MainPageViewModel, CustomerOrdersViewModel, OrderViewModelDetail
   - Add ViewModelLocator as an Application resource: 
    <locators:ViewModelLocator x:Key="Locator" />
     
5. Add a Window to the Views folder: OrderDetailView
   - Set height to 310, width to 480
   - Add following namespaces:
     xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
     xmlns:ei="http://schemas.microsoft.com/expression/2010/interactions"
   - Set data context:
     DataContext="{Binding Source={StaticResource Locator}, Path=OrderViewModelDetail}"
   - Add loaded event trigger:
    <i:Interaction.Triggers>
        <i:EventTrigger EventName="Loaded">
            <ei:CallMethodAction TargetObject="{Binding}"
                MethodName="LoadProducts"/>
        </i:EventTrigger>
    </i:Interaction.Triggers>
   - Replace Grid
   - Edit the view code-behind to subscribe to view model events
   - Handle Unloaded event to unsubscribe to view model events

6. Add a UserControl to the Views folder: CustomerView
   - Change same namespaces as above
   - Replace the Grid in the XAML view
   - Edit the view code-behind to subscribe to view model events
   - Handle Unloaded event to unsubscribe to view model events
     
7. Set MainWindow properties
   - Height, Width, Title
   - DataContext="{Binding Source={StaticResource Locator}, Path=MainPageViewModel}"
   - Replace Grid:
    <StackPanel>
        <TextBlock Text="{Binding Path=BannerText}" FontFamily="Verdana" FontSize="18" FontWeight="Bold" HorizontalAlignment="Center" Margin="0,15,0,0" />
        <my:CustomerView />
    </StackPanel>
