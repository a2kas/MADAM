#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat
FROM dockerrepoapi.tamro.lt/dotnet/sdk:8.0.200-bookworm-slim AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY . ./
#COPY DataMonitor/NuGet.config ./
RUN dotnet restore Tamro.Madam.sln
# Copy everything else and build
RUN dotnet publish Tamro.Madam.sln -c Release

# Build runtime image
FROM dockerrepoapi.tamro.lt/dotnet/aspnet:8.0.200-bookworm-slim AS runtime
WORKDIR /app
COPY --from=build /app/Tamro.Madam.Ui/bin/Release/net8.0/publish .

ENV ASPNETCORE_URLS=http://+:7000
ARG APP_ENV
ENV ASPNETCORE_ENVIRONMENT ${APP_ENV}
ARG APP_VERSION
ENV APP_VERSION ${APP_VERSION}

RUN sed -i '/\[openssl_init\]/a ssl_conf = ssl_sect' /etc/ssl/openssl.cnf
RUN printf "\n[ssl_sect]\nsystem_default = system_default_sect\n" >> /etc/ssl/openssl.cnf
RUN printf "\n[system_default_sect]\nMinProtocol = TLSv1.2\nCipherString = DEFAULT@SECLEVEL=0" >> /etc/ssl/openssl.cnf

EXPOSE 7000/tcp
#ENTRYPOINT ["sleep","3600"] for testing purposes
ENTRYPOINT ["dotnet", "Tamro.Madam.Ui.dll"]
