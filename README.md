# MoneyGo
Esta aplicación fue un ejercicio en el master que realicé. En ella se utiliza, SQL server para la base de datos, Javascript y Bootstrap para la parte frontal y .Net Core para el servidor.

## Trasfondo de la aplicación
AL ser un  ejercicio, comenzó siendo una aplicacón sencilla, sin usuarios y con un encriptado básico.
Con el paso del tiempo y con el avance de las clases, se fue migrando de una aplicación monoliotica. Primero se llevo todas las funciones del servidor a una API. 
Luego las bases de datos se alojaron, primero en Azure y después en AWS.
La aplicación se desplego en Azure y en maquina EC2 de Amazon.

Como la aplicación empezo con unas funcionalidades básicas, fue sencillo añadir funcionalidades que hicieron crecer la aplicación.
Empece con la integración del algoritmo AES256 para el cifrado de contraseñas. 
Después integre la funcionalidad de recuperación y envio de un welcome Pack a los nuevos usuarios.
Para recuperar la contraseña, tuve que implementar un sistema de tokens que identificaban al usuario.
Además, en el panel de control de usuario, se integrarón nuevas caracteristicas quepermitian un control total de sua cuenta al usuario.

##Imagenes de la aplicación

![image](https://user-images.githubusercontent.com/40211718/236894860-f7842ae3-d23a-4d3a-b1fa-052d9ee179a2.png)
![image](https://user-images.githubusercontent.com/40211718/236895514-3ae08fa9-91d6-43cb-81ce-32ab582aff4c.png)

Login
![image](https://user-images.githubusercontent.com/40211718/236897220-3774caf9-bdc8-4311-be8b-dd54ef4f7932.png)


Register
![image](https://user-images.githubusercontent.com/40211718/236894711-853f83c9-6369-4376-8654-702a1a710d64.png)
