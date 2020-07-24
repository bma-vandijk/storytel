# Storytime

A mobile application that collects data about how children develop the art of storytelling.

## Getting Started

!Refer to general guides on how to clone the repository!
!Refer to general guides on how to prepare Visual Studio for Xamarin.Forms development!

Once you have the repository on your local machine, you will see it consists of a single solution containing three projects:

```
+ Storytime.sln               // The solution
+--+ Storytime.csproj         // The cross platform section of the app (Xamarin.Forms)
+--+ Storytime.Android.csproj // The android specific section of the app
+--+ Storytime.IOs.csproj     // The ios specific section of the app
```

These files contain information about how Visual Studio needs to handle the directory system and dependencies. Don't manually change these files.

To open the solution, simply open Storytime.sln using Visual Studio, and it will load the full solution including the three projects. 
In the Solution Explorer (the directory structure) you see the solution and projects. One of the projects will be bolded, indicating that is the startup project.
Make sure Storytime.Android is the startup project (if you are debugging on IOs, make Storytime.IOs the startup project, but that is not guaranteed to work).

The app runs as a single-page application, meaning the only page that is ever alive is MainPage.xaml(.cs), and it uses a view-swapper to change views. 
This is done by changing the ActiveView property on the context of the MainPage (MainPageVM.cs), or by changing it directly on the ViewHost.xaml(.cs) view container in the MainPage.

## Authors

* **Insert Name** - *Work done* - [HyperLinkTagHere](LinkHere)
