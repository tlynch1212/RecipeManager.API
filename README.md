[![Build Status](https://travis-ci.com/tlynch1212/RecipeManager.API.svg?token=9yh5dvb5xvXyjzKPfH36&branch=master)](https://travis-ci.com/tlynch1212/RecipeManager.API)


# RecipeManager.API
Backend for the recipe manager

# Postgres with psadmin4
## Postgres DB Server 
> Linux Container
```bash
docker volume create postgres_data
docker run -d -p 7978:5432 --name postgres_server --restart=always -e "POSTGRES_PASSWORD=password" -v postgres_data:/var/lib/postgresql/data postgres
```
Note: The username will be `postgres` by default. Use it with the password in the command.
---
## Postgres psadmin4
> Linux Container
```bash
docker run -d -p 7977:80 --name postgres_adminui --restart=always -e "PGADMIN_DEFAULT_EMAIL=email" -e "PGADMIN_DEFAULT_PASSWORD=password" dpage/pgadmin4
```

*Note: When connecting to a server via pgadmin4 UI, use the hostname `host.docker.internal` instead of the IP or local hostname...*