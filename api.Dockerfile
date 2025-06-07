FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build-env

WORKDIR /App

# Copy 
COPY ./src .

# Build Api
WORKDIR /App/Cms.ImageService/src/Api/src

# Build and publish a release
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine as runtime-env

WORKDIR /App

COPY --from=build-env /App/Cms.ImageService/src/Api/src/out .

ENTRYPOINT ["dotnet", "Cms.ImageService.Api.dll"]

