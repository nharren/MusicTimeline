/*
   Monday, August 10, 201511:16:44 PM
   User: 
   Server: .\SQLEXPRESS
   Database: ClassicalMusicDb
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
CREATE TABLE dbo.Tmp_Genre
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name nvarchar(255) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Genre SET (LOCK_ESCALATION = TABLE)
GO
SET IDENTITY_INSERT dbo.Tmp_Genre ON
GO
IF EXISTS(SELECT * FROM dbo.Genre)
	 EXEC('INSERT INTO dbo.Tmp_Genre (Id, Name)
		SELECT Id, CONVERT(nvarchar(255), Name) FROM dbo.Genre WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_Genre OFF
GO
DROP TABLE dbo.Genre
GO
EXECUTE sp_rename N'dbo.Tmp_Genre', N'Genre', 'OBJECT' 
GO
ALTER TABLE dbo.Genre ADD CONSTRAINT
	IX_Genre UNIQUE NONCLUSTERED 
	(
	Name
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
COMMIT
