docker build -t gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest .
echo gldt-EVDRJQ22KDWyDMsbsrnw | docker login -u gitlab+deploy-token-portal-salud gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente --password-stdin
docker push gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest

rem Docker pull
rem gitlab+deploy-token-read-salud
rem gldt--sq6q3_f-4ezcqq4_2yB

rem echo gldt--sq6q3_f-4ezcqq4_2yB | docker login -u gitlab+deploy-token-read-salud gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente --password-stdin
rem docker pull gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest