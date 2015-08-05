/*
   Monday, August 3, 201511:27:49 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:33:52 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.Composition SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
ALTER TABLE dbo.CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:35:29 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.Composition SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:36:44 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.Composition SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:37:49 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.Composition SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:38:41 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
ALTER TABLE dbo.Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
ALTER TABLE dbo.CompositionType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:39:19 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
ALTER TABLE dbo.Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
ALTER TABLE dbo.CompositionType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:40:06 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
ALTER TABLE dbo.Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
ALTER TABLE dbo.CompositionType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:41:21 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
ALTER TABLE dbo.Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
ALTER TABLE dbo.CompositionType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:41:59 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
ALTER TABLE dbo.Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:42:31 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
ALTER TABLE dbo.Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:43:10 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Instrumentation
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation ON
GO
IF EXISTS(SELECT * FROM dbo.Instrumentation)
	 EXEC('INSERT INTO dbo.Tmp_Instrumentation (ID, Name)
		SELECT ID, Name FROM dbo.Instrumentation WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
DROP TABLE dbo.Instrumentation
GO
EXECUTE sp_rename N'dbo.Tmp_Instrumentation', N'Instrumentation', 'OBJECT' 
GO
ALTER TABLE dbo.Instrumentation ADD CONSTRAINT
	PK_Instrumentation PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Instrumentation_Insert ON dbo.Instrumentation FOR INSERT AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Update ON dbo.Instrumentation FOR UPDATE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Delete ON dbo.Instrumentation FOR DELETE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:43:45 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.Location SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Instrumentation
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation ON
GO
IF EXISTS(SELECT * FROM dbo.Instrumentation)
	 EXEC('INSERT INTO dbo.Tmp_Instrumentation (ID, Name)
		SELECT ID, Name FROM dbo.Instrumentation WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
DROP TABLE dbo.Instrumentation
GO
EXECUTE sp_rename N'dbo.Tmp_Instrumentation', N'Instrumentation', 'OBJECT' 
GO
ALTER TABLE dbo.Instrumentation ADD CONSTRAINT
	PK_Instrumentation PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Instrumentation_Insert ON dbo.Instrumentation FOR INSERT AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Update ON dbo.Instrumentation FOR UPDATE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Delete ON dbo.Instrumentation FOR DELETE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Key
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(15) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Key SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Key ON
GO
IF EXISTS(SELECT * FROM dbo.[Key])
	 EXEC('INSERT INTO dbo.Tmp_Key (ID, Name)
		SELECT ID, Name FROM dbo.[Key] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Key OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
DROP TABLE dbo.[Key]
GO
EXECUTE sp_rename N'dbo.Tmp_Key', N'Key', 'OBJECT' 
GO
ALTER TABLE dbo.[Key] ADD CONSTRAINT
	PK_Key PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Key_Insert ON dbo.[Key] FOR INSERT AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Key_Update ON dbo.[Key] FOR UPDATE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Key_Delete ON dbo.[Key] FOR DELETE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
/*
   Monday, August 3, 201511:44:19 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Instrumentation
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation ON
GO
IF EXISTS(SELECT * FROM dbo.Instrumentation)
	 EXEC('INSERT INTO dbo.Tmp_Instrumentation (ID, Name)
		SELECT ID, Name FROM dbo.Instrumentation WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
DROP TABLE dbo.Instrumentation
GO
EXECUTE sp_rename N'dbo.Tmp_Instrumentation', N'Instrumentation', 'OBJECT' 
GO
ALTER TABLE dbo.Instrumentation ADD CONSTRAINT
	PK_Instrumentation PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Instrumentation_Insert ON dbo.Instrumentation FOR INSERT AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Update ON dbo.Instrumentation FOR UPDATE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Delete ON dbo.Instrumentation FOR DELETE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Key
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(15) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Key SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Key ON
GO
IF EXISTS(SELECT * FROM dbo.[Key])
	 EXEC('INSERT INTO dbo.Tmp_Key (ID, Name)
		SELECT ID, Name FROM dbo.[Key] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Key OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
DROP TABLE dbo.[Key]
GO
EXECUTE sp_rename N'dbo.Tmp_Key', N'Key', 'OBJECT' 
GO
ALTER TABLE dbo.[Key] ADD CONSTRAINT
	PK_Key PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Key_Insert ON dbo.[Key] FOR INSERT AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Key_Update ON dbo.[Key] FOR UPDATE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Key_Delete ON dbo.[Key] FOR DELETE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Movement SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Location
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Location SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Location ON
GO
IF EXISTS(SELECT * FROM dbo.Location)
	 EXEC('INSERT INTO dbo.Tmp_Location (ID, Name)
		SELECT ID, Name FROM dbo.Location WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Location OFF
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.RecordingLocation
	DROP CONSTRAINT FK_RecordingLocation_LocationID
GO
DROP TABLE dbo.Location
GO
EXECUTE sp_rename N'dbo.Tmp_Location', N'Location', 'OBJECT' 
GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	PK_Location PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	UQ_Location_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Location_Insert ON dbo.Location FOR INSERT AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Location_Update ON dbo.Location FOR UPDATE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Location_Delete ON dbo.Location FOR DELETE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingLocation ADD CONSTRAINT
	FK_RecordingLocation_LocationID FOREIGN KEY
	(
	LocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingLocation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:44:51 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Location
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Location SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Location ON
GO
IF EXISTS(SELECT * FROM dbo.Location)
	 EXEC('INSERT INTO dbo.Tmp_Location (ID, Name)
		SELECT ID, Name FROM dbo.Location WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Location OFF
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.RecordingLocation
	DROP CONSTRAINT FK_RecordingLocation_LocationID
GO
DROP TABLE dbo.Location
GO
EXECUTE sp_rename N'dbo.Tmp_Location', N'Location', 'OBJECT' 
GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	PK_Location PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	UQ_Location_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Location_Insert ON dbo.Location FOR INSERT AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Location_Update ON dbo.Location FOR UPDATE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Location_Delete ON dbo.Location FOR DELETE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingLocation ADD CONSTRAINT
	FK_RecordingLocation_LocationID FOREIGN KEY
	(
	LocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingLocation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Instrumentation
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation ON
GO
IF EXISTS(SELECT * FROM dbo.Instrumentation)
	 EXEC('INSERT INTO dbo.Tmp_Instrumentation (ID, Name)
		SELECT ID, Name FROM dbo.Instrumentation WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
DROP TABLE dbo.Instrumentation
GO
EXECUTE sp_rename N'dbo.Tmp_Instrumentation', N'Instrumentation', 'OBJECT' 
GO
ALTER TABLE dbo.Instrumentation ADD CONSTRAINT
	PK_Instrumentation PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Instrumentation_Insert ON dbo.Instrumentation FOR INSERT AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Update ON dbo.Instrumentation FOR UPDATE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Delete ON dbo.Instrumentation FOR DELETE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Key
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(15) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Key SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Key ON
GO
IF EXISTS(SELECT * FROM dbo.[Key])
	 EXEC('INSERT INTO dbo.Tmp_Key (ID, Name)
		SELECT ID, Name FROM dbo.[Key] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Key OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
DROP TABLE dbo.[Key]
GO
EXECUTE sp_rename N'dbo.Tmp_Key', N'Key', 'OBJECT' 
GO
ALTER TABLE dbo.[Key] ADD CONSTRAINT
	PK_Key PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Key_Insert ON dbo.[Key] FOR INSERT AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Key_Update ON dbo.[Key] FOR UPDATE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Key_Delete ON dbo.[Key] FOR DELETE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Movement
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Number smallint NOT NULL,
	CompositionID int NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Movement SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Movement ON
GO
IF EXISTS(SELECT * FROM dbo.Movement)
	 EXEC('INSERT INTO dbo.Tmp_Movement (ID, Name, Number, CompositionID, IsPopular)
		SELECT ID, Name, Number, CompositionID, IsPopular FROM dbo.Movement WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Movement OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_MovementID
GO
DROP TABLE dbo.Movement
GO
EXECUTE sp_rename N'dbo.Tmp_Movement', N'Movement', 'OBJECT' 
GO
ALTER TABLE dbo.Movement ADD CONSTRAINT
	PK_Movement PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Movement_Insert ON dbo.Movement FOR INSERT AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Update ON dbo.Movement FOR UPDATE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Delete ON dbo.Movement FOR DELETE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_MovementID FOREIGN KEY
	(
	MovementID
	) REFERENCES dbo.Movement
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:50:11 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Instrumentation
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation ON
GO
IF EXISTS(SELECT * FROM dbo.Instrumentation)
	 EXEC('INSERT INTO dbo.Tmp_Instrumentation (ID, Name)
		SELECT ID, Name FROM dbo.Instrumentation WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
DROP TABLE dbo.Instrumentation
GO
EXECUTE sp_rename N'dbo.Tmp_Instrumentation', N'Instrumentation', 'OBJECT' 
GO
ALTER TABLE dbo.Instrumentation ADD CONSTRAINT
	PK_Instrumentation PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Instrumentation_Insert ON dbo.Instrumentation FOR INSERT AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Update ON dbo.Instrumentation FOR UPDATE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Delete ON dbo.Instrumentation FOR DELETE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Key
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(15) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Key SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Key ON
GO
IF EXISTS(SELECT * FROM dbo.[Key])
	 EXEC('INSERT INTO dbo.Tmp_Key (ID, Name)
		SELECT ID, Name FROM dbo.[Key] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Key OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
DROP TABLE dbo.[Key]
GO
EXECUTE sp_rename N'dbo.Tmp_Key', N'Key', 'OBJECT' 
GO
ALTER TABLE dbo.[Key] ADD CONSTRAINT
	PK_Key PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Key_Insert ON dbo.[Key] FOR INSERT AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Key_Update ON dbo.[Key] FOR UPDATE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Key_Delete ON dbo.[Key] FOR DELETE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Movement
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Number smallint NOT NULL,
	CompositionID int NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Movement SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Movement ON
GO
IF EXISTS(SELECT * FROM dbo.Movement)
	 EXEC('INSERT INTO dbo.Tmp_Movement (ID, Name, Number, CompositionID, IsPopular)
		SELECT ID, Name, Number, CompositionID, IsPopular FROM dbo.Movement WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Movement OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_MovementID
GO
DROP TABLE dbo.Movement
GO
EXECUTE sp_rename N'dbo.Tmp_Movement', N'Movement', 'OBJECT' 
GO
ALTER TABLE dbo.Movement ADD CONSTRAINT
	PK_Movement PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Movement_Insert ON dbo.Movement FOR INSERT AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Update ON dbo.Movement FOR UPDATE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Delete ON dbo.Movement FOR DELETE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_MovementID FOREIGN KEY
	(
	MovementID
	) REFERENCES dbo.Movement
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Location
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Location SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Location ON
GO
IF EXISTS(SELECT * FROM dbo.Location)
	 EXEC('INSERT INTO dbo.Tmp_Location (ID, Name)
		SELECT ID, Name FROM dbo.Location WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Location OFF
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.RecordingLocation
	DROP CONSTRAINT FK_RecordingLocation_LocationID
GO
DROP TABLE dbo.Location
GO
EXECUTE sp_rename N'dbo.Tmp_Location', N'Location', 'OBJECT' 
GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	PK_Location PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	UQ_Location_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Location_Insert ON dbo.Location FOR INSERT AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Location_Update ON dbo.Location FOR UPDATE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Location_Delete ON dbo.Location FOR DELETE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingLocation ADD CONSTRAINT
	FK_RecordingLocation_LocationID FOREIGN KEY
	(
	LocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingLocation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Nationality
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Nationality SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Nationality ON
GO
IF EXISTS(SELECT * FROM dbo.Nationality)
	 EXEC('INSERT INTO dbo.Tmp_Nationality (ID, Name)
		SELECT ID, Name FROM dbo.Nationality WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Nationality OFF
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_NationalityID
GO
DROP TABLE dbo.Nationality
GO
EXECUTE sp_rename N'dbo.Tmp_Nationality', N'Nationality', 'OBJECT' 
GO
ALTER TABLE dbo.Nationality ADD CONSTRAINT
	PK_Nationality PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Nationality ADD CONSTRAINT
	UQ_Nationality_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Nationality_Insert ON dbo.Nationality FOR INSERT AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Nationality_Update ON dbo.Nationality FOR UPDATE AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Nationality_Delete ON dbo.Nationality FOR DELETE AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_NationalityID FOREIGN KEY
	(
	NationalityID
	) REFERENCES dbo.Nationality
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:50:52 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Performer
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Performer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Performer ON
GO
IF EXISTS(SELECT * FROM dbo.Performer)
	 EXEC('INSERT INTO dbo.Tmp_Performer (ID, Name)
		SELECT ID, Name FROM dbo.Performer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Performer OFF
GO
ALTER TABLE dbo.RecordingPerformer
	DROP CONSTRAINT FK_RecordingPerformer_PerformerID
GO
DROP TABLE dbo.Performer
GO
EXECUTE sp_rename N'dbo.Tmp_Performer', N'Performer', 'OBJECT' 
GO
ALTER TABLE dbo.Performer ADD CONSTRAINT
	PK_Performer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Performer_Insert ON dbo.Performer FOR INSERT AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Performer_Update ON dbo.Performer FOR UPDATE AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Performer_Delete ON dbo.Performer FOR DELETE AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingPerformer ADD CONSTRAINT
	FK_RecordingPerformer_PerformerID FOREIGN KEY
	(
	PerformerID
	) REFERENCES dbo.Performer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingPerformer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:51:20 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Nationality
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Nationality SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Nationality ON
GO
IF EXISTS(SELECT * FROM dbo.Nationality)
	 EXEC('INSERT INTO dbo.Tmp_Nationality (ID, Name)
		SELECT ID, Name FROM dbo.Nationality WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Nationality OFF
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_NationalityID
GO
DROP TABLE dbo.Nationality
GO
EXECUTE sp_rename N'dbo.Tmp_Nationality', N'Nationality', 'OBJECT' 
GO
ALTER TABLE dbo.Nationality ADD CONSTRAINT
	PK_Nationality PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Nationality ADD CONSTRAINT
	UQ_Nationality_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Nationality_Insert ON dbo.Nationality FOR INSERT AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Nationality_Update ON dbo.Nationality FOR UPDATE AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Nationality_Delete ON dbo.Nationality FOR DELETE AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Instrumentation
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation ON
GO
IF EXISTS(SELECT * FROM dbo.Instrumentation)
	 EXEC('INSERT INTO dbo.Tmp_Instrumentation (ID, Name)
		SELECT ID, Name FROM dbo.Instrumentation WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
DROP TABLE dbo.Instrumentation
GO
EXECUTE sp_rename N'dbo.Tmp_Instrumentation', N'Instrumentation', 'OBJECT' 
GO
ALTER TABLE dbo.Instrumentation ADD CONSTRAINT
	PK_Instrumentation PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Instrumentation_Insert ON dbo.Instrumentation FOR INSERT AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Update ON dbo.Instrumentation FOR UPDATE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Delete ON dbo.Instrumentation FOR DELETE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Key
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(15) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Key SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Key ON
GO
IF EXISTS(SELECT * FROM dbo.[Key])
	 EXEC('INSERT INTO dbo.Tmp_Key (ID, Name)
		SELECT ID, Name FROM dbo.[Key] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Key OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
DROP TABLE dbo.[Key]
GO
EXECUTE sp_rename N'dbo.Tmp_Key', N'Key', 'OBJECT' 
GO
ALTER TABLE dbo.[Key] ADD CONSTRAINT
	PK_Key PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Key_Insert ON dbo.[Key] FOR INSERT AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Key_Update ON dbo.[Key] FOR UPDATE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Key_Delete ON dbo.[Key] FOR DELETE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Performer
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Performer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Performer ON
GO
IF EXISTS(SELECT * FROM dbo.Performer)
	 EXEC('INSERT INTO dbo.Tmp_Performer (ID, Name)
		SELECT ID, Name FROM dbo.Performer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Performer OFF
GO
ALTER TABLE dbo.RecordingPerformer
	DROP CONSTRAINT FK_RecordingPerformer_PerformerID
GO
DROP TABLE dbo.Performer
GO
EXECUTE sp_rename N'dbo.Tmp_Performer', N'Performer', 'OBJECT' 
GO
ALTER TABLE dbo.Performer ADD CONSTRAINT
	PK_Performer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Performer_Insert ON dbo.Performer FOR INSERT AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Performer_Update ON dbo.Performer FOR UPDATE AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Performer_Delete ON dbo.Performer FOR DELETE AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Location
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Location SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Location ON
GO
IF EXISTS(SELECT * FROM dbo.Location)
	 EXEC('INSERT INTO dbo.Tmp_Location (ID, Name)
		SELECT ID, Name FROM dbo.Location WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Location OFF
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.RecordingLocation
	DROP CONSTRAINT FK_RecordingLocation_LocationID
GO
DROP TABLE dbo.Location
GO
EXECUTE sp_rename N'dbo.Tmp_Location', N'Location', 'OBJECT' 
GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	PK_Location PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	UQ_Location_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Location_Insert ON dbo.Location FOR INSERT AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Location_Update ON dbo.Location FOR UPDATE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Location_Delete ON dbo.Location FOR DELETE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_NationalityID FOREIGN KEY
	(
	NationalityID
	) REFERENCES dbo.Nationality
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Sample SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Movement
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Number smallint NOT NULL,
	CompositionID int NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Movement SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Movement ON
GO
IF EXISTS(SELECT * FROM dbo.Movement)
	 EXEC('INSERT INTO dbo.Tmp_Movement (ID, Name, Number, CompositionID, IsPopular)
		SELECT ID, Name, Number, CompositionID, IsPopular FROM dbo.Movement WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Movement OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_MovementID
GO
DROP TABLE dbo.Movement
GO
EXECUTE sp_rename N'dbo.Tmp_Movement', N'Movement', 'OBJECT' 
GO
ALTER TABLE dbo.Movement ADD CONSTRAINT
	PK_Movement PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Movement_Insert ON dbo.Movement FOR INSERT AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Update ON dbo.Movement FOR UPDATE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Delete ON dbo.Movement FOR DELETE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Recording
	(
	ID int NOT NULL IDENTITY (1, 1),
	Dates nvarchar(255) NOT NULL,
	AlbumID smallint NULL,
	TrackNumber smallint NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL,
	MovementID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Recording SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Recording ON
GO
IF EXISTS(SELECT * FROM dbo.Recording)
	 EXEC('INSERT INTO dbo.Tmp_Recording (ID, Dates, AlbumID, TrackNumber, CompositionCollectionID, CompositionID, MovementID)
		SELECT ID, Dates, AlbumID, TrackNumber, CompositionCollectionID, CompositionID, MovementID FROM dbo.Recording WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Recording OFF
GO
ALTER TABLE dbo.RecordingLocation
	DROP CONSTRAINT FK_RecordingLocation_RecordingID
GO
ALTER TABLE dbo.RecordingPerformer
	DROP CONSTRAINT FK_RecordingPerformer_RecordingID
GO
DROP TABLE dbo.Recording
GO
EXECUTE sp_rename N'dbo.Tmp_Recording', N'Recording', 'OBJECT' 
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	PK_Recording PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_MovementID FOREIGN KEY
	(
	MovementID
	) REFERENCES dbo.Movement
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Recording_Insert ON dbo.Recording FOR INSERT AS INSERT INTO RecordingHistory(ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,Operation) SELECT ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Recording_Update ON dbo.Recording FOR UPDATE AS INSERT INTO RecordingHistory(ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,Operation) SELECT ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Recording_Delete ON dbo.Recording FOR DELETE AS INSERT INTO RecordingHistory(ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,Operation) SELECT ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingPerformer ADD CONSTRAINT
	FK_RecordingPerformer_PerformerID FOREIGN KEY
	(
	PerformerID
	) REFERENCES dbo.Performer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingPerformer ADD CONSTRAINT
	FK_RecordingPerformer_RecordingID FOREIGN KEY
	(
	RecordingID
	) REFERENCES dbo.Recording
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingPerformer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingLocation ADD CONSTRAINT
	FK_RecordingLocation_LocationID FOREIGN KEY
	(
	LocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingLocation ADD CONSTRAINT
	FK_RecordingLocation_RecordingID FOREIGN KEY
	(
	RecordingID
	) REFERENCES dbo.Recording
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingLocation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
/*
   Monday, August 3, 201511:51:53 PM
   User: 
   Server: .\SQLEXPRESS
   Database: Music
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Instrumentation
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Instrumentation SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation ON
GO
IF EXISTS(SELECT * FROM dbo.Instrumentation)
	 EXEC('INSERT INTO dbo.Tmp_Instrumentation (ID, Name)
		SELECT ID, Name FROM dbo.Instrumentation WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Instrumentation OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_InstrumentationID
GO
DROP TABLE dbo.Instrumentation
GO
EXECUTE sp_rename N'dbo.Tmp_Instrumentation', N'Instrumentation', 'OBJECT' 
GO
ALTER TABLE dbo.Instrumentation ADD CONSTRAINT
	PK_Instrumentation PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Instrumentation_Insert ON dbo.Instrumentation FOR INSERT AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Update ON dbo.Instrumentation FOR UPDATE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Instrumentation_Delete ON dbo.Instrumentation FOR DELETE AS INSERT INTO InstrumentationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Key
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(15) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Key SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Key ON
GO
IF EXISTS(SELECT * FROM dbo.[Key])
	 EXEC('INSERT INTO dbo.Tmp_Key (ID, Name)
		SELECT ID, Name FROM dbo.[Key] WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Key OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_KeyID
GO
DROP TABLE dbo.[Key]
GO
EXECUTE sp_rename N'dbo.Tmp_Key', N'Key', 'OBJECT' 
GO
ALTER TABLE dbo.[Key] ADD CONSTRAINT
	PK_Key PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Key_Insert ON dbo.[Key] FOR INSERT AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Key_Update ON dbo.[Key] FOR UPDATE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Key_Delete ON dbo.[Key] FOR DELETE AS INSERT INTO KeyHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionType
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionType SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionType)
	 EXEC('INSERT INTO dbo.Tmp_CompositionType (ID, Name)
		SELECT ID, Name FROM dbo.CompositionType WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionType OFF
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
DROP TABLE dbo.CompositionType
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionType', N'CompositionType', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionType ADD CONSTRAINT
	PK_CompositionType PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Performer
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Performer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Performer ON
GO
IF EXISTS(SELECT * FROM dbo.Performer)
	 EXEC('INSERT INTO dbo.Tmp_Performer (ID, Name)
		SELECT ID, Name FROM dbo.Performer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Performer OFF
GO
ALTER TABLE dbo.RecordingPerformer
	DROP CONSTRAINT FK_RecordingPerformer_PerformerID
GO
DROP TABLE dbo.Performer
GO
EXECUTE sp_rename N'dbo.Tmp_Performer', N'Performer', 'OBJECT' 
GO
ALTER TABLE dbo.Performer ADD CONSTRAINT
	PK_Performer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Performer_Insert ON dbo.Performer FOR INSERT AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Performer_Update ON dbo.Performer FOR UPDATE AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Performer_Delete ON dbo.Performer FOR DELETE AS INSERT INTO PerformerHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Album
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Album SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Album ON
GO
IF EXISTS(SELECT * FROM dbo.Album)
	 EXEC('INSERT INTO dbo.Tmp_Album (ID, Name)
		SELECT ID, Name FROM dbo.Album WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Album OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_AlbumID
GO
DROP TABLE dbo.Album
GO
EXECUTE sp_rename N'dbo.Tmp_Album', N'Album', 'OBJECT' 
GO
ALTER TABLE dbo.Album ADD CONSTRAINT
	PK_Album PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Album_Insert ON dbo.Album FOR INSERT AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Album_Update ON dbo.Album FOR UPDATE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Album_Delete ON dbo.Album FOR DELETE AS INSERT INTO AlbumHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCollection
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCollection SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCollection)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCollection (ID, Name, IsPopular)
		SELECT ID, Name, IsPopular FROM dbo.CompositionCollection WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCollection OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCollectionID
GO
ALTER TABLE dbo.Composition
	DROP CONSTRAINT FK_Composition_CompositionCollectionID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_CompositionCollectionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionCollectionID
GO
DROP TABLE dbo.CompositionCollection
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCollection', N'CompositionCollection', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCollection ADD CONSTRAINT
	PK_CompositionCollection PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_CompositionCollection_Insert ON dbo.CompositionCollection FOR INSERT AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Update ON dbo.CompositionCollection FOR UPDATE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCollection_Delete ON dbo.CompositionCollection FOR DELETE AS INSERT INTO CompositionCollectionHistory(ID,Name,IsPopular,Operation) SELECT ID,Name,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composition
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Dates nvarchar(50) NULL,
	Nickname nvarchar(50) NULL,
	IsPopular bit NOT NULL,
	CompositionCollectionID smallint NULL,
	Comment nvarchar(255) NULL,
	Premiere nvarchar(255) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composition SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composition ON
GO
IF EXISTS(SELECT * FROM dbo.Composition)
	 EXEC('INSERT INTO dbo.Tmp_Composition (ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID)
		SELECT ID, Name, Dates, Nickname, IsPopular, CompositionCollectionID, Comment, Premiere, Dedication, Occasion, CompositionTypeID, KeyID, InstrumentationID FROM dbo.Composition WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composition OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
DROP TABLE dbo.Composition
GO
EXECUTE sp_rename N'dbo.Tmp_Composition', N'Composition', 'OBJECT' 
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	PK_Composition PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_CompositionTypeID FOREIGN KEY
	(
	CompositionTypeID
	) REFERENCES dbo.CompositionType
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_KeyID FOREIGN KEY
	(
	KeyID
	) REFERENCES dbo.[Key]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composition ADD CONSTRAINT
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionLink
	(
	ID int NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionLink)
	 EXEC('INSERT INTO dbo.Tmp_CompositionLink (ID, URL, CompositionID)
		SELECT ID, URL, CompositionID FROM dbo.CompositionLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionLink OFF
GO
DROP TABLE dbo.CompositionLink
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionLink', N'CompositionLink', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	PK_CompositionLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionLink ADD CONSTRAINT
	FK_CompositionLink_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionLink_Insert ON dbo.CompositionLink FOR INSERT AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Update ON dbo.CompositionLink FOR UPDATE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionLink_Delete ON dbo.CompositionLink FOR DELETE AS INSERT INTO CompositionLinkHistory(ID,URL,CompositionID,Operation) SELECT ID,URL,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Movement
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL,
	Number smallint NOT NULL,
	CompositionID int NOT NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Movement SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Movement ON
GO
IF EXISTS(SELECT * FROM dbo.Movement)
	 EXEC('INSERT INTO dbo.Tmp_Movement (ID, Name, Number, CompositionID, IsPopular)
		SELECT ID, Name, Number, CompositionID, IsPopular FROM dbo.Movement WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Movement OFF
GO
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_MovementID
GO
DROP TABLE dbo.Movement
GO
EXECUTE sp_rename N'dbo.Tmp_Movement', N'Movement', 'OBJECT' 
GO
ALTER TABLE dbo.Movement ADD CONSTRAINT
	PK_Movement PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Movement WITH NOCHECK ADD CONSTRAINT
	FK_Movement_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Movement_Insert ON dbo.Movement FOR INSERT AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Update ON dbo.Movement FOR UPDATE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Movement_Delete ON dbo.Movement FOR DELETE AS INSERT INTO MovementHistory(ID,Name,Number,CompositionID,IsPopular,Operation) SELECT ID,Name,Number,CompositionID,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Recording
	(
	ID int NOT NULL IDENTITY (1, 1),
	Dates nvarchar(255) NOT NULL,
	AlbumID smallint NULL,
	TrackNumber smallint NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL,
	MovementID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Recording SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Recording ON
GO
IF EXISTS(SELECT * FROM dbo.Recording)
	 EXEC('INSERT INTO dbo.Tmp_Recording (ID, Dates, AlbumID, TrackNumber, CompositionCollectionID, CompositionID, MovementID)
		SELECT ID, Dates, AlbumID, TrackNumber, CompositionCollectionID, CompositionID, MovementID FROM dbo.Recording WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Recording OFF
GO
ALTER TABLE dbo.RecordingLocation
	DROP CONSTRAINT FK_RecordingLocation_RecordingID
GO
ALTER TABLE dbo.RecordingPerformer
	DROP CONSTRAINT FK_RecordingPerformer_RecordingID
GO
DROP TABLE dbo.Recording
GO
EXECUTE sp_rename N'dbo.Tmp_Recording', N'Recording', 'OBJECT' 
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	PK_Recording PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Recording WITH NOCHECK ADD CONSTRAINT
	FK_Recording_AlbumID FOREIGN KEY
	(
	AlbumID
	) REFERENCES dbo.Album
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Recording ADD CONSTRAINT
	FK_Recording_MovementID FOREIGN KEY
	(
	MovementID
	) REFERENCES dbo.Movement
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Recording_Insert ON dbo.Recording FOR INSERT AS INSERT INTO RecordingHistory(ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,Operation) SELECT ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Recording_Update ON dbo.Recording FOR UPDATE AS INSERT INTO RecordingHistory(ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,Operation) SELECT ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Recording_Delete ON dbo.Recording FOR DELETE AS INSERT INTO RecordingHistory(ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,Operation) SELECT ID,Dates,AlbumID,TrackNumber,CompositionCollectionID,CompositionID,MovementID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingPerformer ADD CONSTRAINT
	FK_RecordingPerformer_PerformerID FOREIGN KEY
	(
	PerformerID
	) REFERENCES dbo.Performer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingPerformer ADD CONSTRAINT
	FK_RecordingPerformer_RecordingID FOREIGN KEY
	(
	RecordingID
	) REFERENCES dbo.Recording
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingPerformer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Nationality
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Nationality SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Nationality ON
GO
IF EXISTS(SELECT * FROM dbo.Nationality)
	 EXEC('INSERT INTO dbo.Tmp_Nationality (ID, Name)
		SELECT ID, Name FROM dbo.Nationality WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Nationality OFF
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_NationalityID
GO
DROP TABLE dbo.Nationality
GO
EXECUTE sp_rename N'dbo.Tmp_Nationality', N'Nationality', 'OBJECT' 
GO
ALTER TABLE dbo.Nationality ADD CONSTRAINT
	PK_Nationality PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Nationality ADD CONSTRAINT
	UQ_Nationality_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Nationality_Insert ON dbo.Nationality FOR INSERT AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Nationality_Update ON dbo.Nationality FOR UPDATE AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Nationality_Delete ON dbo.Nationality FOR DELETE AS INSERT INTO NationalityHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Era
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(12) NOT NULL,
	Dates nvarchar(9) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Era SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Era ON
GO
IF EXISTS(SELECT * FROM dbo.Era)
	 EXEC('INSERT INTO dbo.Tmp_Era (ID, Name, Dates)
		SELECT ID, Name, Dates FROM dbo.Era WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Era OFF
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_EraID
GO
DROP TABLE dbo.Era
GO
EXECUTE sp_rename N'dbo.Tmp_Era', N'Era', 'OBJECT' 
GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	PK_Era PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Era ADD CONSTRAINT
	UQ_Era_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Era_Insert ON dbo.Era FOR INSERT AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Era_Update ON dbo.Era FOR UPDATE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Era_Delete ON dbo.Era FOR DELETE AS INSERT INTO EraHistory(ID,Name,Dates,Operation) SELECT ID,Name,Dates,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Location
	(
	ID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Location SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Location ON
GO
IF EXISTS(SELECT * FROM dbo.Location)
	 EXEC('INSERT INTO dbo.Tmp_Location (ID, Name)
		SELECT ID, Name FROM dbo.Location WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Location OFF
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_BirthLocationID
GO
ALTER TABLE dbo.Composer
	DROP CONSTRAINT FK_Composer_DeathLocationID
GO
ALTER TABLE dbo.RecordingLocation
	DROP CONSTRAINT FK_RecordingLocation_LocationID
GO
DROP TABLE dbo.Location
GO
EXECUTE sp_rename N'dbo.Tmp_Location', N'Location', 'OBJECT' 
GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	PK_Location PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Location ADD CONSTRAINT
	UQ_Location_Name UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TRIGGER TR_Location_Insert ON dbo.Location FOR INSERT AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Location_Update ON dbo.Location FOR UPDATE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Location_Delete ON dbo.Location FOR DELETE AS INSERT INTO LocationHistory(ID,Name,Operation) SELECT ID,Name,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RecordingLocation ADD CONSTRAINT
	FK_RecordingLocation_LocationID FOREIGN KEY
	(
	LocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingLocation ADD CONSTRAINT
	FK_RecordingLocation_RecordingID FOREIGN KEY
	(
	RecordingID
	) REFERENCES dbo.Recording
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RecordingLocation SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Composer
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Dates nvarchar(50) NOT NULL,
	BirthLocationID int NULL,
	DeathLocationID int NULL,
	Biography nvarchar(MAX) NULL,
	IsPopular bit NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Composer SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Composer ON
GO
IF EXISTS(SELECT * FROM dbo.Composer)
	 EXEC('INSERT INTO dbo.Tmp_Composer (ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular)
		SELECT ID, Name, Dates, BirthLocationID, DeathLocationID, Biography, IsPopular FROM dbo.Composer WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Composer OFF
GO
ALTER TABLE dbo.Sample
	DROP CONSTRAINT FK_Sample_ComposerID
GO
ALTER TABLE dbo.ComposerEra
	DROP CONSTRAINT FK_ComposerEra_ComposerID
GO
ALTER TABLE dbo.ComposerImage
	DROP CONSTRAINT FK_ComposerImage_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_ComposerID
GO
ALTER TABLE dbo.ComposerInfluence
	DROP CONSTRAINT FK_ComposerInfluence_InfluenceID
GO
ALTER TABLE dbo.ComposerLink
	DROP CONSTRAINT FK_ComposerLink_ComposerID
GO
ALTER TABLE dbo.ComposerNationality
	DROP CONSTRAINT FK_ComposerNationality_ComposerID
GO
ALTER TABLE dbo.CompositionCatalog
	DROP CONSTRAINT FK_CompositionCatalog_ComposerID
GO
ALTER TABLE dbo.CompositionCollectionComposer
	DROP CONSTRAINT FK_CompositionCollectionComposer_ComposerID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_ComposerID
GO
DROP TABLE dbo.Composer
GO
EXECUTE sp_rename N'dbo.Tmp_Composer', N'Composer', 'OBJECT' 
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	PK_Composer PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_BirthLocationID FOREIGN KEY
	(
	BirthLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Composer ADD CONSTRAINT
	FK_Composer_DeathLocationID FOREIGN KEY
	(
	DeathLocationID
	) REFERENCES dbo.Location
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Composer_Insert ON dbo.Composer FOR INSERT AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Update ON dbo.Composer FOR UPDATE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composer_Delete ON dbo.Composer FOR DELETE AS INSERT INTO ComposerHistory(ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,Operation) SELECT ID,Name,Dates,BirthLocationID,DeathLocationID,Biography,IsPopular,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer ADD CONSTRAINT
	FK_CompositionComposer_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.CompositionCollectionComposer ADD CONSTRAINT
	FK_CompositionCollectionComposer_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer WITH NOCHECK ADD CONSTRAINT
	FK_CompositionCollectionComposer_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CompositionCollectionComposer SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CompositionCatalog
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Prefix nvarchar(10) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CompositionCatalog SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog ON
GO
IF EXISTS(SELECT * FROM dbo.CompositionCatalog)
	 EXEC('INSERT INTO dbo.Tmp_CompositionCatalog (ID, Prefix, ComposerID)
		SELECT ID, Prefix, ComposerID FROM dbo.CompositionCatalog WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CompositionCatalog OFF
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionCatalogID
GO
DROP TABLE dbo.CompositionCatalog
GO
EXECUTE sp_rename N'dbo.Tmp_CompositionCatalog', N'CompositionCatalog', 'OBJECT' 
GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	PK_CompositionCatalog PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CompositionCatalog ADD CONSTRAINT
	FK_CompositionCatalog_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CompositionCatalog_Insert ON dbo.CompositionCatalog FOR INSERT AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Update ON dbo.CompositionCatalog FOR UPDATE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CompositionCatalog_Delete ON dbo.CompositionCatalog FOR DELETE AS INSERT INTO CompositionCatalogHistory(ID,Prefix,ComposerID,Operation) SELECT ID,Prefix,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_CatalogNumber
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Number nvarchar(15) NOT NULL,
	CompositionCatalogID smallint NOT NULL,
	CompositionCollectionID smallint NULL,
	CompositionID int NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_CatalogNumber SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber ON
GO
IF EXISTS(SELECT * FROM dbo.CatalogNumber)
	 EXEC('INSERT INTO dbo.Tmp_CatalogNumber (ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID)
		SELECT ID, Number, CompositionCatalogID, CompositionCollectionID, CompositionID FROM dbo.CatalogNumber WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_CatalogNumber OFF
GO
DROP TABLE dbo.CatalogNumber
GO
EXECUTE sp_rename N'dbo.Tmp_CatalogNumber', N'CatalogNumber', 'OBJECT' 
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	PK_CatalogNumber PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionCatalogID FOREIGN KEY
	(
	CompositionCatalogID
	) REFERENCES dbo.CompositionCatalog
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber ADD CONSTRAINT
	FK_CatalogNumber_CompositionCollectionID FOREIGN KEY
	(
	CompositionCollectionID
	) REFERENCES dbo.CompositionCollection
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.CatalogNumber WITH NOCHECK ADD CONSTRAINT
	FK_CatalogNumber_CompositionID FOREIGN KEY
	(
	CompositionID
	) REFERENCES dbo.Composition
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_CatalogNumber_Insert ON dbo.CatalogNumber FOR INSERT AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Update ON dbo.CatalogNumber FOR UPDATE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_CatalogNumber_Delete ON dbo.CatalogNumber FOR DELETE AS INSERT INTO CatalogNumberHistory(ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,Operation) SELECT ID,Number,CompositionCatalogID,CompositionCollectionID,CompositionID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality ADD CONSTRAINT
	FK_ComposerNationality_NationalityID FOREIGN KEY
	(
	NationalityID
	) REFERENCES dbo.Nationality
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerNationality SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerLink
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	URL nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerLink SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerLink)
	 EXEC('INSERT INTO dbo.Tmp_ComposerLink (ID, URL, ComposerID)
		SELECT ID, URL, ComposerID FROM dbo.ComposerLink WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerLink OFF
GO
DROP TABLE dbo.ComposerLink
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerLink', N'ComposerLink', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	PK_ComposerLink PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerLink ADD CONSTRAINT
	FK_ComposerLink_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerLink_Insert ON dbo.ComposerLink FOR INSERT AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Update ON dbo.ComposerLink FOR UPDATE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerLink_Delete ON dbo.ComposerLink FOR DELETE AS INSERT INTO ComposerLinkHistory(ID,URL,ComposerID,Operation) SELECT ID,URL,ComposerID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence ADD CONSTRAINT
	FK_ComposerInfluence_InfluenceID FOREIGN KEY
	(
	InfluenceID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerInfluence SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_ComposerImage
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	ComposerID smallint NOT NULL,
	Image varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ComposerImage SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage ON
GO
IF EXISTS(SELECT * FROM dbo.ComposerImage)
	 EXEC('INSERT INTO dbo.Tmp_ComposerImage (ID, ComposerID, Image)
		SELECT ID, ComposerID, Image FROM dbo.ComposerImage WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_ComposerImage OFF
GO
DROP TABLE dbo.ComposerImage
GO
EXECUTE sp_rename N'dbo.Tmp_ComposerImage', N'ComposerImage', 'OBJECT' 
GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	PK_ComposerImage PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ComposerImage ADD CONSTRAINT
	FK_ComposerImage_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_ComposerImage_Insert ON dbo.ComposerImage FOR INSERT AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Update ON dbo.ComposerImage FOR UPDATE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Update' FROM Inserted
GO
CREATE TRIGGER TR_ComposerImage_Delete ON dbo.ComposerImage FOR DELETE AS INSERT INTO ComposerImageHistory(ID,ComposerID,Image,Operation) SELECT ID,ComposerID,Image,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra ADD CONSTRAINT
	FK_ComposerEra_EraID FOREIGN KEY
	(
	EraID
	) REFERENCES dbo.Era
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ComposerEra SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.Tmp_Sample
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Audio varbinary(MAX) NOT NULL,
	Title nvarchar(255) NOT NULL,
	Artists nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Sample SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Sample ON
GO
IF EXISTS(SELECT * FROM dbo.Sample)
	 EXEC('INSERT INTO dbo.Tmp_Sample (ID, Audio, Title, Artists, ComposerID)
		SELECT ID, Audio, Title, Artists, ComposerID FROM dbo.Sample WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Sample OFF
GO
DROP TABLE dbo.Sample
GO
EXECUTE sp_rename N'dbo.Tmp_Sample', N'Sample', 'OBJECT' 
GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	PK__Sample__3214EC27E638CCBF PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Sample ADD CONSTRAINT
	FK_Sample_ComposerID FOREIGN KEY
	(
	ComposerID
	) REFERENCES dbo.Composer
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
CREATE TRIGGER TR_Sample_Insert ON dbo.Sample FOR INSERT AS INSERT INTO SampleHistory(ID,Audio,Title,Artists,ComposerID,Operation) SELECT ID,Audio,Title,Artists,ComposerID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Sample_Update ON dbo.Sample FOR UPDATE AS INSERT INTO SampleHistory(ID,Audio,Title,Artists,ComposerID,Operation) SELECT ID,Audio,Title,Artists,ComposerID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Sample_Delete ON dbo.Sample FOR DELETE AS INSERT INTO SampleHistory(ID,Audio,Title,Artists,ComposerID,Operation) SELECT ID,Audio,Title,Artists,ComposerID,'Delete' FROM Deleted
GO
COMMIT
