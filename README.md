# SensitiveWordsAPI

A C# .NET 8 microservice that detects and censors sensitive words. Built with Clean Architecture, Dapper, and Swagger.

## Features
- REST API with Swagger docs
- API Key authentication
- Word CRUD endpoints (internal)
- Sanitization endpoint (external)
- Exception handling middleware
- xUnit + Moq unit tests

## Getting Started

1. Clone the repo
2. Run `dotnet restore`
3. Update `appsettings.json` with your API key
4. Run the project: `dotnet run`

API Docs available at `/swagger`.

## Author

Wouter Human – wouterhuman@gmail.com


For Deployment to Azure

Option 1: CI/CD Deployment to Azure with GitHub Actions
1. Prerequisites
Azure Subscription

Create an Azure App Service for .NET 8

Install Azure CLI and login:

bash
Copy
Edit
az login
2. Configure Azure App Service for Deployment
Create the App Service and Resource Group if not already done:

bash
Copy
Edit
az group create --name SensitiveWordsRG --location "West Europe"
az appservice plan create --name SensitiveWordsPlan --resource-group SensitiveWordsRG --sku B1 --is-linux
az webapp create --resource-group SensitiveWordsRG --plan SensitiveWordsPlan --name sensitivewords-api --runtime "DOTNET|8.0" --deployment-local-git

3. Add GitHub Actions Workflow for Azure
Inside .github/workflows/azure.yml:

yaml
Copy
Edit
name: Build and Deploy to Azure

on:
  push:
    branches:
      - main

jobs:
  build-deploy:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore --configuration Release

    - name: Test
      run: dotnet test --no-build --verbosity normal

    - name: Publish
      run: dotnet publish -c Release -o ./publish

    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: sensitivewords-api
        slot-name: production
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
In Azure, go to the App Service → Get publish profile, then upload that to GitHub as a secret named AZURE_WEBAPP_PUBLISH_PROFILE.


To deploy to AWS

Option 2: CI/CD Deployment to AWS Elastic Beanstalk
1. Prerequisites
AWS account

Create an Elastic Beanstalk Environment (.NET Core on Linux)

Install AWS CLI and EB CLI:

bash
Copy
Edit
pip install awsebcli --upgrade
aws configure

2. Initialize and Configure
From the project root:

bash
Copy
Edit
eb init -p "dotnet-core" SensitiveWordsAPI --region eu-west-1
eb create SensitiveWords-env

3. Add GitHub Actions Workflow for AWS
Inside .github/workflows/aws.yml:

yaml
Copy
Edit
name: Build and Deploy to AWS

on:
  push:
    branches:
      - main

jobs:
  deploy:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout code
      uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore --configuration Release

    - name: Test
      run: dotnet test --no-build

    - name: Publish
      run: dotnet publish -c Release -o ./publish

    - name: Deploy to Elastic Beanstalk
      env:
        AWS_ACCESS_KEY_ID: ${{ secrets.AWS_ACCESS_KEY_ID }}
        AWS_SECRET_ACCESS_KEY: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
      run: |
        zip -r deploy.zip ./publish
        eb deploy
Add the following GitHub secrets:

AWS_ACCESS_KEY_ID

AWS_SECRET_ACCESS_KEY



Test Deployment
Once you've pushed changes to the main branch:

Go to GitHub → Actions tab → See your workflow running.

Once deployed, access your API via:

Azure: https://sensitivewords-api.azurewebsites.net/swagger

AWS: http://<elastic-beanstalk-env>.elasticbeanstalk.com/swagger


