#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Vote_API/Vote_API.csproj", "Vote_API/"]
RUN dotnet restore "Vote_API/Vote_API.csproj"
COPY . .
WORKDIR "/src/Vote_API"
RUN dotnet build "Vote_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vote_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vote_API.dll"]