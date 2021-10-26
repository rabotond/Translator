# Translator app homework Botond Rakoczi
Backend written on dotnet, frontend on angular by brakoczi

# Building and running docker images

Build docker images:  
docker build -t translator-api:latest -f .\LanguageWire.Api\Dockerfile .  
cd LanguageWire.WebApp  
docker build -t translator-webapp:latest -f .\Dockerfile .

Run docker images:  
docker run -p 5000:5000/tcp translator-api  
docker run -p 5001:80/tcp translator-webapp

Frontend access: http://localhost:5001/index.html  
Api access: http://localhost:5000/index.html

# PreRequirements 
To run the services without docker you need:
- dotnet core 3.1 LTS
- node 8<= 
- npm 14<=
- python3
- pyton3 libs(requests)

To run dotnet tests you need to modify "inMemorySettings" to point to your python installation folder