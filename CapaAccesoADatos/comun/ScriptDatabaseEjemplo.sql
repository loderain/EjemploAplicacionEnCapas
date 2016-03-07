/* 
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 * Author:  loderain
 * Created: 01-mar-2016
 */
create database ejemplo_capas
go

use ejemplo_capas
go

create table personas(
	idpersona int primary key identity,
	nombre_persona varchar(40),
	edad_persona integer)
go

create table mascotas(
	idmascota int primary key identity,
	nombre_mascota varchar(40),
	edad_mascota integer,
	persona integer foreign key references personas(idpersona))
go

insert into personas(nombre_persona,edad_persona) values ('Pepe',30);
insert into personas(nombre_persona,edad_persona) values ('Ana',17);
insert into personas(nombre_persona,edad_persona) values ('Phil',41);
insert into personas(nombre_persona,edad_persona) values ('Ramon',58);

--Mascotas de Pepe
insert into mascotas(nombre_mascota,edad_mascota,persona) values('Lila',18,1);
insert into mascotas(nombre_mascota,edad_mascota,persona) values('Pico',38,1);

--Mascotas de Ana
insert into mascotas(nombre_mascota,edad_mascota,persona) values('Lupo',3,2);

--Mascotas de Phil
insert into mascotas(nombre_mascota,edad_mascota,persona) values('Nika',42,3);
insert into mascotas(nombre_mascota,edad_mascota,persona) values('Beethoven',12,3);
insert into mascotas(nombre_mascota,edad_mascota,persona) values('Mozart',12,3);
