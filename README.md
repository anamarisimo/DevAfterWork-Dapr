How to run the applications locally

Prerequisites: Docker

Steps:

1. Install the dapr-cli: https://docs.dapr.io/getting-started/install-dapr-cli/

2. Initialize Dapr in your local environment: https://docs.dapr.io/getting-started/install-dapr-selfhost/

3. Run the following:

docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:management

docker pull mcr.microsoft.com/mssql/server:2022-latest
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password123!"-p 1433:1433 --name dapr-sqlserver -d mcr.microsoft.com/mssql/server:2022-latest![image](https://github.com/user-attachments/assets/24b6b516-8897-4128-a8dc-bfa73b719c57)

4. Have fun playing around! :)

