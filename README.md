# IrBusWebService

Iran Bus Service Package

Webservice package for iran bus api on safar724 website

## Development 

### Before posting new issues: [Test samples](https://bitbucket.org/keyone2693/irbuswebservice/src/344175d760ee1c2d88a9ce9ee217304301dc6d04/ExampleTest/?at=master)

Note that: you should register on safar724 website and get your username and password to using this package

[![Build status](https://img.shields.io/appveyor/ci/keyone2693/irbuswebservice.svg)](https://ci.appveyor.com/project/keyone2693/irbuswebservice)
[![NuGet](https://img.shields.io/nuget/v/IrBusWebService.svg)](https://www.nuget.org/packages/IrBusWebService/)

#### Current version: 1.5.x [Stable]

## Overview

## Cross-platform by design
you can use it in both .net core and .net framework 
its use .net standard

## Easy to install
Use library as dll, reference from [nuget](https://www.nuget.org/packages/IrBusWebService/)
or just use this in package manager console
```java
Install-Package IrBusWebService
```

## Features

Currently the library supports following method:

***
- [*] Getcode
- [*] GetToken
- [*] ValidateToken
- [*] GetCities
- [*] GetServices
- [*] GetBusService
- [*] BuyTicket
- [*] InfoBuyTicket
- [*] RefundOverviewTicket
- [*] RefundTicket
- [*] GetTokenAsync
- [*] GetCitiesAsync
- [*] GetServicesAsync
- [*] GetBusServiceAsync
- [*] BuyTicketAsync
- [*] InfoBuyTicketAsync
- [*] RefundOverviewTicketAsync
- [*] RefundTicketAsync
***

## Easy to use
#### Get Token (save it in database)
```java
IBusApi api = new BusApi();        
var token = api.GetToken("Username","Password");       

```
##### Note: the grant_type is password by default

#### Validate Token
```java
bool flag = api.ValidateToken(token.Created,token.ExpireIn);
```

#### GetCities:
###### This method lists all of the cities with a specific ID. Provided ID is required for further methods.Since list of cities rarely changes, caching the list for feature use is highly recommended:
```java
IBusApi _api = new BusApi(token.AccessToken);
var cities = _api.GetCities();
```

#### GetServices:
###### 
```java
Output Array of the Bus Summary object
```

#### GetBusService:
```java
Get a specific service
This method returns detailed information of the specific bus
```

#### BuyTicket:
```java
this method books a ticket from the specific service and returns aticket ID.
If online payment method is selected, this method also will returna payment endpoint.
To finalize the booking processfor the online payment method,
user must be redirected to the provided endpoint and should complete payment process within 10 minutes,
then ticket issuccessfully booked and ticket number isissued
```

#### InfoBuyTicket:
```java
Get a specific ticket
This method returns detailed information of the specific ticket
```
#### RefundOverviewTicket:
```java
Get refund overview of a ticket
This method returns detailed information about a ticket which is subjectto refund
```

#### RefundTicket:
```java
This method actually refunds the ticket
```



#### [MoreInfoAboutApi](https://bitbucket.org/keyone2693/irbuswebservice/src/master/IrBusWebService/Doc/safar724Doc.pdf)



# License

MIT

