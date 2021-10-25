# Translator app homework

Build docker images:
docker build -t translator-api:latest -f .\LanguageWire.Api\Dockerfile .
docker build -t translator-webapp:latest -f .\LanguageWire.WebApp\Dockerfile .

Run docker images: 
docker run -p 5000:5000/tcp translator-api
docker run -p 5001:80/tcp translator-webapp