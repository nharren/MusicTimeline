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
	DROP CONSTRAINT FK_Composition_KeyID
GO
ALTER TABLE dbo.[Key] SET (LOCK_ESCALATION = TABLE)
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
	DROP CONSTRAINT FK_Composition_CompositionTypeID
GO
ALTER TABLE dbo.CompositionType SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
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
	Comment nvarchar(MAX) NULL,
	Premiere nvarchar(300) NULL,
	Dedication nvarchar(255) NULL,
	Occasion nvarchar(255) NULL,
	CompositionTypeID smallint NULL,
	KeyID smallint NULL,
	InstrumentationID smallint NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
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
ALTER TABLE dbo.Recording
	DROP CONSTRAINT FK_Recording_CompositionID
GO
ALTER TABLE dbo.Movement
	DROP CONSTRAINT FK_Movement_CompositionID
GO
ALTER TABLE dbo.CompositionLink
	DROP CONSTRAINT FK_CompositionLink_CompositionID
GO
ALTER TABLE dbo.CatalogNumber
	DROP CONSTRAINT FK_CatalogNumber_CompositionID
GO
ALTER TABLE dbo.CompositionComposer
	DROP CONSTRAINT FK_CompositionComposer_CompositionID
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
	FK_Composition_InstrumentationID FOREIGN KEY
	(
	InstrumentationID
	) REFERENCES dbo.Instrumentation
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
CREATE TRIGGER TR_Composition_Insert ON dbo.Composition FOR INSERT AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Insert' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Update ON dbo.Composition FOR UPDATE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Update' FROM Inserted
GO
CREATE TRIGGER TR_Composition_Delete ON dbo.Composition FOR DELETE AS INSERT INTO CompositionHistory(ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,Operation) SELECT ID,Name,Dates,Nickname,IsPopular,CompositionCollectionID,Comment,Premiere,Dedication,Occasion,CompositionTypeID,KeyID,InstrumentationID,'Delete' FROM Deleted
GO
COMMIT
BEGIN TRANSACTION
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
ALTER TABLE dbo.CatalogNumber SET (LOCK_ESCALATION = TABLE)
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

EXECUTE GenerateHistoryTable 'Composition', 'dbo', 'History', 1
GO
