# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application codee
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

#!##############################################################################

# Use the official ASP.NET runtime image to run the application
# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 

# Set the working directory
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/out .

# Expose the port the app runs on
# ENV ASPNETCORE_URLS=http://*:8080
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "gcp_api.dll"]