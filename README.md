---
title: ReqResponse Readme
---

By Stewart Hyde
===============

Introduction
============

I design this GitHub project for demonstrating the ability to create a testing
environment designed to test XML based service to a simulated device which could
be replaced with a hardware device communicating via sockets.

I have found in my struggle during the last six months that I must improve my
knowledge of new technique because my previous job was behind the time in
Technology. But the internet can improve one’s knowledge and using experience
from the past and knowledge of systems, we can create a reliable platform. We
could use the end design on the hardware device and we can use the included unit
test during build cycle but depending on additional hardware requirements. We
could test the unit test code with the hardware or a simulated base on xml
requirements.

We can test the Blazor application with QA department and reports will be stored
in SQL database and if any issue comes up, it can be later determine because of
testing or because of an actual change in code on the device that is Connected
via the socket connection. Any issues can be setup to email so that It will make
notification to the user.

Basic application design
========================

This is project is completely in .Net Core 3.1 but upgraded to .Net 5 and uses
the latest Blazor technology to show results to use and email any errors to a
specific user. The logic for the simulated device is basic XML request and
response system. With 4 primary operations which are Add, Subtract, Multiple and
Divide.

First part was to create ReqResponse.dll which contains the 4 methods, common
xml models and service to process the methods. There are basically two methods:
a local method and connected network method. The communications to the server
are based on techniques from Microsoft but upgraded to .net Core, which can be
found with the following link for server.

<https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcplistener?redirectedfrom=MSDN&view=netframework-4.7.2>

And for the client

<https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netframework-4.7.2>

ReqResponse. Service.exe is a simple command line server, which calls a routine
in ReqResponse.dll

![](media/b4cb34e6663d8735ecc4c7e86a531161.png)

The following sample appsettings.json for server configuration settings.

![](media/756da10e9e544b95df18f70186ae5777.jpg)

With server code logging turn on, the following is output

![](media/539f19fbac2b49c22ed38f2211c7e227.png)

In addition, I have created ReqResponse.BackupServer which if ReqResponse.Server
is down, the client will switch to the Backup Server – but once it comes primary
server comes back up it will switch back. I have found that TcpClient connection
times are around 2 seconds. Depending on what is being connected if would be
better for performance reasons to use Stay Connected mode which only connects
for the first connections. But if down it switches to open / disconnect mode.

Connected ReqResponse.Test is a MSTest project is possible with Test Explorer.
The of Test project Connected is not available and Remote is off by default and
turn on by Options.cs.

The following is example screen used by Test Explorer.

![](media/4994ec929fde2245aa7cbbab8f8ec3b7.png)

There is also a small include TestApp.exe console application, I found this was
a simple way to test method during development.

Blazor Web Test application
===========================

This is a visual way to display the results of test is a web application that
can results can be stored in SQL database and saved for later analysis. Current
support for different socket connections is not currently available. There are 3
ways to test services.

1.  Local: where is is local request call.

2.  Remote: This remote service call by default to localhost and port 11000 and
    by default does 4 requests. The connection is closed on every call, but
    slower.

3.  Connected: This is similar Remote except that connection and stream are kept
    open and does 10 requests. If disconnected, it will change behavior to 4 max
    requests.

Requests are limited so that user interface to refresh.

![](media/61a18efa2959a2cafc24fdc6f06b543c.jpg)

The following screen is home screen for same applications as Blazor Web Assembly

![](media/6b871bfbb5d9f6a1e83bfb633af82f25.png)

For this example, local execution of methods is possible without the server. I
did this first, and local requests are significantly faster than remote
connections to a server.

![](media/fb4d1bdc72a432c4b70192ae1219159e.jpg)

Remote connections to server need special attention, I specifically the designed
to methods not to use async connections to simulate communication with device.
Because of deigned of Blazor screen, I figured out a method to stage requests 4
at time executing communicating to the server.

When the client is done with executing all requests, the message shows it has
finish processing.

Once it has finished the request, it saves the results in SQL database using
Dapper Stored Procedures.

Memory resident class for in-memory database service using dependency injection.

![](media/1dc9518202b024c9ceeb6b5bd8dd2b24.jpg)

Connected: This is an optimized version of Remote Request, where TcpClient and
NetworkStream is kept open for the request.

This reduces connection time.

![](media/e7359ae3d31b2d7ecfd69b3bece55f75.jpg)

To help isolated issues, a summary is used to display results with the following
information

Request Option: There are 3 type of Requests: Local, Remote or Connected

Time Executed: This time executed from client side.

ResponseSetId: Unique Id for all requests for this test

Successful count: This is the count of successful tests

Failed Count: This is a count of failed tests and there are 2 test that always
failed

Ok Count: This is a count of tests that work logically base on input and output

Error Count: This is a count of tests that have a logical error base on input
and output

The difference between Failed and Error is that ErrorCount are a programmatic
test for a specific error, like divide by zero for divide operation or math
overruns.

![](media/27b386c5cdb3074f7bc556dabcb878c9.jpg)

Once the screen is refreshed, it provides the error list on the screen showing
the records which have errors. Here, there are two specific errors.

1.  Multiple test where included a test of expected value is not correct

2.  Multiple test it marked where expected result should MathError but as Ok.

![](media/66f5ebb54041efdb6f3fafabf1904791.png)

There is also screen that allows emailing of request to email address in Json.
Future enhancement will store the current and time in the database.

![](media/e493f66de400002b82648907dc7c74a2.png)

Note if the ReqResponse.Server.exe is not loaded and response comes back as the
FailedConnection and success is reported as false. This a not a problem on
server, but that connection is not connected during the test. On the device, it
could mean that it has some network issues that will need to be a look at.

![](media/888d0f13fb5fd8ecf665362e86373de9.jpg)

With connected calls, it will switch to remote mode and only checks every 4
requests instead 10.

![](media/cbe4bce23e51102ae5ed3abc71869e63.jpg)

If connection, resume it will return to normal after finish disconnected loop

![](media/6099d38bf571ed02eaa564d04fc545c4.jpg)

The system can report only errors with the following screen as below.

![](media/1049b4c49633c15bc6b784c4ef657e7d.jpg)

Blazor Web Simulated default Requests
=====================================

The ReqResponse.DataLayer.dll there is logic to load the simulated data into
memory and it stores data into SQL database. The following is a screenshot from
class.

ReqResponse.DataLayer.Data.Sim. RequestSimDataService

![](media/3acedc95ef89a30bcb4f61e4440362f8.png)

Blazor Web SQL Sample Data
==========================

Requests are stored into SQL tabled and will later be loaded from this Table. If
the table is empty, this table will contain the data loaded from simulated
routine CreateDefaultTestRequests

![](media/1c30948801af51f575d0840952ad4962.png)

When a test is performed using the Blaser application, it stores the results in
the following table. The actual requests is referenced by RequestId.

![](media/40e1ae837ac30685bd0ac09acaeb0966.png)

There is also a table that is used to calculate a summary for ResponseSetId,
which is all rows in the Requests table.

![](media/36a135a8c447ae14f751e27e64a87064.jpg)

Blazor example Email
====================

The Blazor application as ability to email the specific user. It uses email
configuration in the Json file.

![](media/5da2959c1bec3165c53432e08c8a18ca.png)

Blazor Json for Connect string and Email
========================================

Note Json files must be a change for your specific configuration.

![](media/ef4103c0130385d382fbf493b74de054.png)

WPF Web Test application
========================

In addition, there is WPF client application that uses same servers with same
services, but screen is not as detail as Blazor application. Application home
grown MVVM and uses only .Net 5.0 packages.

The following screen is Home screen, which is same as Index screen in Blazor.

![](media/8473123e167035c1fc9f44119b23918a.jpg)

The following screen is use load local testing of requests.

![](media/bb1f9cdb62e43ddb1860900dd3a9d2c9.jpg)

The following screen is for remote requests, which means it connects and
disconnects to each service request.

![](media/f4af1b9d8ad0295e203a2fecfb102fca.jpg)

When remote requests finish, the following screen is displayed.

![](media/889da0d678983eff25018aab53838eb9.jpg)

The following is for connected screen which connects opens and keeps connected
for 10 requests. It shows the following screen when finish.

![](media/9a902dba6c59d58c9fcb29be6bd30a16.jpg)

The following is summary, which currently only lists summary records without
details.

![](media/c7a60ce2dd6a0b37d8a750602abfd307.jpg)

The following screen shows errors and allows email if there are errors.

![](media/e79267e93b68db7f276c8ddc8c9ebb13.jpg)

ReqResponse.Command command line application
============================================

A command line application is provided, it designed so that it can be use when
building a device or ROM and then verifying that it executes correctly.

![](media/bc0232e15f3b0fda8fa713ebfe558f34.jpg)

Updates
=======

02/24/2021 Updated Project to Net 5.0 and some cleanup.

02/26/2021 Updated Project for performance considerations and separating data
and service layers from user interface.

03/03/2021 Updated Project to include new WPF (.Net 5.0) client using same
services and data.

03/04/2021 Updated Project to command line version

03/12/2021 Updated Add Blazor Webassembly plus backup server for requests

