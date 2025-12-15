docker build -t saludportalweb .

rem ejecutar localmente: docker run --name saludportal --rm -p 8080:80 saludportal:latest

docker build -t gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest .
echo gldt-EVDRJQ22KDWyDMsbsrnw | docker login -u gitlab+deploy-token-portal-salud gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente --password-stdin
docker push gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest
