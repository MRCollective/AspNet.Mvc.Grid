AspNet.Mvc.Grid
===============

[![Build status](https://ci.appveyor.com/api/projects/status/y0p8apolcrgf1jmb?svg=true)](https://ci.appveyor.com/project/MRCollective/aspnet-mvc-grid)
[![NuGet downloads](https://img.shields.io/nuget/dt/AspNet.Mvc.Grid.svg)](https://www.nuget.org/packages/AspNet.Mvc.Grid) 
[![NuGet version](https://img.shields.io/nuget/vpre/AspNet.Mvc.Grid.svg)](https://www.nuget.org/packages/AspNet.Mvc.Grid)

The Grid control from [mvccontrib.codeplex.com](https://mvccontrib.codeplex.com) updated for ASP.NET MVC5 / .NET 4.5 and without dependencies (e.g. Mvc4Futures).

See [the documentation](https://mvccontrib.codeplex.com/wikipage?title=Grid&referringTitle=Documentation).

We created this because this is the only useful component we regularly use from MVC Contrib and it's not being kept up to date with the latest versions of MVC. We have had issues with the MVC5 breaking changes in using the MVCContrib library and we still want to use this functionality with MVC5.

Installation
------------

    Install-Package AspNet.Mvc.Grid

Changes from MVCContrib
-----------------------

In the spirit of the [Apache 2.0 license](https://tldrlegal.com/license/apache-license-2.0-%28apache-2.0%29) here are the major changes to the code:
* Compiled under .NET 4.5
* All namespaces changed
