USE Music;

CREATE TABLE [Sample] (
	ID smallint PRIMARY KEY,
	Audio varbinary(MAX) NOT NULL,
	Title nvarchar(255) NOT NULL,
	Artists nvarchar(255) NOT NULL,
	ComposerID smallint NOT NULL,
	CONSTRAINT FK_Sample_ComposerID FOREIGN KEY (ComposerID) REFERENCES Composer(ID)
);