FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Worktastic/Worktastic.csproj", "Worktastic/"]
RUN dotnet restore "Worktastic/Worktastic.csproj"
COPY . .
WORKDIR "/src/Worktastic"
RUN dotnet build "Worktastic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Worktastic.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worktastic.dll"]
