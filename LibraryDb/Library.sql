DROP TABLE [Recordings];

CREATE TABLE [Recording] (
	[ID] INT PRIMARY KEY,
	[MDBID] INT NOT NULL,
	[FilePath] varchar(260) NOT NULL UNIQUE
);