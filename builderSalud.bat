docker build -t saludportalweb .

rem ejecutar localmente: docker run --name saludportal --rm -p 8080:80 saludportal:latest

docker build -t gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest .
echo gldt-EVDRJQ22KDWyDMsbsrnw | docker login -u gitlab+deploy-token-portal-salud gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente --password-stdin
docker push gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest

rem Docker pull
rem gitlab+deploy-token-read-salud
rem gldt--sq6q3_f-4ezcqq4_2yB

rem echo gldt--sq6q3_f-4ezcqq4_2yB | docker login -u gitlab+deploy-token-read-salud gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente --password-stdin
rem docker pull gitlab.whitebox.com.ar:5005/salud-portalpaciente/salud-portalpaciente:latest


1.	Formato de Excel en los lotes precargados. Permitir enviar en formato xls y xlsx

2.	Cédulas manuales, verificar si salen los mails y que se pueda configurar para que no haya que seleccionar el tipo de cédula.

3.	Sesión que se caduca y no permite volver a ingresar.

4.	Formato de Excel de lotes precargados.

5.	Verificar lotes enviados por Cristian.

53019
53020
53021