USE Music;
GO

CREATE TABLE CompositionLink(
	ID int,
	URL nvarchar(255) NOT NULL,
	CompositionID int NOT NULL,
	CONSTRAINT PK_CompositionLink PRIMARY KEY (ID),
	CONSTRAINT FK_CompositionLink_CompositionID FOREIGN KEY (CompositionID) REFERENCES Composition(ID)
);

CREATE TABLE CompositionType(
	ID smallint,
	Name nvarchar(128) NOT NULL,
	CONSTRAINT PK_CompositionType PRIMARY KEY (ID)
);

CREATE TABLE Instrumentation(
	ID smallint,
	Name nvarchar(255) NOT NULL,
	CONSTRAINT PK_Instrumentation PRIMARY KEY (ID)
);

CREATE TABLE [Key](
	ID smallint,
	Name nvarchar(15) NOT NULL,
	CONSTRAINT PK_Key PRIMARY KEY (ID)
);	

ALTER TABLE Composition
ADD Comment nvarchar(255),
	Premiere nvarchar(255),
	Dedication nvarchar(255),
	Occasion nvarchar(255),
	CompositionTypeID smallint,
	KeyID smallint,
	InstrumentationID smallint,
	CONSTRAINT FK_Composition_CompositionTypeID FOREIGN KEY (CompositionTypeID) REFERENCES CompositionType(ID),
	CONSTRAINT FK_Composition_KeyID FOREIGN KEY (KeyID) REFERENCES [Key](ID),
	CONSTRAINT FK_Composition_InstrumentationID FOREIGN KEY (InstrumentationID) REFERENCES Instrumentation(ID);

EXECUTE GenerateHistoryTable 'CompositionLink';
EXECUTE GenerateHistoryTable 'CompositionCategory';
EXECUTE GenerateHistoryTable 'Instrumentation';
EXECUTE GenerateHistoryTable 'Key';
EXECUTE GenerateHistoryTable 'Composition', 'dbo', 'History', 1;
GO