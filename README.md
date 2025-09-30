# SMS Gateway (sms sending application)

This is a high-performance sample application designed for efficiently sending SMS messages.

## Solution Overview

The solution contains two executable projects:

- **Web API project**  
  Handles incoming user requests.

- **Console Worker project**  
  A simple background worker that processes SMS sending using Kafka and pushes results back to Kafka.

## Prerequisites

- Kafka broker
- SQL Server instance

If you have prior experience with ASP.NET projects, configuring the connection strings and other settings will be straightforward:

- For the **Web API**, configuration is done via the `appsettings.json` file.
- For the **Console Worker**, configurations are passed via command-line arguments.

## Building and Running

- You can build the projects using the `.NET CLI` or `MSBuild`.
- Dockerfiles are included for both projects, allowing you to create Docker images and run containers.
- A .NET Aspire-based approach will be available soon.

## Current Limitations & Improvements Needed

1. No unit tests have been implemented yet.
2. Exception handling requires significant improvement.
3. Logging and monitoring are not yet integrated and must be added.

