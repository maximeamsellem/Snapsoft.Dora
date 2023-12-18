docker run --name local-postgres -e POSTGRES_PASSWORD=postgres123456 `
    --restart unless-stopped -p 5432:5432 -d postgres:16-alpine3.19