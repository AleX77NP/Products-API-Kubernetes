FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["products.csproj", "./"]
RUN dotnet restore "products.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "products.dll"]
