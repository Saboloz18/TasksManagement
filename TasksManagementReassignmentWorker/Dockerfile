FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . /src

RUN dotnet restore "TasksManagementReassignmentWorker/TasksManagementReassignmentWorker.csproj"
RUN dotnet build "TasksManagementReassignmentWorker/TasksManagementReassignmentWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "TasksManagementReassignmentWorker/TasksManagementReassignmentWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TasksManagementReassignmentWorker.dll"]