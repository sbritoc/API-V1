/*MODIFICACION A LAS TABLAS*/

alter table Distribucion alter column Doc_Cenabast int NOT NULL;
alter table Distribucion alter column Factura int;
alter table Distribucion alter column Guia int;

ALTER TABLE Distribucion DROP CONSTRAINT PK_informacionLogistica;/*elimnar PK*/
ALTER TABLE Distribucion ADD CONSTRAINT PK_Doc_Cenabast PRIMARY KEY (Doc_Cenabast);

alter table DistribucionDetalle alter column Doc_Cenabast int NOT NULL;
alter table DistribucionDetalle alter column Cantidad int;
alter table DistribucionDetalle alter column Articulo int;

alter table DistribucionMovimiento alter column Doc_Cenabast int NOT NULL;

truncate table DistribucionDetalle
truncate table DistribucionMovimiento
truncate table Distribucion


}
