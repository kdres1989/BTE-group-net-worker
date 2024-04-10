
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
ENV TZ=America/Bogota
WORKDIR /app
EXPOSE 80

FROM base AS final
WORKDIR /app
COPY _#{Build.Repository.Name}#/Artifact-#{Build.Repository.Name}#  .
ENTRYPOINT ["dotnet", "BTE-group-net-worker.dll"]
