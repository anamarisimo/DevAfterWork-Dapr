# How to run the apps locally

Prerequisites: Docker

## Steps:

- Install the dapr-cli: https://docs.dapr.io/getting-started/install-dapr-cli/

- Initialize Dapr in your local environment: https://docs.dapr.io/getting-started/install-dapr-selfhost/

- Run the following commands:

  docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management

  docker pull mcr.microsoft.com/mssql/server:2022-latest
  docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!"-p 1433:1433 --name dapr-sqlserver -d mcr.microsoft.com/mssql/server:2022-latest

- Have fun playing around! :)

