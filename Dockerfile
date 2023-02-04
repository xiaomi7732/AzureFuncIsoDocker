FROM mcr.microsoft.com/dotnet/sdk:6.0 AS installer-env
EXPOSE 80
# Build requires 3.1 SDK
COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet /usr/share/dotnet

COPY . /src/dotnet-function-app
RUN cd /src/dotnet-function-app && \
    mkdir -p /home/site/wwwroot && \
    dotnet publish *.csproj --output /home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet-isolated:3.0-dotnet-isolated5.0-appservice
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0

ENV APPINSIGHTS_INSTRUMENTATIONKEY=5d8258e7-abb2-4066-89a5-8c73071b74ff
ENV COMPlus_EnableDiagnostics=0
ENV AzureWebJobsStorage=DefaultEndpointsProtocol=https;AccountName=saarsazfuncstorage;AccountKey=lfEl/DL3HQ0YZDr/8sbqu8P4xcJ0i641tTfgY4JyyR6dz4POhO+sVLyFFPZhEWs+Zdu9y+0+SoF6/lT1SJsUpg==;EndpointSuffix=core.windows.net
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]