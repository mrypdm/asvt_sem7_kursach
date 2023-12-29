all: clean build

clean:
	dotnet clean

build:
	dotnet restore
	dotnet build -c Release --no-restore

install:
	dotnet publish -c Release -o ./install --no-build