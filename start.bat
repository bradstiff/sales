start "Proposing.API" /d "src\Services\Proposing\Proposing.API" cmd
start "Sales.BFF" /d "src\ApiGateway\Sales.Bff" cmd
start "Client" /d "src\sales-client" npm start
