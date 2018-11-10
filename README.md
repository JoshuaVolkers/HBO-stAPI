# HBO-stAPI
(HBO-stagemarkt API)

.NET Core 2.0 API.

## Features

- .NET Core Dependency Injection.
- AutoMapper 7.0 with dynamic IDataReader to object mapping.
- JWT authentication and authorization.
- Refresh token implementation.
- Full Swagger docs using Swashbuckle .NET Core.
- Automatic builds and deployments using Azure DevOps.
- Integrated functional tests.

## Endpoints

- Users
- Companies
- Vacancies
- Options (root-data)

## Documentatie

Swagger docs:
https://app.swaggerhub.com/apis/JoshuaVolkers/PoohAPI_DEV/V1

## Build-/Deployment-pipeline

Azure DevOps integration for automatic building and deploying of API and resources on Azure.
Deployment-pipeline with integrated testing and testreport generation.

### dev-branch
Build:

[![Build Status](https://tobedev.visualstudio.com/HBO-stAPI/_apis/build/status/Build%20Dev)](https://tobedev.visualstudio.com/HBO-stAPI/_build/latest?definitionId=4)

API Deployment:

[![Deploy API Status](https://tobedev.vsrm.visualstudio.com/_apis/public/Release/badge/a24b79ce-b1bb-4117-979f-dd97c366e23a/3/3)](https://tobedev.vsrm.visualstudio.com/_apis/public/Release/badge/a24b79ce-b1bb-4117-979f-dd97c366e23a/3/3)

Tests:

[![Run Tests Status](https://tobedev.vsrm.visualstudio.com/_apis/public/Release/badge/a24b79ce-b1bb-4117-979f-dd97c366e23a/3/8)](https://tobedev.vsrm.visualstudio.com/_apis/public/Release/badge/a24b79ce-b1bb-4117-979f-dd97c366e23a/3/8)


### master-branch
Build:

[![Build Status](https://tobedev.visualstudio.com/HBO-stAPI/_apis/build/status/Build%20Master)](https://tobedev.visualstudio.com/HBO-stAPI/_build/latest?definitionId=5)

API Deployment:

[![Deploy API Status](https://tobedev.vsrm.visualstudio.com/_apis/public/Release/badge/a24b79ce-b1bb-4117-979f-dd97c366e23a/4/4)](https://tobedev.vsrm.visualstudio.com/_apis/public/Release/badge/a24b79ce-b1bb-4117-979f-dd97c366e23a/4/4)

