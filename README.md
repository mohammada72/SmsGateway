# SmsGateway

This is a sample application for sending sms efficiently and high performant. 


These are two executable project in solution:
1. WebApi project that handle requests from user.
2. Console app project that is a simple worker working only kafak to send sms and push result of them to kafka

You need kafa broker and sql server to run it. If you already worked on asp.net project you know how can configure those configuration in appsettings.json file for webApi and commandline argument for console app.

You can build them with .net cli or msbuild and also exists docker files in solution. so you can create images and run container. .Net aspire approach will be ready soon. 
Some things need to be improved :
1- There is no unit tests yet
2- exception handling is awful
3- logging and monitoring must be added.
