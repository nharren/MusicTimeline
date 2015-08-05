-- Procedure based on http://www.codeproject.com/Articles/21068/Audit-Trail-Generator-for-Microsoft-SQL

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GenerateHistoryTable]
	@TableName varchar(128),
	@Owner varchar(128) = 'dbo',
	@AuditNameExtention varchar(128) = 'History',
	@DropAuditTable bit = 0
AS
BEGIN

	-- Check if table exists
	IF not exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		PRINT 'ERROR: Table does not exist'
		RETURN
	END

	-- Check @AuditNameExtention
	IF @AuditNameExtention is null
	BEGIN
		PRINT 'ERROR: @AuditNameExtention cannot be null'
		RETURN
	END

	-- Drop audit table if it exists and drop should be forced
	IF (exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + @AuditNameExtention + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1) and @DropAuditTable = 1)
	BEGIN
		PRINT 'Dropping audit table [' + @Owner + '].[' + @TableName + @AuditNameExtention + ']'
		EXEC ('drop table ' + @TableName + @AuditNameExtention)
	END

	-- Declare cursor to loop over columns
	DECLARE TableColumns CURSOR Read_Only
	FOR SELECT b.name, c.name as TypeName, b.length, b.isnullable, b.collation, b.xprec, b.xscale
		FROM sysobjects a 
		inner join syscolumns b on a.id = b.id 
		inner join systypes c on b.xtype = c.xtype and c.name <> 'sysname' 
		WHERE a.id = object_id(N'[' + @Owner + '].[' + @TableName + ']') 
		and OBJECTPROPERTY(a.id, N'IsUserTable') = 1 
		ORDER BY b.colId

	OPEN TableColumns

	-- Declare temp variable to fetch records into
	DECLARE @ColumnName varchar(128)
	DECLARE @ColumnType varchar(128)
	DECLARE @ColumnLength smallint
	DECLARE @ColumnNullable int
	DECLARE @ColumnCollation sysname
	DECLARE @ColumnPrecision tinyint
	DECLARE @ColumnScale tinyint

	-- Declare variable to build statements
	DECLARE @CreateStatement varchar(8000)
	DECLARE @ListOfFields varchar(2000)
	SET @ListOfFields = ''


	-- Check if audit table exists
	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + @AuditNameExtention + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- AuditTable exists, update needed
		PRINT 'Table already exists. Only triggers will be updated.'

		FETCH Next FROM TableColumns
		INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@ColumnType <> 'text' and @ColumnType <> 'ntext' and @ColumnType <> 'image' and @ColumnType <> 'timestamp')
			BEGIN
				SET @ListOfFields = @ListOfFields + @ColumnName + ','
			END

			FETCH Next FROM TableColumns
			INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale

		END
	END
	ELSE
	BEGIN
		-- AuditTable does not exist, create new

		-- Start of create table
		SET @CreateStatement = 'CREATE TABLE [' + @Owner + '].[' + @TableName + @AuditNameExtention + '] ('
		SET @CreateStatement = @CreateStatement + '[AuditID] [bigint] IDENTITY (1, 1) NOT NULL,'

		FETCH Next FROM TableColumns
		INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@ColumnType <> 'text' and @ColumnType <> 'ntext' and @ColumnType <> 'image' and @ColumnType <> 'timestamp')
			BEGIN
				SET @ListOfFields = @ListOfFields + @ColumnName + ','
		
				SET @CreateStatement = @CreateStatement + '[' + @ColumnName + '] [' + @ColumnType + '] '
				
				IF @ColumnType in ('binary', 'char', 'nchar', 'nvarchar', 'varbinary', 'varchar')
				BEGIN
					IF (@ColumnLength = -1)
						Set @CreateStatement = @CreateStatement + '(max) '	 	
					ELSE
						SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnLength as varchar(10)) + ') '	 	
				END
		
				IF @ColumnType in ('decimal', 'numeric')
					SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnPrecision as varchar(10)) + ',' + cast(@ColumnScale as varchar(10)) + ') '	 	
		
				IF @ColumnType in ('char', 'nchar', 'nvarchar', 'varchar', 'text', 'ntext')
					SET @CreateStatement = @CreateStatement + 'COLLATE ' + @ColumnCollation + ' '
		
				IF @ColumnNullable = 0
					SET @CreateStatement = @CreateStatement + 'NOT '	 	
		
				SET @CreateStatement = @CreateStatement + 'NULL, '	 	
			END

			FETCH Next FROM TableColumns
			INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		END
		
		-- Add audit trail columns
		SET @CreateStatement = @CreateStatement + '[Operation] [char] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,'
		SET @CreateStatement = @CreateStatement + '[Time] [datetime] NOT NULL ,'
		SET @CreateStatement = @CreateStatement + '[User] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,'
		SET @CreateStatement = @CreateStatement + '[Location] [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL)' 

		-- Create audit table
		PRINT 'Creating audit table [' + @Owner + '].[' + @TableName + @AuditNameExtention + ']'
		EXEC (@CreateStatement)

		-- Set primary key and default values
		SET @CreateStatement = 'ALTER TABLE [' + @Owner + '].[' + @TableName + @AuditNameExtention + '] ADD '
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [DF_' + @TableName + @AuditNameExtention + '_Time] DEFAULT (SYSDATETIMEOFFSET()) FOR [Time],'
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [DF_' + @TableName + @AuditNameExtention + '_User] DEFAULT (CURRENT_USER) FOR [User],CONSTRAINT [PK_' + @TableName + @AuditNameExtention + '] PRIMARY KEY  CLUSTERED '
		SET @CreateStatement = @CreateStatement + '([AuditID])  ON [PRIMARY], '
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [DF_' + @TableName + @AuditNameExtention + '_Location]  DEFAULT (CONVERT(NVARCHAR, CONNECTIONPROPERTY(''client_net_address''))) for [Location]'

		EXEC (@CreateStatement)

	END

	CLOSE TableColumns
	DEALLOCATE TableColumns

	/* Drop Triggers, if they exist */
	PRINT 'Dropping triggers'
	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TR_' + @TableName + '_Insert]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TR_' + @TableName + '_Insert]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TR_' + @TableName + '_Update]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TR_' + @TableName + '_Update]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TR_' + @TableName + '_Delete]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TR_' + @TableName + '_Delete]')

	/* Create triggers */
	PRINT 'Creating triggers' 
	EXEC ('CREATE TRIGGER TR_' + @TableName + '_Insert ON ' + @Owner + '.' + @TableName + ' FOR INSERT AS INSERT INTO ' + @TableName + @AuditNameExtention + '(' +  @ListOfFields + 'Operation) SELECT ' + @ListOfFields + '''Insert'' FROM Inserted')

	EXEC ('CREATE TRIGGER TR_' + @TableName + '_Update ON ' + @Owner + '.' + @TableName + ' FOR UPDATE AS INSERT INTO ' + @TableName + @AuditNameExtention + '(' +  @ListOfFields + 'Operation) SELECT ' + @ListOfFields + '''Update'' FROM Inserted')

	EXEC ('CREATE TRIGGER TR_' + @TableName + '_Delete ON ' + @Owner + '.' + @TableName + ' FOR DELETE AS INSERT INTO ' + @TableName + @AuditNameExtention + '(' +  @ListOfFields + 'Operation) SELECT ' + @ListOfFields + '''Delete'' FROM Deleted')

END
GO

USE Music
GO
EXECUTE GenerateHistoryTable 'Album'
EXECUTE GenerateHistoryTable 'CatalogNumber'
EXECUTE GenerateHistoryTable 'Composer'
EXECUTE GenerateHistoryTable 'ComposerEra'
EXECUTE GenerateHistoryTable 'ComposerImage'
EXECUTE GenerateHistoryTable 'ComposerInfluence'
EXECUTE GenerateHistoryTable 'ComposerLink'
EXECUTE GenerateHistoryTable 'ComposerNationality'
EXECUTE GenerateHistoryTable 'Composition'
EXECUTE GenerateHistoryTable 'CompositionCatalog'
EXECUTE GenerateHistoryTable 'CompositionCollection'
EXECUTE GenerateHistoryTable 'CompositionCollectionComposer'
EXECUTE GenerateHistoryTable 'CompositionComposer'
EXECUTE GenerateHistoryTable 'Era'
EXECUTE GenerateHistoryTable 'Location'
EXECUTE GenerateHistoryTable 'Movement'
EXECUTE GenerateHistoryTable 'Nationality'
EXECUTE GenerateHistoryTable 'Performer'
EXECUTE GenerateHistoryTable 'Recording'
EXECUTE GenerateHistoryTable 'RecordingLocation'
EXECUTE GenerateHistoryTable 'RecordingPerformer'
EXECUTE GenerateHistoryTable 'Sample'
GO


