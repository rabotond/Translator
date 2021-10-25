# Translator app homework

Build docker images:
docker build -t translator-api:latest -f .\LanguageWire.Api\Dockerfile .

Run docker images: 
docker run -p 5000:5000/tcp translator-api