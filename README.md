# Translator app homework Botond Rakoczi

Build docker images:  
docker build -t translator-api:latest -f .\LanguageWire.Api\Dockerfile .  
cd LanguageWire.WebApp  
docker build -t translator-webapp:latest -f .\Dockerfile .

Run docker images:  
docker run -p 5000:5000/tcp translator-api  
docker run -p 5001:80/tcp translator-webapp

Frontend access: http://localhost:5001/index.html  
Api access: http://localhost:5000/index.html
