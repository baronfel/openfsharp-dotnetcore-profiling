FROM mcr.microsoft.com/dotnet/core/sdk:3.0 as build

WORKDIR /app

COPY . /app

RUN dotnet publish -c Release -r debian.10-x64 -f netcoreapp3.0

FROM mcr.microsoft.com/dotnet/core/sdk:3.0 as debug_tools

ENV TOOL_VERSION=3.0.0-preview9.19454.1

# install dotnet tools for dump and sos
ENV PATH=$PATH:$HOME/.dotnet/tools
RUN dotnet tool install -g dotnet-symbol
RUN dotnet tool install -g dotnet-trace --version ${TOOL_VERSION}

FROM debug_tools

COPY --from=build  /app/bin/Release/netcoreapp3.0/debian.10-x64/publish /app

ENV PATH=/root/.dotnet/tools:$PATH
ENV COMPlus_PerfMapEnabled=1

WORKDIR /app

# preload framework PDBs
RUN dotnet symbol *.dll

CMD [ "./Profile" ]