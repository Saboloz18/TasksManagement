FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . /src

RUN dotnet restore "TasksManagementAPI/TasksManagementAPI.csproj"
RUN dotnet build "TasksManagementAPI/TasksManagementAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "TasksManagementAPI/TasksManagementAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TasksManagementAPI.dll"]