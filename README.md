# Polish-election-simulation

## Running the project using docker via terminal
```bash
docker build -f Dockerfile -t PolishElectionImage .

docker stop PolishElection || true
docker rm PolishElection || true

docker run --name PolishElection \
  -e ConnectionStrings__DefaultConnection="YOUR_DB_CONNECTION_STRING" \
  -e Jwt__Key="YOUR_JWT_SECRET_KEY" \
  -e Jwt__Issuer="YOUR_JWT_ISSUER" \
  -e Jwt__Audience="YOUR_JWT_AUDIENCE" \
  -e Jwt__ExpireMinutes=60 \
  PolishElectionImage
```
### Environment variables & secrets

All -e flags define environment variables consumed by the backend at runtime.

These values are secrets or environment-specific configuration and must not be committed to Git.
You are expected to provide your own values.

### Recommended: .env file

Instead of passing secrets inline, you can use an .env file:
```
ConnectionStrings__DefaultConnection=...
Jwt__Key=...
Jwt__Issuer=...
Jwt__Audience=...
Jwt__ExpireMinutes=60
```

Run the container with:
```bash
docker build -f Dockerfile -t PolishElectionImage .

docker stop PolishElection || true
docker rm PolishElection || true
docker run --name PolishElection --env-file .env PolishElectionImage
```
