CREATE DATABASE Historial;
GO

USE Historial;
GO

CREATE TABLE operacion (
id_operacion INT PRIMARY KEY IDENTITY (1,1),
expresion NVARCHAR(255) NOT NULL
);
GO

CREATE TABLE resultado (
id_resultado INT PRIMARY KEY IDENTITY (1,1),
resultado FLOAT NOT NULL
);
GO

CREATE TABLE calculadora (
id_calculo INT PRIMARY KEY IDENTITY (1,1),
id_operacion INT NOT NULL,
id_resultado INT NOT NULL,
fecha_operacion DATETIME NOT NULL DEFAULT GETDATE(),

FOREIGN KEY (id_operacion) REFERENCES operacion(id_operacion),
FOREIGN KEY (id_resultado) REFERENCES resultado(id_resultado)
);
GO
