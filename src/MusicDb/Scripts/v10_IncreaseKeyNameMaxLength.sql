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
CREATE TABLE dbo.Tmp_Key
	(
	ID smallint NOT NULL IDENTITY (1, 1),
	Name nvarchar(128) NOT NULL
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
ALTER TABLE dbo.Composition SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

EXECUTE GenerateHistoryTable 'Key', 'dbo', 'History', 1
GO